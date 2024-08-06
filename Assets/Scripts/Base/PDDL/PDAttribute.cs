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
public interface PDDLClass
{
    public abstract Pop GetPop(string name);
}
/// <summary>
/// PDDL基类
/// </summary>
public class PDDLClass<T>:PDDLClass
{
    public Dictionary<string, PDDLProperty> mapNodes;
    public StringBuilder stringBuilder;
    /// <summary>
    /// PDDLClass
    /// </summary>
    /// <param name="obj"></param>
    public PDDLClass()
    {
        stringBuilder = new StringBuilder();
        mapNodes = new Dictionary<string, PDDLProperty>();
    }
    public virtual string GetProblem(T obj)
    {
        stringBuilder.Clear();
        return null;
    }
    public virtual string GetDomain(T obj)
    {
        stringBuilder.Clear();
        return null;
    }

	public Pop GetPop(string name)
	{
        return mapNodes[name].GetPop();
	}
}

public class PDDLMapVal<T>
{
    public Func<T, Pop> pop;
    public Func<T, string> val;
    public PDDLMapVal(Func<T, Pop> pop, Func<T, string> val)
    {
        this.pop = pop;
        this.val = val;
    }
}
public class Person_PDDL:PDDLClass<Person>
{
    public PDDLMapVal<Person> isPlayer;
    public PDDLMapVal<Person> money;
    public Person_PDDL():base()
    {
        stringBuilder = new StringBuilder();
        isPlayer = new PDDLMapVal<Person>(
        (obj)=> 
        {
            return new Predicate("Person_isPlayer");
        },
        (obj)=>
        {
            return obj.isPlayer.ToString();
        });
    }
    public override string GetProblem(Person obj)
    {
        stringBuilder.Clear();
        return stringBuilder.ToString();
    }
}


