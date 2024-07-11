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
        while (GameLogic.hasTime&&acts.Count>0)
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
        if (acts.Count == 0)
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
            sels.Add(new SelectInf("1","2",s,1));
        }
        yield return GameArchitect.get.AddDecision(
            new SelectTex("test","test1",sels,
            ()=> {
                var selPipline = new List<Trans>();
                for (int i = 0; i < sels.Count; i++)
                {
                    if (sels[i].num == 1)
                        selPipline.Add((Trans)sels[i].obj);
                }
                obj.pipLineManager.SetTrans(selPipline);
                return true;
            }
            )
        );
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
