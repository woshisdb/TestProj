using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Michsky.MUIP;
using PimDeWitte.UnityMainThreadDispatcher;
using QFramework;
using Sirenix.OdinInspector;
using System.Text.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Newtonsoft.Json;
/// <summary>
/// ���»���
/// </summary>
public class EnvirUpdateEvent
{

}
/// <summary>
/// ��ʼѡ��
/// </summary>
public class BeginSelectEvent
{

}
public class EndSelectEvent
{
}
/// <summary>
/// ��������Ĺ���
/// </summary>
public class CodeSystemData
{
    /// <summary>
    /// ÿһ��Ĺ���
    /// </summary>
    public int nowTime;
    public string name;
    /// <summary>
    /// 
    /// </summary>
    public List<List<CodeData>> work;
    /// <summary>
    /// ��ȡ��ǰ�Ĺ����б�
    /// </summary>
    /// <returns></returns>
    public List<CodeData> GetNowWorks()
    {
        return work[nowTime];
    }
    public void NextDay()
    {
        nowTime=(nowTime+1)%work.Count;
    }
    public CodeSystemData()
    {
        work= new List<List<CodeData>>();
        nowTime=0;
    }
    //public virtual IEnumerator EditCodeSystem(PersonObj PersonObj, Obj obj,CodeData codeData)
    //{
    //    List<Activity> activities = obj.InitActivities();
    //    List<CardInf> sels = new List<CardInf>();
    //    Activity activity = null;
    //    foreach (var x in activities)
    //    {
    //        var data = x;
    //        sels.Add(new CardInf("ѡ��:" + data.activityName, data.detail,
    //            () =>
    //            {
    //                activity = data;
    //            }
    //        ));
    //    }

    //    yield return GameArchitect.gameLogic.AddDecision(PersonObj, new DecisionTex(
    //    "����������", "ѡ���������ݽ��й���", sels));
    //    foreach (var x in work)
    //    {
    //        x.obj = obj;
    //    }
    //    /////////////////////////////////////////////////////////////////
    //    yield return GetActDec(codeData, obj, activity);
    //}
    //public IEnumerator GetActDec(CodeData codeData, Obj obj,Activity activity)
    //{
    //    var w= codeData;
    //    PersonObj tempPersonObj = new PersonObj();//�Խ�PersonObj
    //    BuildingObj buildingObj = new BuildingObj();
    //    w.activity = activity;
    //    List<WinData> winDatas = new List<WinData>();
    //    var eff = activity.Effect(obj, tempPersonObj, winDatas);
    //    tempPersonObj.SetAct(eff);
    //    tempPersonObj.isPlayer = true;
    //    Debug.Log(eff.GetType().Name);
    //    bool hasTime = GameLogic.hasTime;
    //    while (tempPersonObj.hasSelect == true)
    //    {
    //        GameLogic.hasTime = true;
    //        yield return tempPersonObj.act.Run(
    //            (result) => {
    //                if (result is EndAct)
    //                    tempPersonObj.RemoveAct();
    //                else if (result is Act)
    //                    tempPersonObj.SetAct((Act)result);
    //            }
    //        );
    //    }
    //    GameLogic.hasTime = hasTime;
    //    ///ѡ��һϵ�л
    //    w.wins = winDatas;
    //}
}
//public class CodeSystemDataWeek: CodeSystemData
//{
//    public CodeSystemDataWeek():base()
//    {
//        code = CodeSystemEnum.week;
//        week = new List<int>(7);
//        for(int i = 0; i < 7; i++)
//        { week.Add(0); }
//    }
//    public override IEnumerator EditCodeSystem(PersonObj PersonObj,Obj obj, CodeData codeData)
//    {
//        List<Activity> activities = obj.InitActivities();
//        List<CardInf> sels = new List<CardInf>();
//        Activity activity = null;
//        foreach(var x in activities)
//        {
//            var data = x;
//            sels.Add(new CardInf("ѡ��:"+data.activityName,data.detail,
//                () =>
//                {
//                    activity = data;
//                }
//            ));
//        }

