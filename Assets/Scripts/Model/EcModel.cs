using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
/// <summary>
/// һϵ�е���Դ
/// </summary>
public class Resource
{
    public Dictionary<Obj,int> resources;
    /// <summary>
    /// ��ȡʳ��
    /// </summary>
    public Resource GetFoods()
    {
        Resource foods = new Resource();
        foreach(var obj in resources)
        {
            if (obj.Key is FoodObj)//�����ʳ������
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
/// ��Ʒ�б�
/// </summary>
public class GoodsManager
{
    // ʵ���Զ���IEqualityComparer<Person>
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
    public Dictionary<Goods,int> items;//��Ʒ����Ʒ���۸�,��Ŀ
    public GoodsManager()
    {
        items = new Dictionary<Goods,int>(new GoodsEqualityComparer());
    }
    /// <summary>
    /// �����Ʒ
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
/// ����ϵͳ
/// </summary>
public class EcModel : AbstractModel
{
    protected override void OnInit()
    {
        
    }
    /// <summary>
    /// ת�˵���Ϊ���루Num��Money����a->b
    /// </summary>
    public void MoneyTransfer(Num a,Num b,int money)
    {
        a.val-=money;
        b.val+=money;
    }
    /// <summary>
    /// ��Ʒ
    /// </summary>
    /// <param name="goods"></param>
    public void Buy(List<Goods> goods)
    {

    }
}
