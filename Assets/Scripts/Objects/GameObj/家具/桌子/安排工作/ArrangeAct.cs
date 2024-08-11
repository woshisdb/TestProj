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
        BuildingObj buildingObj=(BuildingObj)Obj;
        List<Contract> contracts = GameArchitect.get.GetModel<ContractModel>().GetUnSignContract(buildingObj.owner);
        var arrange = new List<CardInf>();
        foreach(Contract contract in contracts)
        {
            var it = contract;
            if (it.CanSign(Person))//能够签字
                arrange.Add(new CardInf(it.cardInf.title, it.cardInf.description, () =>
                    {
                        GameArchitect.get.GetModel<ContractModel>().SignContract(it,Person);
                    }
                )
                );
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
        var o = (BuildingObj)obj;
        return o.owner != null && GameArchitect.get.GetModel<ContractModel>().GetUnSignContract(o.owner).Count > 0;//((BuildingObj)obj).remainBuilder == 0;
    }
    
    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
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
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
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
        List<CardInf> sels = new List<CardInf>();
        var contracModel = GameArchitect.get.GetModel<ContractModel>();
        //****************************************选择协议类型
        Contract nowContract=null;
        foreach(var contract in contracModel.contractTemplate)
        {
            var it =contract;
            var key = new CardInf(contract.cardInf.title,contract.cardInf.description,
            () =>
            {
                nowContract = it;
            }
            );
            sels.Add(key);
        }
        yield return GameArchitect.gameLogic.AddDecision(person,new DecisionTex(
            "创建一个合约", "选择一个合适的模板创造吧",sels
        ));
        nowContract=Tool.DeepClone(nowContract);
        //*******************************发放多少个协议
        int num = 0;
        List <SelectInf> sx=new List<SelectInf>();
        sx.Add(new SelectInf("数目", "选择协议数目", nowContract, 1000));
        yield return GameArchitect.gameLogic.AddDecision(person, new SelectTex("选择数目", "选择协议数目",
            sx, () => { num = sx[0].num; return true; }
        ));
        nowContract.count = num;
        //*******************************
        

        List<CardInf> types = new List<CardInf>();
        CodeSystemData code=null;
        foreach(var x in nowContract.GetDats())
        {
            types.Add(new CardInf(x.name,"",
                () => { code = x; }
            ));
        }
        yield return GameArchitect.gameLogic.AddDecision(person, new DecisionTex(
            "选择规划", "选择合适的活动规划", types
        ));
        nowContract.codeData = Tool.DeepClone(code);
        nowContract.ap = person;
        yield return nowContract.Editor(nowContract.ap);
        //*********************************选择人物的具体细节

        foreach(var x in nowContract.codeData.work)
        {
            List<CardInf> objs = new List<CardInf>();
            Obj intObj = null;
            foreach(var intAct in GameArchitect.get.tableAsset.tableSaver.objs)
            {
                var data = intAct;
                objs.Add( new CardInf(data.name,data.cardInf.description,
                    () =>
                    {
                        intObj = data;
                    }
                ));
            }
            yield return AddDecision(person,new DecisionTex("选择交互对象","",objs));
            yield return nowContract.codeData.EditCodeSystem(person, intObj, x);
        }


        GameArchitect.get.GetModel<ContractModel>().RegistContract(nowContract);

        ////////////////////////////////////////
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
        return true;// ((BuildingObj)obj).remainBuilder == 0;
    }

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
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
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new AddContractA(person, obj), obj, person,winDatas,objs); ;
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

    //public override PAction GetAction()
    //{
    //    PAction action = new PAction();
    //    return action;
    //}
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
    public override Act Effect(Obj obj, Person person, List<WinData> winDatas = null, params object[] objs)
    {
        return GetActs( new RemoveContractA(person, obj), obj, person,winDatas,objs); ;
    }
}