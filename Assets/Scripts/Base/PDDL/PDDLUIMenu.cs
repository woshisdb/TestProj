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
    static string ModifyString(string x)
    {
        if (x == "Obj")
            return "PType";
        // 检查 x 是否以 "Obj" 结尾
        if (x.EndsWith("Obj"))
        {
            // 如果是，则用 "Type" 替换 "Obj"
            return x.Substring(0, x.Length - 3) + "Type";
        }
        else
        {
            // 否则，直接在 x 的末尾添加 "Type"
            return x + "Type";
        }
    }
    public static void PDDLGen(CNode cNode)
    {
        StringBuilder strbuilder = new StringBuilder();
        string STRType = ModifyString(cNode.type.Name);
        strbuilder.AppendLine($@"
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class {cNode.type.Name}_PDDL:PDDLClass<{cNode.type.Name},{STRType}>{{");
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
            Debug.Log(t.prex);
            strbuilder.AppendLine($"public PDDLValRef<{t.TypeName},{t.clasx}> {t.prex};");
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
                   return new Bool(new Predicate(""{cNode.type.Name}_{t.prex}"", obj.GetPtype()),()=>{{return obj.{t.prex};}});
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
                   return new Num(new Func(""{cNode.type.Name}_{t.prex}"", obj.GetPtype()),()=>{{return obj.{t.prex};}});
                }});
            ");
        }
        foreach (var t in cNode.enums)
        {
            strbuilder.AppendLine($@"{t.prex}=  ({t.TypeName})PDDLClassGet.Generate(typeof({t.clasx}));");
            strbuilder.AppendLine($@"{t.prex}.SetObj(()=>{{return obj.{t.prex};}});");
        }
        foreach (var t in cNode.dics)
        {
            //strbuilder.AppendLine($@"{t.prex}=  ({t.TypeName})PDDLClassGet.Generate(typeof({t.clasx}));");
            strbuilder.AppendLine($@"
            {t.prex} = new PDDLValRef<{t.TypeName},{t.clasx}>(
            (p)=> {{ return new Predicate(""{cNode.type.Name}_{t.prex}"", obj.GetPtype(), p); }},
            () => {{ return new Predicate(""{cNode.type.Name}_{t.prex}"",obj.GetPtype(),obj.{t.prex}.GetPtype()); }},
            () => {{ return ({t.TypeName})(obj.{t.prex}.GetPDDLClass()); }},
            () => {{ return obj.{t.prex}; }});
            ");
            //strbuilder.AppendLine($@"{t.prex}.SetObj(obj.{t.prex});");
        }
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($@"
            {t.prex} = new PDDLValRef<{t.TypeName},{t.clasx}>(
            (p)=> {{ return new Predicate(""{cNode.type.Name}_{t.prex}"", obj.GetPtype(), p); }},
            () => {{ return new Predicate(""{cNode.type.Name}_{t.prex}"",obj.GetPtype(),obj.{t.prex}.GetPtype()); }},
            () => {{ return ({t.TypeName})(obj.{t.prex}.GetPDDLClass()); }},
            () => {{ return obj.{t.prex}; }});
            ");
            //strbuilder.AppendLine($@"{t.prex}=  ({t.TypeName})PDDLClassGet.Generate(typeof({t.clasx}));");
            //strbuilder.AppendLine($@"{t.prex}.SetObj(obj.{t.prex});");
        }
        strbuilder.AppendLine($@"}}");

        strbuilder.AppendLine($@"public override void SetObj(object obj){{
            this.obj=({cNode.type.Name})obj;
            (({cNode.type.Name})obj).pddl = this;");
		//foreach (var t in cNode.enums)
		//{
		//	strbuilder.AppendLine($@"{t.prex}.SetObj((({cNode.type.Name})obj).{t.prex});");
		//	strbuilder.AppendLine($@"  (({cNode.type.Name})obj).{t.prex}.pddl = {t.prex};  ");
		//}
		//foreach (var t in cNode.dics)
  //      {
  //          strbuilder.AppendLine($@"{t.prex}.SetObj((({cNode.type.Name})obj).{t.prex});");
  //          strbuilder.AppendLine($@"  (({cNode.type.Name})obj).{t.prex}.pddl = {t.prex};  ");
  //      }
  //      foreach (var t in cNode.custs)
  //      {
  //          strbuilder.AppendLine($@"{t.prex}.SetObj((({cNode.type.Name})obj).{t.prex});");
  //          strbuilder.AppendLine($@"  (({cNode.type.Name})obj).{t.prex}.pddl = {t.prex};  ");
  //      }
        strbuilder.AppendLine($@"}}");

        strbuilder.AppendLine($@"public override List<Predicate> GetPreds()
        {{
            var ret= new List<Predicate>() {{");
        foreach (var t in cNode.bools) {
            strbuilder.AppendLine($@"(Predicate){t.prex}.pop(),");
        }
        foreach (var t in cNode.custs)
        {
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
        strbuilder.AppendLine($@"public override List<Bool> GetPredsVal(){{var ret= new List<Bool>();");
        //foreach (var t in cNode.ints)
        //{
        //    strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        //}
        foreach (var t in cNode.bools)
        {
            strbuilder.AppendLine($@"ret.Add( (Bool) ({t.prex}.val()));");
        }
        //foreach (var t in cNode.enums)
        //{
        //    strbuilder.AppendLine($@"ret.Add( {t.prex}.pop() );");
        //}
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($@"ret.Add( new Bool( {t.prex}.pop(),true ) );");
        }
        strbuilder.AppendLine($@"return ret;}}");
        strbuilder.AppendLine($@"public override List<Num> GetFuncsVal(){{var ret= new List<Num>();");
        foreach (var t in cNode.ints)
        {
            strbuilder.AppendLine($@"ret.Add( (Num)( {t.prex}.val() ) );");
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

        strbuilder.AppendLine($@"public override List<PType> GetTypes(){{
            var ret=new List<PType>();");
        strbuilder.AppendLine($@"ret.Add(obj.GetPtype());");
        foreach (var t in cNode.enums)
            strbuilder.AppendLine($@"ret.Add({t.prex}.GetPType());");
        foreach(var t in cNode.dics)
            strbuilder.AppendLine($@"ret.Add({t.prex}.GetPType());");
        foreach (var t in cNode.custs)
            strbuilder.AppendLine($@"ret.Add({t.prex}.GetPType());");
        strbuilder.AppendLine("return ret;");
        strbuilder.AppendLine("     }");

        strbuilder.AppendLine($@"public override List<PDDLClass> GetPddls(){{
        var ret = new List<PDDLClass>();");
        strbuilder.AppendLine($@"ret.Add(this);");
        foreach (var t in cNode.enums)
            strbuilder.AppendLine($@"ret.Add({t.prex}.pVal());");
        foreach (var t in cNode.dics)
            strbuilder.AppendLine($@"ret.Add({t.prex}.pVal());");
        foreach (var t in cNode.custs)
            strbuilder.AppendLine($@"ret.Add({t.prex}.pVal());");
        strbuilder.AppendLine($@"return ret;}}");
        strbuilder.AppendLine("}");
        File.WriteAllText($"Assets/Scripts/Base/PDDL/PDDLClass/{cNode.type.Name}_PDDLClASS.cs", strbuilder.ToString());
        AssetDatabase.Refresh();

    }
}
[SerializeField]
public abstract class PDDLSet
{
    public PDDLSet()
    {
    }
    public abstract PDDLClass Add();
    public abstract void Remove(PDDLClass pDDL);
    public abstract HashSet<PDDLClass> GetPddls();
}
[SerializeField]
public class PDDLSet<T>:PDDLSet
    where T: PDDLClass,new()
{
    public HashSet<PDDLClass> use;
    public Queue<PDDLClass> free;
    public PDDLSet():base()
    {
        use = new HashSet<PDDLClass>();
        free = new Queue<PDDLClass>();
    }

    public override PDDLClass Add()
    {
        if(free.Count>0)
        {
            var x= free.Dequeue();
            use.Add(x);
            return x;
        }
        else
        {
            var t=new T();
            use.Add(t);
            return t;
        }
    }

    public override HashSet<PDDLClass> GetPddls()
    {
        return use;
    }

    public override void Remove(PDDLClass pDDL)
    {
        free.Enqueue(pDDL);
        use.Remove(pDDL);
    }
}



/// <summary>
/// 用来设置PDDL类和删除PDDL类
/// </summary>
public class PDDLClassGet:PDDLClassGetBase
{
    public static Dictionary<Type, PDDLSet> kv { get { return GameArchitect.get.pddlSet; } }
    public PDDLClassGet()
    {
    }
    public void Init()
    {
    }
    public static PType GetPType(Type genericType)
    {
        if (genericType.IsGenericType)
        {
            // 获取泛型类型的定义，例如 EX<T, F>
            Type genericTypeDefinition = genericType.GetGenericTypeDefinition();

            // 假设 EXType<T, F> 的名称为 "EXType" + 泛型类型的名称
            string targetTypeName = genericTypeDefinition.Name + "Type";

            // 获取泛型参数，例如 T, F
            Type[] genericArguments = genericType.GetGenericArguments();

            // 生成目标类型，例如 EXType<T, F>
            Type targetType = Type.GetType($"{genericTypeDefinition.Namespace}.{targetTypeName}`{genericArguments.Length}");

            if (targetType != null)
            {
                // 将泛型参数应用到目标类型，例如 EXType<T, F>
                Type constructedType = targetType.MakeGenericType(genericArguments);

                // 创建该类型的实例
                return (PType)Activator.CreateInstance(constructedType);
            }
            else
            {
                throw new InvalidOperationException($"Cannot find type '{targetTypeName}' in namespace '{genericTypeDefinition.Namespace}'.");
            }
        }
        else
        {
            var ptype = Assembly.GetExecutingAssembly().GetType(genericType.Name + "Type");
            return (PType)Activator.CreateInstance(ptype);
        }
    }

    public static PDDLClass Generate(Type type)
    {
        if(kv.ContainsKey(type)==false)
        {
            var pddlType = Assembly.GetExecutingAssembly().GetType(type.Name+"_PDDL");
            Debug.Log(type.Name + "_PDDL");
            Type genericTypeDefinition = typeof(PDDLSet<>);
            Type specificType = genericTypeDefinition.MakeGenericType(pddlType);
            object pddlSetInstance = Activator.CreateInstance(specificType);
            kv.TryAdd(type, (PDDLSet)pddlSetInstance);
        }
        return kv[type].Add();
    }
    public static PDDLClass Generate(Type type,Type pddl)
    {
        if (kv.ContainsKey(type) == false)
        {
            Type genericTypeDefinition = typeof(PDDLSet<>);
            Type specificType = genericTypeDefinition.MakeGenericType(pddl.GetType());
            object pddlSetInstance = Activator.CreateInstance(specificType);
            kv.TryAdd(type, (PDDLSet)pddlSetInstance);
        }
        return kv[type].Add();
    }
    public static void Remove(PDDLClass pDDLClass)
    {
        kv[pDDLClass.GetType()].Remove(pDDLClass);
    }
}