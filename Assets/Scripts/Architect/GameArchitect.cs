using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QFramework;
using UnityEngine;


public class GameArchitect : Architecture<GameArchitect>
{
    public static GameArchitect get
    {
        get { return (GameArchitect)GameArchitect.Interface; }

    }
    public static ActDecisionUI decisionUI;
    public static ActSelectUI selectUI;
    public static List<WinCon> winCons;
    //一系列获得行为
    public static Dictionary<Type, List<Activity>> activities;
    public static List<Person> persons;
    public static GameLogic gameLogic;
    public static Person nowPerson;
    public TableAsset tableAsset;
    public ObjAsset objAsset;
    public Person player;
    //public static Dictionary<string, Activity> actDic;
    protected override void Init()
    {
        winCons = new List<WinCon>();
        this.objAsset = Resources.Load<ObjAsset>("ObjAsset");
        /****************初始化对象*********************/
        MapInit();
        /*********************************************/
        //InitActivities();
        var tableRess = Resources.Load<TableAsset>("Table/TableAsset");
        this.tableAsset = tableRess;
        SaveSystem.Instance.Load();
        persons = tableAsset.tableSaver.personList;
        objAsset.map.Init();
        //for(int i=0;i<persons.Count;i++)
        //{
        //    persons[i].contractManager = new ContractManager(persons[i]);
        //}
        player = persons.Find(e => { return e.isPlayer; });
        GameArchitect.gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        this.RegisterModel<PersonsOptionModel>(new PersonsOptionModel(persons));
        this.RegisterModel<TableModelSet>(new TableModelSet(tableAsset));
        this.RegisterModel<ThinkModelSet>(new ThinkModelSet());
        this.RegisterModel<TimeModel>(new TimeModel());
        this.RegisterModel<EcModel>(new EcModel());
        var cM = tableAsset.tableSaver.contractModel;
        if (cM == null)
        {
            cM = new ContractModel();
            tableAsset.tableSaver.contractModel = cM;
        }
        else
        {
            foreach (var person in GameArchitect.persons)
            {
                if (!cM.aContract.ContainsKey(person))
                {
                    cM.aContract.Add(person, new List<Contract>());
                }
            }
            foreach (var person in GameArchitect.persons)
            {
                if (!cM.bContract.ContainsKey(person))
                {
                    cM.bContract.Add(person, new List<Contract>());
                }
            }
        }
        this.RegisterModel<ContractModel>(cM);
        //DomainGenerator.GenerateDomain();
    }
    public void MapInit()
    {
        Map.Instance.Init();
    }
    public IEnumerator AddDecision(WinCon winCon)
    {
        Debug.Log(winCons.Count);
        winCons.Add(winCon);
        while (winCons.Count > 0)
        {
            yield return CallDecision();
        }
        yield return winCon.data;
    }
    public IEnumerator CallDecision()
    {
        Debug.Log(":"+winCons.Count);
        decisionUI.gameObject.SetActive(false);
        selectUI.gameObject.SetActive(false);
        Debug.Log(">>>>" + winCons.Count);
        if (winCons.Count>0)
        {
            var data = winCons[0];
            winCons.RemoveAt(0);
            Debug.Log(">>>>" + winCons.Count);
            if (data is DecisionTex)
            {
                decisionUI.gameObject.SetActive(true);
                yield return decisionUI.AddDecision((DecisionTex)data);
            }
            else if(data is SelectTex)
            {
                selectUI.gameObject.SetActive(true);
                yield return selectUI.AddDecision((SelectTex)data);
            }
        }
        else
        {
            decisionUI.gameObject.SetActive(false);
            selectUI.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 创建一系列的活动
    /// </summary>
    public void InitActivities()
    {
        activities = new Dictionary<Type, List<Activity>>();
        foreach (Type key in ((Map)Map.Instance).kv.Keys)
        {
            Obj instance = (Obj)Activator.CreateInstance(key, new object[] { null });
            activities.Add(key, instance.InitActivities());
        }
        //actDic = new Dictionary<string, Activity>();
        //Assembly assembly = Assembly.GetExecutingAssembly();
        //var typesWithMapAttribute = assembly.GetTypes()
        //    .Where(t => t.GetCustomAttributes(typeof(ActAttribute), false).Length > 0)
        //    .ToList();
        //foreach (var type in typesWithMapAttribute)
        //{
        //    actDic.Add(type.Name, (Activity)Activator.CreateInstance(type));
        //}
    }
}