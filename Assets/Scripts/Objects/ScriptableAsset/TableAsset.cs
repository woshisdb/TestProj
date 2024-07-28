using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

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
    public ContractModel contractModel;
    public TableSaver()
    {
        tables = new List<TableModel>();
        objs=new List<Obj>();
        personList = new List<Person>();
    }
}

[CreateAssetMenu(fileName = "NewTableObject", menuName = "ScriptableObjects/TableAsset")]
public class TableAsset : SerializedScriptableObject
{
    public TableSaver tableSaver;
    [OdinSerialize]
    public List<CodeSystemData> codeDatas;
    [Button]
    public void CreateTable(string name,int size)
    {
        TableModel table = new TableModel();
        table.tableNo = "t" + Nm.num;
        table.TableName = name;
        table.size = size;
        tableSaver.tables.Add(table);
        codeDatas = new List<CodeSystemData>();
        GameArchitect.Interface.GetModel<TableModelSet>().AddTable(tableSaver.tables.Count-1);
    }
}
