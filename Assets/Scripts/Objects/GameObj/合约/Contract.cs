using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public enum ContractEnum
{
    canFinish,//可以被结算
    conBreak,//协议被打破
    unFinish,//未结束
}

/// <summary>
/// 人与人之间的合约模板
/// </summary>
public abstract class Contract
{
    //public CodeData codeData;
    public CardInf cardInf;
    /// <summary>
    /// 协议甲方
    /// </summary>
    public Person ap;
    /// <summary>
    /// 协议乙方
    /// </summary>
    public Person bp;
    public int signTime;
    public int aimTime;
    /// <summary>
    /// 选择要做的行为
    /// </summary>
    public CodeSystemData codeData;
    public bool hasSign;
    public Contract()
    {
        hasSign = false;
    }
    /// <summary>
    /// 签署协议
    /// </summary>
    public virtual void Sign()
    {
        signTime = GameArchitect.get.GetModel<TimeModel>().GetTime();
        aimTime = GameArchitect.get.GetModel<TimeModel>().NextDay(30);
        hasSign = true;
    }
    /// <summary>
    /// 效果
    /// </summary>
    /// <param name="str"></param>
    public abstract void Effect();
}
/// <summary>
/// 雇佣工作的协议
/// </summary>
public class WorkContract : Contract
{
    public WorkContract():base()
    {
        cardInf = new CardInf("工作","做工作");
    }
    /// <summary>
    /// 目标时间
    /// </summary>
    public override void Sign()
    {
        base.Sign();
    }

    public override void Effect()
    {
        return;
    }
}

public class ContractModel : AbstractModel
{
    /// <summary>
    /// 合约的模板
    /// </summary>
    public List<Contract> contractTemplate;
    /// <summary>
    /// 未签订的合约
    /// </summary>
    public List<Contract> unSignContract;
    /******************************已签署合约********************************/
    /// <summary>
    /// 作为甲方的合约
    /// </summary>
    public Dictionary<Person, List<Contract>> aContract;
    /// <summary>
    /// 作为乙方的合约
    /// </summary>
    public Dictionary<Person, List<Contract>> bContract;
    /***********************未签署的合约******************************/
    protected override void OnInit()
    {
        
    }
    public ContractModel()
    {
        aContract = new Dictionary<Person, List<Contract>>();
        bContract = new Dictionary<Person, List<Contract>>();
        unSignContract = new List<Contract>();
        foreach (var person in GameArchitect.persons)
        {
            if (!aContract.ContainsKey(person))
            {
                aContract.Add(person, new List<Contract>());
            }
        }
        foreach (var person in GameArchitect.persons)
        {
            if (!bContract.ContainsKey(person))
            {
                bContract.Add(person, new List<Contract>());
            }
        }
        contractTemplate = new List<Contract>();
        //996的活动
        contractTemplate.Add(new WorkContract());
    }
    public List<Contract> GetUnSignContract()
    {
        return unSignContract;
    }
    /// <summary>
    /// 是否有协议
    /// </summary>
    /// <returns></returns>
    public bool AlreadyHasContract(Contract contract,Person ap,Person bp)
    {
        var data=bContract.GetValueOrDefault(bp).Find(e => { return e.ap == ap && Tool.IsSameClass(e,contract); });
        return data != null;
    }

    /// <summary>
    /// 乙方签订合约
    /// </summary>
    /// <param name="contract"></param>
    public virtual void SignContract(Contract contract,Person person)
    {
        unSignContract.Remove(contract);
        contract.hasSign = true;
        contract.bp = person;
        bContract.GetValueOrDefault(person).Add(contract);
    }
    /// <summary>
    /// 甲方发起合约,制定协议
    /// </summary>
    public virtual void RegistContract(Contract contract)
    {
        unSignContract.Add(contract);
        aContract.GetValueOrDefault(contract.ap).Add(contract);
    }
    /// <summary>
    /// 甲方取消合约
    /// </summary>
    public virtual void RemoveContract(Contract contract)
    {
        aContract.GetValueOrDefault(contract.ap).Remove(contract);
    }
    /// <summary>
    /// 合约结束
    /// </summary>
    public virtual void FinishContract(Contract contract)
    {
        aContract.GetValueOrDefault(contract.ap).Remove(contract);
        bContract.GetValueOrDefault(contract.bp).Remove(contract);
    }
}
