using System.Collections;
using System.Collections.Generic;
using QFramework;
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
    public Person bp;
    public int signTime;
    public int aimTime;
    /// <summary>
    /// ѡ��Ҫ������Ϊ
    /// </summary>
    public CodeSystemData codeData;
    public bool hasSign;
    public Contract()
    {
        hasSign = false;
    }
    /// <summary>
    /// ǩ��Э��
    /// </summary>
    public virtual void Sign()
    {
        signTime = GameArchitect.get.GetModel<TimeModel>().GetTime();
        aimTime = GameArchitect.get.GetModel<TimeModel>().NextDay(30);
        hasSign = true;
    }
    /// <summary>
    /// Ч��
    /// </summary>
    /// <param name="str"></param>
    public abstract void Effect();
}
/// <summary>
/// ��Ӷ������Э��
/// </summary>
public class WorkContract : Contract
{
    public WorkContract():base()
    {
        cardInf = new CardInf("����","������");
    }
    /// <summary>
    /// Ŀ��ʱ��
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
    /// ��Լ��ģ��
    /// </summary>
    public List<Contract> contractTemplate;
    /// <summary>
    /// δǩ���ĺ�Լ
    /// </summary>
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
        contractTemplate.Add(new WorkContract());
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
        contract.hasSign = true;
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
