using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Sirenix.Serialization;
using System.IO;
using Sirenix.OdinInspector;

public class SaveSystem :Singleton<SaveSystem>
{
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
            game.tableAsset.tableSaver.time = GameArchitect.get.GetModel<TimeModel>().Time.val;
        byte[] tablebytes = SerializationUtility.SerializeValue(game.tableAsset.tableSaver, DataFormat.JSON);
        File.WriteAllBytes(tablesavePath, tablebytes);
        Debug.Log("Game Saved.");
    }
    [Button("Load Game")]
    public void Load()
    {
        GameArchitect game = (GameArchitect)GameArchitect.Interface;
        if (File.Exists(tablesavePath))
        {
            byte[] tableBytes = File.ReadAllBytes(tablesavePath);
            game.tableAsset.tableSaver=SerializationUtility.DeserializeValue<TableSaver>(tableBytes, DataFormat.JSON); // 假设TableType是game.tableAsset的类型
            Nm.x = game.tableAsset.tableSaver.lastNm;
            Debug.Log("Table data loaded.");
            GameArchitect.get.InitActivities();
            foreach(var obj in game.tableAsset.tableSaver.objs)
            {
                //Debug.Log(obj.GetType().Name);
                obj.activities = GameArchitect.activities[obj.GetType()];//一系列的活动
                obj.cardInf.effect = () => { obj.SendEvent<SelectObjEvent>(new SelectObjEvent(obj)); };
            }
        }
        else
        {
            Debug.LogWarning("No table save file found.");
        }
        if(game.tableAsset.tableSaver==null)
        {
            game.tableAsset.tableSaver = new TableSaver();
        }
        //GameArchitect.get.GetModel<TimeModel>().Time.val = game.tableAsset.tableSaver.time;
    }
}
