using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjContBase
{
    public int size;//容量
    public int remain;//剩余
    public virtual void Add(int num,Obj obj=null)
    {
        size += num;
        remain += num;
    }
    public virtual void Remove(int num, Obj obj = null, int time = 0)
    {
        size += num;
        remain += num;
    }
    public ObjContBase(int size,int remain)
    {
        this.size = size;
        this.remain = remain;
    }
}

public class ObjSingle:ObjContBase, ICanRegisterEvent
{
    public List<Obj> objs;
	public override void Add(int num, Obj obj = null)
	{
		base.Add(num, obj);
        objs.Add(obj);
	}
	public override void Remove(int num, Obj obj = null, int time = 0)
	{
		base.Remove(num, obj);
        objs.Remove(obj);
	}
	public IArchitecture GetArchitecture()
    {
        return GameArchitect.get;
    }
    public ObjSingle(Obj obj,int size, int remain) : base(size,remain)
    {
        objs = new List<Obj>();
        objs.Add(obj);
    }

}
public class ObjTime<T> : ObjContBase, ICanRegisterEvent where T : PassTime, new()
{
    public T time;
    public Dictionary<int,int> objs;
    public Action<T> act;
    public override void Add(int num, Obj obj = null)
    {
        base.Add(num, obj);
        objs[time.NowTime()]+=num;
    }
    public override void Remove(int num, Obj obj = null,int time=0)
    {
        base.Remove(num, obj);
        objs[time]-=num;
    }
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.get;
    }
    public ObjTime(int x,int y) : base(x,y)
    {
        time = new T();
        objs = new Dictionary<int, int>();
        act = (e) =>
         {
             objs.Add(e.NowTime(),0);
         };
        this.RegisterEvent<T>(
            act
        );
    }
    ~ObjTime()
    {
        this.UnRegisterEvent<T>(act);
    }
}
/// <summary>
/// 资源数目
/// </summary>
public class ObjCont:ObjContBase
{
    public Obj obj;
    public ObjCont(Obj obj,int x,int y):base(x,y)
    {
        this.obj = obj;
        size = x;
        remain = y;
    }
}

/// <summary>
/// 一系列的资源
/// </summary>
public class Resource
{
    public Dictionary<ObjEnum, ObjContBase> resources;
    public Resource()
    {
        resources = new Dictionary<ObjEnum, ObjContBase>();
    }
    [Button]
    public void Add(ObjEnum objtype, int num,Obj obj=null)
    {
        if (!resources.ContainsKey(objtype))
        {
            var type = Map.Instance.GetSaver(objtype).saveTye;
            if (type == SaveTye.single)
            {
                resources.Add(objtype, new ObjSingle(obj, 0, 0));
            }
            else if (type == SaveTye.set)
            {
                resources.Add(objtype, new ObjCont(Map.Instance.GetObj(objtype), 0, 0));
            }
            else if (type==SaveTye.day)
            {
                resources.Add(objtype, new ObjTime<PassDay>(0, 0));
            }
            else if (type == SaveTye.month)
            {
                resources.Add(objtype, new ObjTime<PassMonth>(0, 0));
            }
            else if (type == SaveTye.year)
            {
                resources.Add(objtype, new ObjTime<PassYear>(0, 0));
            }
        }
        resources[objtype].Add(num, obj);
    }
    public void Remove(ObjEnum objType, int num,Obj obj=null,int time=0)
    {
        if (resources.ContainsKey(objType))
        {
            resources[objType].Remove(num,obj,time);
        }
        if (resources[objType].size==0)
        {
            resources.Remove(objType);
        }
    }
    public void Remove(Obj obj, int num)
    {
        if (resources.ContainsKey(obj.Enum()))
        {
            resources[obj.Enum()].remain -= num;
            resources[obj.Enum()].size -= num;
            if (resources[obj.Enum()].size == 0)
            {
                resources.Remove(obj.Enum());
            }
        }
    }
    public int Get(ObjEnum obj)
    {
        return resources[obj].remain;
    }
    public void Use(ObjEnum obj, int num)
    {
        resources[obj].remain -= num;
    }
    public void Release(ObjEnum obj, int num)
    {
        resources[obj].remain += num;
    }
    public int Get(Obj obj)
    {
        return resources[obj.Enum()].remain;
    }
    public void Use(Obj obj, int num)
    {
        resources[obj.Enum()].remain -= num;
    }
    public void Release(Obj obj, int num)
    {
        resources[obj.Enum()].remain += num;
    }
    public ObjContBase Find(ObjEnum objEnum)
    {
        return resources[objEnum];
    }
}

/// <summary>
/// 商品管理列表
/// </summary>
public class GoodsManager
{
    /// <summary>
    /// 一系列的商品
    /// </summary>
    public Dictionary<Goods,int> goods;
    public Resource originResource;
    public Resource resource;
    public Obj obj;
    public GoodsManager(Resource originresource,Obj obj)
    {
        this.obj = obj;
        this.originResource = originresource;
        this.resource = new Resource();
        goods = new Dictionary<Goods, int>();
    }
    public void SellEc(Goods goodsItem,int n)
    {
        goods[goodsItem] -= n;
        if (goods[goodsItem]==0)
            goods.Remove(goodsItem);
        resource.Remove(goodsItem.sellO,goodsItem.sellNum*n);
        originResource.Add(goodsItem.buyO, goodsItem.buyNum*n);
    }
    public void Add(ObjEnum sell,int sellNum,ObjEnum buy,int buyNum,int sum)
    {
        var x = new Goods(sell, sellNum, buy, buyNum);
        if (goods.ContainsKey(x))
        {
            goods[x] = sum;
        }
        goods[x] += sum;
        originResource.Remove(sell, sellNum * sum);
        resource.Add(sell,sellNum*sum);
    }
}

/// <summary>
/// 交易系统
/// </summary>
public class EcModel : AbstractModel
{
    protected override void OnInit()
    {
        
    }
    /// <summary>
    /// g1->g2
    /// </summary>
    public void Ec(Goods goods,int sum,GoodsManager g1,Resource g2)
    {
        g1.SellEc(goods,sum);
        g2.Add(goods.buyO, goods.buyNum*sum);
        g2.Remove(goods.sellO, goods.sellNum*sum);
    }
    public bool TryEc(Dictionary<Goods, int> resource, GoodsManager g1, Resource g2)
    {
        Resource resource1=new Resource();
        foreach (var x in resource)
        {
            resource1.Add(x.Key.buyO, x.Key.buyNum * x.Value);
        }
        foreach (var x in resource1.resources)
        {
            if ( !g2.resources.ContainsKey(x.Key)||g2.resources[x.Key].remain<x.Value.remain )
            {
                return false;
            }
        }
        foreach (var x in resource)
        {
            Ec(x.Key,x.Value,g1,g2);
        }
        return true;
    }
}
