using System.Collections;
using System.Collections.Generic;
using System.Text;
using QFramework;
using UnityEngine;

public class TableModelType:ObjType
{

}

public struct TableChangeEvent
{
    public TableModel Model;
}

public class GoodsMapper
{
    public 
}
public class TableGoodsManager
{
    /// <summary>
    /// 每种商品的类型
    /// </summary>
    public Dictionary<ObjEnum, GoodsMapper> goods;
}

[SerializeField,Class]
public class TableModel: IPDDL, ICanRegisterEvent
{
    public PDDLClass pddl;
    public PType obj { get { return pddl.GetPType(); } }
    public string TableName;
    /// <summary>
    /// 场景大小
    /// </summary>
    public int size;
    public List<PersonObj> PersonObjs;
    public List<Obj> objs;
    public TableControl control;//Table控制器
    public Rule rule;
    public TableModel()
    {
        PersonObjs = new List<PersonObj>();
        objs = new List<Obj>();
        InitPDDLClass();
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
    /// <param name="PersonObj"></param>
    public void EnterTable(PersonObj PersonObj)
    {
        PersonObjs.Add(PersonObj);
        objs.Add(PersonObj);
        PersonObj.belong = this;
        if (PersonObj.isPlayer)
            GameArchitect.gameLogic.camera.transform.position = PersonObj.belong.control.CenterPos();
        UpdateTable();
    }
    /// <summary>
    /// 离开场景
    /// </summary>
    /// <param name="PersonObj"></param>
    public void LeaveTable(PersonObj PersonObj)
    {
        PersonObjs.Remove(PersonObj);
        objs.Remove(PersonObj);
        PersonObj.belong = null;
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
        GameArchitect.get.tableAsset.tableSaver.objs.Remove(obj);
        UpdateTable();
    }

    public PType GetPtype()
    {
        return pddl.GetPType();
    }

    public void InitPDDLClass()
    {
        pddl = PDDLClassGet.Generate(this.GetType());
        pddl.SetObj(this);
    }

    public PDDLClass GetPDDLClass()
    {
        return pddl;
    }
    ~TableModel()
    {
        PDDLClassGet.Remove(pddl);
    }
}
public class TableModelSet : AbstractModel
{
    public List<TableModel> tableModels { get { return GameArchitect.get.tableAsset.tableSaver.tables; } }
    GameObject table;
    public TableModelSet(TableAsset tableAsset)
    {
        table = Resources.Load<GameObject>("Controler/Table");
        for (int i = 0; i < tableModels.Count; i++)
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
        for (int i = 0; i < tableModels.Count; i++)
        {
            tableModels[i].UpdateTable();
        }
        foreach (var obj in GameArchitect.get.tableAsset.tableSaver.objs)
        {
            obj.belong = tableModels.Find(x => { return x.TableName == obj.belong.TableName; });
        }
    }
    protected override void OnInit()
    {
        
    }
    public TableModel Get(string name)
    {
        return tableModels.Find(x => { return name == x.TableName; });
    }
    public void AddTable(int i)
    {
        var data = GameObject.Instantiate<GameObject>(table);
        //Debug.Log(tableModels.Count);
        data.GetComponent<TableControl>().tableModel = tableModels[i];
        tableModels[i].control = data.GetComponent<TableControl>();//它的控制器
        data.GetComponent<TableControl>().Init();
        var root = GameObject.Find("Tables");
        data.transform.SetParent(root.transform);
        data.transform.localPosition = new Vector3(i * 25, 0, 0);
    }
}
