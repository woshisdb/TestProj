using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Func<ObjSaver, int> sum;
    public Sit(Func<ObjSaver,int> func)
    {
        this.sum = func;
    }
    public Sit()
    {
        
    }
}
/// <summary>
/// ĳһ��Ϊ�Ĺ�����
/// </summary>
public class Rate
{
    public TransationEnum transType;
    public Func<ObjSaver, int> func;//��ȡ����
    public Func<ObjSaver,bool> can;
    public Resource resource;//��Ʒ����Ŀ
    public int nowCount;
    public Rate(Func<ObjSaver,int> func,Func<ObjSaver,bool> can,Resource resource)
    {
        this.func = func;
        this.can = can;
        this.resource = resource;
        nowCount = 0;
    }
    public void AddRate(Obj obj,int num)
    {
        if(can(obj.objSaver)==true)
        {
            nowCount += func(obj.objSaver);
        }
    }
    public void AddRate(ObjEnum obj, int num)
    {
        if (can(Map.Instance.GetSaver(obj)) == true)
        {
            nowCount += func(Map.Instance.GetSaver(obj));
        }
    }
    public int Get(ObjSaver obj)
    {
        return func(obj);
    }
    public void Use(Obj obj, int num=1)
    {
        nowCount += Get(obj.objSaver) * num;
        resource.resources[obj.Enum()].remain-= num;
    }
    public void Release(Obj obj, int num = 1)
    {
        nowCount -= Get(obj.objSaver) * num;
        resource.resources[obj.Enum()].remain += num;
    }
    public Dictionary<ObjEnum,ObjContBase> ObjList()
    {
        var s=resource.resources.Keys.Where(kv => can(Map.Instance.GetSaver(kv))).ToList();
        return resource.resources.Where(kv => s.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}

public class Source
{
    public BuildingObj obj;
    public Trans trans;
    public List<int> nums;
    public Resource resource;
    public int sour;
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
        sour += nums[nums.Count - 1];//�����Ŀ
        foreach (var data in trans.to.source)
        {
            resource.Add(data.x, data.y * (sour/trans.edge.time));
        }
        sour=sour%trans.edge.time;
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
        sour = 0;
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
    [SerializeField]
    public Resource resource;
    /*******************************************************************/
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
        resource = new Resource();
        if (rates==null)
            rates = new Dictionary<TransationEnum, Rate>();
        /*******************���Rate*******************/
        rates.Add(TransationEnum.cook , new Rate(
            (objSaver) => { return objSaver.canCook.count; },
            (objSaver) => { return objSaver.canCook.can; },
            resource
        ));
        rates.Add(TransationEnum.qieGe, new Rate(
            (objSaver) => { return objSaver.qieGe.count; },
            (objSaver) => { return objSaver.qieGe.can; },
            resource
        ));
        rates.Add(TransationEnum.gengZhong, new Rate(
            (objSaver) => { return objSaver.gengZhong.count; },
            (objSaver) => { return objSaver.gengZhong.can; },
            resource
        ));
        rates.Add(TransationEnum.zaiZhong, new Rate(
            (objSaver) => { return objSaver.zaiZhong.count; },
            (objSaver) => { return objSaver.zaiZhong.can; },
            resource
        ));
        rates.Add(TransationEnum.shouHuo, new Rate(
            (objSaver) => { return objSaver.shouHuo.count; },
            (objSaver) => { return objSaver.shouHuo.can; },
            resource
        ));
        resource.SetRate(rates);
        goodsManager = new GoodsManager(resource, this);
        pipLineManager = new PipLineManager(this);
        sits = new Dictionary<SitEnum, Sit>();
        sits.Add(SitEnum.bed, new Sit((saver =>{ return saver.sleep; })));
        sits.Add(SitEnum.set, new Sit(saver => { return saver.set; }));
        resource.SetSites(sits);
        resource.Add(ObjEnum.PlaceObjE,GetSaver().container);
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
            x.Value.nowCount = x.Value.nowCount;
        }
        foreach (var item in pipLineManager.piplineItem)//���ݹ��߶����ݽ��д���
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
