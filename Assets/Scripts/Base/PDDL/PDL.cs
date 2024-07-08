using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
public class PDDL: PD
{
    public static Pop Wup()
    {
        return Increase(NowT(),IT());
    }
    public static Or Or(params Pop[] numbers)
    {
        return new Or(numbers);
    }
    public static And And(params Pop[] numbers)
    {
        return new And(numbers);
    }
    public static To To(Pop x, Pop y)
    {
        return new To(x, y);
    }
    public static L L(Pop x, Pop y)
    {
        return new L(x, y);
    }
    public static Eq Eq(Pop x, Pop y)
    {
        return new Eq(x, y);
    }
    public static G G(Pop x, Pop y)
    {
        return new G(x, y);
    }
    public static Add Add(Pop x, Pop y)
    {
        return new Add(x, y);
    }
    public static Increase Increase(Pop x, Pop y)
    {
        return new Increase(x, y);
    }
    public static Dec Dec(Pop x, Pop y)
    {
        return new Dec(x, y);
    }
    public static Mul Mul(Pop x, Pop y)
    {
        return new Mul(x, y);
    }
    public static Div Div(Pop x, Pop y)
    {
        return new Div(x, y);
    }
    public static As As(Pop x, Pop y)
    {
        return new As(x, y);
    }
    public static Not Not(Pop number)
    {
        return new Not(number);
    }
    public static OverALL OverALL(Pop number)
    {
        return new OverALL(number);
    }
    public static AtEnd AtEnd(Pop number)
    {
        return new AtEnd(number);
    }
    public static AtStart AtStart(Pop number)
    {
        return new AtStart(number);
    }
    public static ForAll ForAll(Pop when, Pop express, params PType[] numbers)
    {
        return new ForAll(when, express, numbers);
    }
    public static Exist Exist(Pop express, params PType[] numbers)
    {
        return new Exist(express, numbers);
    }
    public static PO PO(PType pType)
    {
        return new PO(pType);
    }
    public static PT PT(PType pType)
    {
        return new PT(pType);
    }
    public static Pop Max(Pop x, Pop y)
    { return new Max(x, y); }
    public static Pop Min(Pop x, Pop y)
    { return new Min(x, y); }
    public static Pop When(Pop x, Pop y)
    { return new When(x, y); }
    /// <summary>
    /// 边界加法
    /// </summary>
    /// <param name="func"></param>
    /// <param name="x"></param>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Pop BoundAdd(Func func,int x,int l,int r)
    {
        return Min(Max(Add(func, (I)x), (I)l),(I)r);
    }
    public static Pop Abs(Pop x)
    {
        return new Abs(x);
    }
    /***************************************/

}
public class Nm
{
    public static int x = 0;
    public static int num { get { x++; return x; } }

}
public class PType: PDDL
{
    public string typeName;//类型名
    //public PType fa;//父类
    public string objName;
    //public bool isVal;
    public PType(string objName="")
    {
        if(objName=="")
        {
            this.objName = this.GetType().Name + "_" + Nm.num;
        }
        else
        this.objName = objName;
        typeName = this.GetType().Name;
    }
    public override string ToString()
    {
        return objName;
        //var s1= typeName;
        //if(GetType().BaseType==typeof(PDDL))
        //{
        //    return s1;
        //}
        //else
        //{
        //    return s1 + " - " + GetType().BaseType.Name;
        //}
    }
    public string ObjName()
    {
        var s1 = typeName;
        if (GetType().BaseType == typeof(PDDL))
        {
            return s1;
        }
        else
        {
            return s1 + " - " + GetType().BaseType.Name;
        }
    }
}
public class CustomPredicate:Predicate
{
    public CustomPredicate(string name,params PType[] x):base(x)
    {
        this.name = name;
    }
}

