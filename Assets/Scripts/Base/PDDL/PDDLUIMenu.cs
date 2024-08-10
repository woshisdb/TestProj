using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        strbuilder.AppendLine($@"
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class {cNode.type.Name}_PDDL:PDDLClass<{cNode.type.Name},{cNode.type.Name}Type>{{");
        foreach(var t in cNode.bools)
        {
            strbuilder.AppendLine($"public {t.TypeName} {t.prex};");
        }
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($"public {t.TypeName} {t.prex};");
        }
        foreach (var t in cNode.dics)
        {
            strbuilder.AppendLine($"public {t.TypeName} {t.prex};");
        }
        foreach (var t in cNode.enums)
        {
            strbuilder.AppendLine($"public {t.TypeName} {t.prex};");
        }
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($"public {t.TypeName} {t.prex};");
        }
        strbuilder.AppendLine($@"public {cNode.type.Name}_PDDL():base(){{
            ");
        foreach (var t in cNode.bools)
        {
            strbuilder.AppendLine($@"
                {t.prex} = new PDDLVal(
                () =>
                {{
                    return new Predicate(""{cNode.type.Name}_{t.prex}"", obj.GetPtype());
                }},
                () =>
                {{
                   return new Bool(new Predicate(""{cNode.type.Name}_{t.prex}"", obj.GetPtype()),obj.{t.prex});
                }});
            ");
        }
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($@"
                {t.prex} = new PDDLVal(
                () =>
                {{
                    return new Func(""{cNode.type.Name}_{t.prex}"", obj.GetPtype());
                }},
                () =>
                {{
                   return new Num(new Func(""{cNode.type.Name}_{t.prex}"", obj.GetPtype()),obj.{t.prex});
                }});
            ");
        }
        foreach (var t in cNode.enums)
        {
            strbuilder.AppendLine($@"{t.prex}=new {t.TypeName}();");
            strbuilder.AppendLine($@"{t.prex}.SetObj(()=>{{return obj.{t.prex};}});");
        }
        foreach (var t in cNode.dics)
        {
            strbuilder.AppendLine($@"{t.prex}=new {t.TypeName}();");
            strbuilder.AppendLine($@"{t.prex}.SetObj(obj.{t.prex});");
        }
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($@"{t.prex}=new {t.TypeName}();");
            strbuilder.AppendLine($@"{t.prex}.SetObj(obj.{t.prex});");
        }
        strbuilder.AppendLine($@"}}
        public override List<Predicate> GetPreds()
        {{
            var ret= new List<Predicate>() {{");
        foreach (var t in cNode.bools) {
            strbuilder.AppendLine($@"(Predicate){t.prex}.pop(),");
        }
        //foreach (var t in cNode.ints)
        //{
        //    strbuilder.AppendLine($@"(Predicate){t.prex}.pop(),");
        //}
        //foreach (var t in cNode.enums)
        //{
        //    strbuilder.AppendLine($@"(Predicate){t.prex}.pop(),");
        //}
        strbuilder.AppendLine($@"}};");
        //foreach (var t in cNode.custs)
        //{
        //    strbuilder.AppendLine($@"ret.AddRange( (Func){t.prex}.GetPreds() );");
        //}
        strbuilder.AppendLine($@"
            return ret;
            }}
        ");
        strbuilder.AppendLine($@"
            public override List<Func> GetFuncs()
            {{
                var ret= new List<Func>() {{");
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($@"               (Func){t.prex}.pop(),");
        }
        //foreach (var t in cNode.bools)
        //{
        //    strbuilder.AppendLine($@"(Func){t.prex}.pop(),");
        //}
        //foreach (var t in cNode.enums)
        //{
        //    strbuilder.AppendLine($@"(Func){t.prex}.pop(),");
        //}
        strbuilder.AppendLine($@"}};");
        //foreach (var t in cNode.custs)
        //{
        //    strbuilder.AppendLine($@"ret.AddRange( (Func){t.prex}.GetFuncs() );");
        //}
        strbuilder.AppendLine($@"
                return ret;
            }}
        ");
        strbuilder.AppendLine($@"public override List<Pop> GetPredsVal(){{var ret= new List<Pop>();");
        //foreach (var t in cNode.ints)
        //{
        //    strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        //}
        foreach (var t in cNode.bools)
        {
            strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        }
        foreach (var t in cNode.enums)
        {
            strbuilder.AppendLine($@"ret.Add( P.Belong( GetObj() , {t.prex}.GetObj() ) );");
        }
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($@"ret.Add( P.Belong( GetObj() , {t.prex}.GetObj() ) );");
        }
        strbuilder.AppendLine($@"return ret;}}");
        strbuilder.AppendLine($@"public override List<Pop> GetFuncsVal(){{var ret= new List<Pop>();");
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        }
        //foreach (var t in cNode.bools)
        //{
        //    strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        //}
        //foreach (var t in cNode.enums)
        //{
        //    strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        //}
        //foreach (var t in cNode.custs)
        //{
        //    strbuilder.AppendLine($@"ret.AddRange({t.prex}.GetPredsVal() );");
        //}
        strbuilder.AppendLine($@"return ret;}}");
        strbuilder.AppendLine("}");
        File.WriteAllText($"Assets/Scripts/Base/PDDL/PDDLClass/{cNode.type.Name}_PDDLClASS.cs", strbuilder.ToString());
        AssetDatabase.Refresh();
    }
}