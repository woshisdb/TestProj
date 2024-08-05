using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using PimDeWitte.UnityMainThreadDispatcher;
using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum OptionUIEnum
{
    NoSelect,
    Select,
    ForceSelect,
}
public struct SelectObjEvent
{
    public Obj obj;
    public SelectObjEvent(Obj obj)
    {
        this.obj = obj;
    }
}
public struct ForceSelectEvent
{
    public List<CardInf> cardInfs;
    public ForceSelectEvent(List<CardInf> cardInfs)
    {
        this.cardInfs = cardInfs;
    }
}
public struct UpdateCode
{
    public Person person;
    public UpdateCode(Person person)
    {
        this.person = person;
    }
}
public class OptionUIControl : MonoBehaviour, IController, ICanRegisterEvent
{
    public GameObject list;
    public List<CardControlUi> nowCards;
    //public OptionUIEnum optionUIEnum;
    public WindowManager windowsmanager;
    public SimpleObjectPool<GameObject> cardPool;
    //public Person person;
    Dictionary<Obj, List<CardControlUi>> cardsUi;
    public List<UnityEvent> unityActions;
    /// <summary>
    /// 代码列表
    /// </summary>
    public GameObject codeList;
    /// <summary>
    /// 代码列表视图
    /// </summary>
    public CardViewList<CardControlUi,CardInf> codeView;
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public void ToForceSelect(List<CardInf> cardInfs)
    {
        GameLogic.optionUIEnum = OptionUIEnum.ForceSelect;
        MainDispatch.Instance().Enqueue(
            () =>
            {
                Debug.Log("s1");
                GameLogic.ToSelect();
            });
        foreach (var t in nowCards)
        {
            cardPool.Recycle(t.gameObject);
        }
        nowCards.Clear();
        foreach(var t in cardInfs)
        {
            var data = cardPool.Allocate();
            data.gameObject.active =true;
            data.GetComponent<CardControlUi>().SetCardInf(t);
            nowCards.Add(data.GetComponent<CardControlUi>());
        }
    }
    public void ToSelect()
    {
        //Debug.Log("s1");
        GameLogic.ToSelect();
    }
    public void ToNoSelect()
    {
        foreach (var t in nowCards)
        {
            t.gameObject.SetActive(false);
        }
        nowCards.Clear();
        //Debug.Log("s2");
        GameLogic.ToNoSelect();
    }
    /// <summary>
    /// 更新code
    /// </summary>
    /// <param name="updateCode"></param>
    public void UpdateCodeView(UpdateCode updateCode)
    {
        ///是玩家
        if(updateCode.person.isPlayer)
        {
            var cMang = updateCode.person.contractManager;
            var res = new List<CardInf>();
            var time = GameArchitect.get.GetModel<TimeModel>().Time;
            for (int i = time; i < 48; i++)
            {
                var code=updateCode.person.contractManager.GetCode(i);
                if (code==null)
                    res.Add(new CardInf(code.codeName, code.activity.activityName, () => { }));
                else
                    res.Add(new CardInf("无", "自由活动", () => { }));
            }
            codeView.UpdataListView(res);
        }
    }
    private void Awake()
    {
        ///代码列表
        codeView = new CardViewList<CardControlUi,CardInf>(codeList);
        this.RegisterEvent<UpdateCode>(
         e => { 
         UpdateCodeView(e);
         }
         );
        cardPool = new SimpleObjectPool<GameObject>(() => {
            var gameobject = Resources.Load<GameObject>("CardUI");
            var y = GameObject.Instantiate(gameobject, list.transform);
            return y;
        }
    , (e) =>
    {
        e.active = false;
    }
    );
        var temp=GameArchitect.Interface;
        //person = GameArchitect.persons.Find((e) => { return e.isPlayer; });
        nowCards = new List<CardControlUi>();
        cardsUi = new Dictionary<Obj, List<CardControlUi>>();
        this.RegisterEvent<SelectObjEvent>(
            e =>
            {
                if (GameLogic.optionUIEnum == OptionUIEnum.Select&& GameLogic.isCoding == false)
                {
                    foreach (var t in nowCards)
                    {
                        t.gameObject.SetActive(false);
                    }
                    nowCards.Clear();
                    if (cardsUi.ContainsKey(e.obj))
                    foreach (var t in cardsUi[e.obj])
                    {
                        t.gameObject.SetActive(true);
                        nowCards.Add(t);
                    }
                }
            }
        );
        this.RegisterEvent<ForceSelectEvent>(
        e =>
        {
            ToForceSelect(e.cardInfs);
        }
        );
        this.RegisterEvent<EndTurnEvent>(
        e => {
            ToNoSelect();
        }    
        );
        this.RegisterEvent<BeginTurnEvent>(
        e => {
            ToSelect();
        }
        );
        IEnumerator ExampleWait(UnityAction eve)
        {
            yield return new WaitForSeconds(3);
            eve();
        }

        this.RegisterEvent<ChangeOptionEvent>(//选择行为
         e => {
             if(e.person.isPlayer)
             {
                 foreach (KeyValuePair<Obj, List<CardControlUi>> entry in cardsUi)
                 {
                     foreach(CardControlUi ui in entry.Value)
                     cardPool.Recycle(ui.gameObject);
                 }
                 cardsUi.Clear();
                 foreach(KeyValuePair<Obj, CardInf[]> entry in e.cardInfs)
                 {
                     cardsUi.Add(entry.Key, new List<CardControlUi>());
                     foreach(CardInf ui in entry.Value)
                     {
                         var data = cardPool.Allocate();
                         data.gameObject.active = false;
                         data.GetComponent<CardControlUi>().SetCardInf(ui);
                         cardsUi[entry.Key].Add(data.GetComponent<CardControlUi>());
                     }    
                 }
                 this.SendEvent<FinishOptionEvent>(new FinishOptionEvent(e.person));
                 //Debug.Log(12346743);
             }
         }
         );
    }
    /// <summary>
    /// 选择代码
    /// </summary>
    public void SelectCode(string name)
    {

    }
}
