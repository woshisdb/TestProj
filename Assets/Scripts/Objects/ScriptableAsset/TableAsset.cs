using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
[Serializable]
public class CodeSaver
{
    [SerializeField]
    public string name;
    [SerializeField]
    public List<CodeData> dataDatas;
    public CodeSaver()
    {
        this.name ="";
        this.dataDatas = new List<CodeData>();
    }
    public bool HasAct(int time)
    {
        var hour=TimeModel.GetHours(time);
        if (dataDatas[hour].hasAct)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Act GetAct(Person person,int time)
    {
        var data=dataDatas[TimeModel.GetHours(time)];
        //if (data.activity.Condition(data.obj,person, data.time))
        //{
        //    return data.activity.Effect(data.obj,person,data.time);
        //}
        //else
        //{
        return null;
        //}
    }
}
[ShowOdinSerializedPropertiesInInspector]
public class TableSaver
{
    [OdinSerialize]
    public int time;
    [OdinSerialize]
    public int lastNm;
    /// <summary>
    /// odinµÄÐòÁÐ»¯
    /// </summary>
    [OdinSerialize]
    public List<Obj> objs;
    [OdinSerialize]
    public List<Person> personList;
    [OdinSerialize]
    public List<TableModel> tables;
    [OdinSerialize]
    public List<CodeSystemData> codeDatas;

    [OdinSerialize]
    public ContractModel contractModel;
    public TableSaver()
    {
        tables = new List<TableModel>();
        objs=new List<Obj>();
        personList = new List<Person>();
        codeDatas = new List<CodeSystemData>();
    }
}

[CreateAssetMenu(fileName = "NewTableObject", menuName = "ScriptableObjects/TableAsset")]
public class TableAsset : SerializedScriptableObject
{
    public TableSaver tableSaver;
    [Button]
    public void CreateTable(string name,int size)
    {
        TableModel table = new TableModel();
        table.tableNo = "t" + Nm.num;
        table.TableName = name;
        table.size = size;
        tableSaver.tables.Add(table);
        GameArchitect.Interface.GetModel<TableModelSet>().AddTable(tableSaver.tables.Count-1);
    }
}
