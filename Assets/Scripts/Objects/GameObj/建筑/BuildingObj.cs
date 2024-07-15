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
/// ĳһ��Ϊ�Ĺ�����
/// </summary>
public class Rate
{
    public TransationEnum transType;
    Func<ObjSaver, int> func;//��ȡ����
    Func<ObjSaver,bool> can;
    public int count;//��ǰģ�������ṩ����Ŀ
    public int nowCount;//��ǰģ����Ŀ
    public Resource objList = new Resource();//��Ʒ����Ŀ
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
    /// ������Դ,����ɱ�
    /// </summary>
    public void Update()
    {
        int count = maxnCount;
        foreach (var t in trans.edge.tras)//ת��ʱ��
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
/// ���߹�����
/// </summary>
public class PipLineManager
{
    public BuildingObj obj;
    /// <summary>
    /// ���ߵ���Ŀ
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
    /// ������С
    /// </summary>
    public int container;
}

[Map()]
public class BuildingObj : Obj
{
    /// <summary>
    /// �洢����Դ
    /// </summary>
    public Resource resource;
    /******************************��⿵�ʳ��*************************************/
    /// <summary>
    /// ��ѡ�Ķ���
    /// </summary>
    public Dictionary<TransationEnum, Rate> rates;
    /// <summary>
    /// ����ѡ���̶�ֵ
    /// </summary>
    public Dictionary<SitEnum, Sit> sits;
    /*******************************************************************/
    /// <summary>
    /// ���ڽ��׵���Ʒ
    /// </summary>
    public GoodsManager goodsManager;
    /// <summary>
    /// building�Ĺ���
    /// </summary>
    public PipLineManager pipLineManager;
    public BuildingObj(BuildingSaver objSaver=null):base(objSaver)
    {
        if(rates==null)
            rates = new Dictionary<TransationEnum, Rate>();
        /*******************���Rate*******************/
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
        ),//˯�߻
        new ArrangeContractAct(),//ǩ��Э��
        new AddContractAct(),//���Э��
        new RemoveContractAct(),//�Ƴ�Э��
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
    /// ����...��Ҫ�޸�
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
        foreach (var item in pipLineManager.piplineItem)//���ݹ��߶����ݽ��д���
        {
            item.Value.Update();
        }
	}
}
