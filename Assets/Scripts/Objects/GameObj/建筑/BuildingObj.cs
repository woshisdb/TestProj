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
[System.Serializable]
public class BuildingSaver : ObjSaver
{
    /// <summary>
    /// 容量大小
    /// </summary>
    public int container;
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
    Func<Obj, int> func;//获取数据
    Func<Obj,bool> can;
    public int count;//当前模块所能提供的数目
    public Resource objList = new Resource();//物品与数目
    public Rate(Func<Obj,int> func,Func<Obj,bool> can)
    {
        count = 0;
        this.func = func;
        this.can = can;
    }
    public void Add(Obj obj,int num)
    {
        if(can(obj)==true)
        {
            objList.Add(obj, num);
        }
    }
    public void Add(ObjEnum obj, int num)
    {
        if (can(obj) == true)
        {
            objList.Add(obj, num);
        }
    }
    public void Remove(Obj obj,int num)
    {
        objList.Remove(obj,num);
    }
    public int Get(Obj obj)
    {
        return func(obj);
    }
    public void Use(Obj obj, int num=1)
    {
        count += Get(obj)*num;
    }
    public void Release(Obj obj, int num = 1)
    {
        count -= Get(obj) * num;
    }
}

public class Source
{
    BuildingObj obj;
    public Trans trans;
    public List<int> nums;
    public Resource resource;
    /// <summary>
    /// 更新资源,输入成本
    /// </summary>
    public void Update(int count)
    {
        foreach (var data in trans.to.source)
        {
            resource.Add(data.x, data.y * nums[nums.Count - 1]);
        }
        for (int i = nums.Count - 1; i>=1; i--)
        {
            nums[i] = nums[i - 1];
        }
        nums[0] = count;
    }
    public Source(BuildingObj obj,Resource resource,Trans trans)
    {
        this.trans = trans;
        this.resource = resource;
        this.obj = obj;
        nums = new List<int>(trans.edge.time);
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


[Map()]
public class BuildingObj : Obj
{
    /// <summary>
    /// 存储的资源
    /// </summary>
    public Resource resource;
    /// <summary>
    /// 床的数目
    /// </summary>
    public Sit BedSit;
    /// <summary>
    /// 位子的数目
    /// </summary>
    public Sit SetSit;
    /******************************烹饪的食物*************************************/
    /// <summary>
    /// 厨具的数目
    /// </summary>
    public Rate CookRate;
    /*******************************************************************/
    /// <summary>
    /// 用于交易的物品
    /// </summary>
    public GoodsManager goodsManager;
    /// <summary>
    /// building的管线
    /// </summary>
    public PipLineManager pipLineManager;
    public BuildingObj(BuildingSaver objSaver):base(objSaver)
    {
        CookRate = new Rate(
            (obj) => { return obj.objSaver.canCook.count; },
            (obj) => { return obj.objSaver.canCook.can; }
        );
    }
    public override void Init()
    {
        base.Init();
        BedSit = new Sit();
        SetSit = new Sit();
        resource = new Resource();
        goodsManager = new GoodsManager();
        pipLineManager = new PipLineManager(this);
    }
    public override void Init(ObjSaver objSaver)
    {
        base.Init(objSaver);
        BedSit = new Sit();
        SetSit = new Sit();
        resource = new Resource();
        goodsManager = new GoodsManager();
        pipLineManager = new PipLineManager(this);
    }
    public override List<Activity> InitActivities()
    {
        return new List<Activity>() {
        new SleepAct(
            (obj, person, objs) => {return BedSit.useSit < BedSit.sit; }
        ),//睡眠活动
        new ArrangeContractAct(),//签署协议
        new AddContractAct(),//添加协议
        new RemoveContractAct(),//移除协议
        /***************************************/
        new SellAct(),//卖东西
        new BuyAct(),//买东西
        new CookAct()//烹饪东西
        };
    }
    public void Add(Obj s)
    {
        //可以睡眠
        if(s.objSaver.canSleep.can)
        {
            BedSit.sit += s.objSaver.canSleep.count;
        }
        //可以坐
        if (s.objSaver.canSet.can)
        {
            SetSit.sit += s.objSaver.canSet.count;
        }
        resource.Add(s,1);
    }
    public void Remove(Obj s)
    {
        if (s.objSaver.canSleep.can)
        {
            BedSit.sit -= s.objSaver.canSleep.count;
        }
        if (s.objSaver.canSet.can)
        {
            SetSit.sit -= s.objSaver.canSet.count;
        }
        resource.Remove(s, 1);
    }
    public BuildingSaver GetSaver()
    {
        return (BuildingSaver)objSaver;
    }
    /// <summary>
    /// 更新
    /// </summary>
	public override void Update()
	{
        foreach (var item in pipLineManager.piplineItem)//根据管线对数据进行处理
        {

        }
	}
}
