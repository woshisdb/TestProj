using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public enum ContractTimeType
{
    None,//没有
    accurateDay,//具体一天
    EveryDay,//每一天
    Week,//星期
    WorkDay//工作日
}
public enum EndTimeType
{
    None,//没有
    Now,//具体一天
    EveryDay,//每天都可以结算
    Week,//星期
    WorkDay,//工作日
    NextMonth//下个月的当日
}
/// <summary>
/// 交易时间
/// </summary>
public class ContractTime
{
    public ContractTimeType type;
    public bool[] week = new bool[7];//一个星期
}
/// <summary>
/// 结算时间
/// </summary>
public class EndTime
{
    public EndTimeType type;
    public bool[] week = new bool[7];//一个星期
}
/// <summary>
/// 破坏规则受惩罚的类型
/// </summary>
public enum BreakEffect
{
    SocialPunish,//社会惩罚,例如我帮你，你帮我。违背受到社会惩罚
    LawPunish,//法律惩罚

}
/// <summary>
/// 人与人之间的合约模板
/// </summary>
public abstract class Contract
{
    /// <summary>
    /// 是否已经签署
    /// </summary>
    public bool hasAccept;
    /// <summary>
    /// 允许一个人签署多个相同的协议？
    /// </summary>
    public bool signOnce;
    public string contractName;
    public string contractInfo;
    /// <summary>
    /// 破坏规则受到的惩罚类型
    /// </summary>
    public BreakEffect breakEffect;
    /// <summary>
    /// Code的执行时间
    /// </summary>
    public ContractTime codeTime;
    /// <summary>
    /// 可选项，规定是否有活动
    /// </summary>
    public bool hasCode;
    /// <summary>
    /// 按照这个就可以实现目标
    /// </summary>
    public CodeSaver code;
    /****************************************************/
    /// <summary>
    /// 结算协议的时间
    /// </summary>
    public EndTime effectTime;
    /// <summary>
    /// 协议发起人
    /// </summary>
    public Person ap;
    /// <summary>
    /// 协议
    /// </summary>
    public Person bp;
    /// <summary>
    /// 允许签署协议
    /// </summary>
    public abstract bool ContractAllow(Person ap, Person bp);//甲方-》乙方
    /**************************************一系列的条件设置************************************/
    /// <summary>
    /// 协议是否生效
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public abstract bool EffectAllow(Person ap, Person bp);
    /// <summary>
    /// 协议能达成的效果
    /// </summary>
    /// <param name="person"></param>
    /// <param name=""></param>
    public abstract void EffectAccept(Person ap, Person bp);
    /// <summary>
    /// 协议无法达成的效果
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="objs"></param>
    public abstract void EffectReject(Person ap, Person bp);
    /// <summary>
    /// 协议生效的概率
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public abstract float EffectProb(Person ap, Person bp);
    public abstract Contract CopyContract();
    public Contract()
    {
        hasAccept = false;
        contractName = "";
        contractInfo = ""; 
    }
}
public class ContractModel : AbstractModel
{
    /// <summary>
    /// 合约的模板
    /// </summary>
    public List<Contract> contractTemplate;
    //未签订的合约
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
        contractTemplate.Add(new Work996Contract());
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
        contract.hasAccept = true;
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

public class Work996Contract : Contract
{
    /// <summary>
    /// 完成一个工作收获一个kpi
    /// </summary>
    public int kpi;
    public Work996Contract():base()
    {
        signOnce = true;
        contractName = "996工作";
        contractInfo = "哈哈，牛马";
        breakEffect = BreakEffect.LawPunish;//如果没有收到收入则法律惩罚
        codeTime = new ContractTime();
        codeTime.type = ContractTimeType.Week;
        codeTime.week[0] = true;
        codeTime.week[0] = true;
        codeTime.week[0] = true;
        codeTime.week[0] = true;
        codeTime.week[0] = true;
        codeTime.week[0] = true;
        codeTime.week[0] = false;
        hasCode = true;
        effectTime = new EndTime();
        effectTime.type = EndTimeType.NextMonth;
        kpi = 0;
    }
    /// <summary>
    /// 协议是否允许签署
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    /// <returns></returns>
    public override bool ContractAllow(Person ap, Person bp)
    {
        var cm=GameArchitect.get.GetModel<ContractModel>();
        return !cm.AlreadyHasContract(this,ap,bp);
    }

    public override Contract CopyContract()
    {
        return new Work996Contract();
    }
    /// <summary>
    /// 协议结算的效果
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    public override void EffectAccept(Person ap, Person bp)
    {
        GameArchitect.get.GetModel<EcModel>();//转账
    }
    /// <summary>
    /// 协议是否生效
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    /// <returns></returns>
    public override bool EffectAllow(Person ap, Person bp)
    {
        return true;
    }
    /// <summary>
    /// 能正常结算的概率
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    /// <returns></returns>
    public override float EffectProb(Person ap, Person bp)
    {
        return 1f;
    }
    /// <summary>
    /// 违约的效果
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    public override void EffectReject(Person ap, Person bp)
    {
        
    }
}
