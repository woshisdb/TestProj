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
    protected Map()
    {
        Init();
    }
    protected void Init()
    {
        if (kv != null)
            return;
        kv = new Dictionary<Type, Type>();
        //ks初始化
        ks = new Dictionary<Type, ObjSaver>();
        enum2Type = new Dictionary<ObjEnum, Type>();
        Assembly assembly = Assembly.GetExecutingAssembly();
        // 查找所有带有 MapAttribute 属性的类
        var typesWithMapAttribute = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(MapAttribute), false).Length > 0)
            .ToList();
        foreach (var type in typesWithMapAttribute)
        {
            var mapAttributes = type.GetCustomAttributes(typeof(MapAttribute), false);
            Type Objtype;
            if (((MapAttribute)mapAttributes[0]).type == null)
                Objtype = assembly.GetType(type.Name.Replace("Obj", "Type"));
            else
            {
                Objtype = ((MapAttribute)mapAttributes[0]).type;
            }
            kv.Add(type, Objtype);
            //Type saverType;
            //if (((MapAttribute)mapAttributes[1]).type == null)
            //    saverType = assembly.GetType(type.Name.Replace("Obj", "Saver"));
            //else
            //{
            //    saverType = ((MapAttribute)mapAttributes[1]).type;
            //}
        }
        //ks.Add(typeof(Person), GameArchitect.get.objAsset.personSaver);
    }
    public ObjType GetObj(Type type,string name)
    {
        Init();
        var s=kv[type];
        return (ObjType)Activator.CreateInstance(s,name);
    }

    /// <summary>
    /// 待完善
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public ObjType GetObj(ObjEnum type, string name)
    {
        Init();
        var s = kv[ enum2Type[type]];
        return (ObjType)Activator.CreateInstance(s, name);
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
