using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// ǩ���Լ
/// </summary>
public class ArrangeA : Act
{
    public ArrangeA(Person person,Obj obj):base(person,obj)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        List<Contract> contracts = GameArchitect.get.GetModel<ContractModel>().GetUnSignContract();
        var arrange = new List<CardInf>();
        foreach(Contract contract in contracts)
        {
            //var it = contract;
            //if(Person.contractManager.CanSign(it))//�ܹ�ǩ��
            //arrange.Add(new CardInf(it.contractName, it.contractInfo, () => 
            //    {
            //        Person.contractManager.Sign(it);//ǩ��
            //    }
            //)
            //);
        }
        yield return GameArchitect.gameLogic.AddDecision(Person, new DecisionTex("ѡ���Լ", "ѡ��һ�����ʵĵĺ�Լ��ʼ�", 
            arrange
        ));
        var act = new EndAct(Person, Obj);
        yield return Ret(act, callback);
    }
}
/// <summary>
/// ǩ���Լ�Ļ
/// </summary>
[Act]
public class ArrangeContractAct : Activity
{
    public ArrangeContractAct()
    {
        activityName = "ǩ��Э��";
        detail = "ǩ��ǰ���ڵ�Э��";
    }
    public override bool Condition(Obj obj, Person person,params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }
    
    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
    [Button]
    public void ShowAction()
    {
        Debug.Log(GetAction().ToString());
    }

    /// <summary>
    /// ˯��Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return new ArrangeA(person, obj);
    }
}

/// <summary>
/// ��Ӻ�Լ
/// </summary>
public class AddContractA : Act
{
    public AddContractA(Person person, Obj obj) : base(person, obj)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Person person = (Person)Person;
        Debug.Log("Arrange");
        List<SelectInf> sels = new List<SelectInf>();
        var contracModel = GameArchitect.get.GetModel<ContractModel>();
        foreach(var contract in contracModel.contractTemplate)
        {
            var it =contract;
            //var key=new SelectInf(it.contractName, it.contractInfo,it);
            //sels.Add(key);
        }
        //����һ����Լ
        //if (sels.Count > 0)
        //yield return GameArchitect.gameLogic.AddDecision(person,
        //    new SelectTex("����һ����Լ", "ѡ��һ�����ʵ�ģ�崴���",
        //    sels,
        //    () =>
        //    {
        //        for(var i = 0; i < sels.Count; i++)
        //        {
        //            var sel = sels[i];
        //            //var data = ((Contract)sel.obj).CopyContract();
        //            data.ap = person;
        //            GameArchitect.get.GetModel<ContractModel>().RegistContract(data);
        //        }
        //        return true;
        //    }
        //    )
        //);
        var act = new EndAct(Person, Obj);
        yield return Ret(act, callback);
    }
}
/// <summary>
/// ��Ӻ�Լ
/// </summary>
[Act]
public class AddContractAct : Activity
{
    public AddContractAct()
    {
        activityName = "���Э��";
        detail = "���һϵ��Э��";
    }
    public override bool Condition(Obj obj, Person person,params object[] objs)
    {
        return ((BuildingObj)obj).remainBuilder == 0;
    }

    public override PAction GetAction()
    {
        PAction action = new PAction();
        return action;
    }
    [Button]
    public void ShowAction()
    {
        Debug.Log(GetAction().ToString());
    }
    /// <summary>
    /// ˯��Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person,params object[] objs)
    {
        return new AddContractA(person, obj);
    }
}
/// <summary>
/// ȡ����Լ
/// </summary>
public class RemoveContractA : Act
{
    public RemoveContractA(Person person, Obj obj) : base(person, obj)
    {
        wastTime = true;
    }
    public override IEnumerator<object> Run(System.Action<Act> callback)
    {
        TC();
        Person person = (Person)Person;
        Debug.Log("Arrange");
        List<CardInf> cards = new List<CardInf>();
        var contracModel = GameArchitect.get.GetModel<ContractModel>();
        //foreach(var card in contracModel.aContract.GetValueOrDefault(person))
        //{
        //    if(card.hasAccept==false)//ȡ����Լ
        //    {
        //        cards.Add(new CardInf(card.contractName, card.contractInfo, () => { contracModel.RemoveContract(card); }));
        //    }
        //}
        //yield return GameArchitect.gameLogic.AddDecision(person,
        //    new DecisionTex("ȡ��һ����Լ", "ѡ��һ�����ʵ�ģ�崴���",
        //    cards
        //    )
        //);
        var act = new EndAct(Person, Obj);
        yield return Ret(act, callback);
    }
}
/// <summary>
/// ȡ����Լ
/// </summary>
[Act]
public class RemoveContractAct : Activity
{
    public RemoveContractAct()
    {
        activityName = "ȡ��Э��";
        detail = "ȡ����δǩ���Э��";
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
    [Button]
    public void ShowAction()
    {
        Debug.Log(GetAction().ToString());
    }
    /// <summary>
    /// ˯��Ч��
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="person"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public override Act Effect(Obj obj, Person person, params object[] objs)
    {
        return new RemoveContractA(person, obj);
    }
}