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
    public ObjType(string name = null) : base(name)
    {

    }
}
[P]
public class ObjSizeF:Func
{
    public ObjSizeF(ObjType objType):base(objType)
    {

    }
}
[P]
public class IsAliveP : Predicate
{
    public IsAliveP(ObjType objType):base(objType)
    {

    }
}
[P]
public class Money:Func
{
    public Money(PersonType objType):base(objType)
    {

    }
}
public class In:Predicate
{
    public In(ObjType x,SceneType sceneType):base(x,sceneType)
    {

    }
}

public class Obj:PDDL,ICanSendEvent
{
    /// <summary>
    /// 对象类型
    /// </summary>
    public ObjType obj;
    public string name;
    public TableModel belong;//所属于的对象
    public ObjSaver objSaver;//对象的类型
    public CardInf cardInf;
    public StringBuilder str;
    [NonSerialized]
    public List<Activity> activities;//角色的所有活动
    public Dictionary<Obj,int> containers;//里面所容纳的各种东西
    /// <summary>
    /// 用于修改ObjSaver初始化
    /// </summary>
    /// <param name="objSaver"></param>
    public virtual void Init(ObjSaver objSaver)
    {
        ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.objs.Add(this);
        this.activities = GameArchitect.activities[GetType()];//一系列的活动
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
            new ArrangeContractAct(),//达成协议
            new AddContractAct(),//添加协议
            new RemoveContractAct()//移除协议
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
        str.AppendLine(new In(obj,belong.sceneType).ToString());//所在的位置
        return str;
    }
    public ObjSaver Saver()
    {
        return objSaver;
    }
}
//public class Obj<T>:Obj where T : ObjSaver
//{
//    public T Saver()
//    {
//        return (T)objSaver;
//    }
//    public Obj(ObjSaver objSaver=null)
//    {

//    }
//}