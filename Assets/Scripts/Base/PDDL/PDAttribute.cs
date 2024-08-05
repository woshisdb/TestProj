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
	void Refresh()
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
            foreach (var field in type.GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(PropertyAttribute), false);
                if (attributes.Any())
                {

                }
            }
        }
    }
}
/// <summary>
/// PDDL基类
/// </summary>
public class PDDLClass
{
    public Obj obj;
    public StringBuilder stringBuilder;
    public List<Func<Obj,string>> fields;
    /// <summary>
    /// PDDLClass
    /// </summary>
    /// <param name="obj"></param>
    public PDDLClass(Obj obj)
    {
        this.obj = obj;
        stringBuilder = new StringBuilder();
    }
    public virtual string GetProblem(Obj obj)
    {
        stringBuilder.Clear();
        return null;
    }
}