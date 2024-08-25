using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using QFramework;
//using Unity.VisualScripting;
//using UnityEngine;
//public class EndTurnNode : Unit, ICanRegisterEvent
//{
//    //这两个是必须有的，是左右的小箭头，只需要固定这么写
//    [DoNotSerialize]
//    public ControlInput inputTrigger;
//    [DoNotSerialize]
//    public ControlOutput outputTrigger;
//    public IArchitecture GetArchitecture()
//    {
//        return GameArchitect.Interface;
//    }

//    protected override void Definition() //The method to set what our node will be doing.
//    {
//        inputTrigger = ControlInput("inputTrigger", (flow) => {
//            this.SendEvent<EndTurnEvent>(new EndTurnEvent(GameArchitect.nowPersonObj));
//            return outputTrigger;
//        });
//        //Making the ControlOutput port visible and setting its key.
//        outputTrigger = ControlOutput("outputTrigger");
//    }
//}
//public class EndWorkNode : Unit, ICanRegisterEvent
//{
//    //这两个是必须有的，是左右的小箭头，只需要固定这么写
//    [DoNotSerialize]
//    public ControlInput inputTrigger;
//    [DoNotSerialize]
//    public ControlOutput outputTrigger;
//    public IArchitecture GetArchitecture()
//    {
//        return GameArchitect.Interface;
//    }

//    protected override void Definition() //The method to set what our node will be doing.
//    {
//        inputTrigger = ControlInput("inputTrigger", (flow) => {
//            this.SendEvent<EndWorkEvent>(new EndWorkEvent(GameArchitect.nowPersonObj));
//            return outputTrigger;
//        });
//        //Making the ControlOutput port visible and setting its key.
//        outputTrigger = ControlOutput("outputTrigger");
//    }
//}
/// <summary>
///工作结束
/// </summary>
public class EndWorkEvent
{
    public PersonObj PersonObj;
    public EndWorkEvent(PersonObj PersonObj)
    {
        this.PersonObj = PersonObj;
    }
}
/// <summary>
/// 角色回合结束
/// </summary>
public class EndTurnEvent
{
    public PersonObj PersonObj;
    public EndTurnEvent(PersonObj PersonObj)
    {
        this.PersonObj = PersonObj;
    }
}
/// <summary>
/// 角色回合开始
/// </summary>
public class BeginTurnEvent
{
    public PersonObj PersonObj;
    public BeginTurnEvent(PersonObj PersonObj)
    {
        this.PersonObj = PersonObj;
    }
}
public class ThinkModel : ICanRegisterEvent
{
    protected TaskCompletionSource<bool> tcs;
    public PersonObj PersonObj;

    public ThinkModel(PersonObj PersonObj)
    {
        tcs = new TaskCompletionSource<bool>();
        this.PersonObj=PersonObj;
        this.RegisterEvent<BeginActEvent>(
        (e) =>
        {
            if (e.PersonObj == PersonObj)
            {
                Exe();
            }
        });
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public virtual Task BeginThink(Dictionary<Obj, CardInf[]> opts)
    {
        return null;
    }
    public IEnumerator<object> RunPersonObj(PersonObj PersonObj, System.Action<object> callback)
    {
        while (GameLogic.hasTime&& PersonObj.hasSelect)//循环搜索
        {
            yield return GameArchitect.gameLogic.StartCoroutine(
                PersonObj.act.Run(callback)
            );
        }
        this.SendEvent<EndActEvent>(new EndActEvent(PersonObj));
    }
    protected void Exe()
    {
        MainDispatch.Instance().Enqueue(
        () =>
        {
            GameArchitect.gameLogic.StartCoroutine(RunPersonObj(PersonObj, (result) => {
                if (result is EndAct)
                    PersonObj.RemoveAct();
                else if (result is Act)
                    PersonObj.SetAct((Act)result);
            }));
        });
    }
}
/// <summary>
/// 角色思考
/// </summary>
public class PlayerThinkModel: ThinkModel
{
    public PlayerThinkModel(PersonObj PersonObj):base(PersonObj)
    {
        this.RegisterEvent<EndTurnEvent>(
        (e) => {
            if (e.PersonObj == this.PersonObj)
            {
                //Debug.Log(tcs);
                tcs.TrySetResult(true);
            }
        }
        );
    }
    /// <summary>
    /// 等待回合结束
    /// </summary>
    /// <returns></returns>
    public override Task BeginThink(Dictionary<Obj, CardInf[]> opts)
    {
        if (!PersonObj.hasSelect)//无活动
        {
            MainDispatch.Instance().Enqueue(
            () =>
            {
                this.SendEvent<BeginTurnEvent>(new BeginTurnEvent(PersonObj));//触发思考活动
            });
            tcs = new TaskCompletionSource<bool>();
            return tcs.Task;
        }
        else//有活动
        {
            return Task.CompletedTask;
        }
    }
}
/// <summary>
/// NPC思考
/// </summary>
public class NPCThinkModel: ThinkModel
{
    public NPCThinkModel(PersonObj PersonObj) : base(PersonObj)
    {

    }
    /// <summary>
    /// NPC思考结束
    /// </summary>
    /// <returns></returns>
    public override Task BeginThink(Dictionary<Obj, CardInf[]> opts)
    {
        return Task.CompletedTask;
    }
}
public class ThinkModelSet: AbstractModel
{
    public Dictionary<PersonObj, ThinkModel> thinks;
    public ThinkModelSet()
    {
        thinks = new Dictionary<PersonObj, ThinkModel>();
        for(int i=0;i<GameArchitect.PersonObjs.Count;i++)
        {
            if ((GameArchitect.PersonObjs[i]).isPlayer)
            {
                thinks.Add(GameArchitect.PersonObjs[i], new PlayerThinkModel(GameArchitect.PersonObjs[i]));
            }
            else
            {
                thinks.Add(GameArchitect.PersonObjs[i],new NPCThinkModel(GameArchitect.PersonObjs[i]));
            }
        }
    }
    public void AddThinkMode(PersonObj PersonObj)
    {
        if (PersonObj.isPlayer)
        {
            thinks.Add(PersonObj, new PlayerThinkModel(PersonObj));
        }
        else
        {
            thinks.Add(PersonObj, new NPCThinkModel(PersonObj));
        }
    }
    protected override void OnInit()
    {
        
    }
}
