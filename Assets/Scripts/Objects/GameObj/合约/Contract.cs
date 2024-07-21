using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public enum ContractTimeType
{
    None,//û��
    accurateDay,//����һ��
    EveryDay,//ÿһ��
    Week,//����
    WorkDay//������
}
public enum EndTimeType
{
    None,//û��
    Now,//����һ��
    EveryDay,//ÿ�춼���Խ���
    Week,//����
    WorkDay,//������
    NextMonth//�¸��µĵ���
}
/// <summary>
/// ����ʱ��
/// </summary>
public class ContractTime
{
    public ContractTimeType type;
    public bool[] week = new bool[7];//һ������
}
/// <summary>
/// ����ʱ��
/// </summary>
public class EndTime
{
    public EndTimeType type;
    public bool[] week = new bool[7];//һ������
}
/// <summary>
/// �ƻ������ܳͷ�������
/// </summary>
public enum BreakEffect
{
    SocialPunish,//���ͷ�,�����Ұ��㣬����ҡ�Υ���ܵ����ͷ�
    LawPunish,//���ɳͷ�

}
/// <summary>
/// ������֮��ĺ�Լģ��
/// </summary>
public abstract class Contract
{
    /// <summary>
    /// �Ƿ��Ѿ�ǩ��
    /// </summary>
    public bool hasAccept;
    /// <summary>
    /// ����һ����ǩ������ͬ��Э�飿
    /// </summary>
    public bool signOnce;
    public string contractName;
    public string contractInfo;
    /// <summary>
    /// �ƻ������ܵ��ĳͷ�����
    /// </summary>
    public BreakEffect breakEffect;
    /// <summary>
    /// Code��ִ��ʱ��
    /// </summary>
    public ContractTime codeTime;
    /// <summary>
    /// ��ѡ��涨�Ƿ��л
    /// </summary>
    public bool hasCode;
    /// <summary>
    /// ��������Ϳ���ʵ��Ŀ��
    /// </summary>
    public CodeSaver code;
    /****************************************************/
    /// <summary>
    /// ����Э���ʱ��
    /// </summary>
    public EndTime effectTime;
    /// <summary>
    /// Э�鷢����
    /// </summary>
    public Person ap;
    /// <summary>
    /// Э��
    /// </summary>
    public Person bp;
    /// <summary>
    /// ����ǩ��Э��
    /// </summary>
    public abstract bool ContractAllow(Person ap, Person bp);//�׷�-���ҷ�
    /**************************************һϵ�е���������************************************/
    /// <summary>
    /// Э���Ƿ���Ч
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public abstract bool EffectAllow(Person ap, Person bp);
    /// <summary>
    /// Э���ܴ�ɵ�Ч��
    /// </summary>
    /// <param name="person"></param>
    /// <param name=""></param>
    public abstract void EffectAccept(Person ap, Person bp);
    /// <summary>
    /// Э���޷���ɵ�Ч��
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="objs"></param>
    public abstract void EffectReject(Person ap, Person bp);
    /// <summary>
    /// Э����Ч�ĸ���
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
    /// ��Լ��ģ��
    /// </summary>
    public List<Contract> contractTemplate;
    //δǩ���ĺ�Լ
    public List<Contract> unSignContract;
    /******************************��ǩ���Լ********************************/
    /// <summary>
    /// ��Ϊ�׷��ĺ�Լ
    /// </summary>
    public Dictionary<Person, List<Contract>> aContract;
    /// <summary>
    /// ��Ϊ�ҷ��ĺ�Լ
    /// </summary>
    public Dictionary<Person, List<Contract>> bContract;
    /***********************δǩ��ĺ�Լ******************************/
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
        //996�Ļ
        contractTemplate.Add(new Work996Contract());
    }
    public List<Contract> GetUnSignContract()
    {
        return unSignContract;
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
        unSignContract.Remove(contract);
        contract.hasAccept = true;
        contract.bp = person;
        bContract.GetValueOrDefault(person).Add(contract);
    }
    /// <summary>
    /// �׷������Լ,�ƶ�Э��
    /// </summary>
    public virtual void RegistContract(Contract contract)
    {
        unSignContract.Add(contract);
        aContract.GetValueOrDefault(contract.ap).Add(contract);
    }
    /// <summary>
    /// �׷�ȡ����Լ
    /// </summary>
    public virtual void RemoveContract(Contract contract)
    {
        aContract.GetValueOrDefault(contract.ap).Remove(contract);
    }
    /// <summary>
    /// ��Լ����
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
    /// ���һ�������ջ�һ��kpi
    /// </summary>
    public int kpi;
    public Work996Contract():base()
    {
        signOnce = true;
        contractName = "996����";
        contractInfo = "������ţ��";
        breakEffect = BreakEffect.LawPunish;//���û���յ��������ɳͷ�
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
    /// Э���Ƿ�����ǩ��
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
    /// Э������Ч��
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    public override void EffectAccept(Person ap, Person bp)
    {
        GameArchitect.get.GetModel<EcModel>();//ת��
    }
    /// <summary>
    /// Э���Ƿ���Ч
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    /// <returns></returns>
    public override bool EffectAllow(Person ap, Person bp)
    {
        return true;
    }
    /// <summary>
    /// ����������ĸ���
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    /// <returns></returns>
    public override float EffectProb(Person ap, Person bp)
    {
        return 1f;
    }
    /// <summary>
    /// ΥԼ��Ч��
    /// </summary>
    /// <param name="ap"></param>
    /// <param name="bp"></param>
    public override void EffectReject(Person ap, Person bp)
    {
        
    }
}
