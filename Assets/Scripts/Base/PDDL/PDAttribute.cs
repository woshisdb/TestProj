using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
/// <summary>
/// PDDL的对象参数
/// </summary>
public class PropertyAttribute : Attribute
{

}
/// <summary>
/// PDDL的对象
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
public class ClassAttribute : Attribute
{
    
}
public static class P
{
    public static And And(params Pop[] numbers)
    {
        return new And(numbers);
    }

    public static Or Or(params Pop[] numbers)
    {
        return new Or(numbers);
    }

    public static To To( Pop x, Pop y)
    {
        return new To(x, y);
    }

    public static When When( Pop x, Pop y)
    {
        return new When(x, y);
    }
    public static Bool Belong(PType x,PType y)
    {
        return new Bool( new Belong(x, y),true);
    }
    public static Less Less( Pop x, Pop y)
    {
        return new Less(x, y);
    }

    public static LessEqual LessEqual( Pop x, Pop y)
    {
        return new LessEqual(x, y);
    }

    public static Equal Equal( Pop x, Pop y)
    {
        return new Equal(x, y);
    }

    public static Great Great( Pop x, Pop y)
    {
        return new Great(x, y);
    }

    public static Abs Abs( Pop x)
    {
        return new Abs(x);
    }
    public static Add Add(Pop x, Pop y)
    {
        return new Add(x, y);
    }
    public static Increase Increase(Pop x, Pop y)
    {
        return new Increase(x, y);
    }
    public static Max Max(Pop x, Pop y)
    {
        return new Max(x, y);
    }
    public static Min Min(Pop x, Pop y)
    {
        return new Min(x, y);
    }
    public static Subtract Subtract(Pop x, Pop y)
    {
        return new Subtract(x, y);
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
    public static Not Not(Pop x)
    {
        return new Not(x);
    }
    public static OverALL OverALL(Pop x)
    {
        return new OverALL(x);
    }
    public static AtEnd AtEnd(Pop x)
    {
        return new AtEnd(x);
    }
    public static AtStart AtStart(Pop x)
    {
        return new AtStart(x);
    }
    public static ForAll ForAll(Pop when, Pop express, params PType[] numbers)
    {
        return new ForAll(when,express,numbers);
    }
    public static Exist Exist(Pop express, params PType[] numbers)
    {
        return new Exist(express,numbers);
    }
}

public class CNode
{
    public Type type;
    public class Node
    {
        public string TypeName;
        public string prex;
        public string clasx;
    }
    public List<Node> bools;
    public List<Node> ints;
    public List<Node> dics;
    public List<Node> enums;
    public List<Node> custs;
    public CNode()
    {
        this.bools = new List<Node>();
        this.ints = new List<Node>();
        dics = new List<Node>();
        enums = new List<Node>();
        custs = new List<Node>();
    }
}

public class PDDLClassGenerater
{
    public static List<CNode> Refresh()
	{
        // 获取当前程序集
        Assembly assembly = Assembly.GetExecutingAssembly();
        // 获取所有类型
        Type[] types = assembly.GetTypes();

        // 查找所有被 ClassAttribute 特性修饰的类
        var classesWithAttribute = types
            .Where(t => t.GetCustomAttributes(typeof(ClassAttribute), false).Any())
            .ToList();
        List<CNode> cNodes =new List<CNode>();
        foreach (var type in classesWithAttribute)
        {
            if (type.IsEnum == false)
            {
                var t = new CNode();
                t.type = type;
                GenerateType(type, t);
                cNodes.Add(t);
            }
        }
        return cNodes;
    }
    public static void GenerateType(Type type,CNode cNode)
    {
        foreach (var field in type.GetFields())
        {
            var attributes = field.GetCustomAttributes(typeof(PropertyAttribute), false);
            if (attributes.Any())
            {
                if (field.FieldType == typeof(bool))
                {
                    var node = new CNode.Node();
                    node.TypeName = "PDDLVal";
                    node.prex = field.Name;
                    node.clasx = field.Name;
                    cNode.bools.Add(node);
                }
                else if (field.FieldType == typeof(int))
                {
                    var node = new CNode.Node();
                    node.TypeName = "PDDLVal";
                    node.prex = field.Name;
                    node.clasx = field.Name;
                    cNode.ints.Add(node);
                }
                else if (field.FieldType is IDictionary)
                {
                    var node=new CNode.Node();
                    Type[] genericArguments = field.FieldType.GetGenericArguments();
                    Type keyType = genericArguments[0];
                    Type valueType = genericArguments[1];
                    node.TypeName = $"Dictionary_PDDL<{keyType.Name},{valueType.Name}>";
                    node.prex = field.Name;
                    node.clasx = field.Name;
                    cNode.dics.Add(node);
                }
                else if(field.FieldType.IsEnum)
                {
                    var node = new CNode.Node();
                    node.TypeName = $"Enum_PDDL<{field.FieldType.Name}>";
                    node.prex = field.Name;
                    node.clasx = field.Name;
                    cNode.enums.Add(node);
                }
                else if(field.FieldType.GetCustomAttributes(typeof(ClassAttribute), false).Any())//如果是个已经生成的类
                {
                    var tf= field.FieldType+"_PDDL";
                    var node = new CNode.Node();
                    node.TypeName = tf;
                    node.prex = field.Name;
                    node.clasx = field.Name;
                    cNode.custs.Add(node);
                }
            }
        }
    }
    //public static void FieldGen(string prexPath,string classPath,Type type,CNode cNode)
    //{
    //    foreach (var field in type.GetFields())
    //    {
    //        var attributes = field.GetCustomAttributes(typeof(PropertyAttribute), false);
    //        if (attributes.Any())
    //        {
    //            Debug.Log(type.Name + ":" + field.Name);
    //            if (field.FieldType == typeof(bool))
    //            {
    //                var node=new CNode.Node();
    //                node.prex=prexPath+field.Name;
    //                node.clasx = classPath+ field.Name;
    //                cNode.bools.Add(node);
    //            }
    //            else if (field.FieldType == typeof(int))
    //            {
    //                var node = new CNode.Node();
    //                node.prex = prexPath+ field.Name;
    //                node.clasx = classPath+ field.Name;
    //                cNode.ints.Add(node);
    //            }
    //            else
    //            {
    //                FieldGen(prexPath+ field.Name+"_", classPath+ field.Name+".",field.FieldType,cNode);
    //            }
    //        }
    //    }
    //}
}
/// <summary>
/// PDDL对象应该能够获取相应的函数和Predx
/// </summary>
public abstract class PDDLClass
{
    public virtual void SetDomain(Domain domain)
    {
        domain.funcs.AddRange(GetFuncs());
        domain.predicates.AddRange(GetPreds());
    }
    public virtual void SetProblem(Problem problem)
    {
        problem.objects.AddRange(GetObjs());//添加自己
        problem.initVal.AddRange(GetPredsVal());
        problem.initVal.AddRange(GetFuncsVal());
    }
    public abstract List<Predicate> GetPreds();
    public abstract List<Func> GetFuncs();
    public abstract List<Pop> GetPredsVal();
    public abstract List<Pop> GetFuncsVal();
    public abstract PType GetPType();
    public virtual List<PType> GetObjs()
    {
        var data= new List<PType>();
        data.Add(GetObj());
        return data;
    }
    public abstract void SetObj(object obj);
    public abstract PType GetObj();
    public abstract List<PAction> GetPActions();
}
/// <summary>
/// PDDL基类
/// </summary>
public class PDDLClass<T,F>:PDDLClass 
where T : IPDDL
where F : PType, new()
{
    public T obj;
    /// <summary>
    /// PDDLClass
    /// </summary>
    /// <param name="obj"></param>
    public PDDLClass()
    {
    }

    public override List<Predicate> GetPreds()
    {
        return null;
    }
    public override List<Func> GetFuncs()
    {
        return null;
    }

    public override void SetObj(object obj)
    {
        this.obj = (T)obj;
    }
    /// <summary>
    /// 初始化Domain
    /// </summary>
    /// <returns></returns>
    public override PType GetPType()
    {
        return new F();
    }

    public override List<Pop> GetPredsVal()
    {
        return null;
    }

    public override List<Pop> GetFuncsVal()
    {
        return null;
    }

    public override List<PAction> GetPActions()
    {
        return null;
        //var acs= obj.InitActivities();
        //var ret = new List<PAction>();
        //foreach (var act in acs)
        //{
        //    ret.Add(act.GetAction());
        //}
        //return ret;
    }

    public override PType GetObj()
    {
        return obj.GetPtype();
    }
}

public class Dic<T, F> : Dictionary<T, F>, IPDDL
{
    public PType obj;
    public PType GetPtype()
    {
        return obj;
    }
    public Dic()
    {
        this.obj = new DicType<T,F>();
    }
}

public interface PDVal
{
    public Pop GetPop();
    public Pop GetVal();
}
public class PDDLVal:PDVal
{
    public Func<Pop> pop;
    public Func<Pop> val;
    public PDDLVal(Func<Pop> pop, Func<Pop> val)
    {
        this.pop = pop;
        this.val = val;
    }

	public Pop GetPop()
	{
        return pop();
	}
    public Pop GetVal()
    {
        return val();
    }
}

public class NumType:PType
{
    
}


