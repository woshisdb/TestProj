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
    /// ������С
    /// </summary>
    public int container;
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
    Func<Obj, int> func;//��ȡ����
    Func<Obj,bool> can;
    public int count;//��ǰģ�������ṩ����Ŀ
    public Resource objList = new Resource();//��Ʒ����Ŀ
    public Rate(Func<Obj,int> func,Func<Obj,bool> can)
    {
        count = 0;
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
    public void Add(ObjEnum obj, int num)
    {
        if (can(obj) == true)
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
    public void Use(Obj obj, int num=1)
    {
        count += Get(obj)*num;
    }
    public void Release(Obj obj, int num = 1)
    {
        count -= Get(obj) * num;
    }
}

public class Source
{
    BuildingObj obj;
    public Trans trans;
    public List<int> nums;
    public Resource resource;
    /// <summary>
    /// ������Դ,����ɱ�
    /// </summary>
    public void Update(int count)
    {
        foreach (var data in trans.to.source)
        {
            resource.Add(data.x, data.y * nums[nums.Count - 1]);
        }
        for (int i = nums.Count - 1; i>=1; i--)
        {
            nums[i] = nums[i - 1];
        }
        nums[0] = count;
    }
    public Source(BuildingObj obj,Resource resource,Trans trans)
    {
        this.trans = trans;
        this.resource = resource;
        this.obj = obj;
        nums = new List<int>(trans.edge.time);
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


[Map()]
public class BuildingObj : Obj
{
    /// <summary>
    /// �洢����Դ
    /// </summary>
    public Resource resource;
    /// <summary>
    /// ������Ŀ
    /// </summary>
    public Sit BedSit;
    /// <summary>
    /// λ�ӵ���Ŀ
    /// </summary>
    public Sit SetSit;
    /******************************��⿵�ʳ��*************************************/
    /// <summary>
    /// ���ߵ���Ŀ
    /// </summary>
    public Rate CookRate;
    /*******************************************************************/
    /// <summary>
    /// ���ڽ��׵���Ʒ
    /// </summary>
    public GoodsManager goodsManager;
    /// <summary>
    /// building�Ĺ���
    /// </summary>
    public PipLineManager pipLineManager;
    public BuildingObj(BuildingSaver objSaver):base(objSaver)
    {
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
        goodsManager = new GoodsManager();
        pipLineManager = new PipLineManager(this);
    }
    public override void Init(ObjSaver objSaver)
    {
        base.Init(objSaver);
        BedSit = new Sit();
        SetSit = new Sit();
        resource = new Resource();
        goodsManager = new GoodsManager();
        pipLineManager = new PipLineManager(this);
    }
    public override List<Activity> InitActivities()
    {
        return new List<Activity>() {
        new SleepAct(
            (obj, person, objs) => {return BedSit.useSit < BedSit.sit; }
        ),//˯�߻
        new ArrangeContractAct(),//ǩ��Э��
        new AddContractAct(),//���Э��
        new RemoveContractAct(),//�Ƴ�Э��
        /***************************************/
        new SellAct(),//������
        new BuyAct(),//����
        new CookAct()//��⿶���
        };
    }
    public void Add(Obj s)
    {
        //����˯��
        if(s.objSaver.canSleep.can)
        {
            BedSit.sit += s.objSaver.canSleep.count;
        }
        //������
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
    /// <summary>
    /// ����
    /// </summary>
	public override void Update()
	{
        foreach (var item in pipLineManager.piplineItem)//���ݹ��߶����ݽ��д���
        {

        }
	}
}
