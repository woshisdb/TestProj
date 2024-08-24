using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

public class EndAct:Act
{

    public EndAct(PersonObj PersonObj, Obj obj):base(PersonObj, obj)
    {
        
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Debug.Log("sfsada");
        yield break;
    }
}

public class SeqAct : Act
{
    List<Act> acts = new List<Act>();
    public SeqAct(PersonObj PersonObj, Obj obj,params Act[] acts) : base(PersonObj, obj)
    {
        wastTime = false;
        this.acts.AddRange(acts);
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        while (GameLogic.hasTime&&acts!=null&&acts.Count>0)
        {
            yield return acts[0].Run(
                (result) =>
                {
                    if (result is EndAct)
                    {
                        acts.RemoveAt(0);
                    }
                    else if (result is Act)
                    {
                        acts[0] = result;
                    }
                }
            );
        }
        if (acts==null||acts.Count == 0)
        {
            yield return Ret(new EndAct(PersonObj, Obj), callback);
        }
    }
    public override void SetWinData(List<WinData> winDatas)
    {
        base.SetWinData(winDatas);
        foreach(var act in acts)
        {
            act.winDatas = winDatas;
        }
    }
}

public class SelectTime : Act
{
    public int[] canSelectTime;
    public Int selectTime;
    public SelectTime(PersonObj PersonObj, Obj obj, int[] canSelectTime,int priority = -1) : base(PersonObj, obj, priority)
    {
        wastTime = false;
        this.canSelectTime = canSelectTime;
        this.selectTime = new Int();
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        List<CardInf> cardInfs = new List<CardInf>();
        Debug.Log(canSelectTime.Length);
        for(int i=0;i<canSelectTime.Length; i++)
        {
            var time = canSelectTime[i];
            CardInf cardInf = new CardInf(time + ":Step", "执行时间", () => {
                selectTime.val = time;
                Debug.Log(selectTime);
            });
            cardInfs.Add(cardInf);
        }
        yield return AddDecision(PersonObj, new DecisionTex("时间选择","打算做多久",
            cardInfs
        ));
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}


public class SelectPiplineA : Act
{
    public SelectPiplineA(PersonObj PersonObj, BuildingObj obj, int priority = -1) : base(PersonObj, obj, priority)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
	{
        BuildingObj obj = (BuildingObj)Obj;
        var pipline = GameArchitect.get.objAsset.nodeGraph.trans;
        var sels = new List<SelectInf>(pipline.Count);
        foreach(var s in pipline)
        {
            sels.Add(new SelectInf(s.title,"",s,1));
        }
        yield return GameArchitect.gameLogic.AddDecision(PersonObj,
            new SelectTex("选择管线","选择制作管线",sels,
            ()=> {
                var selPipline = new List<Trans>();
                for (int i = 0; i < sels.Count; i++)
                {
                    if (sels[i].num == 1)
                        selPipline.Add((Trans)sels[i].obj);
                    Debug.Log(sels[i].num);
                }
                obj.pipLineManager.SetTrans(selPipline);
                Debug.Log(11);
                return true;
            }
            )
        );
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}

public class SelPipLineAct:Activity
{
    public int use;
    public SelPipLineAct() : base()
    {
        use = 0;
        activityName = "选择管线";
        detail = "选择自己的生产管线";
    }
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return true;// ((BuildingObj)obj).remainBuilder == 0;
    }
    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(
            new SelectPiplineA(PersonObj,(BuildingObj) obj), obj, PersonObj,winDatas, objs);
    }
}

public class SetPiplineA : Act
{
    public SetPiplineA(PersonObj PersonObj, BuildingObj obj, int priority = -1) : base(PersonObj, obj, priority)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        BuildingObj obj = (BuildingObj)Obj;
        var pipline = GameArchitect.get.objAsset.nodeGraph.trans;
        var sels = new List<SelectInf>(obj.pipLineManager.piplineItem.Count);
        foreach (var s in obj.pipLineManager.piplineItem)
        {
            sels.Add(new SelectInf(s.Key.title, "", s.Value,9999));
        }
        yield return GameArchitect.gameLogic.AddDecision(PersonObj,
            new SelectTex("设置管线", "设置管线比例", sels,
            () => {
                var selPipline = new List<Trans>();
                for (int i = 0; i < sels.Count; i++)
                {
                    ((Source)sels[i].obj).maxnCount = sels[i].num;
                }
                return true;
            }
            )
        );
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}

public class SetPipLineAct : Activity
{
    public int use;
    public SetPipLineAct() : base()
    {
        use = 0;
        activityName = "设定管线限度";
        detail = "选择管线的生产限度";
    }
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return true;// ((BuildingObj)obj).remainBuilder == 0;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}

    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(
            new SetPiplineA(PersonObj, (BuildingObj)obj), obj, PersonObj,winDatas, objs);
    }
}


public class UseToolAct : Activity
{
    public UseToolAct():base()
    {
        activityName = "使用工具";
        detail = "使用工具来工作";
    }
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return true;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}

    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        int[] times = { 1,2,3,4,5,6,7,8,9,10,11,12};
        var seleA = new SelectTime(PersonObj, obj, times);
        var useA=new UseToolA(PersonObj, (BuildingObj)obj);
        var relA = new ReleaseToolA(PersonObj, (BuildingObj)obj,useA.tool);
        return GetActs(
            new SeqAct(PersonObj,obj,
            useA,
            seleA,
            new WasteTimeA(PersonObj,obj,seleA.selectTime),
            relA
            ),
        obj, PersonObj,winDatas, objs);
    }
}

