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
public class Rate
{
    Func<Obj, int> func;//获取数据
    Func<Obj,bool> can;
    public Resource objList = new Resource();//物品与数目
    public Rate(Func<Obj,int> func,Func<Obj,bool> can)
    {
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
    public void Remove(Obj obj,int num)
    {
        objList.Remove(obj,num);
    }
    public int Get(Obj obj)
    {
        return func(obj);
    }
}
public class CookItem
{
    /// <summary>
    /// 烹饪数目
    /// </summary>
    public int cookSum;
    /// <summary>
    /// 要烹饪的对象
    /// </summary>
    public FoodObj cookObj;
    /// <summary>
    /// 花费的时间
    /// </summary>
    public int cookRate;
}
public class CookItems
{
    public List<CookItem> cookItems;//要烹饪的食物
    public void Cook(int time)
    {
        if(cookItems.Count==0)
        {
        }
        else
        {
            cookItems[0].cookRate += time;
            if (cookItems[0].cookRate >= cookItems[0].cookObj.GetSaver().wastTime)
            {
                cookItems[0].cookSum--;
                if (cookItems[0].cookSum==0)
                {
                    cookItems.RemoveAt(0);
                }
            }
        }
    }
    public Resource resource;
    public CookItems(Resource resource)
    {
        this.resource = resource;
        cookItems = new List<CookItem>();
    }
    public CookItems()
    {
        cookItems = new List<CookItem>();
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
    /// <summary>
    /// 厨具的数目
    /// </summary>
    public Rate CookRate;
    /// <summary>
    /// 烹饪食材的列表
    /// </summary>
    public CookItems CookItems;
    /******************************烹饪的食物*************************************/
    /// <summary>
    /// 容纳的对象
    /// </summary>
    public Dictionary<Obj, int> objs;
    /// <summary>
    /// 用于交易的物品
    /// </summary>
    public GoodsManager goodsManager;
    public BuildingObj(BuildingSaver objSaver):base(objSaver)
    {
        objs=new Dictionary<Obj, int>();
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
        CookItems = new CookItems(resource);
        goodsManager = new GoodsManager();
    }
    public override void Init(ObjSaver objSaver)
    {
        base.Init(objSaver);
        BedSit = new Sit();
        SetSit = new Sit();
        resource = new Resource();
        CookItems = new CookItems(resource);
        goodsManager = new GoodsManager();
    }
    public override List<Activity> InitActivities()
    {
        return new List<Activity>() {
        new SleepAct(
            (obj, person, objs) => {return BedSit.useSit < BedSit.sit; },
            (obj, person,objs) =>
            {
                var selectTime = new SelectTime(person, obj, new int[] { 1 });
                return new SeqAct(person, obj,
                selectTime,
                new SleepA(person,obj,selectTime.selectTime));
            }
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
}
