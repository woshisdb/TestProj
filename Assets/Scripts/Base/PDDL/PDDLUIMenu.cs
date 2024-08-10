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
        // ��ȡ��ǰ����
        Assembly assembly = Assembly.GetExecutingAssembly();

        // ��ȡ���������е�����
        Type[] types = assembly.GetTypes();

        // ����һ���б����洢��PAttribute���ε���
        List<Type> classesWithPAttribute = new List<Type>();

        // �����������ͣ�����Ƿ���PAttribute
        foreach (Type type in types)
        {
            // ��������Ƿ�PAttribute����
            if (type.GetCustomAttributes(typeof(PAttribute), false).Any())
            {
                classesWithPAttribute.Add(type);
            }
        }

        // �����PAttribute���ε������������
        foreach (Type type in classesWithPAttribute)
        {
            var constructor = type.GetConstructors()[0];
            strbuilder.Append("public static "+ type.Name +" "+ type.Name + "(");

                // ��ȡ���캯���Ĳ���
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