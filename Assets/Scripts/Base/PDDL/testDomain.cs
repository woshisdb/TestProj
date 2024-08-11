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
    protected class MyCustomComparer : IEqualityComparer<PType>
    {
        public bool Equals(PType x, PType y)
        {
            return x.typeName == y.typeName;
        }

        public int GetHashCode(PType obj)
        {
            return obj.typeName.GetHashCode();
        }
    }

    protected class FuncComparer : IEqualityComparer<Func>
    {
        public bool Equals(Func x, Func y)
        {
            return x.name == y.name;
        }

        public int GetHashCode(Func obj)
        {
            return obj.name.GetHashCode();
        }
    }

    protected class PredicateComparer : IEqualityComparer<Predicate>
    {
        public bool Equals(Predicate x, Predicate y)
        {
            return x.name == y.name;
        }

        public int GetHashCode(Predicate obj)
        {
            return obj.name.GetHashCode();
        }
    }

    protected class PActionComparer : IEqualityComparer<PAction>
    {
        public bool Equals(PAction x, PAction y)
        {
            return x.actionName == y.actionName;
        }

        public int GetHashCode(PAction obj)
        {
            return obj.actionName.GetHashCode();
        }
    }

    public HashSet<PType> pTypes;
    public HashSet<Predicate> predicates;
    public HashSet<Func> funcs;
    public HashSet<PAction> pActions;
    public string domainName;
    public Domain()
    {
        pTypes = new HashSet<PType>(new MyCustomComparer());
        predicates = new HashSet<Predicate>(new PredicateComparer());
        funcs = new HashSet<Func>(new FuncComparer());
        pActions = new HashSet<PAction>(new PActionComparer());
    }
    public void Print()
    {
        StringBuilder str = new StringBuilder();
        str.AppendLine("(");
        str.AppendFormat("(domain {0})\n", domainName);
        if (pTypes.Count > 0)
        {
            str.AppendLine("(:types\n");
            str.AppendLine("PType");
            foreach (var p in pTypes)
            {
                str.AppendFormat("{0}-{1}\n", p.typeName, p.GetType().BaseType.GetType().Name);
            }
            str.AppendLine("\n)\n");
        }
        if (predicates.Count > 0)
        {
            str.AppendLine("(:predicates\n");

            foreach (var p in predicates)
            {
                p.f(str);
                str.Append(" ");
            }
            str.AppendLine("\n)");
        }
        if (funcs.Count > 0)
        {
            str.AppendLine("(:functions\n");

            foreach (var p in funcs)
            {
                str.Append(p.ToString() + " ");
            }
            str.AppendLine("\n)");
        }

        foreach (var p in pActions)
        {
            str.AppendLine(p.ToString());
        }
        //for (int i = 0; i < axioms.Count; i++)
        //{
        //    str.AppendLine(axioms[i].ToString());
        //}
        Debug.Log( str.ToString());
    }
    public void AddTypes(List<PType> ps)
    {
        if (ps != null)
        {
            foreach (var x in ps)
            {
               var has= pTypes.Contains(x);
                if(!has)
                {
                    pTypes.Add(x);
                }
            }
        }
    }
    public void AddFuncs(List<Func> ps)
    {
        if (ps != null)
        {
            foreach (var x in ps)
            {
                var has = funcs.Contains(x);
                if (!has)
                {
                    funcs.Add(x);
                }
            }
        }
    }

    public void AddPreds(List<Predicate> ps)
    {
        if (ps != null)
        {
            foreach (var x in ps)
            {
                var has = predicates.Contains(x);
                if (!has)
                {
                    predicates.Add(x);
                }
            }
        }
    }
}