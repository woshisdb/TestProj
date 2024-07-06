using System.Collections;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

public class SceneType:PType
{
    public SceneType(string name=""):base(name)
    {

    }
}

public struct TableChangeEvent
{
    public TableModel Model;
}
public class TableModel:ICanSendEvent
{
    public StringBuilder str;
    public string tableNo;
    public string TableName;
    public Person owner;
    /// <summary>
    /// 场景大小
    /// </summary>
    public int size;
    public List<Person> persons;
    public List<Obj> objs;
    public TableControl control;//Table控制器
    public Rule rule;
    public SceneType sceneType;
    public TableModel()
    {
        str = new StringBuilder();
        persons = new List<Person>();
        objs = new List<Obj>();
        sceneType=new SceneType(TableName);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }

    public void UpdateTable()
    {
        this.SendEvent<TableChangeEvent>(new TableChangeEvent() { Model = this });
    }
    /// <summary>
    /// 进入场景
    /// </summary>
    /// <param name="person"></param>
    public void EnterTable(Person person)
    {
        persons.Add(person);
        objs.Add(person);
        person.belong = this;
        if (person.isPlayer)
            GameArchitect.gameLogic.camera.transform.position = GameArchitect.get.player.belong.control.CenterPos();
        UpdateTable();
    }
    /// <summary>
    /// 离开场景
    /// </summary>
    /// <param name="person"></param>
    public void LeaveTable(Person person)
    {
        persons.Remove(person);
        objs.Remove(person);
        person.belong = null;
        UpdateTable();
    }
    public void AddToTable(Obj obj)
    {
        obj.belong = this;
        objs.Add(obj);
        UpdateTable();
    }
    public void RemoveToTable(Obj obj)
    {
        objs.Remove(obj);
        UpdateTable();
    }
    //public StringBuilder GetString()
    //{
    //    str.Clear();
    //    str.AppendLine("");
    //    return str;
    //}
}
public class TableModelSet : AbstractModel
{
    public List<TableModel> tableModels;
    GameObject table;
    public TableModelSet(TableAsset tableAsset)
    {
        tableModels = tableAsset.tableSaver.tables;
    }
    protected override void OnInit()
    {
        var ts = ((GameArchitect)GameArchitect.Interface).tableAsset;
        tableModels = ts.tableSaver.tables;
        table = Resources.Load<GameObject>("Controler/Table");
        for (int i=0;i<tableModels.Count; i++)
        {
            AddTable(i);
            //var data=GameObject.Instantiate<GameObject>(table);
            //data.GetComponent<TableControl>().tableModel = tableModels[i];
            //tableModels[i].control = data.GetComponent<TableControl>();//它的控制器
            //data.GetComponent<TableControl>().Init();
            //var root=GameObject.Find("Tables");
            //data.transform.SetParent(root.transform);
            //data.transform.localPosition = new Vector3(i*25,0, 0);
        }
        for(int i=0;i<tableModels.Count;i++)
        {
            tableModels[i].UpdateTable();
        }
    }
    public void AddTable(int i)
    {
        var data = GameObject.Instantiate<GameObject>(table);
        data.GetComponent<TableControl>().tableModel = tableModels[i];
        tableModels[i].control = data.GetComponent<TableControl>();//它的控制器
        data.GetComponent<TableControl>().Init();
        var root = GameObject.Find("Tables");
        data.transform.SetParent(root.transform);
        data.transform.localPosition = new Vector3(i * 25, 0, 0);
    }
}
