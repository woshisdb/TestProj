using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType : ObjType
{
    public BuildingType(string name = null) : base(name)
    {

    }
}

public class Sit
{
    public int sit=0;
    public int useSit=0;
}
/// <summary>
/// 某一行为的工作量
/// </summary>
public class Rate
{
    public TransationEnum transType;
    Func<ObjSaver, int> func;//获取数据
    Func<ObjSaver,bool> can;
    public int count;//当前模块所能提供的数目
    public int nowCount;//当前模块数目
    public Resource objList = new Resource();//物品与数目
    public Rate(Func<ObjSaver,int> func,Func<ObjSaver,bool> can)
    {
        count = 0;
        this.func = func;
        this.can = can;
    }
    public void Add(Obj obj,int num)
    {
        if(can(obj.objSaver)==true)
        {
            objList.Add(obj, num);
        }
    }
    public void Add(ObjEnum obj, int num)
    {
        if (can(Map.Instance.GetSaver(obj)) == true)
        {
            objList.Add(obj, num);
        }
    }
    public void Remove(Obj obj,int num)
    {
        objList.Remove(obj,num);
    }
    public int Get(ObjSaver obj)
    {
        return func(obj);
    }
    public void Use(Obj obj, int num=1)
    {
        count += Get(obj.objSaver)*num;
    }
    public void Use(ObjSaver obj, int num = 1)
    {
        count += Get(obj) * num;
    }
    public void Release(Obj obj, int num = 1)
    {
        count -= Get(obj.objSaver) * num;
    }
    public void Release(ObjSaver obj, int num = 1)
    {
        count -= Get(obj) * num;
    }
}

public class Source
{
    public BuildingObj obj;
    public Trans trans;
    public List<int> nums;
    public Resource resource;
    public int maxnCount=99999;
    /// <summary>
    /// 更新资源,输入成本
    /// </summary>
    public void Update()
    {
        int count = maxnCount;
        foreach (var t in trans.edge.tras)//转移时间
        {
            count=Math.Min(obj.rates[t.x].nowCount / t.y,count);
        }
        Debug.Log(">>"+count);
        foreach (var t in trans.edge.tras)
        {
            obj.rates[t.x].nowCount-=count*t.y;
        }
        foreach (var data in trans.to.source)
        {
            resource.Add(data.x, data.y * nums[nums.Count - 1]);
        }
        for (int i = nums.Count - 1; i>=1; i--)
        {
            nums[i] = nums[i - 1];
        }
        foreach (var data in trans.from.source)
        {
            resource.Remove(data.x,data.y*count);
        }
        nums[0] = count;
    }
    public Source(BuildingObj obj,Resource resource,Trans trans)
    {
        this.trans = trans;
        this.resource = resource;
        this.obj = obj;
        maxnCount = 99999;
        nums = new List<int>();
        for(int i=0;i< trans.edge.time; i++)
        {
            nums.Add(0);
        }
    }
}

/// <summary>
/// 管线管理器
/// </summary>
public class PipLineManager
{
    public BuildingObj obj;
    /// <summary>
    /// 管线的条目
    /// </summary>
    public Dictionary<Trans,Source> piplineItem;
    public void SetTrans(List<Trans> trans)
    {
        piplineItem.Clear();
        foreach (var x in trans)
        {
            piplineItem.Add(x, new Source(obj,obj.resource,x));
        }
    }
    public PipLineManager(BuildingObj obj)
    {
        this.obj = obj;
        piplineItem = new Dictionary<Trans, Source>();
    }
}
[System.Serializable]
public class BuildingSaver : ObjSaver
{
    /// <summary>
    /// 容量大小
    /// </summary>
    public int container;
}

[Map()]
public class BuildingObj : Obj
{
    /// <summary>
    /// 存储的资源
    /// </summary>
    public Resource resource;
    /******************************烹饪的食物*************************************/
    /// <summary>
    /// 可选的对象
    /// </summary>
    public Dictionary<TransationEnum, Rate> rates;
    /// <summary>
    /// 不可选，固定值
    /// </summary>
    public Dictionary<SitEnum, Sit> sits;
    /*******************************************************************/
    /// <summary>
    /// 用于交易的物品
    /// </summary>
    public GoodsManager goodsManager;
    /// <summary>
    /// building的管线
    /// </summary>
    public PipLineManager pipLineManager;
    public BuildingObj(BuildingSaver objSaver=null):base(objSaver)
    {
        if(rates==null)
            rates = new Dictionary<TransationEnum, Rate>();
        /*******************添加Rate*******************/
        rates.Add(TransationEnum.cook , new Rate(
            (obj) => { return objSaver.canCook.count; },
            (obj) => { return objSaver.canCook.can; }
        ));
        rates.Add(TransationEnum.qieGe, new Rate(
            (obj) => { return objSaver.qieGe.count; },
            (obj) => { return objSaver.qieGe.can; }
        ));
        rates.Add(TransationEnum.gengZhong, new Rate(
            (obj) => { return objSaver.gengZhong.count; },
            (obj) => { return objSaver.gengZhong.can; }
        ));
        rates.Add(TransationEnum.zaiZhong, new Rate(
            (obj) => { return objSaver.zaiZhong.count; },
            (obj) => { return objSaver.zaiZhong.can; }
        ));
        rates.Add(TransationEnum.shouHuo, new Rate(
            (obj) => { return objSaver.shouHuo.count; },
            (obj) => { return objSaver.shouHuo.can; }
        ));
        resource = new Resource();
        goodsManager = new GoodsManager(resource, this);
        pipLineManager = new PipLineManager(this);
        sits = new Dictionary<SitEnum, Sit>();
        sits.Add(SitEnum.bed, new Sit());
        sits.Add(SitEnum.set, new Sit());

    }
    public override void Init()
    {
        base.Init();
    }
    public override void Init(ObjSaver objSaver)
    {
        base.Init(objSaver);
    }
    public override List<Activity> InitActivities()
    {
        return new List<Activity>() {
        new SleepAct(
            (obj, person, objs) => {return sits[SitEnum.bed].useSit < sits[SitEnum.bed].sit; }
        ),//睡眠活动
        new ArrangeContractAct(),//签署协议
        new AddContractAct(),//添加协议
        new RemoveContractAct(),//移除协议
        /***************************************/
        new SellAct(),
        new BuyAct(),
        new CookAct(),
        new SetPipLineAct()
        };
    }
    public void Add(Obj s)
    {
        sits[SitEnum.bed].sit += s.objSaver.sleep;
        sits[SitEnum.set].sit += s.objSaver.set;
        resource.Add(s,1);
    }
    public void Remove(Obj s)
    {
        sits[SitEnum.bed].sit -= s.objSaver.sleep;
        sits[SitEnum.set].sit -= s.objSaver.set;
        resource.Remove(s, 1);
    }
    public BuildingSaver GetSaver()
    {
        return (BuildingSaver)objSaver;
    }
    /// <summary>
    /// 更新...需要修改
    /// </summary>
	public override void LatUpdate()
	{
        Debug.Log(">>?"+pipLineManager.piplineItem.Count);
        if (pipLineManager == null)
        {
            pipLineManager = new PipLineManager(this);
        }
        foreach(var x in rates)
        {
            x.Value.nowCount = x.Value.count;
        }
        foreach (var item in pipLineManager.piplineItem)//根据管线对数据进行处理
        {
            item.Value.Update();
        }
	}
}
