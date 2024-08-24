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
    /// odin�����л�
    /// </summary>
    [OdinSerialize]
    public List<Obj> objs;
    /// <summary>
    /// ��Ҫ��ȷ����Ľ�ɫ
    /// </summary>
    [OdinSerialize]
    public List<PersonObj> PersonObjList;
    /// <summary>
    /// һϵ��Ԥ������������
    /// </summary>
    [OdinSerialize]
    public List<NPCObj> npcs;
    [OdinSerialize]
    public List<TableModel> tables;
    [OdinSerialize]
    public Dictionary<Type, PDDLSet> pddlSet;
    [OdinSerialize]
    public ContractModel contractModel;
    public TableSaver()
    {
        tables = new List<TableModel>();
        objs=new List<Obj>();
        PersonObjList = new List<PersonObj>();
        npcs = new List<NPCObj>();
        pddlSet=new Dictionary<Type, PDDLSet>();
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
        table.TableName = name;
        table.size = size;
        tableSaver.tables.Add(table);
        //Debug.Log(tableSaver.tables.Count);
        codeDatas = new List<CodeSystemData>();
        GameArchitect.Interface.GetModel<TableModelSet>().AddTable(tableSaver.tables.Count-1);
    }
}
