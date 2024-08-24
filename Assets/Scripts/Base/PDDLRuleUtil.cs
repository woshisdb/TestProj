using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using System.Reflection;

/// <summary>
/// PDDL的Rule规则
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RuleAttribute : Attribute
{

}
public delegate void QMethodDelegate();
public static class PDDLRuleUtil
{
    public static List<QMethodDelegate> GetQMarkedMethods()
    {
        // 获取当前程序集中的所有类型
        var types = Assembly.GetExecutingAssembly().GetTypes();

        // 创建一个列表来保存被 RuleAttribute 修饰的方法的委托
        var delegateList = new List<QMethodDelegate>();

        // 字典来存储虚方法及其重写的方法
        var baseMethodDict = new Dictionary<MethodInfo, List<MethodInfo>>();

        // 遍历所有类型
        foreach (var type in types)
        {
            // 获取该类型的所有公共实例方法
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                // 检查方法是否被 RuleAttribute 修饰
                if (method.GetCustomAttributes(typeof(RuleAttribute), false).Length > 0)
                {
                    // 添加虚方法到字典
                    if (method.IsVirtual)
                    {
                        if (!baseMethodDict.ContainsKey(method))
                        {
                            baseMethodDict[method] = new List<MethodInfo>();
                        }
                        baseMethodDict[method].Add(method);
                    }
                    else
                    {
                        // 创建类型的实例
                        var instance = Activator.CreateInstance(type);

                        // 创建委托并添加到列表
                        QMethodDelegate del = (QMethodDelegate)Delegate.CreateDelegate(typeof(QMethodDelegate), instance, method);
                        delegateList.Add(del);
                    }
                }
            }
        }

        // 查找重写的方法
        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                if (method.GetBaseDefinition() != method)
                {
                    var baseMethod = method.GetBaseDefinition();
                    if (baseMethodDict.ContainsKey(baseMethod))
                    {
                        baseMethodDict[baseMethod].Add(method);
                    }
                }
            }
        }

        // 创建委托并添加到列表
        foreach (var baseMethod in baseMethodDict.Keys)
        {
            foreach (var method in baseMethodDict[baseMethod])
            {
                var instance = Activator.CreateInstance(method.DeclaringType);
                QMethodDelegate del = (QMethodDelegate)Delegate.CreateDelegate(typeof(QMethodDelegate), instance, method);
                delegateList.Add(del);
            }
        }

        return delegateList;
    }
}
///// <summary>
///// 原料类型与数量
///// </summary>
//public class Nj
//{
//    public RawType rawObj;
//    public int num;
//    public Nj(RawType rawObj,int num)
//    {
//        this.rawObj = rawObj;
//        this.num = num;
//    }
//}

///// <summary>
///// 总结各个原材料间的生成关系
///// </summary>
//public class GeneratorNode
//{
//    /// <summary>
//    /// 用来生产的设施，
//    /// 花费的时间,
//    /// 需要的人工数目，
//    /// 
//    /// <returns></returns>
//    public static PAction GenerateRaw(IndustrialEquipmentObj InduObj,int wasterTime,int PersonObjNum,Nj[] input, Nj[] output,Pop otherRequire=null)
//    {
//        PAction ret = new PAction();
//        List<PType> inputs=new List<PType>();
//        inputs.Add(InduObj.obj);
//        for(int i=0;i<input.Length;i++)
//        {
//            inputs.Add(input[i].rawObj);
//        }
//        for(int i=0;i<output.Length;i++)
//        {
//            inputs.Add(output[i].rawObj);
//        }
//        List<Pop> requires=new List<Pop>();
//        //输入长度
//        for(int i=0;i<input.Length;i++)
//        {
//            var data=InduObj.rawCount[input[i].rawObj.GetType()];
//            requires.Add(PDDL.G(data.nowCapacity,(I)input[i].num));//要求里面的东西大于
//        }
//        if(otherRequire!=null)
//        {
//            requires.Add(otherRequire);
//        }
//        List<Pop> effects = new List<Pop>();
//        for(int i=0;i<output.Length;i++)
//        {
//            var data = InduObj.outRawCount[output[i].rawObj.GetType()];
//            requires.Add(PDDL.G((I)data.maxCapacity, PDDL.Add(data.nowCapacity,(I) output[i].num)));
//            effects.Add(PDDL.Increase(data.nowCapacity.func,(I)output[i].num));
//        }
//        requires.Add(PDDL.G(InduObj.nowPersonObj,(I)PersonObjNum));
//        var action= new PAction();
//        action.Init(
//            "Generate_Raw" + Nm.num,
//            inputs.ToArray(),
//            PDDL.And(requires.ToArray()),
//            PDDL.And(effects.ToArray()),
//            (I)wasterTime
//        );
//        return action;
//    }
//}