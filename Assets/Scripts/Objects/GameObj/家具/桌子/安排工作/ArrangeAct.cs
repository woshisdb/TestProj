using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 签署合约
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
            //if(Person.contractManager.CanSign(it))//能够签字
            //arrange.Add(new CardInf(it.contractName, it.contractInfo, () => 
            //    {
            //        Person.contractManager.Sign(it);//签名
            //    }
            //)
            //);
        }
        yield return GameArchitect.gameLogic.AddDecision(Person, new DecisionTex("选择合约", "选择一个合适的的合约开始活动", 
            arrange
        ));
        var act = new EndAct(Person, Obj);
        yield return Ret(act, callback);
    }
}
/// <summary>
/// 签署合约的活动
/// </summary>
[Act]
public class ArrangeContractAct : Activity
{
    public ArrangeContractAct()
    {
        activityName = "签署协议";
        detail = "签署当前存在的协议";
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
    /// 睡觉效果
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
/// 添加合约
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
        //创建一个合约
        //if (sels.Count > 0)
        //yield return GameArchitect.gameLogic.AddDecision(person,
        //    new SelectTex("创建一个合约", "选择一个合适的模板创造吧",
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
/// 添加合约
/// </summary>
[Act]
public class AddContractAct : Activity
{
    public AddContractAct()
    {
        activityName = "添加协议";
        detail = "添加一系列协议";
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
    /// 睡觉效果
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
/// 取消合约
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
        //    if(card.hasAccept==false)//取消合约
        //    {
        //        cards.Add(new CardInf(card.contractName, card.contractInfo, () => { contracModel.RemoveContract(card); }));
        //    }
        //}
        //yield return GameArchitect.gameLogic.AddDecision(person,
        //    new DecisionTex("取消一个合约", "选择一个合适的模板创造吧",
        //    cards
        //    )
        //);
        var act = new EndAct(Person, Obj);
        yield return Ret(act, callback);
    }
}
/// <summary>
/// 取消合约
/// </summary>
[Act]
public class RemoveContractAct : Activity
{
    public RemoveContractAct()
    {
        activityName = "取消协议";
        detail = "取消还未签署的协议";
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
    /// 睡觉效果
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