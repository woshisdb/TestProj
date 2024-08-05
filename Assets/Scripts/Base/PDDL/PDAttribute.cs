using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
/// <summary>
/// PDDL�Ķ������
/// </summary>
public class PropertyAttribute : Attribute
{

}
/// <summary>
/// PDDL�Ķ���
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ClassAttribute : Attribute
{

}

public class PDDLClassGenerater
{
	public static void Refresh()
	{
        // ��ȡ��ǰ����
        Assembly assembly = Assembly.GetExecutingAssembly();
        // ��ȡ��������
        Type[] types = assembly.GetTypes();

        // �������б� ClassAttribute �������ε���
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
/// <summary>
/// PDDL����
/// </summary>
public class PDDLClass<T>
{
    public T obj;
    public StringBuilder stringBuilder;
    /// <summary>
    /// PDDLClass
    /// </summary>
    /// <param name="obj"></param>
    public PDDLClass(T obj)
    {
        this.obj = obj;
        stringBuilder = new StringBuilder();
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
}

public class Person_PDDL:PDDLClass<Person>
{
    public Bool isPlayer;
    public Num money;
    public Person_PDDL(Person obj):base(obj)
    {
        this.obj = obj;
        stringBuilder = new StringBuilder();
        isPlayer = new Bool(new Predicate("Person_isPlayer",obj.obj),()=> { return obj.isPlayer; });
        money = new Num(new Func("Person_Money", obj.obj), () => { return obj.money; });
    }
    public override string GetProblem(Person obj)
    {
        stringBuilder.Clear();

        return stringBuilder.ToString();
    }
}