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

/// <summary>
/// ������֮��ĺ�Լģ��
/// </summary>
public abstract class Contract
{
    //public CodeData codeData;
    public CardInf cardInf;
    /// <summary>
    /// Э��׷�
    /// </summary>
    public Person ap;
    /// <summary>
    /// Э���ҷ�
    /// </summary>
    public HashSet<Person> bp;
    public int count;
    public int beginTime;
    public int endTime;
    /// <summary>
    /// ѡ��Ҫ������Ϊ
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
    /// ǩ��Э��
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
    /// һϵ�еĶ���
    /// </summary>
    public Resource obj;
    /// <summary>
    /// ������Ʒ
    /// </summary>
    public Resource assur;
    public ContractEff()
    {
        obj = new Resource();
        assur = new Resource();
    }
}
/// <summary>
/// ��Ӷ������Э��
/// </summary>
public class WorkContract : Contract
{
    public WorkContract():base()
    {
        cardInf.title = "����";
        cardInf.description="������";
    }
    /// <summary>
    /// ִ����Ϊ�����֣�Э���ʱ�䣬ǩԼ��
    /// </summary>
    /// <param name="codeData"></param>
    /// <param name="contractTime"></param>
    /// <param name="ap"></param>
    public WorkContract(string codeData,Person ap):base()
    {
        cardInf = new CardInf("����", "������");
        var t=GameArchitect.get.tableAsset.codeDatas.Find(x => x.name == codeData);
        this.codeData = t;
        hasSign = false;
        this.ap = ap;
    }
    /// <summary>
    /// Ŀ��ʱ��
    /// </summary>
    public override void Sign()
    {
        base.Sign();
    }
    /// <summary>
    /// ѡ����ֹʱ��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Editor(Person person)
    {
        List<CardInf> time = new List<CardInf>();
        for(int i=0;i<10;i++)
        {
            int t = i;
            CardInf cardInf = new CardInf("��ʼʱ��",t+"�����ں�",
            () =>
            {
                beginTime=GameArchitect.get.GetModel<TimeModel>().GetBeginWeek()+t* TimeModel.timeStep*7;
            }
            );
            time.Add(cardInf);
        }
        yield return GameArchitect.gameLogic.AddDecision(person, new DecisionTex(
            "��ʼʱ��", "Э��Ŀ�ʼʱ��", time
        ));
        List<CardInf> time1 = new List<CardInf>();
        for (int i = 0; i < 12; i++)
        {
            int t = i;
            CardInf cardInf = new CardInf("ά��ʱ��", t + "����",
            () =>
            {
                endTime = beginTime+t*30 * TimeModel.timeStep;
            }
            );
            time1.Add(cardInf);
        }
        yield return GameArchitect.gameLogic.AddDecision(person, new DecisionTex(
            "ά��ʱ��", "Э��ĳ���ʱ��", time1
        ));
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
    public Dictionary<Person, List<Contract>> aContract;
    /// <summary>
    /// ��Ϊ�ҷ��ĺ�Լ
    /// </summary>
    [OdinSerialize]
    public Dictionary<Person, List<Contract>> bContract;
    [OdinSerialize]
    public HashSet<Contract> contracts;
    /***********************δǩ��ĺ�Լ******************************/
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
        //996�Ļ
        contractTemplate.Add(new WorkContract());
    }
    /// <summary>
    /// �Ƿ���Э��
    /// </summary>
    /// <returns></returns>
    public bool AlreadyHasContract(Contract contract,Person ap,Person bp)
    {
        var data=bContract.GetValueOrDefault(bp).Find(e => { return e.ap == ap && Tool.IsSameClass(e,contract); });
        return data != null;
    }

    /// <summary>
    /// �ҷ�ǩ����Լ
    /// </summary>
    /// <param name="contract"></param>
    public virtual void SignContract(Contract contract,Person person)
    {
        contract.hasSign = true;
        contract.bp.Add(person);
        bContract.GetValueOrDefault(person).Add(contract);

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
