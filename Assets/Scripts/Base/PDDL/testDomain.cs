using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
///// <summary>
///// 物品
///// </summary>
//public class ObjType :PType
//{
//    public ObjType(string name = null,bool val=false) :base(name,val)
//    {

//    }
//}
///// <summary>
///// 场景
///// </summary>
//public class SceneType : PType
//{
//    public SceneType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}

///// <summary>
///// 有实体的物体
///// </summary>
//public class PlacerType : ObjType
//{
//    public PlacerType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}
///// <summary>
///// 有自主性的物体
///// </summary>
//public class DynamicType : PlacerType
//{
//    public DynamicType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}
///// <summary>
///// 被动执行的物体
///// </summary>
//public class StaticType : PlacerType
//{
//    public StaticType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}
///// <summary>
///// 人类
///// </summary>
///// 
//public class PersonType :DynamicType
//{
//    public PersonType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//    //public static implicit operator PersonType(Person x)
//    //{
//    //    return new PersonType("person_"+x.personName);
//    //}
//}



///**********************谓词****************************/

///// <summary>
///// 有从A到B的路径
///// </summary>
//public class HaveRoad : Predicate
//{
//    public HaveRoad(SceneType s1, SceneType s2)
//    {
//        objects.Add(s1);
//        objects.Add(s2);
//    }
//}



//public class PosX:Func
//{
//    public PosX(PlacerType placer)
//    {
//        objects.Add(placer);
//    }
//}
//public class PosY : Func
//{
//    public PosY(PlacerType placer)
//    {
//        objects.Add(placer);
//    }
//}
//public class Distance : Func
//{
//    public Distance(SceneType s1, SceneType s2)
//    {
//        objects.Add(s1);
//        objects.Add(s2);
//    }
//}
//public class TestDomain : Domain
//{
//    public static ObjType objType = new ObjType();
//    public static SceneType sceneType = new SceneType();
//    public static PersonType personType = new PersonType();
//    public static PlacerType placerType = new PlacerType();
//    public TestDomain(Person person):base()
//    {
//        //types = new List<PType>() {objType,sceneType,personType,placerType};
//        //predicates = new List<Predicate>() { };
//        //TimeF time=new TimeF();
//        //funcs=new List<Func>(){ SleepRate(new PersonType()),time };
//        ////睡眠活动(最少一个小时)
//        ////6回合一小时
//        ////4消耗
//        ////10回复
//        //actions = new List<Action>() { };
//        ////移动到场景
//        //{
//        //    SceneType p1 = new SceneType("p1");
//        //    SceneType p2 = new SceneType("p2");
//        //    PersonType ps = new PersonType("person");
//        //    PType[] runAction = {p1,p2,ps};
//        //    actions.Add(new Action(
//        //        "run"
//        //        ,
//        //        runAction
//        //        ,
//        //        And(HaveRoad(p1,p2),IsThere(ps,p1))
//        //        ,
//        //        And(IsThere(ps,p2),Not(IsThere(ps,p1)))
//        //        ,
//        //        Distance(p1,p2)
//        //    ));
//        //}

//        //axioms = new List<Derived>();
//    }

//}
//public class TestProblem : Problem
//{
//    public TestProblem(Person person, TestDomain testDomain) :base(person)
//    {
//        domain = testDomain;

//    }
//}
public class Domain
{
    public List<Type> pTypes;
    public List<Predicate> predicates;
    public List<Func> funcs;
    public List<PAction> pActions;
    public string domainName;
    public Domain()
    {
        pTypes = new List<Type>();
        predicates = new List<Predicate>();
        funcs = new List<Func>();
        pActions = new List<PAction>();
    }
    public void Print()
    {
        StringBuilder str = new StringBuilder();
        str.AppendLine("(");
        str.AppendFormat("(domain {0})\n", domainName);

        if (pTypes.Count > 0)
        {
            str.AppendLine("(:types\n");
            str.AppendLine("PType``");
            for (int i = 0; i < pTypes.Count; i++)
            {
                str.AppendFormat("{0}-{1}\n", pTypes[i].Name, pTypes[i].BaseType.Name);
            }
            str.AppendLine("\n)\n");
        }
        if (predicates.Count > 0)
        {
            str.AppendLine("(:predicates\n");

            for (int j = 0; j < predicates.Count; j++)
            {
                predicates[j].f(str);
                str.Append(" ");
            }
            str.AppendLine("\n)");
        }
        if (funcs.Count > 0)
        {
            str.AppendLine("(:functions\n");

            for (int j = 0; j < funcs.Count; j++)
            {
                str.Append(funcs[j].ToString() + " ");
            }
            str.AppendLine("\n)");
        }

        for (int i = 0; i < pActions.Count; i++)
        {
            str.AppendLine(pActions[i].ToString());
        }
        //for (int i = 0; i < axioms.Count; i++)
        //{
        //    str.AppendLine(axioms[i].ToString());
        //}
        Debug.Log( str.ToString());
    }
}