//        yield return GameArchitect.gameLogic.AddDecision(PersonObj,new DecisionTex(
//        "����������","ѡ���������ݽ��й���",sels));
//        foreach(var x in work)
//        {
//            x.obj = obj;
//        }
//        /////////////////////////////////////////////////////////////////
//        yield return GetActDec(codeData, obj,activity);
//    }
//}

//public class CodeSystemDataMove : CodeSystemData
//{
//    public CodeSystemDataMove() : base()
//    {
//        code = CodeSystemEnum.week;
//        week = new List<int>(7);
//        for (int i = 0; i < 7; i++)
//        { week.Add(0); }
//    }
//    public override IEnumerator EditCodeSystem(PersonObj PersonObj, Obj obj, CodeData codeData)
//    {
//        List<Activity> activities = obj.InitActivities();
//        List<CardInf> sels = new List<CardInf>();
//        Activity activity = null;
//        foreach (var x in activities)
//        {
//            var data = x;
//            sels.Add(new CardInf("ѡ��:" + data.activityName, data.detail,
//                () =>
//                {
//                    activity = data;
//                }
//            ));
//        }

//        yield return GameArchitect.gameLogic.AddDecision(PersonObj, new DecisionTex(
//        "����������", "ѡ���������ݽ��й���", sels));
//        foreach (var x in work)
//        {
//            x.obj = obj;
//        }
//        var w = work.Find((x) => { return x.codeName == "����"; });
//        PersonObj tempPersonObj = new PersonObj();//�Խ�PersonObj
//        BuildingObj buildingObj = new BuildingObj();
//        w.activity = activity;
//        List<WinData> winDatas = new List<WinData>();
//        var eff = activity.Effect(obj, tempPersonObj, winDatas);
//        tempPersonObj.SetAct(eff);
//        tempPersonObj.isPlayer = true;
//        Debug.Log(eff.GetType().Name);
//        bool hasTime = GameLogic.hasTime;
//        while (tempPersonObj.hasSelect == true)
//        {
//            GameLogic.hasTime = true;
//            yield return tempPersonObj.act.Run(
//                (result) => {
//                    if (result is EndAct)
//                        tempPersonObj.RemoveAct();
//                    else if (result is Act)
//                        tempPersonObj.SetAct((Act)result);
//                }
//            );
//        }
//        GameLogic.hasTime = hasTime;
//        ///ѡ��һϵ�л
//        w.wins = winDatas;
//    }
//}



/// <summary>
/// ѡ����Ϊ
/// </summary>
public class WinData
{
    
}

