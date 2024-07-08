using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
/// <summary>
/// 一系列的资源
/// </summary>
public class Resource
{
    public Dictionary<Obj,int> resources;
    /// <summary>
    /// 获取食物
    /// </summary>
    public Resource GetFoods()
    {
        Resource foods = new Resource();
        foreach(var obj in resources)
        {
            if (obj.Key is FoodObj)//如果是食物类型
            {
                foods.Add(obj.Key,obj.Value);
            }
        }
        return foods;
    }

    public void Add(Obj obj, int num)
    {
        if (!resources.ContainsKey(obj))
            resources.Add(obj, num);
        else
        {
            resources[obj] += num;
        }
    }
    public void Remove(Obj obj, int num)
    {
        if (resources.ContainsKey(obj))
        {
            resources[obj] -= num;
            if (resources[obj] == 0)
            {
                resources.Remove(obj);
            }
        }
    }
    public int Get(Obj obj)
    {
        return resources[obj];
    }
}

/// <summary>
/// 商品列表
/// </summary>
public class GoodsManager
{
    // 实现自定义IEqualityComparer<Person>
    public class GoodsEqualityComparer : IEqualityComparer<Goods>
    {
        public bool Equals(Goods x, Goods y)
        {
            return x.obj.objSaver == y.obj.objSaver && x.cost == y.cost&&x.num==y.num;
        }

        public int GetHashCode(Goods goods)
        {
            return goods.obj.GetHashCode();
        }
    }
    public Dictionary<Goods,int> items;//商品（物品，价格）,数目
    public GoodsManager()
    {
        items = new Dictionary<Goods,int>(new GoodsEqualityComparer());
    }
    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="goods"></param>
    /// <param name="sum"></param>
    public void AddGoods(Goods goods,int sum)
    {
        if(!items.ContainsKey(goods))
        items.Add(goods, 0);
        else items[goods]+=sum;
    }
    public void RemoveGoods(Goods goods,int sum)
    {
        items[goods] -= sum;
        items.Remove(goods);
    }
}

/// <summary>
/// 金融系统
/// </summary>
public class EcModel : AbstractModel
{
    protected override void OnInit()
    {
        
    }
    /// <summary>
    /// 转账的行为传入（Num（Money））a->b
    /// </summary>
    public void MoneyTransfer(Num a,Num b,int money)
    {
        a.val-=money;
        b.val+=money;
    }
    /// <summary>
    /// 商品
    /// </summary>
    /// <param name="goods"></param>
    public void Buy(List<Goods> goods)
    {

    }
}
