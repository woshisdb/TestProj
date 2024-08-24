using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using System.Reflection;

/// <summary>
/// PDDL��Rule����
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
        // ��ȡ��ǰ�����е���������
        var types = Assembly.GetExecutingAssembly().GetTypes();

        // ����һ���б������汻 RuleAttribute ���εķ�����ί��
        var delegateList = new List<QMethodDelegate>();

        // �ֵ����洢�鷽��������д�ķ���
        var baseMethodDict = new Dictionary<MethodInfo, List<MethodInfo>>();

        // ������������
        foreach (var type in types)
        {
            // ��ȡ�����͵����й���ʵ������
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var method in methods)
            {
                // ��鷽���Ƿ� RuleAttribute ����
                if (method.GetCustomAttributes(typeof(RuleAttribute), false).Length > 0)
                {
                    // ����鷽�����ֵ�
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
                        // �������͵�ʵ��
                        var instance = Activator.CreateInstance(type);

                        // ����ί�в���ӵ��б�
                        QMethodDelegate del = (QMethodDelegate)Delegate.CreateDelegate(typeof(QMethodDelegate), instance, method);
                        delegateList.Add(del);
                    }
                }
            }
        }

        // ������д�ķ���
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

        // ����ί�в���ӵ��б�
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
///// ԭ������������
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
///// �ܽ����ԭ���ϼ�����ɹ�ϵ
///// </summary>
//public class GeneratorNode
//{
//    /// <summary>
//    /// ������������ʩ��
//    /// ���ѵ�ʱ��,
//    /// ��Ҫ���˹���Ŀ��
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
//        //���볤��
//        for(int i=0;i<input.Length;i++)
//        {
//            var data=InduObj.rawCount[input[i].rawObj.GetType()];
//            requires.Add(PDDL.G(data.nowCapacity,(I)input[i].num));//Ҫ������Ķ�������
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