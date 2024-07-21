using System.Collections;
using System.Collections.Generic;
using QFramework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class PersonType : AnimalType
{
    public PersonType(string name = null) : base(name)
    {

    }
}



[P]
public class SleepValF:Func
{
    public SleepValF(ObjType objType):base(objType)
    {

    }
}
[P]
public class FoodValF : Func
{
    public FoodValF(ObjType objType) : base(objType)
    {

    }
}

[P]
public class HasSelectP : Predicate
{
    public HasSelectP(ObjType objType) : base(objType)
    {

    }
}
[P]
public class IsUseObjP : Predicate
{
    public IsUseObjP(AnimalType personType, ObjType objType) : base(personType, objType)
    {

    }
}
//[P]
//public class HasCloth : Predicate
//{
//    public HasCloth(PersonType personType, ObjType objType) : base(personType, objType)
//    {

//    }
//}
//[P]
//public class FinancialKnowledge:Func
//{
//    public FinancialKnowledge(PersonType personType):base(personType)
//    {

//    }
//}
//[P]
//public class ArtKnowledge : Func
//{
//    public ArtKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
//[P]
//public class LanguageKnowledge : Func
//{
//    public LanguageKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
//[P]
//public class MedicalKnowledge : Func
//{
//    public MedicalKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
//[P]
//public class ManagementKnowledge : Func
//{
//    public ManagementKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
//[P]
//public class AgriculturalKnowledge : Func
//{
//    public AgriculturalKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
//[P]
//public class IndustrialKnowledge : Func
//{
//    public IndustrialKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
//[P]
//public class GeneralHistoryKnowledge : Func
//{
//    public GeneralHistoryKnowledge(PersonType personType) : base(personType)
//    {

//    }
//}
public interface Need
{
    public Pop retSatifP();
    public float retSatif();
}
public class PersonState:Need
{
    public AnimalObj person;
    //public int l;
    public int r;
    public Num val;
    public PersonState(AnimalObj person,int r)
    {
        this.person = person;
        //this.l = l;
        this.r = r;
    }
    public PersonState(AnimalObj person, PersonSVal sv)
    {
        this.person = person;
        //this.l = sv.l;
        this.r = sv.r;
    }
    public Pop Add(Pop pop)
    {
        return PDDL.Min(PDDL.Max(PDDL.Add(val,pop),(I)0),(I)r);
    }
    public float retSatif()//���������
    {
        return ((float)r - (float)val.val) / (float)r;
    }
    public Pop retSatifP()
    {
        return PDDL.Div(PDDL.Dec((I)r,val), (I)r);
    }
}
[System.Serializable]
public class PersonSVal
{
    [SerializeField]
    public int r;
    [SerializeField]
    public int val;
}


[System.Serializable]
public class SafetyVals
{
    public int baseLive;
    public int enoughLive;
    public int richLive;
    public List<int> foodMoney;
}
public class FoodSumF:Func
{
    public FoodSumF(PersonType personType):base(personType)
    {
    }
}

/// <summary>
/// ��δ����ȫ���������,��ǰ�Ľ�Ǯ�ܹ��������ʳ��
/// </summary>
public class SafetyState:Need
{
    public Person person;
    public int baseLive;//�������������
    public int enoughLive;//����һ�����ڵ�����
    public int richLive;//����һ�����ϵ�����
    public CircularQueue<int> foodMoney;//һ�����������ѵļ۸�
    public Num foodSum;//�������ʳ��������

    public SafetyState(Person person,SafetyVals safetyVals)
    {
        this.person = person;
        baseLive = safetyVals.baseLive;
        enoughLive = safetyVals.enoughLive;
        richLive = safetyVals.richLive;
        foodMoney = new CircularQueue<int>(7);
        foodSum = new Num(new FoodSumF((PersonType)person.obj));
    }

    public Pop retSatifP()
    {
        var data=(int)CircularQueue<int>.CalculateAverage(foodMoney);
        var foodAll = person.foodState.r;//������
        var mySum = PDDL.Add(foodSum,person.foodState.val.func);
        mySum=PDDL.Add(mySum,PDDL.Mul((I)data,person.money));
        return PDDL.Div(mySum, (I)foodAll);//�ܹ�������
    }
    public float retSatif()
    {
        float data= (float)CircularQueue<int>.CalculateAverage(foodMoney);
        float foodAll = person.foodState.r;//������
        float mySum = foodSum.val + person.foodState.val.val;// PDDL.Add(foodSum, person.foodState.val.func);
        mySum = mySum + (data*(float)person.money.val);//PDDL.Add(mySum, PDDL.Mul((I)data, person.money));
        return mySum / foodAll;//PDDL.Div(mySum, (I)foodAll);//�ܹ�������
    }
}
[System.Serializable]
//����������
public class HungryVals
{
    [SerializeField]
    public int r;//��ʾ����ĳ��ֵ�����������
}
//����������
public class HungryState : Need
{
    Person person;
    public int r;//��ʾ����ĳ��ֵ�����������
    public HungryState(Person person,HungryVals hungryVals)
    {
        this.person = person;
        this.r = hungryVals.r;
    }

    public float retSatif()
    {
        return person.foodState.val.val / (float)r;
    }

    public Pop retSatifP()
    {
        return PDDL.Div(person.foodState.val , (I)r);
    }
}
[System.Serializable]
public class PersonSaver : AnimalSaver
{
    [SerializeField]
    public string PersonName;
    [SerializeField]
    public bool isPlayer;
    [SerializeField]
    public SafetyVals SafetyVals;
    [SerializeField]
    public HungryVals HungryVals;
}

