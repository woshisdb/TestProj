using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

public class EndAct:Act
{

    public EndAct(Person person, Obj obj):base(person, obj)
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
    public SeqAct(Person person, Obj obj,params Act[] acts) : base(person, obj)
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
            yield return Ret(new EndAct(Person, Obj), callback);
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
    public SelectTime(Person person, Obj obj, int[] canSelectTime,int priority = -1) : base(person, obj, priority)
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
            CardInf cardInf = new CardInf(time + ":Step", "ִ��ʱ��", () => {
                selectTime.val = time;
                Debug.Log(selectTime);
            });
            cardInfs.Add(cardInf);
        }
        yield return AddDecision(Person, new DecisionTex("ʱ��ѡ��","���������",
            cardInfs
        ));
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}


public class SelectPiplineA : Act
{
    public SelectPiplineA(Person person, BuildingObj obj, int priority = -1) : base(person, obj, priority)
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
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("ѡ�����","ѡ����������",sels,
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
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

public class SelPipLineAct:Activity
{
    public int use;
    public SelPipLineAct(Func<Obj, Person, object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        use = 0;
        activityName = "ѡ�����";
        detail = "ѡ���Լ�����������";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }

    /// <summary>
    /// Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(
            new SelectPiplineA(person,(BuildingObj) obj), obj, person,winDatas, objs);
    }
}

public class SetPiplineA : Act
{
    public SetPiplineA(Person person, BuildingObj obj, int priority = -1) : base(person, obj, priority)
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
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("���ù���", "���ù��߱���", sels,
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
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

public class SetPipLineAct : Activity
{
    public int use;
    public SetPipLineAct(Func<Obj, Person, object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        use = 0;
        activityName = "�趨�����޶�";
        detail = "ѡ����ߵ������޶�";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }

    /// <summary>
    /// Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs(
            new SetPiplineA(person, (BuildingObj)obj), obj, person,winDatas, objs);
    }
}


public class UseToolAct : Activity
{
    public int use;
    public UseToolAct(Func<Obj, Person, object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        use = 0;
        activityName = "ʹ�ù���";
        detail = "ʹ�ù���������";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }

    /// <summary>
    /// Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        int[] times = { 1,2,3,4,5,6,7,8,9,10,11,12};
        var seleA = new SelectTime(person, obj, times);
        var useA=new UseToolA(person, (BuildingObj)obj);
        var relA = new ReleaseToolA(person, (BuildingObj)obj,useA.tool);
        return GetActs(
            new SeqAct(person,obj,
            useA,
            seleA,
            new WasteTimeA(person,obj,seleA.selectTime),
            relA
            ),
        obj, person,winDatas, objs);
    }
}

public class UseToolA : Act
{
    public Enum<ObjEnum> tool;
    public UseToolA(Person person, BuildingObj obj, int priority = -1) : base(person, obj, priority)
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
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new DecisionTex("ʹ�ù���", "ѡ�񹤾�������",
            sels
            )
        );
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
public class ReleaseToolA : Act
{
    Enum<ObjEnum> v;
    public ReleaseToolA(Person person, BuildingObj obj,Enum<ObjEnum> v, int priority = -1) : base(person, obj, priority)
    {
        wastTime = true;
        this.v = v;
    }
    public override IEnumerator<object> Run(Action<Act> callback)
    {
        BuildingObj obj = (BuildingObj)Obj;
        Debug.Log(v.value);
        obj.resource.Release(v.value,1);
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}

/// <summary>
/// ����
/// </summary>
public class BuildA : Act
{
    public BuildA(Person person, BuildingObj obj, int priority = -1) : base(person, obj, priority)
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
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("���ù���", "���ù��߱���", sels,
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
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// ������ʩ�Ļ
/// </summary>
public class BuildAct : Activity
{
    public int use;
    public BuildAct(Func<Obj, Person, object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        use = 0;
        activityName = "�趨�����޶�";
        detail = "ѡ����ߵ������޶�";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }

    /// <summary>
    /// Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person,List<WinData> winDatas=null, params object[] objs)
    {
        return GetActs(
            new BuildA(person, (BuildingObj)obj), obj, person,winDatas, objs);
    }
}

/// <summary>
/// ����
/// </summary>
public class WaKuangA : Act
{
    Int time;
    public WaKuangA(Person person, BuildingObj obj,Int time, int priority = -1) : base(person, obj, priority)
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
        kuang.resource.Add(t.Key,t.Value);
        if(time<=0)
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// ������ʩ�Ļ
/// </summary>
public class WaKuangAct : Activity
{
    public int use;
    public WaKuangAct(Func<Obj, Person, object[], bool> cond = null, Func<Obj, Person, object[], Act> eff = null) : base(cond, eff)
    {
        use = 0;
        activityName = "�ڿ�";
        detail = "�ھ����";
    }
    public override bool Condition(Obj obj, Person person, params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }

    /// <summary>
    /// Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        var selTime =new SelectTime(person,obj,new int[] {1,2,3,4,5,6,7,8});
        var t = new WaKuangA(person, (BuildingObj)obj, selTime.selectTime);
        return GetActs(new SeqAct(person,obj,selTime,t), obj, person, winDatas, objs);
    }
}









public abstract class Act
{
    /// <summary>
    /// ���ȼ�:
    /// 0��ʾ������ȼ�����������ǰ�棩
    /// ��Խ�����ʾԽ��ǰ
    /// ������ʾ����������ԼС��ʾҪ��Խ��
    /// </summary>
    public int priority;
    public Person Person;
    public Obj Obj;
    public bool wastTime;
    public List<WinData> winDatas;
    public Act(Person person, Obj obj,int priority=-1)
    {
        Person = person;
        Obj = obj;
        this.priority = priority;
        winDatas = new List<WinData>();//һϵ�еľ���
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


    public IEnumerator AddDecision(Person person, WinCon decision)
    {
        yield return GameArchitect.gameLogic.AddDecision(person,decision);
        winDatas.Add(decision.data);
    }
    public virtual void SetWinData(List<WinData> winDatas)
    {
        this.winDatas = winDatas;
    }
}
