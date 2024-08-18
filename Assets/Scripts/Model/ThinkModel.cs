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
//            this.SendEvent<EndTurnEvent>(new EndTurnEvent(GameArchitect.nowPerson));
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
//            this.SendEvent<EndWorkEvent>(new EndWorkEvent(GameArchitect.nowPerson));
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
    public Person person;
    public EndWorkEvent(Person person)
    {
        this.person = person;
    }
}
/// <summary>
/// 角色回合结束
/// </summary>
public class EndTurnEvent
{
    public Person person;
    public EndTurnEvent(Person person)
    {
        this.person = person;
    }
}
/// <summary>
/// 角色回合开始
/// </summary>
public class BeginTurnEvent
{
    public Person person;
    public BeginTurnEvent(Person person)
    {
        this.person = person;
    }
}
public class ThinkModel : ICanRegisterEvent
{
    protected TaskCompletionSource<bool> tcs;
    public Person person;

    public ThinkModel(Person person)
    {
        tcs = new TaskCompletionSource<bool>();
        this.person=person;
        this.RegisterEvent<BeginActEvent>(
        (e) =>
        {
            if (e.Person == person)
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
    public IEnumerator<object> RunPerson(Person person, System.Action<object> callback)
    {
        while (GameLogic.hasTime&& person.hasSelect)//循环搜索
        {
            yield return GameArchitect.gameLogic.StartCoroutine(
                person.act.Run(callback)
            );
        }
        this.SendEvent<EndActEvent>(new EndActEvent(person));
    }
    protected void Exe()
    {
        MainDispatch.Instance().Enqueue(
        () =>
        {
            //Debug.Log("Turn");
            GameArchitect.gameLogic.StartCoroutine(RunPerson(person, (result) => {
                if (result is EndAct)
                    person.RemoveAct();
                else if (result is Act)
                    person.SetAct((Act)result);
            }));
        });
    }
}
/// <summary>
/// 角色思考
/// </summary>
public class PlayerThinkModel: ThinkModel
{
    public PlayerThinkModel(Person person):base(person)
    {
        this.RegisterEvent<EndTurnEvent>(
        (e) => {
            if (e.person == this.person)
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
        if (!person.hasSelect)//无活动
        {
            var c = person.contractManager.GetCode();
            if (c != null)//有可执行的活动
            {
                if (c.obj.belong == person.belong && c.activity.Condition(c.obj, person))
                {
                    person.SetAct(c.activity.Effect(c.obj, person));
                    return Task.CompletedTask;
                }
            }
            MainDispatch.Instance().Enqueue(
            () =>
            {
                this.SendEvent<BeginTurnEvent>(new BeginTurnEvent(person));//触发思考活动
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
    public NPCThinkModel(Person person) : base(person)
    {

    }
    /// <summary>
    /// NPC思考结束
    /// </summary>
    /// <returns></returns>
    public override Task BeginThink(Dictionary<Obj, CardInf[]> opts)
    {
        //return Task.CompletedTask;
        if (!person.hasSelect)//无活动
        {
            var c = person.contractManager.GetCode();
            if (c != null)//有可执行的活动
            {
                if(c.obj.belong==person.belong&&c.activity.Condition(c.obj,person))
                {
                    person.SetAct(c.activity.Effect(c.obj,person));
                    return Task.CompletedTask;
                }
            }
            var action = opts.GetValueOrDefault(person)[0];
            action.effect.Invoke();
            return person.pathGenerator.GetPath(person.domainGenerator, person.problemGenerator);//路径生成
        }
        else
        {
            return Task.CompletedTask;
        }
    }
}
public class ThinkModelSet: AbstractModel
{
    public Dictionary<Person, ThinkModel> thinks;
    public ThinkModelSet()
    {
        thinks = new Dictionary<Person, ThinkModel>();
        for(int i=0;i<GameArchitect.persons.Count;i++)
        {
            if ((GameArchitect.persons[i]).isPlayer)
            {
                thinks.Add(GameArchitect.persons[i], new PlayerThinkModel(GameArchitect.persons[i]));
            }
            else
            {
                thinks.Add(GameArchitect.persons[i],new NPCThinkModel(GameArchitect.persons[i]));
            }
        }
    }
    public void AddThinkMode(Person person)
    {
        if (person.isPlayer)
        {
            thinks.Add(person, new PlayerThinkModel(person));
        }
        else
        {
            thinks.Add(person, new NPCThinkModel(person));
        }
    }
    protected override void OnInit()
    {
        
    }
}
