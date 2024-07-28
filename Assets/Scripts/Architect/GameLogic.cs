using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Michsky.MUIP;
using PimDeWitte.UnityMainThreadDispatcher;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 更新环境
/// </summary>
public class EnvirUpdateEvent
{

}
/// <summary>
/// 开始选择活动
/// </summary>
public class BeginSelectEvent
{

}
public class EndSelectEvent
{
}
public enum CodeSystemEnum
{
    week,
    month,
    year,
}
public class CodeSystemData
{
    public string name;
    /// <summary>
    /// 类型
    /// </summary>
    public CodeSystemEnum code;
    public List<int> week;
    public List<CodeData> work;
    /// <summary>
    /// 今天的工作
    /// </summary>
    public List<List<string>> dayWork;
    public CodeData GetNowWork()
    {
        var t = GameArchitect.get.GetModel<TimeModel>();
        if (code== CodeSystemEnum.week)
        {
            return work.Find((x) => {return x.codeName== dayWork[week[t.GetWeek()]][t.GetDay()]; }); 
        }
        else if(code== CodeSystemEnum.month)
        {
            return work.Find((x) => { return x.codeName == dayWork[week[t.GetMonth()]][t.GetDay()]; });
        }
        else
        {
            return work.Find((x) => { return x.codeName == dayWork[week[t.GetYear()]][t.GetDay()]; });
        }
    }
    public CodeData GetNowWork(int time)
    {
        var t = GameArchitect.get.GetModel<TimeModel>();
        if (code == CodeSystemEnum.week)
        {
            return work.Find((x) => { return x.codeName == dayWork[week[t.GetWeek(time)]][t.GetDay(time)]; });
        }
        else if (code == CodeSystemEnum.month)
        {
            return work.Find((x) => { return x.codeName == dayWork[week[t.GetMonth(time)]][t.GetDay(time)]; });
        }
        else
        {
            return work.Find((x) => { return x.codeName == dayWork[week[t.GetYear(time)]][t.GetDay(time)]; });
        }
    }
    public CodeSystemData()
    {
        work= new List<CodeData>();
        dayWork= new List<List<string>>();
    }
    [Button]
    public void AddDayWork()
    {
        var t = new List<string>();
        for(int i=0;i<48;i++)
        {
            t.Add("empty");
        }
        dayWork.Add(t);
    }
    public virtual IEnumerator EditCodeSystem(Person person, Obj obj, List<Activity> activities)
    {
        yield break;
    }
}
public class CodeSystemDataWeek: CodeSystemData
{
    public CodeSystemDataWeek():base()
    {
        code = CodeSystemEnum.week;
        week = new List<int>(7);
        for(int i = 0; i < 7; i++)
        { week.Add(0); }
    }
    public override IEnumerator EditCodeSystem(Person person,Obj obj,List<Activity> activities)
    {
        List<CardInf> sels = new List<CardInf>();
        Activity activity = null;
        foreach(var x in activities)
        {
            var data = x;
            sels.Add(new CardInf("选择:"+data.activityName,data.detail,
                () =>
                {
                    activity = data;
                }
            ));
        }

        yield return GameArchitect.gameLogic.AddDecision(person,new DecisionTex(
        "工作的内容","选择工作的内容进行工作",sels));
        var w=work.Find((x) => { return x.codeName == "工作"; });
        Person tempPerson = new Person();//自建Person
        BuildingObj buildingObj=new BuildingObj();
        w.activity = activity;
        List<WinData> winDatas = new List<WinData>();
        var eff=activity.Effect(buildingObj,person,winDatas);
        tempPerson.SetAct(eff);
        tempPerson.isPlayer = true;
        Debug.Log(eff.GetType().Name);
        bool hasTime = GameLogic.hasTime;
        GameLogic.hasTime = true;
        while (tempPerson.hasSelect.val==true)
        {
             yield return tempPerson.act.Run(
                (result) => {
                    if (result is EndAct)
                        tempPerson.RemoveAct();
                    else if (result is Act)
                        tempPerson.SetAct((Act)result);
                }
            );
        }
        GameLogic.hasTime = hasTime;
        ///选择一系列活动
        w.wins = winDatas;
    }
}
public class CodeSystemDataMonth : CodeSystemData
{
    public CodeSystemDataMonth() : base()
    {
        code = CodeSystemEnum.month;
        week = new List<int>(30);
        for (int i = 0; i < 30; i++)
        { week.Add(0); }
    }
}
public class CodeSystemDataYear : CodeSystemData
{
    public CodeSystemDataYear() : base()
    {
        code = CodeSystemEnum.year;
        week = new List<int>(360);
        for (int i = 0; i < 360; i++)
        { week.Add(0); }
    }
}

