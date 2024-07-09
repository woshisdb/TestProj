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
public class Rate
{
    Func<Obj, int> func;//��ȡ����
    Func<Obj,bool> can;
    public Resource objList = new Resource();//��Ʒ����Ŀ
    public Rate(Func<Obj,int> func,Func<Obj,bool> can)
    {
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
    public void Remove(Obj obj,int num)
    {
        objList.Remove(obj,num);
    }
    public int Get(Obj obj)
    {
        return func(obj);
    }
}
public class CookItem
{
    /// <summary>
    /// �����Ŀ
    /// </summary>
    public int cookSum;
    /// <summary>
    /// Ҫ��⿵Ķ���
    /// </summary>
    public FoodObj cookObj;
    /// <summary>
    /// ���ѵ�ʱ��
    /// </summary>
    public int cookRate;
}
public class CookItems
{
    public List<CookItem> cookItems;//Ҫ��⿵�ʳ��
    public void Cook(int time)
    {
        if(cookItems.Count==0)
        {
        }
        else
        {
            cookItems[0].cookRate += time;
            if (cookItems[0].cookRate >= cookItems[0].cookObj.GetSaver().wastTime)
            {
                cookItems[0].cookSum--;
                if (cookItems[0].cookSum==0)
                {
                    cookItems.RemoveAt(0);
                }
            }
        }
    }
    public Resource resource;
    public CookItems(Resource resource)
    {
        this.resource = resource;
        cookItems = new List<CookItem>();
    }
    public CookItems()
    {
        cookItems = new List<CookItem>();
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
    /// <summary>
    /// ���ߵ���Ŀ
    /// </summary>
    public Rate CookRate;
    /// <summary>
    /// ���ʳ�ĵ��б�
    /// </summary>
    public CookItems CookItems;
    /******************************��⿵�ʳ��*************************************/
    /// <summary>
    /// ���ɵĶ���
    /// </summary>
    public Dictionary<Obj, int> objs;
    /// <summary>
    /// ���ڽ��׵���Ʒ
    /// </summary>
    public GoodsManager goodsManager;
    public BuildingObj(BuildingSaver objSaver):base(objSaver)
    {
        objs=new Dictionary<Obj, int>();
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
        CookItems = new CookItems(resource);
        goodsManager = new GoodsManager();
    }
    public override void Init(ObjSaver objSaver)
    {
        base.Init(objSaver);
        BedSit = new Sit();
        SetSit = new Sit();
        resource = new Resource();
        CookItems = new CookItems(resource);
        goodsManager = new GoodsManager();
    }
    public override List<Activity> InitActivities()
    {
        return new List<Activity>() {
        new SleepAct(
            (obj, person, objs) => {return BedSit.useSit < BedSit.sit; },
            (obj, person,objs) =>
            {
                var selectTime = new SelectTime(person, obj, new int[] { 1 });
                return new SeqAct(person, obj,
                selectTime,
                new SleepA(person,obj,selectTime.selectTime));
            }
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
}
