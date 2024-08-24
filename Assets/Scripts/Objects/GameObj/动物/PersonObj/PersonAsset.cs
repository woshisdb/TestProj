using System.Collections;
using System.Collections.Generic;
using QFramework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


[System.Serializable]
public class PersonSaver : AnimalSaver
{
    [SerializeField]
    public string PersonObjName;
    [SerializeField]
    public bool isPlayer;
}
/// <summary>
/// 活动结构，时间和活动
/// </summary>
[System.Serializable]
public class CodeData
{
    public string codeName { get { return activity.activityName; } }
    /// <summary>
    /// 对象
    /// </summary>
    public Obj obj;
    /// <summary>
    /// 需要执行的活动
    /// </summary>
    public Activity activity;
    [SerializeField]
    /// <summary>
    /// 需要连续执行的活动
    /// </summary>
    public List<WinData> wins;
    public CodeData()
    {
        wins = new List<WinData>();
    }
    public CodeData(CodeData codeData)
    {
        obj = codeData.obj;
        activity = codeData.activity;
        wins = new List<WinData>();
        wins.AddRange(codeData.wins);
    }
    public static IEnumerable Objs()
    {
        var ret = new ValueDropdownList<Obj>();
        foreach (var x in GameArchitect.get.tableAsset.tableSaver.objs)
        {
            ret.Add(x.name, x);
        }
        return ret;
    }
}
/// <summary>
/// 一个角色的协议管理
/// </summary>
public class ContractManager
{
    public IPerson PersonObj;
    public ContractManager(IPerson PersonObj)
    {
        this.PersonObj = PersonObj;
    }
    /// <summary>
    /// 获取当前的工作所做的事
    /// </summary>
    /// <returns></returns>
    public CodeData GetWorkCode()
    {
        var t = GameArchitect.get.GetModel<TimeModel>();
        ///当前的时间
        return PersonObj.GetWorkData().GetNowWork(t.GetTime());
        //var contracts = GameArchitect.get.GetModel<ContractModel>().bContract[PersonObj];
        //if (contracts == null)
        //{
        //    contracts = new List<Contract>();
        //}
        //for(var x=0;x<contracts.Count;x++)
        //{
        //    var code = contracts[x].GetNowWork();
        //    if(code!=null)
        //    return code;
        //}
        //return null;
    }
    public CodeData GetWorkCode(int time)
    {
        return PersonObj.GetWorkData().GetNowWork(time);
    }
}
public class PersonType:AnimalType
{
}

[Map,Class]
public class PersonObj : AnimalObj,IPerson
{
    //***************生成器****************
    public DomainGenerator domainGenerator;
    public ProblemGenerator problemGenerator;
    public PathGenerator pathGenerator;
    /// <summary>
    /// 是角色
    /// </summary>
    [Property]
    public bool isPlayer;
    /// <summary>
    /// 角色的名字
    /// </summary>
    [Property]
    public string PersonObjName;
    /// <summary>
    /// 自己所拥有的所有资源
    /// </summary>
    [Property]
    public Resource resource;
    [Property]
    public int money { get { return resource.Get(ObjEnum.MoneyObjE); } set { resource.Add(ObjEnum.MoneyObjE,value); } }
    /// <summary>
    /// 时间规划
    /// </summary>
    [Property]
    public CodeData codeData;
    /// <summary>
    /// 生活方式
    /// </summary>
    [Property]
    public LifeStyle lifeStyle;
    /// <summary>
    /// 当前的活动
    /// </summary>
    public Act act;
    /// <summary>
    /// 是否有选择活动
    /// </summary>
    public bool hasSelect { get { return act != null; } }
    /// <summary>
    /// 协议管理器
    /// </summary>
    public ContractManager contractManager;
    /// <summary>
    /// 一系列标记角色性质的标签
    /// </summary>
    public List<Tag> tags = new List<Tag>();

