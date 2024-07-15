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
        yield return GameArchitect.gameLogic.AddDecision(Person, new DecisionTex("ʱ��ѡ��","���������",
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
        return true;
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
    public override Act Effect(Obj obj, Person person, params object[] objs)
    {
        return GetActs(
            new SelectPiplineA(person,(BuildingObj) obj), obj, person, objs);
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
        return true;
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
    public override Act Effect(Obj obj, Person person, params object[] objs)
    {
        return GetActs(
            new SetPiplineA(person, (BuildingObj)obj), obj, person, objs);
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

    public Act(Person person, Obj obj,int priority=-1)
    {
        Person = person;
        Obj = obj;
        this.priority = priority;
    }
    public IEnumerator Ret(Act act, System.Action<Act> callback)
    {
        callback(act);
        yield break;
    }
    public void TC()
    {
        if (wastTime)
            GameLogic.hasTime = false;
    }
    public abstract IEnumerator<object> Run(System.Action<Act> callback);
}
