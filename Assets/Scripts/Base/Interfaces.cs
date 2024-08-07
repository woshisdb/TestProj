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
    public Map()
    {

    }
    public void Insert(ObjEnum objEnum,Obj obj,ObjSaver objSaver)
    {
        Debug.Log(objEnum);
        Debug.Log(obj.GetType().Name);
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
        result.Sort((type1, type2) => type1.IsSubclassOf(type2) ? 1 : type2.IsSubclassOf(type1) ? -1 : 0);
        return result;
    }
    public static Domain InitPDDL()
    {
        Domain domain = new Domain();
        var domainTypes = GetAllSubclasses(typeof(PType));
        domain.pTypes = domainTypes;
        var datas=GetData<ActAttribute>();
        ///一系列的Actions
        foreach(var x in datas)
        {
            var act=(Activity)Activator.CreateInstance(x);
            domain.pActions.Add(act.GetAction());
            domain.predicates.AddRange(act.GetPredicates());
            domain.funcs.AddRange(act.GetFuncs());
        }
        List<PDDLClass> pddlClasss = new List<PDDLClass>();
        pddlClasss.Add(new Person_PDDL(new PersonType()));
        foreach(var x in pddlClasss)
        {
            domain.predicates.AddRange(x.GetPreds());
            domain.funcs.AddRange(x.GetFuncs());
        }
        return domain;
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
