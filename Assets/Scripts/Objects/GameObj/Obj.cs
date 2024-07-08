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
    sleep,//˯����Ϊ
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
    /// ��������
    /// </summary>
    public ObjType obj;
    public string name;
    public TableModel belong;//�����ڵĶ���
    public ObjSaver objSaver;//���������
    public CardInf cardInf;
    public StringBuilder str;
    [NonSerialized]
    public List<Activity> activities;//��ɫ�����л
    public Dictionary<Obj,int> containers;//���������ɵĸ��ֶ���
    /// <summary>
    /// �����޸�ObjSaver��ʼ��
    /// </summary>
    /// <param name="objSaver"></param>
    public virtual void Init(ObjSaver objSaver)
    {
        ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.objs.Add(this);
        this.activities = GameArchitect.activities[GetType()];//һϵ�еĻ
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
    /// Ĭ�ϳ�ʼ��
    /// </summary>
    public virtual void Init()
    {
        objSaver = Map.Instance.GetSaver(GetType());//Ĭ�ϳ�ʼ��
        this.name = getName();
    }
    ///// <summary>
    ///// �ݻٵķ���
    ///// </summary>
    ///// <returns></returns>
    //public virtual List<GeneratorNode> Destory()
    //{
    //    return new List<GeneratorNode>();
    //}
    public virtual List<Activity> InitActivities()
    {
        return new List<Activity>() {
            new ArrangeContractAct(),//���Э��
            new AddContractAct(),//���Э��
            new RemoveContractAct()//�Ƴ�Э��
        };//һϵ�еĻ
    }
    public void Refresh()
    {
        belong.UpdateTable();
    }

    /// <summary>
    /// ����ַ���
    /// </summary>
    /// <returns></returns>
    public virtual StringBuilder GetString()
    {
        str.Clear();//���
        str.AppendLine(new In(obj,belong.sceneType).ToString());//���ڵ�λ��
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