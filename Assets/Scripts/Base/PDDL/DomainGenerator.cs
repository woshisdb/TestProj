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
/// 生成一个问题
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
    /// 生成最需要的心情
    /// </summary>
    public Pop Needs()
    {
        var metric = PDDL.Min(person.foodState.retSatifP(), person.sleepState.retSatifP());//目标优化
        metric = PDDL.Min(metric,person.safetyState.retSatifP());
        metric = PDDL.Min(metric,person.hungryState.retSatifP());
        return metric;
    }

    public string GenerateProblem()
    {
        //[0代表非常痛苦，1代表无事发生]
        var sleepval = person.sleepState.retSatif(); //睡眠
        var foodval= person.foodState.retSatif();//食物
        var metric = Needs();
        stringBuilder =new StringBuilder();
        stringBuilder.AppendFormat("(define (problem {0})\n","Problem"+person.name);//问题名字
        stringBuilder.AppendLine("(:domain robot-domain)\n");//域名字
        //生成对象
        stringBuilder.AppendLine("(:objects\n");
        foreach(var p in GameArchitect.get.tableAsset.tableSaver.tables)//生成Table的约束
        {
            stringBuilder.AppendFormat("{0} - {1}\n",p.sceneType.objName,p.sceneType.typeName);
        }
        foreach(var x in GameArchitect.get.tableAsset.tableSaver.objs)//初始化一系列的对象
        {
            //stringBuilder.AppendFormat("{0} - {1}\n",x.obj.objName,x.obj.typeName);
        }
        stringBuilder.AppendLine("\n)");
        //初始化
        stringBuilder.AppendLine("(:init\n");
        //添加Time
        stringBuilder.AppendLine(GameArchitect.get.GetModel<TimeModel>().Time.func.ToString());
        foreach (var x in GameArchitect.get.tableAsset.tableSaver.objs)//初始化一系列的对象
        {
            stringBuilder.AppendFormat(x.GetString().ToString());
        }
        stringBuilder.AppendLine("\n)");
        //目标是增加在给定时间做决策
        stringBuilder.AppendLine("(:goal");
        Pop goal=null;//= PDDL.G((I)(GameArchitect.get.GetModel<TimeModel>().Time.val+TimeModel.GetStep(1)), GameArchitect.get.GetModel<TimeModel>().Time);
        stringBuilder.AppendLine(goal.ToString());
        stringBuilder.AppendLine(")");
        //***************目标最小化痛苦
        stringBuilder.Append("(:metric\n");
        stringBuilder.Append(metric.ToString());
        stringBuilder.AppendLine(")");
        stringBuilder.AppendLine(")");
        return stringBuilder.ToString();
    }
}

/// <summary>
/// 生成一个领域
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
    /// 生成域的信息
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
        //定义一系列的class
        str.AppendLine("(:types");
        for (int i=0;i<pTypes.Count;i++)
        {
            str.AppendLine(pTypes[i].ObjName());
        }
        str.AppendLine(")");
        //定义predicated
        str.AppendLine("(:predicates");
        List<Type> predicateTypes = GetSubclassesOf(typeof(Predicate), assembly);
        foreach(Type t in predicateTypes)
        {
            if(t!=typeof(Predicate)&&t!=typeof(CustomPredicate))
            str.AppendLine(Predicate.f(t));
        }
        str.AppendLine(")");
        //定义predicated
        str.AppendLine("(:functions");
        List<Type> functionTypes = GetSubclassesOf(typeof(Func), assembly);
        foreach (Type t in functionTypes)
        {
            if (t != typeof(Func) && t != typeof(CustomFunc))
                str.AppendLine(Func.f(t));
        }
        str.AppendLine(")");
        //定义一系列的活动
        foreach (var x in GameArchitect.activities)
        {
            var acts=x.Value;
            foreach(var act in acts)
            {
                str.AppendLine(act.GetAction().ToString());//添加字符串
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
                    AddSubclasses(type); // 递归查找子类的子类
                }
            }
        }

        // 先添加基类自身
        result.Add(baseType);
        visited.Add(baseType);

        // 查找所有子类
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
