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
[AttributeUsage(AttributeTargets.Class)]
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

public class PDDLClassGenerater
{
	public static void Refresh()
	{
        // 获取当前程序集
        Assembly assembly = Assembly.GetExecutingAssembly();
        // 获取所有类型
        Type[] types = assembly.GetTypes();

        // 查找所有被 ClassAttribute 特性修饰的类
        var classesWithAttribute = types
            .Where(t => t.GetCustomAttributes(typeof(ClassAttribute), false).Any())
            .ToList();
        foreach(var type in classesWithAttribute)
        {
            GenerateType(type);
        }
    }
    public static void GenerateType(Type type)
    {
        foreach (var field in type.GetFields())
        {
            var attributes = field.GetCustomAttributes(typeof(PropertyAttribute), false);
            if (attributes.Any())
            {
                Debug.Log(type.Name + ":" + field.Name);
                if(field.FieldType==typeof(bool))
                {

                }
                else if (field.FieldType == typeof(int))
                {

                }
                else if(field.FieldType == typeof(int))
                {

                }
            }
        }
    }
}
public abstract class PDDLClass
{
    public abstract string PDDLDomain();
    public abstract List<Predicate> GetPreds();
    public abstract List<Func> GetFuncs();
}
/// <summary>
/// PDDL基类
/// </summary>
public class PDDLClass<T,F>:PDDLClass 
where T : Obj
where F : PType
{
    public Dictionary<string, PDDLVal> mapNodes;
    public StringBuilder stringBuilder;
    public F pType;
    public T obj;
    /// <summary>
    /// PDDLClass
    /// </summary>
    /// <param name="obj"></param>
    public PDDLClass()
    {
        stringBuilder = new StringBuilder();
        mapNodes = new Dictionary<string, PDDLVal>();
    }
	public Pop GetPop(Obj obj,string name)
	{
        return mapNodes[name].pop();
	}

    public override List<Predicate> GetPreds()
    {
        return null;
    }
    public override List<Func> GetFuncs()
    {
        return null;
    }
    public override string PDDLDomain()
    {
        return null;
    }

    public void SetObj(T obj)
    {
        this.obj = obj;
    }
}
public interface PDVal
{
    public Pop GetPop();
    public string GetVal();
}
public class PDDLVal:PDVal
{
    public Func<Pop> pop;
    public Func<string> val;
    public PDDLVal(Func<Pop> pop, Func<string> val)
    {
        this.pop = pop;
        this.val = val;
    }

	public Pop GetPop()
	{
        return pop();
	}

    public string GetVal()
	{
        return val();
	}
}

public class NumType:PType
{
    
}
public class PDDLMap
{
    
}
/// <summary>
/// 处理集合对象
/// </summary>
public class PDDLObjs
{
    public PType type;
    public List<Func> funcs;
}

public class Person_PDDL:PDDLClass<Person,PersonType>
{
    public PDDLVal isPlayer;
    public PDDLVal money;
    public Person_PDDL(PersonType pType):base()
    {
        this.pType = pType;
        stringBuilder = new StringBuilder();
        isPlayer = new PDDLVal(
        ()=> 
        {
            return new Predicate("Person_isPlayer",pType);
        },
        ()=>
        {
            return ((Person)obj).isPlayer.ToString();
        });
        mapNodes.Add("Person_isPlayer",isPlayer);
        money = new PDDLVal(
        () =>
        {
            return new Func("Person_money", pType);
        },
        () =>
        {
            return obj.money.ToString();
        });
    }
    public override List<Predicate> GetPreds()
    {
        return new List<Predicate>() {
            (Predicate)isPlayer.pop(),
        };
    }
    public override List<Func> GetFuncs()
    {
        return new List<Func>(){
            (Func)money.pop(),
        };
    }
}