public class UseToolA : Act
{
    public Enum<ObjEnum> tool;
    public UseToolA(PersonObj PersonObj, BuildingObj obj, int priority = -1) : base(PersonObj, obj, priority)
    {
        wastTime = true;
        tool= new Enum<ObjEnum>(ObjEnum.ObjE);
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        BuildingObj obj = (BuildingObj)Obj;
        var pipline = GameArchitect.get.objAsset.nodeGraph.trans;
        var sels = new List<CardInf>();
        var objs = obj.resource.GetObjs(ObjEnum.ToolObjE);
        foreach (var o in objs)
        {
            var x = o;
            if(x.Value.remain>0)
            sels.Add(new CardInf( Map.Instance.GetSaver(o.Key).title , Map.Instance.GetSaver(o.Key).description,
                () =>
                {
                    obj.resource.Use(x.Key,1);
                    tool.value = x.Key;
                }
            ));
        }
        yield return GameArchitect.gameLogic.AddDecision(PersonObj,
            new DecisionTex("使用工具", "选择工具来工作",
            sels
            )
        );
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}
public class ReleaseToolA : Act
{
    Enum<ObjEnum> v;
    public ReleaseToolA(PersonObj PersonObj, BuildingObj obj,Enum<ObjEnum> v, int priority = -1) : base(PersonObj, obj, priority)
    {
        wastTime = true;
        this.v = v;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        BuildingObj obj = (BuildingObj)Obj;
        Debug.Log(v.value);
        obj.resource.Release(v.value,1);
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}

/// <summary>
/// 建造活动
/// </summary>
public class BuildA : Act
{
    public BuildA(PersonObj PersonObj, BuildingObj obj, int priority = -1) : base(PersonObj, obj, priority)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        BuildingObj obj = (BuildingObj)Obj;
        var pipline = GameArchitect.get.objAsset.nodeGraph.trans;
        var sels = new List<SelectInf>(obj.pipLineManager.piplineItem.Count);
        foreach (var s in obj.pipLineManager.piplineItem)
        {
            sels.Add(new SelectInf(s.Key.title, "", s.Value, 9999));
        }
        yield return GameArchitect.gameLogic.AddDecision(PersonObj,
            new SelectTex("设置管线", "设置管线比例", sels,
            () => {
                var selPipline = new List<Trans>();
                for (int i = 0; i < sels.Count; i++)
                {
                    ((Source)sels[i].obj).maxnCount = sels[i].num;
                }
                return true;
            }
            )
        );
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}
/// <summary>
/// 建造设施的活动
/// </summary>
public class BuildAct : Activity
{
    public int use;
    public BuildAct() : base()
    {
        use = 0;
        activityName = "设定管线限度";
        detail = "选择管线的生产限度";
    }
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}

    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj,List<WinData> winDatas=null, params object[] objs)
    {
        return GetActs(
            new BuildA(PersonObj, (BuildingObj)obj), obj, PersonObj,winDatas, objs);
    }
}

/// <summary>
/// 建造活动
/// </summary>
public class WaKuangA : Act
{
    Int time;
    public WaKuangA(PersonObj PersonObj, BuildingObj obj,Int time, int priority = -1) : base(PersonObj, obj, priority)
    {
        this.time = time;
        wastTime = true;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        time--;
        Debug.Log("Wa");
        var kuang = (KuangMiningObj)Obj;
        var t = kuang.GetRes(10);
        kuang.resource.Add(kuang.GetObj(),t.Value);
        if(time<=0)
        yield return Ret(new EndAct(PersonObj, Obj), callback);
    }
}
/// <summary>
/// 建造设施的活动
/// </summary>
public class WaKuangAct : Activity
{
    public int use;
    public WaKuangAct() : base()
    {
        use = 0;
        activityName = "挖矿";
        detail = "挖掘矿物";
    }
    public override bool Condition(Obj obj, PersonObj PersonObj, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}

    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="PersonObj"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, PersonObj PersonObj, List<WinData> winDatas = null, params object[] objs)
    {
        var selTime =new SelectTime(PersonObj,obj,new int[] {1,2,3,4,5,6,7,8});
        var t = new WaKuangA(PersonObj, (BuildingObj)obj, selTime.selectTime);
        return GetActs(new SeqAct(PersonObj,obj,selTime,t), obj, PersonObj, winDatas, objs);
    }
}

public abstract class Act
{
    /// <summary>
    /// 优先级:
    /// 0表示随机优先级（非最后和最前面）
    /// 数越高则表示越靠前
    /// 负数表示尽量靠后，数约小表示要求越晚
    /// </summary>
    public int priority;
    public PersonObj PersonObj;
    public Obj Obj;
    public bool wastTime;
    public List<WinData> winDatas;
    public Act(PersonObj PersonObj, Obj obj,int priority=-1)
    {
        this.PersonObj = PersonObj;
        Obj = obj;
        this.priority = priority;
        winDatas = new List<WinData>();//一系列的决策
    }
    public IEnumerator Ret(Act act, System.Action<Act> callback)
    {
        act.SetWinData(winDatas);
        callback(act);
        yield break;
    }
    public void TC()
    {
        if (wastTime)
            GameLogic.hasTime = false;
    }
    public abstract IEnumerator<object> Run(System.Action<Act> callback);


    public IEnumerator AddDecision(PersonObj PersonObj, WinCon decision)
    {
        yield return GameArchitect.gameLogic.AddDecision(PersonObj,decision);
        winDatas.Add(decision.data);
    }
    public virtual void SetWinData(List<WinData> winDatas)
    {
        this.winDatas = winDatas;
    }
}
