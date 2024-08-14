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
    static string ModifyString(string x)
    {
        if (x == "Obj")
            return "PType";
        // ��� x �Ƿ��� "Obj" ��β
        if (x.EndsWith("Obj"))
        {
            // ����ǣ����� "Type" �滻 "Obj"
            return x.Substring(0, x.Length - 3) + "Type";
        }
        else
        {
            // ����ֱ���� x ��ĩβ��� "Type"
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
        //    strbuilder.AppendLine($@"{t.prex}.SetObj((({cNode.type.Name})obj).{t.prex});");
        //    strbuilder.AppendLine($@"  (({cNode.type.Name})obj).{t.prex}.pddl = {t.prex};  ");
        //    //strbuilder.AppendLine($@"{t.prex}.SetObj(()=>{{return obj.{t.prex};}});");
        //}
        foreach (var t in cNode.dics)
        {
            strbuilder.AppendLine($@"{t.prex}.SetObj((({cNode.type.Name})obj).{t.prex});");
            strbuilder.AppendLine($@"  (({cNode.type.Name})obj).{t.prex}.pddl = {t.prex};  ");
            //strbuilder.AppendLine($@"{t.prex}.SetObj(obj.{t.prex});");
        }
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($@"{t.prex}.SetObj((({cNode.type.Name})obj).{t.prex});");
            strbuilder.AppendLine($@"  (({cNode.type.Name})obj).{t.prex}.pddl = {t.prex};  ");
        }
        strbuilder.AppendLine($@"}}");

        strbuilder.AppendLine($@"public override List<Predicate> GetPreds()
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
        strbuilder.AppendLine($@"public override List<Bool> GetPredsVal(){{var ret= new List<Bool>();");
        //foreach (var t in cNode.ints)
        //{
        //    strbuilder.AppendLine($@"ret.Add({t.prex}.val());");
        //}
        foreach (var t in cNode.bools)
        {
            strbuilder.AppendLine($@"ret.Add( (Bool) ({t.prex}.val()));");
        }
        foreach (var t in cNode.enums)
        {
            strbuilder.AppendLine($@"ret.Add( P.Is( GetObj() , {t.prex}.GetObj() ) );");
        }
        foreach (var t in cNode.custs)
        {
            strbuilder.AppendLine($@"ret.Add( P.Is( GetObj() , {t.prex}.GetObj() ) );");
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
        strbuilder.AppendLine("}");
        File.WriteAllText($"Assets/Scripts/Base/PDDL/PDDLClass/{cNode.type.Name}_PDDLClASS.cs", strbuilder.ToString());
        AssetDatabase.Refresh();

    }
}

public abstract class PDDLSet
{
    public PDDLSet()
    {
    }
    public abstract PDDLClass Add();
    public abstract void Remove(PDDLClass pDDL);
    public abstract HashSet<PDDLClass> GetPddls();
}
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

public class PDDLClassGet
{
    public static Dictionary<Type,PDDLSet> kv;
    public PDDLClassGet()
    {
        kv = new Dictionary<Type,PDDLSet>();
    }
    public void Init()
    {
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
    public static void Remove(PDDLClass pDDLClass)
    {
        kv[pDDLClass.GetType()].Remove(pDDLClass);
    }
}