public class ContractManager
{
    public Person person;
    public ContractManager(Person person)
    {
        this.person = person;
    }

    public bool Sign(Contract contract)
    {
        if(contract.ContractAllow(contract.ap,person))//����Э��
        {
            contract.bp=person;
            var contractModel=GameArchitect.get.GetModel<ContractModel>();
            contractModel.SignContract(contract,person);
            return true;
        }
        return false;
    }
    public bool CanSign(Contract contract)
    {
        if (contract.ContractAllow(contract.ap, person))//����Э��
        {
            return true;
        }
        return false;
    }
}

[Map(typeof(PersonType), "personSaver")]
public class Person : AnimalObj
{
    //***************************������********************************
    public DomainGenerator domainGenerator;
    public ProblemGenerator problemGenerator;
    public PathGenerator pathGenerator;
    //**************************************************************
    public PersonControler personControler;//��ɫ������
    public Num money;//�Լ����ϵĵ�Ǯ
    /*************************����˼��******************************/
    public SafetyState safetyState;//�԰�ȫ����Ҫ
    public HungryState hungryState;//�Լ���������
    /******************************************************/
    public bool isPlayer;
    public string PersonName;
    public Resource resource;
    ///*********************֪ʶ**************************/
    ///// <summary>
    ///// ����֪ʶ
    ///// </summary>
    //public PersonState financialKnowledge;
    ///// <summary>
    ///// ����֪ʶ
    ///// </summary>
    //public PersonState artKnowledge;
    ///// <summary>
    ///// ����֪ʶ
    ///// </summary>
    //public PersonState languageKnowledge;
    ///// <summary>
    ///// ҽ��֪ʶ
    ///// </summary>
    //public PersonState medicalKnowledge;
    ///// <summary>
    ///// ����֪ʶ
    ///// </summary>
    //public PersonState managementKnowledge;
    ///// <summary>
    ///// ũҵ֪ʶ
    ///// </summary>
    //public PersonState agriculturalKnowledge;
    ///// <summary>
    ///// ��ҵ֪ʶ
    ///// </summary>
    //public PersonState industrialKnowledge;
    ///// <summary>
    ///// ͨʷ֪ʶ
    ///// </summary>
    //public PersonState generalHistoryKnowledge;
    ///*************************����**********************/
    ///// <summary>
    ///// ����
    ///// </summary>
    //public PersonState strength;
    ///// <summary>
    ///// ����
    ///// </summary>
    //public PersonState dexterity;
    ///// <summary>
    ///// ����
    ///// </summary>
    //public PersonState constitution;
    ///// <summary>
    ///// ����
    ///// </summary>
    //public PersonState intelligence;
    ///// <summary>
    ///// ��֪
    ///// </summary>
    //public PersonState perception;
    ///// <summary>
    ///// ����
    ///// </summary>
    //public PersonState charisma;
    ///// <summary>
    ///// ��־
    ///// </summary>
    //public PersonState willpower;
    ///// <summary>
    ///// ����
    ///// </summary>
    //public PersonState luck;
    public Act act;
    public SetP<FoodObj> foodSet;
    public CodeSaver codeData;//������ִ�еļƻ�
    /// <summary>
    /// Э�������
    /// </summary>
    public ContractManager contractManager;
    public void Init()
    {
        PersonName = ((PersonSaver)objSaver).PersonName;
        isPlayer = ((PersonSaver)objSaver).isPlayer;
        if(cardInf==null)
        {
            cardInf = new CardInf();
        }
        cardInf.title = PersonName;
        cardInf.description = "This is a Test Case";
    }
    public Person(ObjSaver personAsset=null) : base(personAsset)
    {
        Init();
        resource = new Resource();
        var t = Resources.Load<GameObject>("Controler/Person");
        var g = GameObject.Instantiate<GameObject>(t);
        g.transform.parent = GameObject.Find("Persons").transform;
        g.GetComponent<PersonControler>().person = this;
        personControler = g.GetComponent<PersonControler>();
        if (isPlayer)
        {

        }   
        else
        {
            pathGenerator=new PathGenerator(this);
        }
        if (personAsset != null)
        {
            PersonSaver ps = (PersonSaver)personAsset;
            safetyState = new SafetyState(this, ps.SafetyVals);//��ȫ����
            hungryState = new HungryState(this, ps.HungryVals);//����������
        }
        //contractManager = new ContractManager(this);
        this.money = new Num(Money((PersonType)obj), 10);
    }
    /// <summary>
    /// ���ִ�еĻ
    /// </summary>
    /// <param name="act"></param>
    public void SetAct(Act act)
    {
        this.act = act;
        hasSelect.val = true;
        this.SendEvent<EndTurnEvent>(new EndTurnEvent(GameArchitect.nowPerson));
    }
    /// <summary>
    /// ���ٻ
    /// </summary>
    public void RemoveAct()
    {
        this.hasSelect.val = false;
        this.act = null;
    }
    public override List<Activity> InitActivities()
    {
        List<Activity> activities = new List<Activity>(base.InitActivities());
        activities.Add(new IdleAct());
        return activities;
    }
    public PersonSaver GetSaver()
    {
        return (PersonSaver)objSaver;
    }
    ///// <summary>
    ///// ��û
    ///// </summary>
    ///// <param name="obj"></param>
    ///// <param name="person"></param>
    ///// <returns></returns>
    //public virtual List<Activity> GetActivity(Obj obj)
    //{
    //    List<Activity> list = new List<Activity>();
    //    for (int i = 0; i < activities.Count; i++)
    //    {
    //        if (activities[i].Condition(obj, this))
    //        {
    //            list.Add(activities[i]);
    //        }
    //    }
    //    return list;
    //}
}
