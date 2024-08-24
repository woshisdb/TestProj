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
/// ��ṹ��ʱ��ͻ
/// </summary>
[System.Serializable]
public class CodeData
{
    public string codeName { get { return activity.activityName; } }
    /// <summary>
    /// ����
    /// </summary>
    public Obj obj;
    /// <summary>
    /// ��Ҫִ�еĻ
    /// </summary>
    public Activity activity;
    [SerializeField]
    /// <summary>
    /// ��Ҫ����ִ�еĻ
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
/// һ����ɫ��Э�����
/// </summary>
public class ContractManager
{
    public IPerson PersonObj;
    public ContractManager(IPerson PersonObj)
    {
        this.PersonObj = PersonObj;
    }
    /// <summary>
    /// ��ȡ��ǰ�Ĺ�����������
    /// </summary>
    /// <returns></returns>
    public CodeData GetWorkCode()
    {
        var t = GameArchitect.get.GetModel<TimeModel>();
        ///��ǰ��ʱ��
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
    //***************������****************
    public DomainGenerator domainGenerator;
    public ProblemGenerator problemGenerator;
    public PathGenerator pathGenerator;
    /// <summary>
    /// �ǽ�ɫ
    /// </summary>
    [Property]
    public bool isPlayer;
    /// <summary>
    /// ��ɫ������
    /// </summary>
    [Property]
    public string PersonObjName;
    /// <summary>
    /// �Լ���ӵ�е�������Դ
    /// </summary>
    [Property]
    public Resource resource;
    [Property]
    public int money { get { return resource.Get(ObjEnum.MoneyObjE); } set { resource.Add(ObjEnum.MoneyObjE,value); } }
    /// <summary>
    /// ʱ��滮
    /// </summary>
    [Property]
    public CodeData codeData;
    /// <summary>
    /// ���ʽ
    /// </summary>
    [Property]
    public LifeStyle lifeStyle;
    /// <summary>
    /// ��ǰ�Ļ
    /// </summary>
    public Act act;
    /// <summary>
    /// �Ƿ���ѡ��
    /// </summary>
    public bool hasSelect { get { return act != null; } }
    /// <summary>
    /// Э�������
    /// </summary>
    public ContractManager contractManager;
    /// <summary>
    /// һϵ�б�ǽ�ɫ���ʵı�ǩ
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
    /// ���ִ�еĻ
    /// </summary>
    /// <param name="act"></param>
    public void SetAct(Act act)
    {
        this.act = act;
        this.SendEvent<EndTurnEvent>(new EndTurnEvent(GameArchitect.nowPersonObj));
    }
    /// <summary>
    /// ���ٻ
    /// </summary>
    public void RemoveAct()
    {
        this.act = null;
    }
    /// <summary>
    /// ���е���Ϊ
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
///// ��Ⱥ
///// </summary>
//[Class]
//public class People:Obj
//{
//    /// <summary>
//    /// һϵ�в���Ҫ���������
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
    /// ��ȡ������Data
    /// </summary>
    /// <returns></returns>
    public CodeSystemData GetWorkData();
    public void SetWorkData(CodeSystemData code);
}

/// <summary>
/// һϵ�в���Ҫ�滮��NPC
/// </summary>
public class NPCObj : IPerson
{
    /// <summary>
    /// ���ʽ
    /// </summary>
    public LifeStyle lifeStyle;
    /// <summary>
    /// ��ǰ��ӵ�еĹٷ����ҵ���Ŀ
    /// </summary>
    public int money;
    /// <summary>
    /// ��ǰ��ӵ�е�ʳ������ά�ֵ�����
    /// </summary>
    public int foods;
    /// <summary>
    /// һϵ�б�ǽ�ɫ���ʵı�ǩ
    /// </summary>
    public List<Tag> tags = new List<Tag>();
    /// <summary>
    /// ��ǰ�Ĺ���
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