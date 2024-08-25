using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
public class ObjContBaseType:PType
{

}

public class ObjContBase:IPDDL
{
    public PDDLClass pddl;
    public int size;//容量
    public int remain;//剩余
    public virtual void Add(int num,Obj obj=null,int time=0)
    {
        size += num;
        remain += num;
    }
    public virtual void Remove(int num, Obj obj = null, int time = 0)
    {
        size -= num;
        remain -= num;
    }
    public ObjContBase(int size,int remain)
    {
        this.size = size;
        this.remain = remain;
    }
    public virtual void Combine(ObjContBase objCont)
    {
        size += objCont.size;
        remain += objCont.remain;
    }
    public virtual void Delete(ObjContBase objContBase)
    {
        size -= objContBase.size;
        remain -= objContBase.remain;
    }

    public PType GetPtype()
    {
        return pddl.GetPType();
    }

    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType());
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }
}

public class ObjSingle:ObjContBase, ICanRegisterEvent
{
    public List<Obj> objs;
	public override void Add(int num, Obj obj = null,int time=0)
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
	public override void Combine(ObjContBase objCont)
	{
		base.Combine(objCont);
        var objc = (ObjSingle)objCont;
        objs=objs.Union(objc.objs).ToList();
    }
	public override void Delete(ObjContBase objContBase)
	{
		base.Delete(objContBase);
        var content = (ObjSingle)objContBase;
        objs = objs.Except(content.objs).ToList();
    }
}

public class Inf
{
    /// <summary>
    /// 所有的数目
    /// </summary>
    public int all;
    /// <summary>
    /// 剩余的数目
    /// </summary>
    public int remain;
    public int price;
}

public class ObjSet : ObjContBase
{
    public Dictionary<Obj, Inf> objDic;
    public ObjSet(int size, int remain) : base(size, remain)
    {
        objDic = new Dictionary<Obj, Inf>();
    }
    public override void Add(int num, Obj obj = null, int time = 0)
    {
        size += num;
        remain += num;
        //如果能保存
        if (objDic.ContainsKey(obj))
        {
            objDic[obj].remain+=num;
            objDic[obj].all += num;
        }
        else
        {
            objDic[obj] = new Inf();
            objDic[obj].remain += num;
            objDic[obj].all += num;
        }
    }
    public override void Remove(int num, Obj obj = null, int time = 0)
    {
        size -= num;
        remain -= num;
        objDic[obj].remain -= num;
        objDic[obj].all -= num;
        if (objDic[obj].all == 0)
        {
            objDic.Remove(obj);
        }
    }
    public override void Combine(ObjContBase objCont)
    {
        size+=objCont.size;
        remain+=objCont.remain;
        var t = (ObjSet)objCont;
        foreach (var x in t.objDic)
        {
            Add(x.Value.all,x.Key);
        }
    }
    public override void Delete(ObjContBase objCont)
    {
        size -= objCont.size;
        remain -= objCont.remain;
        var t = (ObjSet)objCont;
        foreach (var x in t.objDic)
        {
            Remove(x.Value.all, x.Key);
        }
    }
}

public class ResourceType:PType
{

}

/// <summary>
/// 一系列的资源
/// </summary>
[Class]
public class Resource : IPDDL
{
    public PDDLClass pddl;
    /// <summary>
    /// 容纳的物品
    /// </summary>
    [Property]
    public int maxSize;
    /// <summary>
    /// 当前使用的大小
    /// </summary>
    [Property]
    public int nowSize;
    [Property]
    public Dic<Enum<ObjEnum>, ObjContBase> resources;
    [Property]
    public Dic<Enum<TransationEnum>, Rate> rates;
    public Dic<Enum<SitEnum>, Sit> sites;
    public int GetSize(ObjEnum objEnum)
    {
        return Map.Instance.GetSaver(objEnum).size;
    }
    public Resource()
    {
        resources = new Dic<Enum<ObjEnum>, ObjContBase>();
        InitPDDLClass();
    }
    public void SetRate(Dic<Enum<TransationEnum>, Rate> rates)
    {
        this.rates = rates;
    }
    public void SetSites(Dic<Enum<SitEnum>, Sit> sites)
    {
        this.sites = sites;
    }
    public void Add(KeyValuePair<Enum<ObjEnum>, ObjContBase> pair)
    {
        if (rates != null)
        foreach (var x in rates)
        {
            x.Value.AddRate(pair.Key, pair.Value.size);
        }
        if (sites != null)
        foreach (var x in sites)
        {
            x.Value.AddSit(pair.Key,pair.Value.size);
        }
        resources[pair.Key].Combine(pair.Value);
        nowSize+=pair.Value.size*GetSize(pair.Key);
    }
    public void Remove(KeyValuePair<Enum<ObjEnum>, ObjContBase> pair)
    {
        //if (rates != null)
        //    foreach (var x in rates)
        //    {
        //        x.Value.RedRate(pair.Key, pair.Value.size);
        //    }
        if (sites != null)
            foreach (var x in sites)
            {
                x.Value.RemoveSit(pair.Key, pair.Value.size);
            }
        resources[pair.Key].Delete(pair.Value);
        nowSize -= pair.Value.size * GetSize(pair.Key);
    }
    public void Add(Resource resource)
    {
        foreach (var x in resource.resources)
        {
            Add(x);
        }
    }
    public void Remove(Resource resource)
    {
        foreach (var x in resource.resources)
        {
            Remove(x);
        }
    }
    [Button]
    public void Add(ObjEnum objtype, int num,Obj obj=null)
    {
        nowSize += num * GetSize(objtype);
        if (rates != null)
        foreach (var x in rates)
        {
            x.Value.AddRate(objtype, num);
        }
        if (sites != null)
        foreach (var x in sites)
        {
            x.Value.AddSit(objtype,num);
        }
        if (!resources.ContainsKey(objtype))
        {
            var type = Map.Instance.GetSaver(objtype).saveTye;
            if (type == SaveTye.single)
            {
                resources.Add(objtype, new ObjSingle(obj, 0, 0));
            }
            else if (type == SaveTye.set)
            {
                resources.Add(objtype, new ObjSet(0, 0));
            }
        }
        resources[objtype].Add(num, obj);
    }
    public void Remove(ObjEnum objType, int num,Obj obj=null)
    {
        nowSize -= num * GetSize(objType);
        if (sites != null)
            foreach (var x in sites)
            {
                x.Value.RemoveSit(objType,num);
            }
        if (resources.ContainsKey(objType))
        {
            resources[objType].Remove(num,obj);
        }
        if (resources[objType].size==0)
        {
            resources.Remove(objType);
        }
    }