public class CustomFunc : Func
{
    public string name;
    public List<PType> objects;
    public CustomFunc(string name,params PType[] x):base(x)
    {
        this.name = name;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("(");
        sb.Append(name + " ");
        for (var i = 0; i < objects.Count; i++)
        {
            sb.AppendFormat(" ?{0}", objects[i].objName);
        }
        sb.Append(")");
        return sb.ToString();
    }
    public string f()
    {
        StringBuilder sb = new StringBuilder("(" + name + " ");
        for (int i = 0; i < objects.Count; i++)
        {
            sb.AppendFormat("?{0}-{1}", objects[i].objName, objects[i].typeName);
        }
        sb.Append(")");
        return sb.ToString();
    }
}

public class Predicate : Pop
{
    public string name;
    public List<PType> objects;
    public Predicate(params PType[] x)
    {
        name=GetType().Name;
        objects= new List<PType>(x);
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("("+name+" ");
        for(int i = 0; i < objects.Count; i++)
        {
            sb.AppendFormat(" {0} ", objects[i].ToString());
        }
        sb.Append(")");
        return sb.ToString();
    }
    public static string f(Type type)
    {
        StringBuilder sb = new StringBuilder("(" + type.Name + " ");
        ConstructorInfo constructors = type.GetConstructors()[0];
        foreach (ParameterInfo parameter in constructors.GetParameters())
            sb.AppendFormat("?{0}-{1} ", parameter.Name, parameter.ParameterType.Name);
        sb.Append(" )");
        return sb.ToString();
    }
}
public class Func : Pop
{
    public string name;
    public List<PType> objects;
    public Func(params PType[] x)
    {
        name = GetType().Name;
        objects = new List<PType>(x);
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("(");
        sb.Append(name + " ");
        for (var i = 0; i < objects.Count; i++)
        {
            sb.AppendFormat(" ?{0}", objects[i].objName);
        }
        sb.Append(")");
        return sb.ToString();
    }
    public static string f(Type type)
    {
        StringBuilder sb = new StringBuilder("(" + type.Name + " ");
        ConstructorInfo constructors = type.GetConstructors()[0];
        foreach (ParameterInfo parameter in constructors.GetParameters())
            sb.AppendFormat("?{0}-{1}", parameter.Name, parameter.ParameterType.Name);
        sb.Append(" )");
        return sb.ToString();
    }
}
public class Pop : PDDL
{
    public override string ToString()
    {
        return base.ToString();
    }
    public static implicit operator Pop(Num num)
    {
        //Debug.Log(num);
        return num.func;
    }
    public static implicit operator Pop(Bool num)
    {
        return num.predicate;
    }
}
public class And:Pop
{
    public List<Pop> pops;
    public And(params Pop[] numbers)
    {
        pops = new List<Pop>(numbers);
    }
    public override string ToString()
    {
        StringBuilder sb=new StringBuilder("( and ");
        for(int i=0;i<pops.Count;i++)
        {
            sb.Append(pops[i].ToString());
            sb.Append(" ");
        }
        sb.Append(")");
        return sb.ToString();
    }
}

