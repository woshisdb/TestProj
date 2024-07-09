using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������Ʒ
/// </summary>
public class BuyA : Act
{
    public BuyA(Person person, Obj obj, int priority = -1) : base(person, obj, priority)
    {
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("����");
        List<SelectInf> selects = new List<SelectInf>();
        BuildingObj buildingObj = (BuildingObj)Obj;//��������
        foreach (var t in buildingObj.goodsManager.items)
        {
            selects.Add(//�������Ķ���
                new SelectInf(t.Key.obj.objSaver.title, t.Key.obj.objSaver.description+"X"+ t.Key.num+":"+t.Key.cost, t.Key.obj)
            );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("����", "ѡ��Ҫ����Ķ���",
            selects,
            () =>
            {
                int sum = 0;
                for(int i=0;i<selects.Count;i++)
                {
                    var sx=(Goods)selects[i].obj;
                    sum += sx.cost * selects[i].num;
                }
                if(sum > Person.money.val)
                {
                    return false;//����ִ��
                }
                else
                {
                    Person.money.val -= sum;
                    return true;
                }
            }));
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// ������Ϊ
/// </summary>
public class BuyAct : Activity
{
    public BuyAct(Func<Obj, Person,object[], bool> cond=null, Func<Obj, Person,object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "��";
        detail = "������Ʒ";
    }

    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return new BuyA(person, obj);
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
/// <summary>
/// �ҵ�������
/// </summary>
public class SellA : Act
{
    public SellA(Person person, Obj obj, int priority = -1) : base(person, obj, priority)
    {
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        BuildingObj buildingObj = (BuildingObj)Obj;//��������
        Debug.Log("����");
        List<SelectInf> selects = new List<SelectInf>();
        foreach(var t in buildingObj.goodsManager.items)
        {
            selects.Add(//����Ʒ��ӵ�����
                new SelectInf(t.Key.obj.objSaver.title, t.Key.obj.objSaver.description,t.Key.obj)
            );
        }
        yield return GameArchitect.gameLogic.AddDecision(Person,
            new SelectTex("��", "ѡ��Ҫ���Ķ���",
            selects,
            () =>
            {
                return true;
            }));
        yield return Ret(new EndAct(Person, Obj), callback);
    }
}
/// <summary>
/// ������Ϊ
/// </summary>
public class SellAct : Activity
{
    public SellAct(Func<Obj, Person,object[], bool> cond = null, Func<Obj, Person,object[], Act> eff = null) : base(cond, eff)
    {
        activityName = "��";
        detail = "����Ʒ";
    }

    public override Act Effect(Obj obj, Person person, params object[] objs)
    {
        return new SellA(person, obj);
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
}
