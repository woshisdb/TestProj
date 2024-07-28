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
    public HashSet<Person> bp;
    public int count;
    public int beginTime;
    public int endTime;
    /// <summary>
    /// 选择要做的行为
    /// </summary>
    public CodeSystemData codeData;
    public bool hasSign;
    public ContractEff contractEff;
    public Contract()
    {
        hasSign = false;
        contractEff = new ContractEff();
        cardInf = new CardInf("","");
        bp = new HashSet<Person>();
    }
    /// <summary>
    /// 签署协议
    /// </summary>
    public virtual void Sign()
    {
        beginTime = GameArchitect.get.GetModel<TimeModel>().GetTime();
        hasSign = true;
    }
    public virtual bool CanSign(Person person)
    {
        return !bp.Contains(person);
    }
    public List<CodeSystemData> GetDats()
    {
        var t = GameArchitect.get.tableAsset.codeDatas;
        return t.FindAll(e => {return e.name.StartsWith(cardInf.title); });
    }
    public virtual IEnumerator Editor(Person person)
    {
        yield break;
    }
}
public class ContractEff
{
    /// <summary>
    /// 一系列的对象
    /// </summary>
    public Resource obj;
    /// <summary>
    /// 担保物品
    /// </summary>
    public Resource assur;
    public ContractEff()
    {
        obj = new Resource();
        assur = new Resource();
    }
}
/// <summary>
/// 雇佣工作的协议
/// </summary>
public class WorkContract : Contract
{
    public WorkContract():base()
    {
        cardInf.title = "工作";
        cardInf.description="做工作";
    }
    /// <summary>
    /// 执行行为的名字，协议的时间，签约人
    /// </summary>
    /// <param name="codeData"></param>
    /// <param name="contractTime"></param>
    /// <param name="ap"></param>
    public WorkContract(string codeData,Person ap):base()
    {
        cardInf = new CardInf("工作", "做工作");
        var t=GameArchitect.get.tableAsset.codeDatas.Find(x => x.name == codeData);
        this.codeData = t;
        hasSign = false;
        this.ap = ap;
    }
    /// <summary>
    /// 目标时间
    /// </summary>
    public override void Sign()
    {
        base.Sign();
    }
    /// <summary>
    /// 选择起止时间
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Editor(Person person)
    {
        List<CardInf> time = new List<CardInf>();
        for(int i=0;i<10;i++)
        {
            int t = i;
            CardInf cardInf = new CardInf("开始时间",t+"个星期后",
            () =>
            {
                beginTime=GameArchitect.get.GetModel<TimeModel>().GetBeginWeek()+t* TimeModel.timeStep*7;
            }
            );
            time.Add(cardInf);
        }
        yield return GameArchitect.gameLogic.AddDecision(person, new DecisionTex(
            "开始时间", "协议的开始时间", time
        ));
        List<CardInf> time1 = new List<CardInf>();
        for (int i = 0; i < 12; i++)
        {
            int t = i;
            CardInf cardInf = new CardInf("维持时间", t + "个月",
            () =>
            {
                endTime = beginTime+t*30 * TimeModel.timeStep;
            }
            );
            time1.Add(cardInf);
        }
        yield return GameArchitect.gameLogic.AddDecision(person, new DecisionTex(
            "维持时间", "协议的持续时间", time1
        ));
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
    public Dictionary<Person, List<Contract>> aContract;
    /// <summary>
    /// 作为乙方的合约
    /// </summary>
    [OdinSerialize]
    public Dictionary<Person, List<Contract>> bContract;
    [OdinSerialize]
    public HashSet<Contract> contracts;
    /***********************未签署的合约******************************/
    protected override void OnInit()
    {
        
    }
    public ContractModel()
    {
        aContract = new Dictionary<Person, List<Contract>>();
        bContract = new Dictionary<Person, List<Contract>>();
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
        contracts = new HashSet<Contract>();
        contractTemplate = new List<Contract>();
        //996的活动
        contractTemplate.Add(new WorkContract());
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
        contract.hasSign = true;
        contract.bp.Add(person);
        bContract.GetValueOrDefault(person).Add(contract);

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
        foreach(var person in contract.bp)
        {
            bContract[person].RemoveAll(x => { return x == contract; });
        }
    }
    public List<Contract> GetUnSignContract(Person person)
    {
        return aContract[person].FindAll(x =>
        {
            return x.ap == person&&x.bp.Count<x.count;
        });
    }
}
