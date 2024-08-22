using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SitType : PType
{

}

public class Sit:IPDDL
{
    public int sit=0;
    public int remainSit=0;
    public Func<ObjSaver, int> sum;
    public Sit(Func<ObjSaver,int> func)
    {
        this.sum = func;
    }
    public Sit()
    {
        
    }
    public void AddSit(ObjEnum objEnum,int num=1)
    {
        var t = sum(Map.Instance.GetSaver(objEnum));
        sit -= num * t;
        remainSit -= num * t;
    }
    public void RemoveSit(ObjEnum objEnum, int num=1)
    {
        var t = sum( Map.Instance.GetSaver(objEnum));
        sit -= num*t;
        remainSit -= num*t;
    }
    public void UseSit(ObjEnum objEnum, int num=1)
    {
        var t = sum(Map.Instance.GetSaver(objEnum));
        remainSit -= num*t;
    }
    public void RelsSit(ObjEnum objEnum, int num = 1)
    {
        var t = sum(Map.Instance.GetSaver(objEnum));
        remainSit += num*t;
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

public class RateType : PType
{

}
public class Rate_PDDL : PDDLClass<Rate, RateType>
{
    
}

/// <summary>
/// ĳһ��Ϊ�Ĺ�����
/// </summary>
public class Rate: IPDDL
{
    public TransationEnum transType;
    public Func<ObjSaver, int> func;//��ȡ����
    public Func<ObjSaver,bool> can;
    public Resource resource;//��Ʒ����Ŀ
    public int nowCount;//��ǰ�����ṩ������
    public int tempCount;
    public Rate(Func<ObjSaver,int> func,Func<ObjSaver,bool> can,Resource resource)
    {
        this.func = func;
        this.can = can;
        this.resource = resource;
        nowCount = 0;
    }
    public void AddRate(Obj obj,int num=1)
    {
        if(can(obj.objSaver)==true)
        {
            nowCount += func(obj.objSaver)*num;
        }
    }
    public void AddRate(ObjEnum obj,int num=1)
    {
        if (can(Map.Instance.GetSaver(obj)) == true)
        {
            nowCount += func(Map.Instance.GetSaver(obj))*num;
        }
    }
    public void RedRate(Obj obj,int num=1)
    {
        if (can(obj.objSaver) == true)
        {
            nowCount -= func(obj.objSaver)*num;
        }
    }
    public void RedRate(ObjEnum obj,int num=1)
    {
        if (can(Map.Instance.GetSaver(obj)) == true)
        {
            nowCount -= func(Map.Instance.GetSaver(obj))*num;
        }
    }
    public int Get(ObjSaver obj)
    {
        return func(obj);
    }
    public Dic<Enum<ObjEnum>,ObjContBase> ObjList()
    {
        var s=resource.resources.Keys.Where(kv => can(Map.Instance.GetSaver(kv))).ToList();
        return resource.resources.Where(kv => s.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
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
//��Դһ�����ṩ
public class Source:IPDDL
{
    public BuildingObj obj;
    public Trans trans;
    public LinkedList<int> nums;
    public Resource resource;
    public int maxnCount=99999;
    /// <summary>
    /// ������Դ,����ɱ�
    /// </summary>
    public virtual void Update()
    {
            //int maxC = 9999999;
            //foreach (var data in trans.from.source)
            //{
            //    maxC = Math.Min(maxC, resource.GetRemain(data.x) / data.y);
            //}
            int count = maxnCount;
            foreach (var t in trans.edge.tras)//ת��ʱ��
            {
                count = Math.Min(obj.rates[t.x].tempCount / t.y, count);
            }
            for (var k = nums.Last; k != null; k = k.Previous)
            {
                int sum = Math.Min(count, k.Value);
                count -= sum;
                foreach (var t in trans.edge.tras)
                {
                    obj.rates[t.x].tempCount -= sum * t.y;
                }
                k.Value -= sum;
                if (k == nums.Last)
                {
                    foreach (var data in trans.to.source)
                    {
                        resource.Add(data.x, data.y * sum);
                    }
                }
                else
                {
                    k.Next.Value += sum;
                }
            }
            int maxC = 9999999;
            foreach (var data in trans.from.source)
            {
                maxC = Math.Min(maxC, resource.GetRemain(data.x) / data.y);
            }
            count = Math.Min(maxC, count);
            maxnCount -= count;
            if (count != 0)
                foreach (var data in trans.from.source)
                {
                    resource.Remove(data.x, data.y * count);
                }
            nums.First.Value = count;
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

	public Source(BuildingObj obj,Resource resource,Trans trans)
    {
        this.trans = trans;
        this.resource = resource;
        this.obj = obj;
        maxnCount = 99999;
        nums = new LinkedList<int>();
        for(int i=0;i< trans.edge.time; i++)
        {
            nums.AddFirst(0);
        }
    }
}
//��Դ��Ҫ�����ṩ
public class IterSource:Source
{
    /// <summary>
    /// ������Դ,����ɱ�
    /// </summary>
    public virtual void Update()
    {
            //int maxC = 9999999;
            //foreach (var data in trans.from.source)
            //{
            //    maxC = Math.Min(maxC, resource.GetRemain(data.x) / data.y);
            //}
            int count = maxnCount;
            int maxC = 9999999;
            foreach (var data in trans.from.source)
            {
                maxC = Math.Min(maxC, resource.GetRemain(data.x) / data.y);
            }
            count = Math.Min(maxC, count);
            foreach (var t in trans.edge.tras)//ת��ʱ��
            {
                count = Math.Min(obj.rates[t.x].tempCount / t.y, count);
            }
            for (var k = nums.Last; k != null; k = k.Previous)
            {
                int sum = Math.Min(count, k.Value);
                count -= sum;
                foreach (var t in trans.edge.tras)
                {
                    obj.rates[t.x].tempCount -= sum * t.y;
                }
                foreach (var data in trans.from.source)
                {
                    resource.Remove(data.x, data.y * sum);
                }
                k.Value -= sum;
                if (k == nums.Last)
                {
                    foreach (var data in trans.to.source)
                    {
                        resource.Add(data.x, data.y * sum);
                    }
                }
                else
                {
                    k.Next.Value += sum;
                }

            }
            maxnCount -= count;
            if (count != 0)
                foreach (var data in trans.from.source)
                {
                    resource.Remove(data.x, data.y * count);
                }
            nums.First.Value = count;
        
    }
    public IterSource(BuildingObj obj, Resource resource, Trans trans):base(obj, resource, trans)
    {
    }
}
/// <summary>
/// ���߹�����
/// </summary>
public class PipLineManager
{
    public BuildingObj obj;
    /// <summary>
    /// ���ߵ���Ŀ
    /// </summary>
    public Dic<Trans,Source> piplineItem;
    /// <summary>
    /// �������̶�����Ĺ���
    /// </summary>
    public Dic<Trans,Source> worldPipline;
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
        piplineItem = new Dic<Trans, Source>();
        worldPipline = new Dic<Trans,Source>();
        foreach (var x in GameArchitect.get.objAsset.nodeGraph.worldRule)
        {
            worldPipline.Add(x, x.AddSource(obj,x));
        }
    }
}
[System.Serializable]
public class BuildingSaver : ObjSaver
{
    /// <summary>
    /// ������С
    /// </summary>
    public int container;
}
public class BuildingType: ObjType
{

}
[Map(), Class]
public class BuildingObj : Obj
{
    public int requireBuilding;
    /// <summary>
    /// ʣ��Ĺ���ʱ��
    /// </summary>
    public int remainBuilder { get { return requireBuilding - resource.Get(ObjEnum.BuildingResObjE); } }//��ǰʣ����Ҫ��������Դ
    /// <summary>
    /// �洢����Դ
    /// </summary>
    [SerializeField]
    public Resource resource;
    /*******************************************************************/
    /// <summary>
    /// ��ѡ�Ķ���
    /// </summary>
    public Dic<Enum<TransationEnum>, Rate> rates;
    /// <summary>
    /// ����ѡ���̶�ֵ
    /// </summary>
    public Dic<Enum<SitEnum>, Sit> sits;
    /*******************************************************************/
    /// <summary>
    /// ���ڽ��׵���Ʒ
    /// </summary>
    public GoodsManager goodsManager;
    /// <summary>
    /// building�Ĺ���
    /// </summary>
    public PipLineManager pipLineManager;
    public Person owner;
    public BuildingObj(BuildingSaver objSaver=null):base(objSaver)
    {
        requireBuilding = GetSaver().size;
        resource = new Resource();
        if (rates==null)
            rates = new Dic<Enum<TransationEnum>, Rate>();
        /*******************���Rate*******************/
        foreach (TransationEnum x in Enum.GetValues(typeof(TransationEnum)))
        {
            var data = x;
            rates.Add(data,new Rate(
                (objSaver) => { return objSaver.TransCount(data); },
                (objSaver) => { return objSaver.TransCan(data); },
                resource
            ));
        }
        //rates.Add(TransationEnum.cook , new Rate(
        //    (objSaver) => { return objSaver.canCook.count; },
        //    (objSaver) => { return objSaver.canCook.can; },
        //    resource
        //));
        //rates.Add(TransationEnum.qieGe, new Rate(
        //    (objSaver) => { return objSaver.qieGe.count; },
        //    (objSaver) => { return objSaver.qieGe.can; },
        //    resource
        //));
        //rates.Add(TransationEnum.gengZhong, new Rate(
        //    (objSaver) => { return objSaver.gengZhong.count; },
        //    (objSaver) => { return objSaver.gengZhong.can; },
        //    resource
        //));
        //rates.Add(TransationEnum.zaiZhong, new Rate(
        //    (objSaver) => { return objSaver.zaiZhong.count; },
        //    (objSaver) => { return objSaver.zaiZhong.can; },
        //    resource
        //));
        //rates.Add(TransationEnum.shouHuo, new Rate(
        //    (objSaver) => { return objSaver.shouHuo.count; },
        //    (objSaver) => { return objSaver.shouHuo.can; },
        //    resource
        //));
        resource.SetRate(rates);
        goodsManager = new GoodsManager(resource, this);
        pipLineManager = new PipLineManager(this);
        sits = new Dic<Enum<SitEnum>, Sit>();
        foreach (SitEnum x in Enum.GetValues(typeof(SitEnum)))
        {
            var data = x;
            sits.Add(data, new Sit((saver => { return saver.SitVal(data
                ); })));
        }
        resource.SetSites(sits);
        //resource.Add(ObjEnum.PlaceObjE,GetSaver().container);
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
        new SleepAct(),//˯�߻
        new ArrangeContractAct(),//ǩ��Э��
        new AddContractAct(),//���Э��
        new RemoveContractAct(),//�Ƴ�Э��
        /***************************************/
        new SellAct(),
        new BuyAct(),
        new CookAct(),
        new SelPipLineAct(),
        new SetPipLineAct(),
        new UseToolAct(),
        };
    }
    public BuildingSaver GetSaver()
    {
        return (BuildingSaver)objSaver;
    }
    /// <summary>
    /// ����...��Ҫ�޸�
    /// </summary>
	public override void LatUpdate()
	{
        
        /////////////////////////////////������ˮ��////////////////////////////////////
        Debug.Log(">>?"+pipLineManager.piplineItem.Count);
        if (pipLineManager == null)
        {
            pipLineManager = new PipLineManager(this);
        }
        foreach(var x in rates)
        {
            x.Value.tempCount = x.Value.nowCount;
        }
        foreach (var item in pipLineManager.piplineItem)//���ݹ��߶����ݽ��д���
        {
            item.Value.Update();
        }
        foreach (var item in pipLineManager.worldPipline)
        {
            item.Value.Update();
        }
        str.Clear();
        Debug.Log(resource.resources);
        foreach(var x in resource.resources)
        {
            str.Append(x.Key.ToString());
            str.Append(":");
            str.Append(x.Value.size);
            str.Append("/");
            str.Append(x.Value.remain);
            str.Append("\n");
        }
        cardInf.description = str.ToString();
        if(cardInf.cardControl)
        cardInf.cardControl.UpdateInf();
	}
}
