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
public class PDDLClass
{
    public Dictionary<string, PDDLMapVal> mapNodes;
    public StringBuilder stringBuilder;
    /// <summary>
    /// PDDLClass
    /// </summary>
    /// <param name="obj"></param>
    public PDDLClass()
    {
        stringBuilder = new StringBuilder();
        mapNodes = new Dictionary<string, PDDLMapVal>();
    }
	public Pop GetPop(Obj obj,string name)
	{
        return mapNodes[name].pop(obj);
	}
}

public class PDDLMapVal
{
    public Func<Obj, Pop> pop;
    public Func<Obj, string> val;
    public PDDLMapVal(Func<Obj, Pop> pop, Func<Obj, string> val)
    {
        this.pop = pop;
        this.val = val;
    }
}
public class Person_PDDL:PDDLClass
{
    public PDDLMapVal isPlayer;
    public Person_PDDL():base()
    {
        stringBuilder = new StringBuilder();
        isPlayer = new PDDLMapVal(
        (obj)=> 
        {
            return new Predicate("Person_isPlayer",obj.obj);
        },
        (obj)=>
        {
            return ((Person)obj).isPlayer.ToString();
        });
        mapNodes.Add("Person_isPlayer",isPlayer);
    }
}


