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
    protected Map()
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
        foreach (var type in typesWithMapAttribute)
        {
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
    public ObjType GetObjType(Type type,string name="")
    {
        //Init();
        var s=kv[type];
        return (ObjType)Activator.CreateInstance(s,name);
    }

    /// <summary>
    /// 待完善
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public ObjType GetObjType(ObjEnum type, string name="")
    {
        //Init();
        var s = kv[ enum2Type[type]];
        return (ObjType)Activator.CreateInstance(s, name);
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
