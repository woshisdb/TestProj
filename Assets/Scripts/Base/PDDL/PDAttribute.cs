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
	void Refresh()
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
/// PDDL����
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