/// <summary>
/// 活动结构，时间和活动
/// </summary>
[System.Serializable]
public class CodeData
{
    /// <summary>
    /// 代码的名字
    /// </summary>
    public string codeName;
    /// <summary>
    /// 当前的对象
    /// </summary>
    [ValueDropdown("Objs")]
    public Obj obj;
    /// <summary>
    /// 需要执行的活动
    /// </summary>
    public Activity activity;
    [SerializeField]
    /// <summary>
    /// 需要连续执行的活动
    /// </summary>
    public List<WinData> wins;
    public CodeData()
    {
        wins = new List<WinData>();
    }
    public static IEnumerable Objs()
    {
        var ret=new ValueDropdownList<Obj>();
        foreach(var x in GameArchitect.get.tableAsset.tableSaver.objs)
        {
            ret.Add(x.name,x);
        }
        return ret;
    }
}

/// <summary>
/// 选择行为
/// </summary>
public class WinData
{
    
}

public class SelData:WinData
{
    public List<System.Tuple<string, int>> selects;//选择的行为
    public SelData()
    {
        selects = new List<System.Tuple<string, int>>();
    }
}

public class DecData : WinData
{
    public string selc;
}

public struct BeginActEvent
{
    public Person Person;
    public BeginActEvent(Person person)
    {
        this.Person = person;
    }
}
public struct EndActEvent
{
    public Person Person;
    public EndActEvent(Person person)
    {
        this.Person = person;
    }
}
public class SelectCardEvent
{
    public CardInf card;
    public Person person;
    public SelectCardEvent(CardInf card, Person person)
    {
        this.card = card;
        this.person = person;
    }
}
public class GameLogic : MonoBehaviour,ICanRegisterEvent
{
    public static OptionUIEnum optionUIEnum;
    public static bool isCoding=false;
    //相机
    public Camera camera;
    public static WindowManager windowsmanager;
    public static List<UnityAction> unityActions;
    protected TaskCompletionSource<bool> tcs;
    public static bool hasTime;
    public CodeControler codeControler;
    // Start is called before the first frame update
    public IEnumerator WindowSwitch()
    {
        while (true)
        {
            if (unityActions.Count > 0)
            {
                unityActions[0].Invoke();
                unityActions.RemoveAt(0);
                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }
    public void Awake()
    {
        //codeControler = GameObject.Find("CodeControler").GetComponent<CodeControler>();
        windowsmanager = GameObject.Find("Window Manager").GetComponent<WindowManager>();
        windowsmanager.onWindowChange.AddListener(e=> { OnWinChange(e); });
        unityActions = new List<UnityAction>();
        StartCoroutine(WindowSwitch());
    }
    public void OnWinChange(int win)//待修改
    {
        Debug.Log(win);
        if (win == 2)
        {
            GameLogic.isCoding = true;
            //-----------------------GameArchitect.gameLogic.codeControler.InitObjCards();
        }
        else
        {
            //GameArchitect.gameLogic.codeControler.nowObj = null;
            //GameArchitect.gameLogic.codeControler.nowActivity = null;
            //GameArchitect.gameLogic.codeControler.nowAct.text = "";
            GameLogic.isCoding = false;
        }
    }
    public static void ToNoSelect()
    {
        GameLogic.optionUIEnum = OptionUIEnum.NoSelect;
        unityActions.Add(()=> { windowsmanager.OpenWindowByIndex(1); });
    }
    public static void ToSelect()
    {
        GameLogic.optionUIEnum = OptionUIEnum.Select;
        unityActions.Add(() => { windowsmanager.OpenWindowByIndex(0); });
    }
    public static void ToCoding()
    {
        GameLogic.isCoding = true;
        //----------------------GameArchitect.gameLogic.codeControler.InitObjCards();
        unityActions.Add(() => {
        windowsmanager.OpenWindowByIndex(2);
        });
    }
    public static void NotCoding()
    {
        GameLogic.isCoding = false;
        unityActions.Add(() => {
            windowsmanager.OpenWindowByIndex(0);
        });
    }
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }

    public void Start()
    {
        this.RegisterEvent<EnvirUpdateEvent>(e=> { UpdateEnvir(e); });
        this.RegisterEvent<BeginSelectEvent>(e=> { BeginSelectEvent(e); });
        this.RegisterEvent<EndActEvent>(
            e =>
            {
                tcs.TrySetResult(true);
            }
        );
        this.SendEvent<EnvirUpdateEvent>();//开始执行
        camera.transform.position = GameArchitect.get. player.belong.control.CenterPos();
    }
    //更新环境信息
    public void UpdateEnvir(EnvirUpdateEvent envirUpdateEvent)
    {
        MainDispatch.Instance().Enqueue(
            () => {
                //Debug.Log(245);
                foreach (var t in GameArchitect.get.tableAsset.tableSaver.objs)//选择后更新
                {
                    t.LatUpdate();
                }
                GameArchitect.Interface.GetModel<TimeModel>().AddTime();
                foreach (var t in GameArchitect.get.tableAsset.tableSaver.objs)//选择前更新
                {
                    t.BefUpdate();
                }
                this.SendEvent<BeginSelectEvent>(new BeginSelectEvent());
                this.SendEvent<UpdateCode>(new UpdateCode(GameArchitect.get.player));
            }
        );
    }
    public IEnumerator AddDecision(Person person,WinCon decision)
    {
        Debug.Log(person.name+","+decision.ToString());
        if(person.isPlayer)
        {
            yield return GameArchitect.get.AddDecision(decision);
        }
        else//调用游戏的决策系统
        {
            //yield return GameArchitect.decisionUI.AddDecision(decision.title, decision.description, decision.cards);
        }
    }
    public int CmpPerson(Person p1,Person p2)
    {
        if(p1.act.priority<p2.act.priority)
        {
            return -1;
        }
        else if(p1.act.priority>p2.act.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    [Button]
    public void AddObj(ObjEnum objEnum,string SceneName,ObjSaver objSaver=null)
    {
        var type = Map.Instance.enum2Type[objEnum];
        var val = (Obj)Activator.CreateInstance(type, new object[] { objSaver });
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        Sc.AddToTable(val);
    }

    [Button]
    public void AddResource(string name,ObjEnum objEnum,int num)
    {
        var obj = (BuildingObj)GameArchitect.get.tableAsset.tableSaver.objs.Find((x) => { return x.name == name; });
        if (obj == null)
            Debug.Log("没有");
        else
            obj.resource.Add(objEnum, num);
    }
    /// <summary>
    /// 初始角色可选对象
    /// </summary>
    /// <param name="beginSelectEvent"></param>
    /// <returns></returns>
    public async void BeginSelectEvent(BeginSelectEvent beginSelectEvent)
    {
        //Debug.Log(789);
        var optionModel = GameArchitect.Interface.GetModel<PersonsOptionModel>();
        var modelset = GameArchitect.Interface.GetModel<TableModelSet>();
        for (int i=0;i<GameArchitect.persons.Count;i++)//遍历每个person
        {
            var person=GameArchitect.persons[i];//当前角色
            GameArchitect.nowPerson = person;//当前角色
            if (!person.hasSelect.val)//没有任务就初始化可执行的活动
            {
                var result = new Dictionary<Obj,CardInf[]>();
                for (int j = 0; j < person.belong.objs.Count; j++)
                {
                    List<CardInf> cardInfList = new List<CardInf>();
                    for (int l = 0; l < person.belong.objs[j].activities.Count; l++)
                    {
                        var res = person.belong.objs[j].activities[l].OutputSelect(person, person.belong.objs[j]);//一个可选项
                        cardInfList.Add(res);
                    }
                    result[person.belong.objs[j]] = cardInfList.ToArray();
                }
                MainDispatch.Instance().Enqueue(
                () =>
                {
                    optionModel.SetPersonOption(person, result);//当前的行为
                });
                await GameArchitect.Interface.GetModel<ThinkModelSet>().thinks[person].BeginThink(result);//等待思考完成
            }
        }
        GameArchitect.persons.Sort((p1, p2) => { return CmpPerson(p1, p2); });
        //更新角色活动,每个角色都执行一遍
        foreach(var person in GameArchitect.persons)
        {
            tcs = new TaskCompletionSource<bool>(false);
            MainDispatch.Instance().Enqueue(
                () =>
                {
                    hasTime = true;
                    this.SendEvent<BeginActEvent>(new BeginActEvent(person));
                }
            );
            await tcs.Task;
        }
        MainDispatch.Instance().Enqueue(
            () =>
            {
                //更新时间
                this.SendEvent<EnvirUpdateEvent>(new EnvirUpdateEvent());
            });
    }
    [Button]
    public void CreatePerson(bool isplayer, string name, bool sex, string SceneName)
    {
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        CreatePerson(isplayer, name, sex, Sc);
    }
    public void CreatePerson(bool isplayer, string name, bool sex, TableModel Sc)
    {
        var s = Tool.DeepClone<PersonSaver>((PersonSaver)Map.Instance.GetSaver(typeof(Person)));
        s.isPlayer = isplayer;
        s.sex = sex;
        s.PersonName =name;
        var p = new Person(s);        
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.personList.Add(p);//创建对象
        ((GameArchitect)GameArchitect.Interface).GetModel<PersonsOptionModel>().AddPerson(p);
        ((GameArchitect)GameArchitect.Interface).GetModel<ThinkModelSet>().AddThinkMode(p);
        Sc.EnterTable(p);
    }
    /// <summary>
    /// 创建一个床
    /// </summary>
    [Button]
    public void CreateBed(string SceneName)
    {
        var s = Tool.DeepClone<BedSaver>((BedSaver)Map.Instance.GetSaver(typeof(BedObj)));
        var p = new BedObj(s);
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        Sc.AddToTable(p);
    }
    [Button]
    public void CreateDesk(string SceneName)
    {
        var s = Tool.DeepClone<DeskSaver>((DeskSaver)Map.Instance.GetSaver(typeof(DeskObj)));
        var p = new DeskObj(s);
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        Sc.AddToTable(p);
    }
    [Button]
    public void CreateRestr(string SceneName)
    {
        var s = Tool.DeepClone<RestaurantSaver>((RestaurantSaver)Map.Instance.GetSaver(typeof(RestaurantObj)));
        var p = new RestaurantObj(s);
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        Sc.AddToTable(p);
    }
    //[Button]
    //public void CreatePath(PathObj pathObj,string SceneName)
    //{
    //    var pathIbj = (PathObj)Activator.CreateInstance(pathObj.GetType(), new object[] { new PathSaver() });
    //    var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
    //    if (Sc == null)
    //    {
    //        Debug.Log("无法创建");
    //        return;
    //    }
    //    Sc.AddToTable(pathIbj);
    //}
    [Button]
    public void DestoryObj(string objName,string tableName)
    {
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == tableName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        var x=Sc.objs.Find(e => { return objName == e.name; });
        Sc.RemoveToTable(x);
    }
    public struct CreatePathObj
    {
        public string y;
        public int time;
    }
    [Button]
    public void CreatePath(string objName, string SceneName)
    {
        var s = Tool.DeepClone<PathSaver>((PathSaver)Map.Instance.GetSaver(ObjEnum.PathObjE));
        var p = new PathObj(s);
        p.cardInf.title = objName;
        p.cardInf.description = "PathToOtherScene";
        p.activities = new List<Activity>();
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        Sc.AddToTable(p);
    }
    [Button]
    public void DestoryPath(string objName, string tableName)
    {
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == tableName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        var x = (PathObj)Sc.objs.Find(e => { return objName == e.name; });
        x.activities.RemoveAll(e => { return ((Go)e).x == Sc; });//删除所有目的地节点
        Sc.RemoveToTable(x);
    }
    /// <summary>
    /// 创建农场
    /// </summary>
    /// <param name="size"></param>
    /// <param name="tableName"></param>
    [Button]
    public void CreateFarm(FarmSaver farmSaver,string tableName)
    {
        var p = new FarmObj(farmSaver);
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == tableName; });
        if (Sc == null)
        {
            Debug.Log("无法创建");
            return;
        }
        Sc.AddToTable(p);
    }
}
