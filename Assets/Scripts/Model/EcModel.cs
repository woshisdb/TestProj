using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
/// <summary>
/// 资源数目
/// </summary>
public class ObjCont
{
    public Obj obj;
    public int size;//容量
    public int remain;//剩余
    public ObjCont()
    {
        
    }
    public ObjCont(Obj obj,int x,int y)
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
    public Dictionary<ObjEnum, ObjCont> resources;
    public Resource()
    {
        resources = new Dictionary<ObjEnum,ObjCont>();
    }

    public void Add(ObjEnum obj, int num)
    {
        if (!resources.ContainsKey(obj))
            resources.Add(obj, new ObjCont(Map.Instance.GetObj(obj), num, num));
        else
        {
            resources[obj].remain += num;
            resources[obj].size += num;
        }
    }
    public void Remove(ObjEnum obj, int num)
    {
        if (resources.ContainsKey(obj))
        {
            resources[obj].remain -= num;
            resources[obj].size -= num;
            if (resources[obj].size == 0)
            {
                resources.Remove(obj);
            }
        }
    }

    public void Add(Obj obj, int num)
    {
        if (!resources.ContainsKey(obj.Enum()))
            resources.Add(obj.Enum(), new ObjCont(obj,num,num));
        else
        {
            resources[obj.Enum()].remain += num;
            resources[obj.Enum()].size += num;
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
    public ObjCont Find(ObjEnum objEnum)
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
