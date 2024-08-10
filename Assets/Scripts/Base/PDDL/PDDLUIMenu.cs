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
    public static void PDDLGen(CNode cNode)
    {
        StringBuilder strbuilder = new StringBuilder();
        strbuilder.AppendLine($@"public class {cNode.type.Name}_PDDL:PDDLClass<{cNode.type.Name},{cNode.type.Name}Type>{{");
        foreach(var t in cNode.bools)
        {
            strbuilder.AppendLine($"public PDDLVal {t.prex};");
        }
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($"public PDDLVal {t.prex};");
        }
        strbuilder.AppendLine($@"public {cNode.type.Name}_PDDL({cNode.type.Name}Type pType):base(){{
            this.pType = pType;
            stringBuilder = new StringBuilder();
            ");
        foreach (var t in cNode.bools)
        {
            strbuilder.AppendLine($@"
                {t.prex} = new PDDLVal(
                () =>
                {{
                    return new Predicate(""{t.prex}"", pType);
                }},
                () =>
                {{
                   return {t.clasx}.ToString();
                }});
            ");
        }
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($@"
                {t.prex} = new PDDLVal(
                () =>
                {{
                    return new Func(""{t.prex}"", pType);
                }},
                () =>
                {{
                   return {t.clasx}.ToString();
                }});
            ");
        }
        strbuilder.AppendLine($@"
            public override List<Predicate> GetPreds()
            {{
                return new List<Predicate>() {{");
        foreach (var t in cNode.bools) {
            strbuilder.AppendLine($@"(Predicate){t.prex}.pop(),");
        }
         strbuilder.AppendLine($@"}};
            }}
        ");
        strbuilder.AppendLine($@"
            public override List<Func> GetFuncs()
            {{
                return new List<Func>() {{");
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($@"(Func){t.prex}.pop(),");
        }
        strbuilder.AppendLine($@"}};
            }}
        ");
        strbuilder.AppendLine("}");
        File.WriteAllText($"Assets/Scripts/Base/PDDL/{cNode.type.Name}_PDDLClASS.cs", strbuilder.ToString());
        AssetDatabase.Refresh();
    }
}