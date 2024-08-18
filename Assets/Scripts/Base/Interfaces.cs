using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QFramework;
using UnityEngine;

public class Map : Singleton<Map>
{
    public Dictionary<Type, Type> kv;
    public Dictionary<Type,ObjSaver> ks;
    public Dictionary<ObjEnum,Type> enum2Type;
    public Dictionary<ObjEnum,Obj> enum2Ins;
    public Dictionary<Type,ObjEnum> saver2Enum;
    public static List<PType> types;
    protected Map():base()
    {

    }
    public void Insert(ObjEnum objEnum,Obj obj,ObjSaver objSaver)
    {
        //Debug.Log(objEnum);
        //Debug.Log(obj.GetType().Name);
        enum2Ins.Add(objEnum,obj);
        //Map.Instance.ks.Add(obj.GetType(), objSaver);
        enum2Type.Add(objEnum, obj.GetType());
        saver2Enum.Add(objSaver.GetType(),objEnum);
    }
    public void Init()
    {
        if (kv != null)
            return;
        //类型-》objType
        kv = new Dictionary<Type, Type>();
        //ks初始化
        ks = new Dictionary<Type, ObjSaver>();
        enum2Type = new Dictionary<ObjEnum, Type>();

        enum2Ins = new Dictionary<ObjEnum, Obj>();
        saver2Enum = new Dictionary<Type,ObjEnum>();

        Assembly assembly = Assembly.GetExecutingAssembly();
        // 查找所有带有 MapAttribute 属性的类
        var typesWithMapAttribute = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(MapAttribute), false).Length > 0).ToList();
        //.ToList();
        foreach (ObjEnum hs1 in Enum.GetValues(typeof(ObjEnum)))
        {
            var str=hs1.ToString().Substring(0, hs1.ToString().Length - 1);
            Debug.Log(str);
            var type = typesWithMapAttribute.Find(e =>{ return String.Equals(str, e.Name); } );
            Debug.Log(type.Name);
            var mapAttributes = type.GetCustomAttributes(typeof(MapAttribute), false);
            Type Objtype;
            if (((MapAttribute)mapAttributes[0]).type == null)
                Objtype = assembly.GetType(type.Name.Replace("Obj", "Type"));
            else
            {
                Objtype = ((MapAttribute)mapAttributes[0]).type;
            }
            kv.Add(type, Objtype);
            string enumName = type.Name + "E";
            string saverName;
            if (((MapAttribute)mapAttributes[0]).saver == null)
            {
                saverName = type.Name.Replace("Obj", "Saver");
                saverName = char.ToLower(saverName[0]) + saverName.Substring(1);
            }
            else
            {
                saverName = ((MapAttribute)mapAttributes[0]).saver;
            }
            Debug.Log(enumName);
            ObjEnum.TryParse<ObjEnum>(enumName, out ObjEnum objEnum);
            var objSaverField = typeof(ObjAsset).GetField(saverName);
            //Debug.Log(type.Name);
            //Debug.Log
            ks.Add(type, (ObjSaver)objSaverField.GetValue(GameArchitect.get.objAsset));
            //Debug.Log(type.Name+"1");
            var ksv = ks[type];
            var data = (Obj)Activator.CreateInstance(type, new object[] { null });
            //Debug.Log(type.Name+"2");
            Insert(objEnum,data , (ObjSaver)objSaverField.GetValue(GameArchitect.get.objAsset));
        }
    }
    public static List<Type> GetData<T> ()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var ret = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(T), false).Length > 0).ToList();
        return ret;
    }
    public static List<Type> GetAllSubclasses(Type baseType)
    {
        List<Type> result = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(baseType))
                {
                    result.Add(type);
                }
            }
        }
        // Sort to ensure that superclasses come before their subclasses
        result=SortTypesByInheritance(result);
        return result;
    }
    public static List<Type> SortTypesByInheritance(List<Type> types)
    {
        var graph = new Dictionary<Type, HashSet<Type>>();
        var inDegree = new Dictionary<Type, int>();

        // 初始化图和入度字典
        foreach (var type in types)
        {
            if (!graph.ContainsKey(type))
                graph[type] = new HashSet<Type>();

            if (!inDegree.ContainsKey(type))
                inDegree[type] = 0;
        }

        // 构建图和计算入度
        foreach (var type in types)
        {
            var baseTypes = type.BaseType != null ? new[] { type.BaseType } : Enumerable.Empty<Type>();
            foreach (var baseType in baseTypes)
            {
                if (types.Contains(baseType))
                {
                    if (!graph.ContainsKey(baseType))
                        graph[baseType] = new HashSet<Type>();

                    if (!graph[baseType].Contains(type))
                        graph[baseType].Add(type);

                    inDegree[type]++;
                }
            }
        }

        // Kahn's Algorithm (拓扑排序)
        var queue = new Queue<Type>(inDegree.Where(x => x.Value == 0).Select(x => x.Key));
        var sortedTypes = new List<Type>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sortedTypes.Add(current);

            if (graph.ContainsKey(current))
            {
                foreach (var neighbor in graph[current])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                        queue.Enqueue(neighbor);
                }
            }
        }

        return sortedTypes;
    }
    static int CompareTypes(Type x, Type y)
    {
        if (x == y) return 0;
        if (IsSubclassOf(x, y)) return 1;
        if (IsSubclassOf(y, x)) return -1;
        return 0;
    }

    static bool IsSubclassOf(Type x, Type y)
    {
        // 检查 x 是否是 y 的子类或后代类
        while (x != null && x != typeof(object))
        {
            if (x == y)
                return true;
            x = x.BaseType;
        }
        return false;
    }
    public static System.Tuple<Domain,Problem> InitPDDL()
    {
        Domain domain = new Domain();
        Problem problem = new Problem();
        var datas=GetData<ActAttribute>();
        if (types == null)
        {
            types = new List<PType>();
            var typeClasss = GetAllSubclasses(typeof(PType));
            foreach(var type in typeClasss)
            {
                if(type!=typeof(DicType<,>))
                {
                    types.Add((PType)Activator.CreateInstance(type));
                }
            }
        }
        domain.AddTypes(types);
        ///一系列的Actions初始化
        foreach (var x in ActionPddls.GetPDDLActions())
        {
            var actData = x.Item2;
            domain.pActions.Add(actData);
            actData.Init(domain,problem);
        }
        //初始化一系列的Object
        var pddlClasss = PDDLClassGet.kv;

        foreach(var x in GameArchitect.get.objs)
        {
            x.GetPDDLClass().SetDomain(domain);
            x.GetPDDLClass().SetProblem(problem);
        }
        foreach(var x in GameArchitect.get.tables)
        {
            x.GetPDDLClass().SetDomain(domain);
            x.GetPDDLClass().SetProblem(problem);
        }
        return new System.Tuple<Domain,Problem>(domain,problem);
    }

    public Obj GetObj(ObjEnum type)
    {
        return enum2Ins[type];
    }
    public ObjSaver GetSaver(Type type)
    {
        return ks[type];
    }
    public ObjSaver GetSaver(ObjEnum type)
    {
        return ks[enum2Type[type]];
    }
}


public interface IUIView
{
    public void Init();
}
