using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Sirenix.Serialization;
using System.IO;
using Sirenix.OdinInspector;
using System;

public class SaveSystem :Singleton<SaveSystem>
{
    public bool firstInit;
    string tablesavePath;
    HashSet<int> savedObj;
    private SaveSystem()
    {
        savedObj = new HashSet<int>();
        Debug.Log(Application.persistentDataPath);
        tablesavePath = "Assets/Resources/SaveData" + "/tablesaveData.dat";
    }
    public Obj Save(Obj obj)
    {
        savedObj.Clear();
        return obj;
    }
    public Obj Load(Obj obj)
    {
        savedObj.Clear();
        return obj;
    }
    [Button("Save Game")]
    public void Save()
    {
        GameArchitect game= (GameArchitect)GameArchitect.Interface;
        game.tableAsset.tableSaver.lastNm = Nm.x;
        if (GameArchitect.get.GetModel<TimeModel>()!=null)
            game.tableAsset.tableSaver.time = GameArchitect.get.GetModel<TimeModel>().Time;
        byte[] tablebytes = SerializationUtility.SerializeValue(game.tableAsset.tableSaver, DataFormat.JSON);
        File.WriteAllBytes(tablesavePath, tablebytes);
        Debug.Log("Game Saved.");
    }
    //protected void PDDLInit()
    //{
    //    foreach (var x in tableAsset.tableSaver.objs)
    //    {
    //        x.InitPDDLClass();
    //    }
    //}
    [Button("Load Game")]
    public void Load()
    {
        GameArchitect game = (GameArchitect)GameArchitect.Interface;
        if (File.Exists(tablesavePath))
        {
            firstInit = false;
            byte[] tableBytes = File.ReadAllBytes(tablesavePath);
            game.tableAsset.tableSaver=SerializationUtility.DeserializeValue<TableSaver>(tableBytes, DataFormat.JSON); // 假设TableType是game.tableAsset的类型
            if (game.tableAsset.tableSaver.pddlSet == null)
            {
                Debug.Log("haja");
                game.tableAsset.tableSaver.pddlSet = new Dictionary<Type, PDDLSet>();
                //PDDLInit();
            }
            else
            {
            }
            Nm.x = game.tableAsset.tableSaver.lastNm;
            Debug.Log("Table data loaded.");
            foreach(var obj in game.tableAsset.tableSaver.objs)
            {
                //Debug.Log(obj.GetType().Name);
                obj.activities = GameArchitect.activities[obj.GetType()];//一系列的活动
                obj.cardInf.effect = () => { obj.SendEvent<SelectObjEvent>(new SelectObjEvent(obj)); };
            }
        }
        else
        {
            firstInit = true;
            game.tableAsset.tableSaver = new TableSaver();
            Nm.x = game.tableAsset.tableSaver.lastNm;
            Debug.Log("Table data loaded.");
            foreach (var obj in game.tableAsset.tableSaver.objs)
            {
                //Debug.Log(obj.GetType().Name);
                obj.activities = GameArchitect.activities[obj.GetType()];//一系列的活动
                obj.cardInf.effect = () => { obj.SendEvent<SelectObjEvent>(new SelectObjEvent(obj)); };
            }
            Debug.LogWarning("No table save file found.");
        }
        //if(game.tableAsset.tableSaver==null)
        //{
        //    game.tableAsset.tableSaver = new TableSaver();
        //    game.tableAsset.CreateTable("TestTable",100000);
        //    var person = new Person(Map.Instance.GetSaver(ObjEnum.PersonE));
        //    GameArchitect.gameLogic.CreatePerson(true,"Person",true,"TestTable");

        //}
        //GameArchitect.get.GetModel<TimeModel>().Time.val = game.tableAsset.tableSaver.time;
    }
}
