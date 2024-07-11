using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
/// <summary>
/// ��Դ��Ŀ
/// </summary>
public class ObjCont
{
    public Obj obj;
    public int size;
    public int remain;
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
/// һϵ�е���Դ
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
