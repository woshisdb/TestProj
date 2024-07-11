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
        var cM = tableAsset.tableSaver.contractModel;
        if (cM == null)
        {
            cM = new ContractModel();
            tableAsset.tableSaver.contractModel = cM;
        }
        this.RegisterModel<ContractModel>(cM);
        //DomainGenerator.GenerateDomain();
    }
    public void MapInit()
    {
        Map.Instance.Init();
        //Map.Instance.enum2Type.Add(ObjEnum.PersonE, typeof(Person));
        //Map.Instance.enum2Type.Add(ObjEnum.BedObjE, typeof(BedObj));
        //Map.Instance.enum2Type.Add(ObjEnum.DeskObjE, typeof(DeskObj));
        //Map.Instance.enum2Type.Add(ObjEnum.AnimalObjE, typeof(AnimalObj));
        //Map.Instance.enum2Type.Add(ObjEnum.ObjE, typeof(Obj));
        //Map.Instance.enum2Type.Add(ObjEnum.PathObjE, typeof(PathObj));
        //Map.Instance.enum2Type.Add(ObjEnum.RawObjE, typeof(RawObj));
        //Map.Instance.enum2Type.Add(ObjEnum.FoodObjE, typeof(FoodObj));
        //Map.Instance.enum2Type.Add(ObjEnum.BuildingObjE, typeof(BuildingObj));
        //Map.Instance.enum2Type.Add(ObjEnum.RestaurantObjE, typeof(RestaurantObj));

        //Map.Instance.enum2Type.Add(ObjEnum.TreeObjE, typeof(TreeObj));
        //Map.Instance.enum2Type.Add(ObjEnum.SeedObjE, typeof(SeedObj));
        //Map.Instance.enum2Type.Add(ObjEnum.WheatObjE, typeof(WheatObj));
        //Map.Instance.enum2Type.Add(ObjEnum.WheatSeedObjE, typeof(WheatSeedObj));
        //Map.Instance.enum2Type.Add(ObjEnum.WheatTreeObjE, typeof(WheatTreeObj));
        //Map.Instance.enum2Type.Add(ObjEnum.PlaceObjE, typeof(PlaceObj));
        //Map.Instance.enum2Type.Add(ObjEnum.WheatPlaceObjE, typeof(WheatPlaceObj));
        //Map.Instance.enum2Type.Add(ObjEnum.FullWheatPlaceObjE, typeof(FullWheatPlaceObj));
        ////
        //Map.Instance.ks.Add(typeof(Person), objAsset.personSaver);
        //Map.Instance.ks.Add(typeof(BedObj), objAsset.bedSaver);
        //Map.Instance.ks.Add(typeof(DeskObj), objAsset.deskSaver);
        //Map.Instance.ks.Add(typeof(AnimalObj), objAsset.animalSaver);
        //Map.Instance.ks.Add(typeof(Obj), objAsset.objSaver);
        //Map.Instance.ks.Add(typeof(PathObj), objAsset.pathSaver);
        //Map.Instance.ks.Add(typeof(RawObj), objAsset.rawSaver);
        //Map.Instance.ks.Add(typeof(FoodObj), objAsset.foodSaver);
        //Map.Instance.ks.Add(typeof(BuildingObj), objAsset.buildingSaver);
        //Map.Instance.ks.Add(typeof(RestaurantObj), objAsset.restaurantSaver);
        //Map.Instance.ks.Add(typeof(TreeObj), objAsset.treeSaver);
        //Map.Instance.ks.Add(typeof(SeedObj), objAsset.seedSaver);
        //Map.Instance.ks.Add(typeof(WheatTreeObj), objAsset.wheatSaver);
        //Map.Instance.ks.Add(typeof(WheatTreeObj), objAsset.wheatTreeSaver);
        //Map.Instance.ks.Add(typeof(WheatObj), objAsset.wheatSaver);
        //Map.Instance.ks.Add(typeof(WheatPlaceObj), objAsset.wheatPlaceSaver);
        //Map.Instance.ks.Add(typeof(FullWheatPlaceObj), objAsset.fullWheatPlaceSaver);
        //Map.Instance.ks.Add(typeof(PlaceObj), objAsset.placeSaver);
        ////
        //Map.Instance.enum2Ins.Add(ObjEnum.PersonE,new Person());
        //Map.Instance.enum2Ins.Add(ObjEnum.BedObjE, new BedObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.DeskObjE, new DeskObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.AnimalObjE, new AnimalObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.ObjE,  new Obj());
        //Map.Instance.enum2Ins.Add(ObjEnum.PathObjE, new PathObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.RawObjE,  new RawObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.FoodObjE, new FoodObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.BuildingObjE, new BuildingObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.RestaurantObjE, new RestaurantObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.SeedObjE, new SeedObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.TreeObjE, new TreeObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.WheatObjE, new WheatObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.WheatSeedObjE, new WheatSeedObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.WheatTreeObjE, new WheatTreeObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.PlaceObjE, new PlaceObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.WheatPlaceObjE, new WheatPlaceObj());
        //Map.Instance.enum2Ins.Add(ObjEnum.FullWheatPlaceObjE, new FullWheatPlaceObj());
    }
    public IEnumerator AddDecision(WinCon winCon)
    {
        Debug.Log(winCons.Count);
        winCons.Add(winCon);
        while (winCons.Count > 0)
        {
            yield return CallDecision();
        }
    }
    public IEnumerator CallDecision()
    {
        decisionUI.gameObject.SetActive(false);
        selectUI.gameObject.SetActive(false);
        if (winCons.Count>0)
        {
            var data = winCons[0];
            winCons.RemoveAt(0);
            if(data is DecisionTex)
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