using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
public class PDDL: PD
{
   public virtual string InitVal()
    {
        return null;
    }
}
public class Nm
{
    public static int x = 0;
    public static int num { get { x++; return x; } }

}
public class PType: PDDL
{
    public string typeName;//类型名
    public string objName;
    public PType(string typeName="",string objName="")
    {
        if (typeName == "")
        {
            this.typeName = this.GetType().Name;
        }
        else
            this.typeName = typeName;
        if (objName == "")
        {
            this.objName = this.GetType().Name + "_" + Nm.num;
        }
        else
            this.objName = objName;
    }
    public PType()
    {
        this.typeName = this.GetType().Name;
        this.objName = this.GetType().Name + "_" + Nm.num;
    }
    public override string ToString()
    {
        return objName;
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

public class Predicate : Pop
{
    public string name;
    public List<PType> objects;
    public Predicate(string name,params PType[] x)
    {
        this.name=name;
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
    public string f(StringBuilder sb)
    {
        sb.Append("(" + name + " ");
        foreach (var parameter in objects)
            sb.AppendFormat("?{0}-{1} ", parameter.objName, parameter.typeName);
        sb.Append(" )");
        return sb.ToString();
    }
}
public class Func : Pop
{
    public string name;
    public List<PType> objects;
    public Func(string name,params PType[] x)
    {
        this.name = name;
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
public class Less : Pop
{
    public Pop x;
    public Pop y;
    public Less(Pop x, Pop y)
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
public class LessEqual : Pop
{
    public Pop x;
    public Pop y;
    public LessEqual(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( <= ");
        sb.Append(x.ToString());
        sb = sb.Append(" ");
        sb.Append(y.ToString());
        sb.Append(") ");
        return sb.ToString();
    }
}
public class Equal : Pop
{
    public Pop x;
    public Pop y;
    public Equal(Pop x, Pop y)
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
public class Great : Pop
{
    public Pop x;
    public Pop y;
    public Great(Pop x, Pop y)
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
public class GreatEqual : Pop
{
    public Pop x;
    public Pop y;
    public GreatEqual(Pop x, Pop y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("( >= ");
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
public class Subtract : Pop
{
    public Pop x;
    public Pop y;
    public Subtract(Pop x, Pop y)
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
public class Is : Predicate
{
    public Is(PType x,PType y) : base("IsPDDL", x,y)
    {

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
    /// <summary>
    /// 条件生成所需要的必要信息
    /// </summary>
    public List<Pop> conditionEnv;
    public PAction()
    {
        objects = new List<PType>();
        condition = null;
        effect = null;
        duration = null;
        conditionEnv = new List<Pop>();
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

        //this.condition = P.And(conditionEnv.ToArray());
        if (condition != null)
        {
            str.AppendLine(":condition ");
            str.AppendLine(condition.ToString());
            str.AppendLine("\n");
        }
        //str.AppendLine(":precondition(");
        //str.AppendLine(condition.ToString());
        //str.AppendLine(")\n");
        if (effect != null)
        {
            str.AppendLine(":effect \n");
            str.AppendLine(effect.ToString());
            str.AppendLine("\n");
        }
        str.AppendLine(")\n");
        return str.ToString();
    }
    public virtual void Init(Domain domain,Problem problem)
    {
    }
    /// <summary>
    /// 注册引用对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="F"></typeparam>
    /// <param name="obj"></param>
    public void RegRefVal<T,F>(PDDLValRef<T,F> obj) where F:IPDDL
    {
        var valInf = obj.val().GetPtype();
        if (objects.Find((e)=> { return valInf == e; })==null)//不存在则将这个Type列入输入参数
		{
            objects.Add(valInf);
		}
        conditionEnv.Add(obj.pop());
    }
    /// <summary>
    /// 注册输入的参数
    /// </summary>
    /// <param name="pType"></param>
    public void RegPType(PType pType)
    {
        if (objects.Find((e) => { return pType == e; }) == null)//不存在则将这个Type列入输入参数
        {
            objects.Add(pType);
        }
    }
    public void RegPDDL(PDDLClass obj)
    {
        if (objects.Find((e) => { return obj.GetObj() == e; }) == null)//不存在则将这个Type列入输入参数
        {
            objects.Add(obj.GetObj());
        }
    }
    /// <summary>
    /// 这个一定要最后调用
    /// </summary>
    /// <param name="condition"></param>
    public void RegCondition(Pop condition)
    {
        this.condition = P.And( P.AtStart( P.And(conditionEnv.ToArray())),condition );
    }
    public void RegEffect(Pop effect)
    {
        this.effect = effect;
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


public interface PDDLProperty
{
    public Pop GetPop();
}
public class Bool:PDDL, PDDLProperty
{
    public Func<bool> val;
    public Predicate predicate;
    public Bool(Predicate predicate,bool val=false)
    {
        this.val =()=> { return val; };
        this.predicate = predicate;
    }
    public Bool(Predicate predicate, Func<bool> val)
    {
        this.val = val;
        this.predicate = predicate;
    }

    public override string ToString()
    {
        if (val())
            return predicate.ToString();
        else
            return new Not(predicate).ToString();
    }
    public override string InitVal()
    {
        if (val())
            return predicate.ToString();
        else
            return new Not(predicate).ToString();
    }
    public Pop GetPop()
	{
        return predicate;
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
public class Num:PDDL, PDDLProperty
{
    public Func<int> val;
    public Func func;
    public Num(Func func, int val = 0)
    {
        this.val = ()=> { return val; };
        this.func = func;
    }
    public Num(Func func,Func<int> val)
    {
        this.val = val;
        this.func = func;
    }
    public override string ToString()
    {
        return "(=" + "(" +func.ToString()+")" +" "+val()+ ")";
    }
    public override string InitVal()
    {
        var str = "";
        //Debug.Log(func.name);
        foreach(var x in func.objects)
        {
            str=str+x.objName+" ";
        }
        return $"({func.name} {str} {val()})";
    }
    public Pop GetPop()
	{
        return func;
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
    public float x;
    public I(int x)
    {
        this.x = x;
    }
    public I(float x)
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

public class Problem : PDDL
{
    protected class ObjComparer : IEqualityComparer<PType>
    {
        public bool Equals(PType x, PType y)
        {
            if (x == null||y==null)
                return false;
            return x.typeName == y.typeName&&x.objName==y.objName;
        }

        public int GetHashCode(PType obj)
        {
            return obj.typeName.GetHashCode();
        }
    }
    public string problemName;
    public string domainName;
    public HashSet<PType> objects;
    public List<PDDL> initVal;
    public Pop goal;
    public Problem()
    {
        problemName = GetType().Name;
        domainName = "TestDomain";
        objects = new HashSet<PType>(new ObjComparer());
        initVal = new List<PDDL>();
        initVal.AddRange(DomainProblemStatic.GetPredVals());
        initVal.AddRange(DomainProblemStatic.GetNums());
    }
    public string Print()
    {
        StringBuilder str = new StringBuilder();
        str.AppendFormat(@"(define
        (problem {0})
        (:domain {1})
        (:objects
        ", problemName, domainName);
        var enums = Map.GetData<ClassAttribute>();
        foreach (var x in enums)
        {
            if (x.IsEnum)
            {
                foreach(var e in Enum.GetValues(x))
                str.AppendFormat("{0}-{1}\n", e.ToString(), x.Name);
            }
        }
        foreach (var p in objects)
        {
            str.AppendFormat("{0}-{1}\n", p.objName, p.typeName);
        }
        str.AppendLine(")\n");

        str.AppendLine("(:init\n");
        
        ////////////////////Enum/////////////////////////
        for(int i=0;i<initVal.Count;i++)
        {
            str.AppendLine(initVal[i].InitVal());
        }
        str.AppendLine("\n)\n");
        if (goal != null)
        {
            str.AppendLine("(:goal\n");
            str.AppendLine(goal.ToString());
            str.AppendLine("\n)\n");
        }
        str.Append(")");
        return str.ToString();
    }
    public void GetObjs(List<PType> pTypes)
    {
        if (pTypes != null)
        {
            foreach (var p in pTypes)
            {
                if (!objects.Contains(p))
                {
                    objects.Add(p);
                }
            }
        }
    }
    public void GetObj(PType pType)
    {
        if(!objects.Contains(pType))
        {
            objects.Add(pType);
        }
    }
}
/// <summary>
/// 当前时间
/// </summary>
[P]
public class NowT:Func
{
    public NowT():base("NowTime")
    {

    }
}
/// <summary>
/// 是否是世界状态，世界状态的更新
/// </summary>
public class WorldState : Predicate
{
    public WorldState() : base("WorldState")
    { 
    }
}

public class HashType<T> : PType
{
    public HashType() : base("Hash_" + typeof(T).Name)
    {

    }
}

public class Hash_PDDL<T>:PDDLClass<Hash<T>,HashType<T>>
where T:IPDDL,new()
{
    public Hash_PDDL()
    {
        ptype = new HashType<T>();
    }
}
[Class(false)]
public class Hash<T> : HashSet<T>, IPDDL
where T : IPDDL
{
    public PDDLClass pddl;
    public Hash():base()
    {

    }
    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }

    public PType GetPtype()
    {
        return pddl.GetObj();
    }

    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType(),typeof(Hash_PDDL<T>));
        pddl.SetObj(pddl);
    }
}

public class DicType<T, F> : PType
{
    public DicType() : base("Dic_" + typeof(T).Name + "_" + typeof(F).Name)
    {

    }
}

public class Dic_PDDL<T, F> : PDDLClass<Dic<T,F>,DicType<T,F>>
where T : IPDDL
where F : IPDDL
{
    /// <summary>
    /// 调用判断是否在里面
    /// </summary>
    /// <returns></returns>
    public Predicate In(PType pType1,PType pType2)
    {
        return new Predicate("Dic_" + typeof(T).Name + "_" + typeof(F).Name + "_IN", GetObj(),pType1,pType2);
    }
    public Dic_PDDL()
    {
        ptype= new DicType<T, F>();
	}

    public override List<Func> GetFuncs()
    {
        return null;
    }

    public override List<Num> GetFuncsVal()
    {
        return null;
    }

    public override PType GetObj()
    {
        return ptype;
    }

    public override List<PAction> GetPActions()
    {
        return new List<PAction>();
    }

    public override List<Predicate> GetPreds()
    {
        List<Predicate> ret = new List<Predicate>() {
            new Predicate("Dic_"+typeof(T).Name+"_"+typeof(F).Name+"_IN", GetObj(),PDDLClassGet.GetPType(typeof(T)),PDDLClassGet.GetPType(typeof(F)))
        };
        return ret;
    }

    public override List<Bool> GetPredsVal()
    {
        var ret =new List<Bool>();
        foreach (var x in obj)
        {
            ret.Add(new Bool(In(x.Key.GetPtype(),x.Value.GetPtype()),true));
        }
        return ret;
    }

    public override void SetObj(object obj)
    {
        this.obj = (Dic<T, F>)(obj);
    }
}
[Class(false)]
public class Dic<T, F> : Dictionary<T, F>, IPDDL
where T : IPDDL
where F : IPDDL
{
    public PDDLClass pddl;
    public PType GetPtype()
    {
        return pddl.GetObj();
    }

    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType(), typeof(Dic_PDDL<T,F>));
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }

    public Dic() : base()
    {
    }
}
//带有Int的Dic
public class DicIntType<T> : PType
{
    public DicIntType() : base("Dic_" + typeof(T).Name)
    {

    }
}

public class DicInt_PDDL<T> : PDDLClass<DicInt<T>, DicIntType<T>>
where T : IPDDL, new()
{
    /// <summary>
    /// 调用判断是否在里面
    /// </summary>
    /// <returns></returns>
    public Predicate In(PType pType1)
    {
        return new Predicate("Dic_" + typeof(T).Name + "_IN", GetObj(), pType1);
    }
    /// <summary>
    /// 调用判断是否在里面
    /// </summary>
    /// <returns></returns>
    public Func Num(PType pType1)
    {
        return new Func("Dic_" + typeof(T).Name + "_IN_NUM", GetObj(), pType1);
    }
    public DicInt_PDDL()
    {
        ptype = new DicIntType<T>();
    }

    public override List<Func> GetFuncs()
    {
        var temp1 = new T();
        List<Func> ret = new List<Func>() {
            Num(temp1.GetPtype())
        };
        return ret;
    }

    public override List<Num> GetFuncsVal()
    {
        var ret = new List<Num>();
        foreach (var x in obj)
        {
            ret.Add(new Num(Num(x.Key.GetPtype()), x.Value));
        }
        return ret;
    }

    public override PType GetObj()
    {
        return ptype;
    }

    public override List<PAction> GetPActions()
    {
        return new List<PAction>();
    }

    public override List<Predicate> GetPreds()
    {
        var temp1 = new T();
        List<Predicate> ret = new List<Predicate>() {
            new Predicate("Dic_"+typeof(T).Name+"_IN", GetObj(),temp1.GetPtype())
        };
        return ret;
    }

    public override List<Bool> GetPredsVal()
    {
        var ret = new List<Bool>();
        foreach (var x in obj)
        {
            ret.Add(new Bool(In(x.Key.GetPtype()), true));
        }
        return ret;
    }

    public override void SetObj(object obj)
    {
        this.obj = (DicInt<T>)(obj);
    }
}
[Class(false)]
public class DicInt<T> : Dictionary<T,int>, IPDDL
where T : IPDDL,new()
{
    public PDDLClass pddl;
    public PType GetPtype()
    {
        return pddl.GetObj();
    }

    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType(),typeof(DicInt_PDDL<T>));
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }

    public DicInt() : base()
    {
    }
}

public class DicBoolType<T> : PType
{
    public DicBoolType() : base("Dic_" + typeof(T).Name)
    {

    }
}

public class DicBool_PDDL<T> : PDDLClass<DicInt<T>, DicIntType<T>>
where T : IPDDL, new()
{
    /// <summary>
    /// 调用判断是否在里面
    /// </summary>
    /// <returns></returns>
    public Predicate In(PType pType1)
    {
        return new Predicate("Dic_" + typeof(T).Name + "_IN", GetObj(), pType1);
    }
    /// <summary>
    /// 调用判断是否在里面
    /// </summary>
    /// <returns></returns>
    public Predicate Num(PType pType1)
    {
        return new Predicate("Dic_" + typeof(T).Name + "_IN_BOOL", GetObj(), pType1);
    }
    public DicBool_PDDL()
    {
        ptype = new DicIntType<T>();
    }

    public override List<Func> GetFuncs()
    {
        return null;
    }

    public override List<Num> GetFuncsVal()
    {
        //var ret = new List<Num>();
        //foreach (var x in obj)
        //{
        //    ret.Add(new Num(Num(x.Key.GetPtype()), x.Value));
        //}
        return null;
    }

    public override PType GetObj()
    {
        return ptype;
    }

    public override List<PAction> GetPActions()
    {
        return new List<PAction>();
    }

    public override List<Predicate> GetPreds()
    {
        var temp1 = new T();
        List<Predicate> ret = new List<Predicate>() {
            new Predicate("Dic_"+typeof(T).Name+"_IN", GetObj(),temp1.GetPtype())
        };
        return ret;
    }

    public override List<Bool> GetPredsVal()
    {
        var ret = new List<Bool>();
        foreach (var x in obj)
        {
            ret.Add(new Bool(In(x.Key.GetPtype()), true));
            ret.Add(new Bool(Num(x.Key.GetPtype()),true));
        }
        return ret;
    }

    public override void SetObj(object obj)
    {
        this.obj = (DicInt<T>)(obj);
    }
}
[Class(false)]
public class DicBool<T> : Dictionary<T, bool>, IPDDL
where T : IPDDL,new()
{
    public PDDLClass pddl;
    public PType GetPtype()
    {
        return pddl.GetObj();
    }
    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType(), typeof(DicBool_PDDL<T>));
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }

    public DicBool() : base()
    {
    }
}