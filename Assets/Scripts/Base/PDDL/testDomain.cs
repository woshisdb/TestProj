using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///// <summary>
///// ��Ʒ
///// </summary>
//public class ObjType :PType
//{
//    public ObjType(string name = null,bool val=false) :base(name,val)
//    {

//    }
//}
///// <summary>
///// ����
///// </summary>
//public class SceneType : PType
//{
//    public SceneType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}

///// <summary>
///// ��ʵ�������
///// </summary>
//public class PlacerType : ObjType
//{
//    public PlacerType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}
///// <summary>
///// �������Ե�����
///// </summary>
//public class DynamicType : PlacerType
//{
//    public DynamicType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}
///// <summary>
///// ����ִ�е�����
///// </summary>
//public class StaticType : PlacerType
//{
//    public StaticType(string name = null, bool val = false) : base(name, val)
//    {

//    }
//}
///// <summary>
///// ����
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



///**********************ν��****************************/

///// <summary>
///// �д�A��B��·��
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
//        ////˯�߻(����һ��Сʱ)
//        ////6�غ�һСʱ
//        ////4����
//        ////10�ظ�
//        //actions = new List<Action>() { };
//        ////�ƶ�������
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