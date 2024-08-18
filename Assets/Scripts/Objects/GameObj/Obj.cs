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
    /// ��������
    /// </summary>
    public PType obj { get { return pddl.GetPType(); } }
    public string name;
    [Property]
    public TableModel belong;//�����ڵĶ���
    public ObjSaver objSaver;//���������
    public CardInf cardInf;
    public StringBuilder str;
    [NonSerialized]
    public List<Activity> activities;//��ɫ�����л
    /// <summary>
    /// �����޸�ObjSaver��ʼ��
    /// </summary>
    /// <param name="objSaver"></param>
    public virtual void Init(ObjSaver objSaver)
    {
        ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.objs.Add(this);
        this.activities = GameArchitect.activities[GetType()];//һϵ�еĻ
        //��ʼ��PDDL��
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
            //new ArrangeContractAct(),//���Э��
            //new AddContractAct(),//���Э��
            //new RemoveContractAct()//�Ƴ�Э��
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
        return str;
    }
    public ObjSaver Saver()
    {
        return objSaver;
    }
    /// <summary>
    /// ʱ�䲽����
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
