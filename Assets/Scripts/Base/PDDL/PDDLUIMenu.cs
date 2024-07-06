using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
[ExecuteAlways]
public class PDDLUIMenu : MonoBehaviour
{
    [ReadOnly]
    public string path= "Assets/Scripts/Base/PDDL/PDDL.cs";
    [Button("Init")]
    public void Init()
    {
        StringBuilder strbuilder = new StringBuilder(@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PD{
");
        // 获取当前程序集
        Assembly assembly = Assembly.GetExecutingAssembly();

        // 获取程序集中所有的类型
        Type[] types = assembly.GetTypes();

        // 创建一个列表来存储被PAttribute修饰的类
        List<Type> classesWithPAttribute = new List<Type>();

        // 遍历所有类型，检查是否有PAttribute
        foreach (Type type in types)
        {
            // 检查类型是否被PAttribute修饰
            if (type.GetCustomAttributes(typeof(PAttribute), false).Any())
            {
                classesWithPAttribute.Add(type);
            }
        }

        // 输出被PAttribute修饰的所有类的名称
        foreach (Type type in classesWithPAttribute)
        {
            var constructor = type.GetConstructors()[0];
            strbuilder.Append("public static "+ type.Name +" "+ type.Name + "(");

                // 获取构造函数的参数
                ParameterInfo[] parameters = constructor.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    strbuilder.Append(parameters[i].ParameterType.Name + " " + parameters[i].Name);
                    if (i < parameters.Length - 1)
                    {
                        strbuilder.Append(", ");
                    }
                }

                strbuilder.Append("){");
                strbuilder.Append("return new "+ type.Name + "(");
                for (int i = 0; i < parameters.Length; i++)
                {
                    strbuilder.Append(parameters[i].Name);
                    if (i < parameters.Length - 1)
                    {
                        strbuilder.Append(", ");
                    }
                }
                strbuilder.Append(");");
                strbuilder.AppendLine("}");
        }
        strbuilder.Append('}');
        File.WriteAllText(path,strbuilder.ToString());
        AssetDatabase.Refresh();
    }
}