public class SelData:WinData
{
    public List<System.Tuple<string, int>> selects;//ѡ�����Ϊ
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
    public PersonObj PersonObj;
    public BeginActEvent(PersonObj PersonObj)
    {
        this.PersonObj = PersonObj;
    }
}
public struct EndActEvent
{
    public PersonObj PersonObj;
    public EndActEvent(PersonObj PersonObj)
    {
        this.PersonObj = PersonObj;
    }
}
public class SelectCardEvent
{
    public CardInf card;
    public PersonObj PersonObj;
    public SelectCardEvent(CardInf card, PersonObj PersonObj)
    {
        this.card = card;
        this.PersonObj = PersonObj;
    }
}
public class GameLogic : MonoBehaviour,ICanRegisterEvent
{
    public static OptionUIEnum optionUIEnum;
    public static bool isCoding=false;
    //���
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
    [Button]
    public void Refresh()
    {
        var cnodes=PDDLClassGenerater.Refresh();
        foreach(var x in cnodes)
        {
            PDDLUIMenu.PDDLGen(x);
        }

    }
    public void OnWinChange(int win)//���޸�
    {
        //Debug.Log(win);
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
        this.SendEvent<EnvirUpdateEvent>();//��ʼִ��
        if (GameArchitect.get.player != null)
        {
            camera.transform.position = GameArchitect.get.player.belong.control.CenterPos();
        }
    }
    //���»�����Ϣ
    public void UpdateEnvir(EnvirUpdateEvent envirUpdateEvent)
    {
        MainDispatch.Instance().Enqueue(
            () => {
                //Debug.Log(245);
                foreach (var t in GameArchitect.get.tableAsset.tableSaver.objs)//ѡ������
                {
                    t.LatUpdate();
                }
                GameArchitect.Interface.GetModel<TimeModel>().AddTime();
                foreach (var t in GameArchitect.get.tableAsset.tableSaver.objs)//ѡ��ǰ����
                {
                    t.BefUpdate();
                }
                this.SendEvent<BeginSelectEvent>(new BeginSelectEvent());
                this.SendEvent<UpdateCode>(new UpdateCode(GameArchitect.get.player));
            }
        );
    }
    public IEnumerator AddDecision(PersonObj PersonObj,WinCon decision)
    {
        if (PersonObj.codeData != null && PersonObj.codeData.wins != null && PersonObj.codeData.wins.Count > 0)
        {
            decision.Decision(PersonObj.codeData.wins[0]);
            PersonObj.codeData.wins.RemoveAt(0);
        }
        else
        {
            Debug.Log(PersonObj.name + "," + decision.ToString());
            if (PersonObj.isPlayer)
            {
                yield return GameArchitect.get.AddDecision(decision);
            }
            else//������Ϸ�ľ���ϵͳ
            {
                //yield return GameArchitect.decisionUI.AddDecision(decision.title, decision.description, decision.cards);
            }
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
            Debug.Log("�޷�����");
            return;
        }
        Sc.AddToTable(val);
    }
    [Button]
    public void GetPDDLAsync()
    {
		var ret = Map.InitPDDL();
        string domainStr = ret.Item1.Print();
        string problemStr = ret.Item2.Print();
  //      Debug.Log(ret.Item1.Print());
		////File.WriteAllText($"Assets/Resources/PDDL/Domain.txt", ret.Item1.Print());
		//Debug.Log(ret.Item2.Print());
		//File.WriteAllText($"Assets/Resources/PDDL/Problem.txt", ret.Item2.Print());
		//AssetDatabase.Refresh();
		string url = "http://localhost:8080/run";
        //StartCoroutine(GetRequest(url));
        SendPostRequestAsync(url,domainStr,problemStr);
        //Debug.Log("Response Content: " + response);
        //Expression<Func<PersonObj, int>> func = (a) => a.resource.maxSize;
        ////TraverseExpression(func);
        //TraverseExpression(func.Body);
    }
    [System.Serializable]
    private class ActionListWrapper
    {
        public List<List<string>> actions;
    }
    public static List<List<string>> ParseJsonString(string jsonString)
    {
        // ʹ��JsonConvert.DeserializeObject��JSON�ַ�������ΪList<List<string>>
        List<List<string>> parsedData = JsonConvert.DeserializeObject<List<List<string>>>(jsonString);
        return parsedData;
    }
    public static async Task<HttpResponseMessage> SendPostRequestAsync(string url, string domainStr, string problemStr)
    {
        using (HttpClient client = new HttpClient())
        {
			//string domainFilePath = Path.Combine(Application.dataPath, "domain.pddl");
			//string problemFilePath = Path.Combine(Application.dataPath, "problem.pddl");

			//// ��ȡ�ļ�����
			//string domainContent = File.ReadAllText(domainFilePath);
			//Debug.Log(domainContent);
			//string problemContent = File.ReadAllText(problemFilePath);
			//Debug.Log(problemContent);

			string domainContent = domainStr;
			string problemContent = problemStr;

			var data = new MultipartFormDataContent();
            data.Add(new StringContent(domainContent), "x");
            data.Add(new StringContent(problemContent),"y");
            HttpResponseMessage response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                // ��ȡ��Ӧ����
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                var result= ParseJsonString(responseBody);
                Debug.Log(result.Count);
			}
            return response;
        }
    }
    //IEnumerator GetRequest(string url)
    //{
    //    // �����ļ�·��
    //    string domainFilePath = Path.Combine(Application.dataPath, "domain.pddl");
    //    string problemFilePath = Path.Combine(Application.dataPath, "problem.pddl");

    //    // ��ȡ�ļ�����
    //    string domainContent = File.ReadAllText(domainFilePath);
    //    string problemContent = File.ReadAllText(problemFilePath);

    //    // ����һ�������ݣ�������MultipartFormDataContent
    //    WWWForm form = new WWWForm();
    //    form.AddField("x", domainContent);
    //    form.AddField("y", problemContent);

    //    // ʹ�� UnityWebRequest ����POST����
    //    UnityWebRequest www = UnityWebRequest.Post(url, form);

    //    // �������󲢵ȴ���Ӧ
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError("POST����ʧ��: " + www.error);
    //    }
    //    else
    //    {
    //        Debug.Log(www.downloadHandler.text);
    //        List<List<string>> result = ParseJsonString(www.downloadHandler.text);
    //        // ��ӡ�����������
    //    if (result != null)
    //    {
    //        foreach (var itemList in result)
    //        {
    //            Debug.Log(string.Join(", ", itemList));
    //        }
    //    }
    //        //Debug.Log("POST����ɹ�����������Ӧ: " + www.downloadHandler.text);
    //    }
    //}
    //private static void TraverseExpression(Expression expr)
    //{
    //    if (expr.NodeType == ExpressionType.Lambda)
    //    {
    //        var lambda = (LambdaExpression)expr;
    //        Debug.Log("Lambda: " + lambda);
    //        TraverseExpression(lambda.Body);
    //        foreach (var param in lambda.Parameters)
    //        {
    //            Debug.Log("Parameter: " + param.Name + " Type: " + param.Type);
    //        }
    //    }
    //    else if (expr.NodeType == ExpressionType.GreaterThan ||
    //             expr.NodeType == ExpressionType.LessThan ||
    //             expr.NodeType == ExpressionType.Equal ||
    //             expr.NodeType == ExpressionType.NotEqual)
    //    {
    //        var binaryExpr = (BinaryExpression)expr;
    //        Debug.Log("Binary Expression: " + binaryExpr.NodeType);
    //        TraverseExpression(binaryExpr.Left);
    //        TraverseExpression(binaryExpr.Right);
    //    }
    //    else if (expr.NodeType == ExpressionType.Parameter)
    //    {
    //        var paramExpr = (ParameterExpression)expr;
    //        Debug.Log("Parameter: " + paramExpr.Name + " Type: " + paramExpr.Type);
    //    }
    //    else if (expr.NodeType == ExpressionType.Constant)
    //    {
    //        var constExpr = (ConstantExpression)expr;
    //        Debug.Log("Constant: " + constExpr.Value + " Type: " + constExpr.Type);
    //    }
    //    else if (expr.NodeType == ExpressionType.MemberAccess)
    //    {
    //        var memberExpr = (MemberExpression)expr;
    //        Debug.Log("Member Access: " + memberExpr.Member.Name);
    //        TraverseExpression(memberExpr.Expression);
    //    }
    //    else if (expr.NodeType == ExpressionType.Call)
    //    {
    //        var callExpr = (MethodCallExpression)expr;
    //        Debug.Log("Method Call: " + callExpr.Method.Name);
    //        foreach (var arg in callExpr.Arguments)
    //        {
    //            TraverseExpression(arg);
    //        }
    //        TraverseExpression(callExpr.Object);
    //    }
    //    else
    //    {
    //        Debug.Log("Unhandled expression type: " + expr.NodeType);
    //    }
    //}
    private static Pop TraverseExpression(Expression expr)
    {
        Pop ret = null;
        if (expr.NodeType == ExpressionType.Lambda)
        {
            var lambda = (LambdaExpression)expr;
            Debug.Log("Lambda: " + lambda);
            TraverseExpression(lambda.Body);
            foreach (var param in lambda.Parameters)
            {
                Debug.Log("Parameter: " + param.Name + " Type: " + param.Type);
            }
        }
        else if (expr.NodeType == ExpressionType.GreaterThan)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Great(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.GreaterThanOrEqual)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new GreatEqual(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.LessThan)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Less(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.LessThanOrEqual)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new LessEqual(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.Equal)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Equal(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.Add)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Add(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.Multiply)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Mul(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.Subtract)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Subtract(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.Divide)
        {
            var binaryExpr = (BinaryExpression)expr;
            Debug.Log("Binary Expression: " + binaryExpr.NodeType);
            ret = new Div(TraverseExpression(binaryExpr.Left), TraverseExpression(binaryExpr.Right));
        }
        else if (expr.NodeType == ExpressionType.Parameter)
        {
            var paramExpr = (ParameterExpression)expr;
            Debug.Log("Parameter: " + paramExpr.Name + " Type: " + paramExpr.Type);
        }
        else if (expr.NodeType == ExpressionType.Constant)
        {
            var constExpr = (ConstantExpression)expr;
            Debug.Log("Constant: " + constExpr.Value + " Type: " + constExpr.Type);
            if (constExpr.Value is int)
                ret = new I((int)constExpr.Value);
            else if (constExpr.Value is float)
                ret = new I((float)constExpr.Value);
        }
        else if (expr.NodeType == ExpressionType.MemberAccess)
        {
            var memberExpr = (MemberExpression)expr;
            Debug.Log("Member Access: " + memberExpr.Member.Name);
            TraverseExpression(memberExpr.Expression);
        }
        else if (expr.NodeType == ExpressionType.Call)
        {
            var callExpr = (MethodCallExpression)expr;
            Debug.Log("Method Call: " + callExpr.Method.Name);
            foreach (var arg in callExpr.Arguments)
            {
                TraverseExpression(arg);
            }
            TraverseExpression(callExpr.Object);
        }
        else
        {
            Debug.Log("Unhandled expression type: " + expr.NodeType);
        }
        return ret;
    }
    [Button]
    public void AddResource(string name,ObjEnum objEnum,int num)
    {
        var obj = (BuildingObj)GameArchitect.get.tableAsset.tableSaver.objs.Find((x) => { return x.name == name; });
        if (obj == null)
            Debug.Log("û��");
        else
            obj.resource.Add(objEnum, num);
    }

    /// <summary>
    /// ����npc����Ϊ
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdataNPC()
    {
        for (int i = 0; i < GameArchitect.npcs.Count; i++)//����ÿ��PersonObj
        {
            var npc = GameArchitect.npcs[i];
            //�����ǰ�й�������ֱ���ҹ���
            if (npc.HasWork())
            {
                var execWork = npc.lifeStyle.code.GetNowWorks();//��ȡһϵ�д�ִ�еĻ
                foreach (var work in execWork)
                {
                    yield return StartCoroutine(work.activity.GetNPCAct().Run());//�ȴ����Э��ִ��
                }
            }
            else
            {
                //�Ȳ���
            }
        }
        yield break;
    }

    public void NPCUpdateCircle()
    {
        StartCoroutine(UpdataNPC());
    }
    /// <summary>
    /// ��ʼ��ɫ��ѡ����
    /// </summary>
    /// <param name="beginSelectEvent"></param>
    /// <returns></returns>
    public async void BeginSelectEvent(BeginSelectEvent beginSelectEvent)
    {
        //Debug.Log(789);
        var optionModel = GameArchitect.Interface.GetModel<PersonObjsOptionModel>();
        var modelset = GameArchitect.Interface.GetModel<TableModelSet>();
        //����ÿһ��Person�ľ���
        for (int i=0;i<GameArchitect.PersonObjs.Count;i++)//����ÿ��PersonObj
        {
            var PersonObj=GameArchitect.PersonObjs[i];//��ǰ��ɫ
            GameArchitect.nowPersonObj = PersonObj;//��ǰ��ɫ
            if (!PersonObj.hasSelect)//û������ͳ�ʼ����ִ�еĻ
            {
                var result = new Dictionary<Obj, CardInf[]>();
                for (int j = 0; j < PersonObj.belong.objs.Count; j++)
                {
                    List<CardInf> cardInfList = new List<CardInf>();
                    for (int l = 0; l < PersonObj.belong.objs[j].activities.Count; l++)
                    {
                        if (PersonObj.belong.objs[j].activities[l].Condition(PersonObj.belong.objs[j], PersonObj))
                        {
                            var res = PersonObj.belong.objs[j].activities[l].OutputSelect(PersonObj, PersonObj.belong.objs[j]);//һ����ѡ��
                            cardInfList.Add(res);
                        }
                    }
                    result[PersonObj.belong.objs[j]] = cardInfList.ToArray();
                }
                MainDispatch.Instance().Enqueue(
                () =>
                {
                    optionModel.SetPersonObjOption(PersonObj, result);//��ǰ����Ϊ
                });
                await GameArchitect.Interface.GetModel<ThinkModelSet>().thinks[PersonObj].BeginThink(result);//�ȴ�˼�����
            }
        }
        //���½�ɫ�,ÿ����ɫ��ִ��һ��
        foreach(var PersonObj in GameArchitect.PersonObjs)
        {
            tcs = new TaskCompletionSource<bool>(false);
            MainDispatch.Instance().Enqueue(
                () =>
                {
                    hasTime = true;
                    this.SendEvent<BeginActEvent>(new BeginActEvent(PersonObj));
                }
            );
            await tcs.Task;
        }
        MainDispatch.Instance().Enqueue(
            () =>
            {
                //����ʱ��
                this.SendEvent<EnvirUpdateEvent>(new EnvirUpdateEvent());
            });
    }
    [Button]
    public void CreatePersonObj(bool isplayer, string name, bool sex, string SceneName)
    {
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        CreatePersonObj(isplayer, name, sex, Sc);
    }
    public void CreatePersonObj(bool isplayer, string name, bool sex, TableModel Sc)
    {
        var s = Tool.DeepClone<PersonSaver>((PersonSaver)Map.Instance.GetSaver(typeof(PersonObj)));
        s.isPlayer = isplayer;
        s.PersonObjName =name;
        var p = new PersonObj(s);
        p.sex = sex;
        if (Sc == null)
        {
            Debug.Log("�޷�����");
            return;
        }
        ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.PersonObjList.Add(p);//��������
        ((GameArchitect)GameArchitect.Interface).GetModel<PersonObjsOptionModel>().AddPersonObj(p);
        ((GameArchitect)GameArchitect.Interface).GetModel<ThinkModelSet>().AddThinkMode(p);
        Sc.EnterTable(p);
    }
    /// <summary>
    /// ����һ����
    /// </summary>
    [Button]
    public void CreateBed(string SceneName)
    {
        var s = Tool.DeepClone<BedSaver>((BedSaver)Map.Instance.GetSaver(typeof(BedObj)));
        var p = new BedObj(s);
        var Sc = ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == SceneName; });
        if (Sc == null)
        {
            Debug.Log("�޷�����");
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
            Debug.Log("�޷�����");
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
            Debug.Log("�޷�����");
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
    //        Debug.Log("�޷�����");
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
            Debug.Log("�޷�����");
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
            Debug.Log("�޷�����");
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
            Debug.Log("�޷�����");
            return;
        }
        var x = (PathObj)Sc.objs.Find(e => { return objName == e.name; });
        x.activities.RemoveAll(e => { return ((Go)e).xname == Sc.TableName; });//ɾ������Ŀ�ĵؽڵ�
        Sc.RemoveToTable(x);
    }
    /// <summary>
    /// ����ũ��
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
            Debug.Log("�޷�����");
            return;
        }
        Sc.AddToTable(p);
    }
}