    public void Init()
    {
        domainGenerator = new DomainGenerator(this);
        problemGenerator=new ProblemGenerator(this);
        pathGenerator = new PathGenerator(this);
        PersonObjName = ((PersonSaver)objSaver).PersonObjName;
        isPlayer = ((PersonSaver)objSaver).isPlayer;
        if(cardInf==null)
        {
            cardInf = new CardInf();
        }
        cardInf.title = PersonObjName;
        cardInf.description = "";
    }
    public PersonObj(ObjSaver PersonObjAsset=null) : base(PersonObjAsset)
    {
        Init();
        resource = new Resource();
        var t = Resources.Load<GameObject>("Controler/PersonObj");
        var g = GameObject.Instantiate<GameObject>(t);
        g.transform.parent = GameObject.Find("PersonObjs").transform;
        g.GetComponent<PersonObjControler>().PersonObj = this;
        if (isPlayer)
        {

        }   
        else
        {
            pathGenerator=new PathGenerator(this);
        }
        if (PersonObjAsset != null)
        {
            PersonSaver ps = (PersonSaver)PersonObjAsset;
        }
        contractManager=new ContractManager(this);
    }
    /// <summary>
    /// 添加执行的活动
    /// </summary>
    /// <param name="act"></param>
    public void SetAct(Act act)
    {
        this.act = act;
        this.SendEvent<EndTurnEvent>(new EndTurnEvent(GameArchitect.nowPersonObj));
    }
    /// <summary>
    /// 减少活动
    /// </summary>
    public void RemoveAct()
    {
        this.act = null;
    }
    /// <summary>
    /// 所有的行为
    /// </summary>
    /// <returns></returns>
    public override List<Activity> InitActivities()
    {
        List<Activity> activities = new List<Activity>(base.InitActivities());
        activities.Add(new IdleAct());
        activities.Add(new FangZhiAct());
        activities.Add(new BanYunAct());
        return activities;
    }
    public PersonSaver GetSaver()
    {
        return (PersonSaver)objSaver;
    }
    public override void LatUpdate()
    {
        base.LatUpdate();
        if(str==null)
        str=new System.Text.StringBuilder();
        str.Clear();
        cardInf.title = PersonObjName;
        if (cardInf.cardControl)
            cardInf.cardControl.UpdateInf();
        cardInf.description = str.ToString();
    }

    public WorkContract GetWork()
    {
        return lifeStyle.work;
    }

    public bool HasWork()
    {
        return lifeStyle.work!=null;
    }

    public List<Tag> GetTags()
    {
        return tags;
    }

    public int GetMoney()
    {
        return money;
    }

    public CodeSystemData GetWorkData()
    {
        return lifeStyle.code;
    }

    public void SetWorkData(CodeSystemData code)
    {
        lifeStyle.code = code;
    }
}

///// <summary>
///// 人群
///// </summary>
//[Class]
//public class People:Obj
//{
//    /// <summary>
//    /// 一系列不需要计算的物体
//    /// </summary>
//    public List<NPCObj> npcs = new List<NPCObj>();
//    public Hash<PersonObj> people = new Hash<PersonObj>();
//}

public interface IPerson
{
    public WorkContract GetWork();
    public bool HasWork();
    public int GetMoney();
    public List<Tag> GetTags();
    /// <summary>
    /// 获取工作的Data
    /// </summary>
    /// <returns></returns>
    public CodeSystemData GetWorkData();
    public void SetWorkData(CodeSystemData code);
}

/// <summary>
/// 一系列不需要规划的NPC
/// </summary>
public class NPCObj : IPerson
{
    /// <summary>
    /// 生活方式
    /// </summary>
    public LifeStyle lifeStyle;
    /// <summary>
    /// 当前所拥有的官方货币的数目
    /// </summary>
    public int money;
    /// <summary>
    /// 当前所拥有的食物所能维持的天数
    /// </summary>
    public int foods;
    /// <summary>
    /// 一系列标记角色性质的标签
    /// </summary>
    public List<Tag> tags = new List<Tag>();
    /// <summary>
    /// 当前的工作
    /// </summary>
    /// <returns></returns>
    public WorkContract GetWork()
    {
        return lifeStyle.work;
    }
    public CodeSystemData GetWorkData()
    {
        return lifeStyle.code;
    }
    public bool HasWork()
    {
        return lifeStyle.work!=null;
    }
    public List<Tag> GetTags()
    {
        return tags;
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetWorkData(CodeSystemData code)
    {
        throw new System.NotImplementedException();
    }
}

public enum TagLable
{
    
}

public class Tag
{
    public Enum<TagLable> tag;
    public int inf;
}