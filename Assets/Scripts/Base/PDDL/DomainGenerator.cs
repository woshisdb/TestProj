using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// ����һ������
/// </summary>
public class ProblemGenerator
{
    public StringBuilder stringBuilder;
    public Person person;
    public ProblemGenerator(Person person)
    {
        this.person = person;
    }
    /// <summary>
    /// ��������Ҫ������
    /// </summary>
    public Pop Needs()
    {
        var metric = PDDL.Min(person.foodState.retSatifP(), person.sleepState.retSatifP());//Ŀ���Ż�
        metric = PDDL.Min(metric,person.safetyState.retSatifP());
        metric = PDDL.Min(metric,person.hungryState.retSatifP());
        return metric;
    }

    public string GenerateProblem()
    {
        //[0����ǳ�ʹ�࣬1�������·���]
        var sleepval = person.sleepState.retSatif(); //˯��
        var foodval= person.foodState.retSatif();//ʳ��
        var metric = Needs();
        stringBuilder =new StringBuilder();
        stringBuilder.AppendFormat("(define (problem {0})\n","Problem"+person.name);//��������
        stringBuilder.AppendLine("(:domain robot-domain)\n");//������
        //���ɶ���
        stringBuilder.AppendLine("(:objects\n");
        foreach(var p in GameArchitect.get.tableAsset.tableSaver.tables)//����Table��Լ��
        {
            stringBuilder.AppendFormat("{0} - {1}\n",p.sceneType.objName,p.sceneType.typeName);
        }
        foreach(var x in GameArchitect.get.tableAsset.tableSaver.objs)//��ʼ��һϵ�еĶ���
        {
            //stringBuilder.AppendFormat("{0} - {1}\n",x.obj.objName,x.obj.typeName);
        }
        stringBuilder.AppendLine("\n)");
        //��ʼ��
        stringBuilder.AppendLine("(:init\n");
        //���Time
        stringBuilder.AppendLine(GameArchitect.get.GetModel<TimeModel>().Time.func.ToString());
        foreach (var x in GameArchitect.get.tableAsset.tableSaver.objs)//��ʼ��һϵ�еĶ���
        {
            stringBuilder.AppendFormat(x.GetString().ToString());
        }
        stringBuilder.AppendLine("\n)");
        //Ŀ���������ڸ���ʱ��������
        stringBuilder.AppendLine("(:goal");
        Pop goal=null;//= PDDL.G((I)(GameArchitect.get.GetModel<TimeModel>().Time.val+TimeModel.GetStep(1)), GameArchitect.get.GetModel<TimeModel>().Time);
        stringBuilder.AppendLine(goal.ToString());
        stringBuilder.AppendLine(")");
        //***************Ŀ����С��ʹ��
        stringBuilder.Append("(:metric\n");
        stringBuilder.Append(metric.ToString());
        stringBuilder.AppendLine(")");
        stringBuilder.AppendLine(")");
        return stringBuilder.ToString();
    }
}

/// <summary>
/// ����һ������
/// </summary>
public class DomainGenerator
{
    public Person person;
    public static StringBuilder str;
    public static string domainText;
    public static  List<PType> pTypes;
    public static string filePath;
    public DomainGenerator(Person person)
    {
        this.person = person;
    }
    /// <summary>
    /// ���������Ϣ
    /// </summary>
    public static void GenerateDomain()
    {
        filePath = "Assets/Resources/SaveData" + "/DomainData.txt";
        str = new StringBuilder();
        Assembly assembly = Assembly.GetExecutingAssembly();
        List<Type> ptypeSubclassNames = GetSubclassesOf(typeof(PType), assembly);
        pTypes = new List<PType>();
        for(int i=0;i<ptypeSubclassNames.Count;i++)
        {
            var data=ptypeSubclassNames[i];
            var tp=(PType)Activator.CreateInstance(data,new object[] { null});
            pTypes.Add(tp);
        }
        str.AppendLine("(define (domain robot-domain)");
        str.AppendLine("(:requirements :typing :fluents :durative-actions :numeric-fluents :numeric-effects)");
        //����һϵ�е�class
        str.AppendLine("(:types");
        for (int i=0;i<pTypes.Count;i++)
        {
            str.AppendLine(pTypes[i].ObjName());
        }
        str.AppendLine(")");
        //����predicated
        str.AppendLine("(:predicates");
        List<Type> predicateTypes = GetSubclassesOf(typeof(Predicate), assembly);
        foreach(Type t in predicateTypes)
        {
            if(t!=typeof(Predicate)&&t!=typeof(CustomPredicate))
            str.AppendLine(Predicate.f(t));
        }
        str.AppendLine(")");
        //����predicated
        str.AppendLine("(:functions");
        List<Type> functionTypes = GetSubclassesOf(typeof(Func), assembly);
        foreach (Type t in functionTypes)
        {
            if (t != typeof(Func) && t != typeof(CustomFunc))
                str.AppendLine(Func.f(t));
        }
        str.AppendLine(")");
        //����һϵ�еĻ
        foreach (var x in GameArchitect.activities)
        {
            var acts=x.Value;
            foreach(var act in acts)
            {
                str.AppendLine(act.GetAction().ToString());//����ַ���
            }
        }
        str.AppendLine(")");
        domainText = str.ToString();
        Debug.Log(domainText);
        File.WriteAllText(filePath,domainText);
    }
    static List<Type> GetSubclassesOf(Type baseType, Assembly assembly)
    {
        List<Type> result = new List<Type>();
        HashSet<Type> visited = new HashSet<Type>();

        void AddSubclasses(Type parentType)
        {
            foreach (Type type in assembly.GetTypes().Where(t => t.IsSubclassOf(parentType)))
            {
                if (!visited.Contains(type))
                {
                    visited.Add(type);
                    result.Add(type);
                    AddSubclasses(type); // �ݹ�������������
                }
            }
        }

        // ����ӻ�������
        result.Add(baseType);
        visited.Add(baseType);

        // ������������
        AddSubclasses(baseType);

        return result;
    }
}

public class PathGenerator
{
    Person person;
    public PathGenerator(Person person)
    {
        this.person = person;
    }
    public Task GetPath(DomainGenerator domainGenerator,ProblemGenerator problemGenerator)
    {
        return Task.CompletedTask;
    }
}
