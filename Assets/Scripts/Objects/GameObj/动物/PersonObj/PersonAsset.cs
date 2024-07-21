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
    public float retSatif()//返回满意度
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
/// 对未来安全的满足情况,当前的金钱能够购买多少食物
/// </summary>
public class SafetyState:Need
{
    public Person person;
    public int baseLive;//满足明天的生存
    public int enoughLive;//满足一个星期的生存
    public int richLive;//满足一月以上的生存
    public CircularQueue<int> foodMoney;//一点能量所花费的价格
    public Num foodSum;//所购买的食物总能量

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
        var foodAll = person.foodState.r;//总能量
        var mySum = PDDL.Add(foodSum,person.foodState.val.func);
        mySum=PDDL.Add(mySum,PDDL.Mul((I)data,person.money));
        return PDDL.Div(mySum, (I)foodAll);//能够满足多久
    }
    public float retSatif()
    {
        float data= (float)CircularQueue<int>.CalculateAverage(foodMoney);
        float foodAll = person.foodState.r;//总能量
        float mySum = foodSum.val + person.foodState.val.val;// PDDL.Add(foodSum, person.foodState.val.func);
        mySum = mySum + (data*(float)person.money.val);//PDDL.Add(mySum, PDDL.Mul((I)data, person.money));
        return mySum / foodAll;//PDDL.Div(mySum, (I)foodAll);//能够满足多久
    }
}
[System.Serializable]
//饥饿的需求
public class HungryVals
{
    [SerializeField]
    public int r;//表示低于某个值会产生饥饿感
}
//饥饿的需求
public class HungryState : Need
{
    Person person;
    public int r;//表示低于某个值会产生饥饿感
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
        if(contract.ContractAllow(contract.ap,person))//允许协议
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
        if (contract.ContractAllow(contract.ap, person))//允许协议
        {
            return true;
        }
        return false;
    }
}

[Map(typeof(PersonType), "personSaver")]
public class Person : AnimalObj
{
    //***************************生成器********************************
    public DomainGenerator domainGenerator;
    public ProblemGenerator problemGenerator;
    public PathGenerator pathGenerator;
    //**************************************************************
    public PersonControler personControler;//角色控制器
    public Num money;//自己手上的的钱
    /*************************个人思想******************************/
    public SafetyState safetyState;//对安全的需要
    public HungryState hungryState;//对饥饿的需求
    /******************************************************/
    public bool isPlayer;
    public string PersonName;
    public Resource resource;
    ///*********************知识**************************/
    ///// <summary>
    ///// 金融知识
    ///// </summary>
    //public PersonState financialKnowledge;
    ///// <summary>
    ///// 美术知识
    ///// </summary>
    //public PersonState artKnowledge;
    ///// <summary>
    ///// 语言知识
    ///// </summary>
    //public PersonState languageKnowledge;
    ///// <summary>
    ///// 医疗知识
    ///// </summary>
    //public PersonState medicalKnowledge;
    ///// <summary>
    ///// 管理知识
    ///// </summary>
    //public PersonState managementKnowledge;
    ///// <summary>
    ///// 农业知识
    ///// </summary>
    //public PersonState agriculturalKnowledge;
    ///// <summary>
    ///// 工业知识
    ///// </summary>
    //public PersonState industrialKnowledge;
    ///// <summary>
    ///// 通史知识
    ///// </summary>
    //public PersonState generalHistoryKnowledge;
    ///*************************特性**********************/
    ///// <summary>
    ///// 力量
    ///// </summary>
    //public PersonState strength;
    ///// <summary>
    ///// 敏捷
    ///// </summary>
    //public PersonState dexterity;
    ///// <summary>
    ///// 体质
    ///// </summary>
    //public PersonState constitution;
    ///// <summary>
    ///// 智力
    ///// </summary>
    //public PersonState intelligence;
    ///// <summary>
    ///// 感知
    ///// </summary>
    //public PersonState perception;
    ///// <summary>
    ///// 魅力
    ///// </summary>
    //public PersonState charisma;
    ///// <summary>
    ///// 意志
    ///// </summary>
    //public PersonState willpower;
    ///// <summary>
    ///// 运气
    ///// </summary>
    //public PersonState luck;
    public Act act;
    public SetP<FoodObj> foodSet;
    public CodeSaver codeData;//有正在执行的计划
    /// <summary>
    /// 协议管理器
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
            safetyState = new SafetyState(this, ps.SafetyVals);//安全需求
            hungryState = new HungryState(this, ps.HungryVals);//饥饿的需求
        }
        //contractManager = new ContractManager(this);
        this.money = new Num(Money((PersonType)obj), 10);
    }
    /// <summary>
    /// 添加执行的活动
    /// </summary>
    /// <param name="act"></param>
    public void SetAct(Act act)
    {
        this.act = act;
        hasSelect.val = true;
        this.SendEvent<EndTurnEvent>(new EndTurnEvent(GameArchitect.nowPerson));
    }
    /// <summary>
    /// 减少活动
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
    ///// 获得活动
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