public class Or : Pop
{
    public List<Pop> pops;
    public Or(params Pop[] numbers)
    {
        pops = new List<Pop>(numbers);
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( or ");
        for (int i = 0; i < pops.Count; i++)
        {
            sb.Append(pops[i].ToString());
            sb.Append(" ");
        }
        sb.Append(") ");
        return sb.ToString();
    }
}
public class To : Pop
{
    public Pop x;
    public Pop y;
    public To(Pop x,Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( imply ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class When : Pop
{
    public Pop x;
    public Pop y;
    public When(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( when ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class L : Pop
{
    public Pop x;
    public Pop y;
    public L(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( < ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Eq : Pop
{
    public Pop x;
    public Pop y;
    public Eq(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( = ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class G : Pop
{
    public Pop x;
    public Pop y;
    public G(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( > ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Abs : Pop
{
    public Pop x;
    public Abs(Pop x)
    {
        this.x = x;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( abs ");
        sb.Append(x.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Add: Pop
{
    public Pop x;
    public Pop y;
    public Add(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( + ");
        sb.Append(x.ToString());
        sb = sb.Append("  ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Increase : Pop
{
    public Pop x;
    public Pop y;
    public Increase(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( increase ");
        sb.Append(x.ToString());
        sb = sb.Append("  ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Max : Pop
{
    public Pop x;
    public Pop y;
    public Max(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( max, ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Min : Pop
{
    public Pop x;
    public Pop y;
    public Min(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( min, ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Dec : Pop
{
    public Pop x;
    public Pop y;
    public Dec(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( -, ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}

public class Mul : Pop
{
    public Pop x;
    public Pop y;
    public Mul(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( *, ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}

public class Div : Pop
{
    public Pop x;
    public Pop y;
    public Div(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( /, ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class As : Pop
{
    public Pop x;
    public Pop y;
    public As(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( assign, ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Not : Pop
{
    public Pop pop;
    public Not(Pop number)
    {
        this.pop = number;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( not ");
        sb.Append(pop.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class OverALL : Pop
{
    public Pop pop;
    public OverALL(Pop number)
    {
        this.pop = number;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( over all ");
        sb.Append(pop.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class AtEnd : Pop
{
    public Pop pop;
    public AtEnd(Pop number)
    {
        this.pop = number;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( at end ");
        sb.Append(pop.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class AtStart : Pop
{
    public Pop pop;
    public AtStart(Pop number)
    {
        this.pop = number;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( at start ");
        sb.Append(pop.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class ForAll : Pop
{
    public Pop when;
    public Pop express;
    public PType[] ptyps;
    public ForAll(Pop when,Pop express, params PType[] numbers)
    {
        this.when = when;
        this.express = express;
        this.ptyps=numbers;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( forall (");
        foreach (var p in ptyps)
        {
            sb.Append(p.ToString());
            sb = sb.Append(" ");
        }
        sb.Append(')');
        if (when != null)
        {
            sb.Append("when(");
            sb.Append(when);
            sb.Append(")");
        }
        sb.Append(express);
        sb.Append(") ");
        return sb.ToString();
    }
}

public class Exist : Pop
{
    public Pop express;
    public PType[] ptyps;
    public Exist(Pop express, params PType[] numbers)
    {
        this.express = express;
        this.ptyps = numbers;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( exists (");
        foreach (var p in ptyps)
        {
            sb.Append(p.ToString());
            sb = sb.Append(" ");
        }
        sb.Append(')');
        sb.Append(express);
        sb.Append(") ");
        return sb.ToString();
    }
}
/// <summary>
/// 
/// </summary>
public class PT:Pop
{
    public string typeName;//类型名
    public PType fa;//父类
    public string objName;
    public PT(PType pType)
    {
        typeName = pType.typeName;
        objName = pType.objName;
        //fa = pType.fa;
    }
    public static implicit operator PT(PType custom)
    {
        return new PT(custom);
    }
    public override string ToString()
    {
        return "?"+objName;
    }
}
public class PO : Pop
{
    public string typeName;//类型名
    public PType fa;//父类
    public string objName;
    public PO(PType pType)
    {
        typeName = pType.typeName;
        objName = pType.objName;
        //fa = pType.fa;
    }
    public static implicit operator PO(PType custom)
    {
        return new PO(custom);
    }
    public override string ToString()
    {
        return objName;
    }
}
public class PAction : PDDL
{
    public string actionName;
    public List<PType> objects;
    public Pop condition;
    public Pop effect;
    public Pop duration;
    public PAction()
    {
        actionName = GetType().Name;
        objects = new List<PType>();
        condition = null;
        effect = null;
        duration = null;
    }
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        if(duration != null)
            str.AppendFormat("(:durative-action {0}\n", actionName);
        else
        str.AppendFormat("(:action {0}\n",actionName);
        str.AppendFormat(":parameters(");
        for (int i = 0; i < objects.Count; i++)
        {
            str.AppendFormat("?{0}-{1} ", objects[i].objName, objects[i].typeName);
        }
        str.AppendLine(")\n");
        if(duration != null)
        {
            str.AppendFormat(":duration(=?duration {0})\n",duration.ToString());
        }
        if(condition != null)
        {
            str.AppendLine(":condition(");
            str.AppendLine(condition.ToString());
            str.AppendLine(")\n");
        }
        //str.AppendLine(":precondition(");
        //str.AppendLine(condition.ToString());
        //str.AppendLine(")\n");
        str.AppendLine(":effect(\n");
        str.AppendLine(effect.ToString());
        str.AppendLine(")\n");
        str.AppendLine(")\n");
        return str.ToString();
    }
    public void Init(string actionName, PType[] objects, Pop condition, Pop effect,Pop duration=null)
    {
        this.actionName = actionName;
        this.objects =new List<PType>(objects);
        this.condition = condition;
        this.effect = effect;
        this.duration = duration;
    }
}
public class Derived
{
    public Predicate dpre;
    public Pop require;
    public Derived(Predicate dpre, Pop require)
    {
        this.dpre = dpre;
        this.require = require;
    }
    public override string ToString()
    {
        StringBuilder ret = new StringBuilder("(:derived\n");

        ret.AppendLine(dpre.ToString());
        ret.AppendLine("\n");
        ret.AppendLine(require.ToString());
        ret.AppendLine("\n)");
        return ret.ToString();
    }
}
/////// <summary>
/////// 描述Bool集合
/////// </summary>
/////// <typeparam name="T"></typeparam>
////public class Set<T> : PDDL
////{
////    public HashSet<T> set;
////    public ObjType obj;
////    public Set(HashSet<T> ts,Predicate predicate)
////    {
////        set = ts;
////    }
////}
//public class BoolMap:PDDL
//{
//    public bool val;
//    public Predicate predicate;
//    public List<PType> pTypes;
//    public BoolMap(Predicate predicate,bool val=false)
//    {
//        this.pTypes = predicate.objects;
//        this.val = val;
//        this.predicate = predicate;
//    }
//}
//public class NumMap : PDDL
//{
//    public bool val;
//    public Func func;
//    public List<PType> pTypes;
//    public NumMap(Func func, bool val = false)
//    {
//        this.pTypes = func.objects;
//        this.val = val;
//        this.func = func;
//    }
//}

/// <summary>
/// 允许自定义
/// </summary>
public class VBool
{
    public static Dictionary<string,Dictionary<string,Derived>> map=new Dictionary<string, Dictionary<string, Derived>>();
    public VBool(Derived derived,ObjType objType)
    {
        map[derived.dpre.name].Add(objType.typeName, derived);//将这个设置到里面
    }
}
public class Bool:PDDL
{
    public bool val;
    public Predicate predicate;
    /// <summary>
    /// 获取和设置对象
    /// </summary>
    public PType obj;
    public Bool(Predicate predicate,bool val=false)
    {
        this.obj = predicate.objects[0];
        this.val = val;
        this.predicate = predicate;
    }
    public override string ToString()
    {
        if (val)
            return predicate.ToString();
        else
            return Not(predicate).ToString();
    }
    public PType this[int row]
    {
        get
        {
            return predicate.objects[row];
        }
        set
        {
            predicate.objects[row] = value;
        }
    }
}
public class Num:PDDL
{
    public int val;
    public Func func;
    /// <summary>
    /// 获取和设置对象
    /// </summary>
    public PType obj;
    public Num(Func func, int val = 0)
    {
        if (func.objects.Count > 0)
        {
            obj = func.objects[0];
        }
        this.val = val;
        this.func = func;
    }
    public void Bind(Func func)
    {

    }
    public override string ToString()
    {
        return "(=" + "(" +func.ToString()+")" +" "+val+ ")";
    }
    public PType this[int row]
    {
        get
        {
            return func.objects[row];
        }
        set
        {
            func.objects[row] = value;
        }
    }
}
public class I : Pop
{
    public int x;
    public I(int x)
    {
        this.x = x;
    }

    public static implicit operator I(int x)
    {
        return new I(x);
    }
    public override string ToString()
    {
        return x.ToString();
    }
}
public class SetP<X> where X : Obj
{
    Num fx;
    public Func func;
    public HashSet<Tuple<X, int>> tuples;
    public class TupleEqual : IEqualityComparer<Tuple<X,int>>
    {
        public bool Equals(Tuple<X, int> x, Tuple<X, int> y)
        {
            return x.Item1.objSaver == y.Item1.objSaver;
        }

        public int GetHashCode(Tuple<X, int> obj)
        {
            return obj.Item1.GetHashCode();
        }
    }
    public SetP(Func func)
    {
        this.func = func;
        fx = new Num(func);
        tuples = new HashSet<Tuple<X, int>>(new TupleEqual());
    }
    public void Add(X data)
    {
        var d = new Tuple<X, int>(data, 1);
        int val = 1;
        if (tuples.TryGetValue(d, out d))
        {
            val = d.Item2 + 1;
        }
        tuples.Add(new Tuple<X, int>(data,val));
    }
    public override string ToString()
    {
        StringBuilder ret = new StringBuilder();
        //foreach(var x in tuples)
        //{
        //    fx[1] = x.Item1;
        //    fx.val = x.Item2;
        //    ret.AppendLine(fx.ToString());
        //}
        return ret.ToString();
    }
}
/// <summary>
/// IT为#t
/// </summary>
[P]
public class IT:Pop
{
    public override string ToString()
    {
        return "#t";
    }
}
[P]
public class Duration:Pop
{
    public override string ToString()
    {
        return "?duration";
    }
}
//public class Domain : PDDL
//{
//    public string domainName;
//    public List<PType> types;
//    public List<Predicate> predicates;
//    public List<Func> funcs;
//    public List<PAction> actions;
//    public List<Derived> axioms;
//    public void AddType(PType type)
//    {
//        types.Add(type);
//    }
//    public Domain()
//    {
//        domainName = this.GetType().Name;
//    }
//    public override string ToString()
//    {
//        StringBuilder str = new StringBuilder();
//        str.AppendLine("(");
//        str.AppendFormat("(domain {0})\n", domainName);

//        if (types.Count > 0)
//        {
//            str.AppendLine("(:types\n");
//            for (int i = 0; i < types.Count; i++)
//            {
//                str.AppendFormat("{0}-{1}\n", types[i].typeName, types[i].GetType().BaseType.Name);
//            }
//            str.AppendLine("\n)\n");
//        }
//        if (predicates.Count > 0)
//        {
//            str.AppendLine("(:predicates\n");

//            for (int j = 0; j < predicates.Count; j++)
//            {
//                str.Append(predicates[j].f() + " ");
//            }
//            str.AppendLine("\n)");
//        }
//        if (funcs.Count > 0)
//        {
//            str.AppendLine("(:functions\n");

//            for (int j = 0; j < funcs.Count; j++)
//            {
//                str.Append(funcs[j].ToString() + " ");
//            }
//            str.AppendLine("\n)");
//        }

//        for (int i = 0; i < actions.Count; i++)
//        {
//            str.AppendLine(actions[i].ToString());
//        }
//        for (int i = 0; i < axioms.Count; i++)
//        {
//            str.AppendLine(axioms[i].ToString());
//        }
//        return str.ToString();
//    }
//}
public abstract class Problem : PDDL
{
    public string problemName;
    public string domainName;
    public List<PType> objects;
    public List<PDDL> initVal;
    public Pop goal;
    public Problem(Person person)
    {
        problemName = GetType().Name;
        objects = new List<PType>();
        initVal = new List<PDDL>();
    }
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.AppendFormat(@"(define
        (problem {0})
        (:domain {1})
        (:objects
        ", problemName, domainName);

        for (int i = 0; i < objects.Count; i++)
        {
            str.AppendFormat("{0}-{1}\n", objects[i].objName, objects[i].typeName);
        }
        str.AppendLine(")\n");

        str.AppendLine("(:init\n");

        for(int i=0;i<initVal.Count;i++)
        {
            str.AppendLine(initVal[i].ToString());
        }
        str.AppendLine("\n)\n");
        str.AppendLine("(:goal\n");
        str.AppendLine(goal.ToString());
        str.AppendLine("\n)\n");
        str.Append(")");
        return str.ToString();
    }
}
/// <summary>
/// 当前时间
/// </summary>
[P]
public class NowT:Func
{
    public NowT():base()
    {

    }
}