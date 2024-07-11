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


/// <summary>
/// ���߹�����
/// </summary>
public class PipLineManager
{
    /// <summary>
    /// ���ߵ���Ŀ
    /// </summary>
    public HashSet<Trans> piplineItem;
    public void AddTrans(HashSet<Trans> tran)
    {
        piplineItem.UnionWith(tran);
    }
    public void RemoveTrans(HashSet<Trans> tran)
    {
        piplineItem.ExceptWith(tran);
    }
    public void AddTrans(List<Trans> tran)
    {
        piplineItem.UnionWith(tran);
    }
    public void RemoveTrans(List<Trans> tran)
    {
        piplineItem.ExceptWith(tran);
    }
    public PipLineManager()
    {
        piplineItem = new HashSet<Trans>();
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
        pipLineManager = new PipLineManager();
    }
    public override void Init(ObjSaver objSaver)
    {
        base.Init(objSaver);
        BedSit = new Sit();
        SetSit = new Sit();
        resource = new Resource();
        goodsManager = new GoodsManager();
        pipLineManager = new PipLineManager();
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
