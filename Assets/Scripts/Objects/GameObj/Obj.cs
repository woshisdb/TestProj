using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using QFramework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public enum ActivityType
{
    sleep,//睡觉行为
}


public class ObjType:PType
{
    
}
public interface IPPDLExtent
{

}
public interface IPDDL
{
    public PType GetPtype();
    public void InitPDDLClass();
    public PDDLClass GetPDDLClass();
}

[Map(null,"objSaver"),Class]
public class Obj:PDDL,ICanRegisterEvent,IPDDL
{
    public PDDLClass pddl;
    /// <summary>
    /// 对象类型
    /// </summary>
    public PType obj { get { return pddl.GetPType(); } }
    public string name;
    [Property]
    public TableModel belong;//所属于的对象
    public ObjSaver objSaver;//对象的类型
    public CardInf cardInf;
    public StringBuilder str;
    [NonSerialized]
    public List<Activity> activities;//角色的所有活动
    /// <summary>
    /// 用于修改ObjSaver初始化
    /// </summary>
    /// <param name="objSaver"></param>
    public virtual void Init(ObjSaver objSaver)
    {
        ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.objs.Add(this);
        this.activities = GameArchitect.activities[GetType()];//一系列的活动
        //初始化PDDL类
    }
    public PType GetPtype()
    {
        return obj;
    }
    public ObjEnum GetEnum()
    {
        return objSaver.GetEnum();
    }
    public Obj(ObjSaver objAsset=null)
    {
        str= new StringBuilder();
        if (objAsset == null)
        {
            Init();
        }
        else
        {
            objSaver=objAsset;
            Init(objSaver);
        }
        cardInf=new CardInf(objSaver.title,objSaver.description,
        () => 
        {
            this.SendEvent<SelectCardEvent>(new SelectCardEvent(cardInf, GameArchitect.get.player));
        }    
        );
        InitPDDLClass();
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public string getName()
    {
        return "" + GetType().Name + "_" + Nm.num;
    }
    /// <summary>
    /// 默认初始化
    /// </summary>
    public virtual void Init()
    {
        objSaver = Map.Instance.GetSaver(GetType());//默认初始化
        this.name = getName();
    }
    ///// <summary>
    ///// 摧毁的方法
    ///// </summary>
    ///// <returns></returns>
    //public virtual List<GeneratorNode> Destory()
    //{
    //    return new List<GeneratorNode>();
    //}
    public virtual List<Activity> InitActivities()
    {
        return new List<Activity>() {
            //new ArrangeContractAct(),//达成协议
            //new AddContractAct(),//添加协议
            //new RemoveContractAct()//移除协议
        };//一系列的活动
    }
    public void Refresh()
    {
        belong.UpdateTable();
    }

    /// <summary>
    /// 获得字符串
    /// </summary>
    /// <returns></returns>
    public virtual StringBuilder GetString()
    {
        str.Clear();//清除
        return str;
    }
    public ObjSaver Saver()
    {
        return objSaver;
    }
    /// <summary>
    /// 时间步更新
    /// </summary>
    public virtual void BefUpdate()
    {
        
    }
    public virtual void LatUpdate()
    {
        
    }

    public void InitPDDLClass()
    {
        //Debug.Log("hhhhhhhhhhhhhhh");
        pddl = PDDLClassGet.Generate(this.GetType());
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }
    ~Obj()
    {
        PDDLClassGet.Remove(pddl);
    }
}


public class WasteTimeA : Act
{
    Int n;
    public WasteTimeA(Person person, Obj obj,Int n) : base(person, obj)
    {
        this.n = n;
        wastTime = true;
    }

    public override IEnumerator<object> Run(Action<Act> callback)
    {
        TC();
        Debug.Log("WasteTime"+n);
        n--;
        if (n == 0)
            yield return Ret(new EndAct(Person, Obj), callback);
        else
        {
            yield return Ret(this,callback);
        }
    }
}
