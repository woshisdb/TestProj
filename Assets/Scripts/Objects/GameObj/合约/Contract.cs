using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using Sirenix.Serialization;
using UnityEngine;

public enum ContractEnum
{
    canFinish,//可以被结算
    conBreak,//协议被打破
    unFinish,//未结束
}

public class ContractType
{
    public ContractType():base()
    {
    }
}

/// <summary>
/// 人与人之间的合约模板
/// </summary>
[Class(true)]
public abstract class Contract: IPDDL
{
    public PDDLClass pddl;
    //public CodeData codeData;
    public CardInf cardInf;
    /// <summary>
    /// 协议的甲方
    /// </summary>
    public IPerson ap;
    /// <summary>
    /// 协议的乙方
    /// </summary>
    public HashSet<IPerson> bp;
    /// <summary>
    /// 共发布了多少协议
    /// </summary>
    public int count;
    public Contract()
    {
        cardInf = new CardInf("","");
        bp = new HashSet<IPerson>();
    }
    public abstract CodeSystemData GetCodeData();
    /// <summary>
    /// 签署时的处理
    /// </summary>
    /// <param name="person"></param>
    public abstract void RegisterContract(IPerson person);
    /// <summary>
    /// 毁约的处理
    /// </summary>
    /// <param name="person"></param>
    public abstract void UnRegisterContract(IPerson person);

    public virtual void Init(IPerson ap,int num)
    {
        this.ap = ap;
        count = num;
    }
    /// <summary>
    /// 签署协议
    /// </summary>
    public virtual void Sign(IPerson person)
    {
        bp.Add(person);
        RegisterContract(person);
        person.SetWorkData(GetCodeData());
    }
    /// <summary>
    /// 是否可以签署协议
    /// </summary>
    /// <param name="PersonObj"></param>
    /// <returns></returns>
    public virtual bool CanSign(IPerson PersonObj)
    {
        return !bp.Contains(PersonObj);
    }
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public List<CodeSystemData> GetDats()
    {
        var t = GameArchitect.get.tableAsset.codeDatas;
        return t.FindAll(e => {return e.name.StartsWith(cardInf.title); });
    }
	public PType GetPtype()
	{
        return pddl.GetObj();
	}

	public void InitPDDLClass()
	{
		throw new NotImplementedException();
	}

	public PDDLClass GetPDDLClass()
	{
		throw new NotImplementedException();
	}
}
public class ContractModel : AbstractModel
{
    [OdinSerialize]
    /// <summary>
    /// 合约的模板
    /// </summary>
    public List<Contract> contractTemplate;
    /******************************已签署合约********************************/
    /// <summary>
    /// 作为甲方的合约
    /// </summary>
    [OdinSerialize]
    public Dictionary<IPerson, List<Contract>> aContract;
    /// <summary>
    /// 作为乙方的合约
    /// </summary>
    [OdinSerialize]
    public Dictionary<IPerson, List<Contract>> bContract;
    [OdinSerialize]
    public HashSet<Contract> contracts;
    /***********************未签署的合约******************************/
    protected override void OnInit()
    {
        
    }
    public ContractModel()
    {
        aContract = new Dictionary<IPerson, List<Contract>>();
        bContract = new Dictionary<IPerson, List<Contract>>();
        foreach (var PersonObj in GameArchitect.PersonObjs)
        {
            if (!aContract.ContainsKey(PersonObj))
            {
                aContract.Add(PersonObj, new List<Contract>());
            }
        }
        foreach (var PersonObj in GameArchitect.PersonObjs)
        {
            if (!bContract.ContainsKey(PersonObj))
            {
                bContract.Add(PersonObj, new List<Contract>());
            }
        }
        contracts = new HashSet<Contract>();
        contractTemplate = new List<Contract>();
        //普通工作
        contractTemplate.Add(new WorkContract());
    }
    /// <summary>
    /// 是否有协议
    /// </summary>
    /// <returns></returns>
    public bool AlreadyHasContract(Contract contract,PersonObj ap,PersonObj bp)
    {
        var data=bContract.GetValueOrDefault(bp).Find(e => { return e.ap == ap && Tool.IsSameClass(e,contract); });
        return data != null;
    }

    /// <summary>
    /// 乙方签订合约
    /// </summary>
    /// <param name="contract"></param>
    public virtual void SignContract(Contract contract,PersonObj PersonObj)
    {
        contract.bp.Add(PersonObj);
        bContract.GetValueOrDefault(PersonObj).Add(contract);

    }
    /// <summary>
    /// 甲方发起合约,制定协议
    /// </summary>
    public virtual void RegistContract(Contract contract)
    {
        contracts.Add(contract);
        aContract.GetValueOrDefault(contract.ap).Add(contract);
    }
    /// <summary>
    /// 甲方取消合约
    /// </summary>
    public virtual void RemoveContract(Contract contract)
    {
        aContract.GetValueOrDefault(contract.ap).Remove(contract);
        contracts.Remove(contract);
        foreach(var PersonObj in contract.bp)
        {
            bContract[PersonObj].RemoveAll(x => { return x == contract; });
        }
    }
    public List<Contract> GetUnSignContract(PersonObj PersonObj)
    {
        return aContract[PersonObj].FindAll(x =>
        {
            return x.ap == PersonObj&&x.bp.Count<x.count;
        });
    }
}