    public int Get(ObjEnum obj)
    {
        if(resources.ContainsKey(obj))
            return resources[obj].remain;
        else
            return 0;
    }
    public int GetRemain(ObjEnum obj)
    {
        if (resources.ContainsKey(obj))
        {
            return resources[obj].remain;
        }
        else
        {
            return 0;
        }
    }
    public Dic<Enum<ObjEnum>, ObjContBase> GetObjs(ObjEnum objEnum)
    {
        var ret = new Dic<Enum<ObjEnum>, ObjContBase>();
        var s = Map.Instance.GetSaver(objEnum);
        foreach (var x in resources)
        {
            var data=Map.Instance.GetSaver(x.Key);
            if (data.GetType().IsSubclassOf(s.GetType())||s.GetType()==data.GetType())
            {
                ret.Add(x.Key,x.Value);
            }
        }
        return ret;
    }
    public void Use(ObjEnum obj, int num)
    {
        if (rates != null)
        foreach (var x in rates)
        {
                x.Value.AddRate(obj,num);
        }
        if (sites != null)
            foreach (var x in sites)
            {
                x.Value.UseSit(obj,num);
            }
        resources[obj].remain -= num;
    }
    public void Release(ObjEnum obj, int num)
    {
        if (rates != null)
        foreach (var x in rates)
        {
            x.Value.RedRate(obj);
        }
        if (sites != null)
            foreach (var x in sites)
            {
                x.Value.RemoveSit(obj,num);
            }
        resources[obj].remain += num;
    }
    public ObjContBase Find(ObjEnum objEnum)
    {
        return resources[objEnum];
    }
    public static void Trans(ObjEnum objEnum,int num,Resource r1,Resource r2)
    {
        r1.Remove(objEnum,num);
        r2.Add(objEnum,num);
    }

    public PType GetPtype()
    {
        return pddl.GetObj();
    }

    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType());
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }
    ~Resource()
    {
        PDDLClassGet.Remove(pddl);
    }
}

/// <summary>
/// 商品管理列表
/// </summary>
public class GoodsManager:IPDDL
{
    /// <summary>
    /// 一系列的(商品,价格)
    /// </summary>
    public Dictionary<ObjEnum, ObjSet> goods;
    /// <summary>
    /// 原先的资源
    /// </summary>
    public Resource originResource;
    public Obj obj;
    public GoodsManager(Resource originresource,Obj obj)
    {
        this.obj = obj;
        this.originResource = originresource;
        goods = new Dictionary<ObjEnum, ObjSet>();
    }
    public void Add(Obj sell,int sellNum,int price)
    {
        if(!goods.ContainsKey(sell.GetEnum()))
        {
            goods.Add(sell.GetEnum(),new ObjSet(0,0));
        }
        goods[sell.GetEnum()].Add(sellNum, sell);
        goods[sell.GetEnum()].objDic[sell].price = price;
    }
    public void Remove(Obj obj,int num)
    {
        goods[obj.GetEnum()].Remove(num, obj);
    }
	public PType GetPtype()
	{
		throw new NotImplementedException();
	}

	public void InitPDDLClass()
	{
		throw new NotImplementedException();
	}

	public PDDLClass GetPDDLClass()
	{
		throw new NotImplementedException();
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
    public void Ec(Obj obj,int num,GoodsManager g1,Resource g2)
    {
        g2.Add(obj.GetEnum(),num,obj);
        g1.Remove(obj, num);
    }
}
