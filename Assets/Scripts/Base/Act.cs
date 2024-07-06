using System;
using System.Collections;
using System.Collections.Generic;
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
            );//���лص�
            //if (acts.Count == 0)
            //{
            //    yield return Ret(new EndAct(Person, Obj), callback);
            //}
            //else
            //{
            //    //Debug.Log("Run" + acts.Count);
            //    yield return acts[0].Run(
            //    (result) =>
            //    {
            //    //Debug.Log("End"+acts.Count);
            //        if (result is EndAct)
            //        {
            //            acts.RemoveAt(0);
            //        }
            //        else if (result is Act)
            //        {
            //            acts[0] = result;
            //        }
            //    }
            //    );//���лص�
            //}
        }
        if (acts.Count == 0)
        {
            yield return Ret(new EndAct(Person, Obj), callback);
        }
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
