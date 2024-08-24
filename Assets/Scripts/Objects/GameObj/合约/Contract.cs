using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using Sirenix.Serialization;
using UnityEngine;

public enum ContractEnum
{
    canFinish,//���Ա�����
    conBreak,//Э�鱻����
    unFinish,//δ����
}

public class ContractType
{
    public ContractType():base()
    {
    }
}

/// <summary>
/// ������֮��ĺ�Լģ��
/// </summary>
[Class(true)]
public abstract class Contract: IPDDL
{
    public PDDLClass pddl;
    //public CodeData codeData;
    public CardInf cardInf;
    /// <summary>
    /// Э��ļ׷�
    /// </summary>
    public IPerson ap;
    /// <summary>
    /// Э����ҷ�
    /// </summary>
    public HashSet<IPerson> bp;
    /// <summary>
    /// �������˶���Э��
    /// </summary>
    public int count;
    public Contract()
    {
        cardInf = new CardInf("","");
        bp = new HashSet<IPerson>();
    }
    public abstract CodeSystemData GetCodeData();
    /// <summary>
    /// ǩ��ʱ�Ĵ���
    /// </summary>
    /// <param name="person"></param>
    public abstract void RegisterContract(IPerson person);
    /// <summary>
    /// ��Լ�Ĵ���
    /// </summary>
    /// <param name="person"></param>
    public abstract void UnRegisterContract(IPerson person);

    public virtual void Init(IPerson ap,int num)
    {
        this.ap = ap;
        count = num;
    }
    /// <summary>
    /// ǩ��Э��
    /// </summary>
    public virtual void Sign(IPerson person)
    {
        bp.Add(person);
        RegisterContract(person);
        person.SetWorkData(GetCodeData());
    }
    /// <summary>
    /// �Ƿ����ǩ��Э��
    /// </summary>
    /// <param name="PersonObj"></param>
    /// <returns></returns>
    public virtual bool CanSign(IPerson PersonObj)
    {
        return !bp.Contains(PersonObj);
    }
    /// <summary>
    /// ��ȡ����
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
    /// ��Լ��ģ��
    /// </summary>
    public List<Contract> contractTemplate;
    /******************************��ǩ���Լ********************************/
    /// <summary>
    /// ��Ϊ�׷��ĺ�Լ
    /// </summary>
    [OdinSerialize]
    public Dictionary<IPerson, List<Contract>> aContract;
    /// <summary>
    /// ��Ϊ�ҷ��ĺ�Լ
    /// </summary>
    [OdinSerialize]
    public Dictionary<IPerson, List<Contract>> bContract;
    [OdinSerialize]
    public HashSet<Contract> contracts;
    /***********************δǩ��ĺ�Լ******************************/
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
        //��ͨ����
        contractTemplate.Add(new WorkContract());
    }
    /// <summary>
    /// �Ƿ���Э��
    /// </summary>
    /// <returns></returns>
    public bool AlreadyHasContract(Contract contract,PersonObj ap,PersonObj bp)
    {
        var data=bContract.GetValueOrDefault(bp).Find(e => { return e.ap == ap && Tool.IsSameClass(e,contract); });
        return data != null;
    }

    /// <summary>
    /// �ҷ�ǩ����Լ
    /// </summary>
    /// <param name="contract"></param>
    public virtual void SignContract(Contract contract,PersonObj PersonObj)
    {
        contract.bp.Add(PersonObj);
        bContract.GetValueOrDefault(PersonObj).Add(contract);

    }
    /// <summary>
    /// �׷������Լ,�ƶ�Э��
    /// </summary>
    public virtual void RegistContract(Contract contract)
    {
        contracts.Add(contract);
        aContract.GetValueOrDefault(contract.ap).Add(contract);
    }
    /// <summary>
    /// �׷�ȡ����Լ
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
