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
        BuildingObj buildingObj=(BuildingObj)Obj;
        List<Contract> contracts = GameArchitect.get.GetModel<ContractModel>().GetUnSignContract(buildingObj.owner);
        var arrange = new List<CardInf>();
        foreach(Contract contract in contracts)
        {
            var it = contract;
            if (it.CanSign(Person))//�ܹ�ǩ��
                arrange.Add(new CardInf(it.cardInf.title, it.cardInf.description, () =>
                    {
                        GameArchitect.get.GetModel<ContractModel>().SignContract(it,Person);
                    }
                )
                );
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
    /// ˯��Ч��
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
        List<CardInf> sels = new List<CardInf>();
        var contracModel = GameArchitect.get.GetModel<ContractModel>();
        //****************************************ѡ��Э������
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
            "����һ����Լ", "ѡ��һ�����ʵ�ģ�崴���",sels
        ));
        nowContract=Tool.DeepClone(nowContract);
        //*******************************���Ŷ��ٸ�Э��
        int num = 0;
        List <SelectInf> sx=new List<SelectInf>();
        sx.Add(new SelectInf("��Ŀ", "ѡ��Э����Ŀ", nowContract, 1000));
        yield return GameArchitect.gameLogic.AddDecision(person, new SelectTex("ѡ����Ŀ", "ѡ��Э����Ŀ",
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
            "ѡ��滮", "ѡ����ʵĻ�滮", types
        ));
        nowContract.codeData = Tool.DeepClone(code);
        nowContract.ap = person;
        yield return nowContract.Editor(nowContract.ap);
        //*********************************ѡ������ľ���ϸ��

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
            yield return AddDecision(person,new DecisionTex("ѡ�񽻻�����","",objs));
            yield return nowContract.codeData.EditCodeSystem(person, intObj, x);
        }


        GameArchitect.get.GetModel<ContractModel>().RegistContract(nowContract);

        ////////////////////////////////////////
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
    /// ˯��Ч��
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
    /// ˯��Ч��
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