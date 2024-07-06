using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CodeSlot
{
    public Color codeColor;
    public Button slot;
    public TextMeshProUGUI text;
    public Activity activity;
    public Obj obj;
}

public struct SelectDataSaver
{
    public CodeSaver codeSaver;
    public SelectDataSaver(CodeSaver codeSaver)
    {
        this.codeSaver = codeSaver;
    }
}
public class CodeControler : MonoBehaviour, IController, ICanSendEvent
{
    public Color ToColor(int name)
    {
        if (name == 0)
            return Color.blue;
        else if (name == 1)
            return Color.white;
        else if (name == 2)
            return Color.green;
        else if (name == 3)
            return Color.yellow;
        else
            return Color.red;
    }
    public CustomDropdown colorDrop;
    public TMP_InputField inputField;
    public CustomDropdown modeDropdown;
    /// <summary>
    /// 一系列的插槽
    /// </summary>
    public List<CodeSlot> codeSlots;
    /// <summary>
    /// 对象列表
    /// </summary>
    public GameObject objList;
    /// <summary>
    /// 活动列表
    /// </summary>
    public GameObject actList;
    public SimpleObjectPool<GameObject> actionPool;
    public SimpleObjectPool<GameObject> cardPool;
    public List<CardControlUi> acts;
    public List<CardControlUi> objs;
    public CodeSaver datas;
    public CodeSaver selectedCode;
    /// <summary>
    /// 当前活动
    /// </summary>
    public Activity nowActivity;
    public TextMeshProUGUI nowAct;
    public TextMeshProUGUI nowTimeAct;
    public Obj nowObj;
    public bool viewMode = false;
    public List<GameObject> timeLine;
    public Transform dataSaverRoot;
    public List<CardControlUi> dataSaverList;
    public SimpleObjectPool<GameObject> dataSaverPool;
    public Button save;
    public bool hasSaveData;//是否保存
    /// <summary>
    /// 拖拽
    /// </summary>
    public WindowDragger windowDragger;
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public void GenerateCodeData()
    {
        codeSlots.Clear();
        for (int i = 0; i < timeLine.Count; i++)
        {
            codeSlots.Add(new CodeSlot());
            codeSlots[i].slot = timeLine[i].GetComponent<Button>();
            codeSlots[i].text = timeLine[i].GetComponentInChildren<TextMeshProUGUI>();
            codeSlots[i].activity = null;
            codeSlots[i].obj = null;
            codeSlots[i].codeColor = Color.white;
        }
        //CodeControler.ProcessCodeDatas(GameArchitect.get.tableAsset.tableSaver.codeDatas[0], ref codeSlots);
    }
    //public void DataSaverInit()
    //{
    //    dataSaverPool = new SimpleObjectPool<GameObject>(
    //        () =>
    //        {
    //            var gameobject = Resources.Load<GameObject>("CardUI");
    //            var y = GameObject.Instantiate(gameobject, dataSaverRoot);
    //            return y;
    //        },
    //         (e) =>
    //         {
    //             e.SetActive(false);
    //         }
    //    );


    //}
    public void DataSaverInit(List<CodeSaver> codeSavers)
    {
        foreach (var obj in dataSaverList)
        {
            dataSaverPool.Recycle(obj.gameObject);
        }
        dataSaverList.Clear();
        foreach (var obj in codeSavers)
        {
            var data = dataSaverPool.Allocate();
            data.gameObject.SetActive(true);
            data.GetComponent<CardControlUi>().SetCardInf(new CardInf(obj.name,"", () => {
                selectedCode = obj;
                //CodeControler.ProcessCodeDatas(datas.dataDatas, ref codeSlots);
            }));
            dataSaverList.Add(data.GetComponent<CardControlUi>());
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        hasSaveData = false;
        dataSaverPool = new SimpleObjectPool<GameObject>(
            () =>
            {
                var gameobject = Resources.Load<GameObject>("CardUI");
                var y = GameObject.Instantiate(gameobject, dataSaverRoot);
                return y;
            },
             (e) =>
             {
                 e.SetActive(false);
             }
        );
        datas = new CodeSaver();
        if (GameArchitect.get.tableAsset.tableSaver.codeDatas == null)
            GameArchitect.get.tableAsset.tableSaver.codeDatas = new List<CodeSaver>();
        windowDragger.gameObject.SetActive(false);
        DataSaverInit(GameArchitect.get.tableAsset.tableSaver.codeDatas);
        GenerateCodeData();
        save.onClick.AddListener(() => {
            if (hasSaveData == false)
            {
                datas.dataDatas = CodeControler.ProcessCodeSlots(codeSlots);
            }
            datas.name = inputField.text;
            var t = GameArchitect.get.tableAsset.tableSaver.codeDatas.Find(e => { return e == datas; });
            if(t==null)
            GameArchitect.get.tableAsset.tableSaver.codeDatas.Add(datas);
            Debug.Log("Save Code Succ");
            hasSaveData=true;
        });
        //nowColor = ToColor(colorDrop.index);
        modeDropdown.onValueChanged.AddListener((e) => { Conceal(e); });
        //colorDrop.onValueChanged.AddListener((e) => { nowColor=ToColor(e); });
        cardPool = new SimpleObjectPool<GameObject>(
            () =>
            {
                var gameobject = Resources.Load<GameObject>("CardUI");
                var y = GameObject.Instantiate(gameobject, objList.transform);
                return y;
            },
             (e) =>
             {
                 e.SetActive(false);
             }
        );
        actionPool = new SimpleObjectPool<GameObject>(
            () =>
            {
                var gameobject = Resources.Load<GameObject>("CardUI");
                var y = GameObject.Instantiate(gameobject, actList.transform);
                return y;
            },
             (e) =>
             {
                 e.SetActive(false);
             }
        );
        this.RegisterEvent<SelectDataSaver>(
            e =>
            {
                selectedCode = e.codeSaver;
            }
        );
        this.RegisterEvent<SelectObjEvent>(
            e =>
            {
                if (GameLogic.isCoding == true)
                {
                    nowObj = e.obj;
                    foreach (var act in acts)
                    {
                        actionPool.Recycle(act.gameObject);
                    }
                    acts.Clear();
                    foreach (var x in e.obj.activities)
                    {
                        var data = actionPool.Allocate();
                        data.gameObject.active = true;
                        data.GetComponent<CardControlUi>().SetCardInf(new CardInf(x.activityName, x.detail, () => {
                            nowActivity = x;
                            nowAct.text = x.activityName + "\n" + nowObj.name;
                        }));
                        acts.Add(data.GetComponent<CardControlUi>());
                    }
                }
            }
        );
        ///一系列的slot
        for (int i = 0; i < codeSlots.Count; i++)
        {
            codeSlots[i].text.text = i / 2 + ":" + (i % 2 == 0 ? "00" : "30");
            var t = i;
            codeSlots[t].slot.onClick.AddListener(() => {
                Debug.Log(t);
                if (!viewMode)
                {
                    if (nowActivity == null || nowObj == null)
                    {
                        codeSlots[t].text.text = t / 2 + ":" + (t % 2 == 0 ? "00" : "30");
                        return;
                    }
                    codeSlots[t].activity = nowActivity;
                    codeSlots[t].obj = nowObj;
                    //if (codeSlots[t].activity != null)
                    //{
                    //    codeSlots[t].text.text = codeSlots[t].activity.activityName + "\n" + nowObj.name;
                    //}
                    //else
                    //{
                    //    codeSlots[t].text.text = t / 2 + ":" + (t % 2 == 0 ? "00" : "30");
                    //}
                }
                else
                {
                    //codeSlots[t].obj = nowObj;
                    codeSlots[t].codeColor = ToColor(colorDrop.index);
                    ShowActivityInf(codeSlots[t]);
                    //codeSlots[t].slot.image.color = ToColor(colorDrop.index);
                }
                SetSlotInf(codeSlots[t],t);
            });
            SetSlotInf(codeSlots[t], t);
        }
    }
    public void New()
    {
        hasSaveData = false;
        datas = new CodeSaver();
        for(int i=0;i<codeSlots.Count;i++)
        {
            datas.dataDatas.Add(new CodeData());
            datas.dataDatas[i].hasAct = false;
            datas.dataDatas[i].color = Color.white;
            datas.dataDatas[i].beginTime = i;
        }
        //GenerateCodeData();
        CodeControler.ProcessCodeDatas(datas.dataDatas,ref codeSlots);
    }
    public void Accept()
    {
        windowDragger.gameObject.SetActive(false);
        CodeControler.ProcessCodeDatas(selectedCode.dataDatas, ref codeSlots);
        datas = selectedCode;
        hasSaveData = true;
        for(int i=0;i<codeSlots.Count;i++)
        {
            SetSlotInf(codeSlots[i], i);
        }
        inputField.text = selectedCode.name;
    }
    public void Conceal()
    {
        windowDragger.gameObject.SetActive(false);
    }
    public void SetSlotInf(CodeSlot codeSlot,int t)
    {
        if (codeSlot.activity != null)
        {
            codeSlot.text.text = codeSlot.activity.activityName + "\n" + codeSlot.obj.name;
        }
        else
        {
            codeSlot.text.text = t / 2 + ":" + (t % 2 == 0 ? "00" : "30");
        }
        codeSlot.slot.image.color = codeSlot.codeColor;
    }
    public void Leave()
    {
        nowObj = null;
        nowActivity = null;
    }
    public void InitObjCards()
    {
        foreach (var obj in objs)
        {
            cardPool.Recycle(obj.gameObject);
        }
        objs.Clear();
        foreach (var obj in GameArchitect.get.tableAsset.tableSaver.objs)
        {
            var data = cardPool.Allocate();
            data.gameObject.active = true;
            data.GetComponent<CardControlUi>().SetCardInf(new CardInf(obj.name, obj.belong.TableName, () => {
                nowObj = obj;
                this.SendEvent<SelectObjEvent>(new SelectObjEvent(obj));
            }));
            objs.Add(data.GetComponent<CardControlUi>());
        }
    }
    public void ShowActivityInf(CodeSlot codeSlot)
    {
        if (nowActivity == null)
        {
            if (codeSlot.activity == null)
                nowTimeAct.text = "";
            else
                nowTimeAct.text = codeSlot.activity.activityName + "\n" + nowObj.name;
        }
    }
    public void Conceal(int val)
    {
        Debug.Log(val);
        viewMode = val == 1;
        nowActivity = null;
    }
    /// <summary>
    /// 加载Datas
    /// </summary>
    public void Load()
    {
        windowDragger.gameObject.SetActive(true);
        DataSaverInit(GameArchitect.get.tableAsset.tableSaver.codeDatas);//
    }
    /// <summary>
    /// 如果保存则可以用
    /// </summary>
    public void Use()
    {
        if(hasSaveData)
        {
            Debug.Log("Use Data");
            GameArchitect.get.player.codeData = datas;
        }
    }

    /// <summary>
    /// 接受活动
    /// </summary>
    
    public static List<CodeData> ProcessCodeSlots(List<CodeSlot> s)
    {
        List<CodeData> t = new List<CodeData>();

        // Iterate through each CodeSlot to populate the CodeData list
        for (int i = 0; i < s.Count; i++)
        {
            CodeSlot currentSlot = s[i];

            // Find the duration of the current activity
            int duration = 1;
            while (i + duration < s.Count && s[i + duration].activity == currentSlot.activity && s[i + duration].codeColor == currentSlot.codeColor && s[i + duration].obj == currentSlot.obj)
            {
                duration++;
            }

            CodeData data = new CodeData
            {
                hasAct = currentSlot.activity != null,
                obj = currentSlot.obj,
                time = duration, // The duration of this activity
                beginTime = i,
                activity = currentSlot.activity,
                color = currentSlot.codeColor,
            };
            t.Add(data);
        }

        if (t.Count > 1)
            for (int i = t.Count - 1; i >= 0; i--)
            {
                if (t[i].activity == t[0].activity && s[i].codeColor == s[0].codeColor && t[i].obj == t[0].obj)
                {
                    t[i].time += t[0].time;
                }
                else
                    break;
            }
        return t;
    }
    public static void ProcessCodeDatas(List<CodeData> s, ref List<CodeSlot> cs)
    {
        if (s != null)
        for(int i=0;i<s.Count;i++)
        {
            cs[i].codeColor = s[i].color;
            cs[i].activity = s[i].activity;
            cs[i].obj = s[i].obj;
            cs[i].slot.image.color = cs[i].codeColor;
            if (cs[i].activity != null)
            {
                cs[i].text.text = cs[i].activity.activityName + "\n" + cs[i].obj.name;
            }
            else
            {
                cs[i].text.text = i / 2 + ":" + (i % 2 == 0 ? "00" : "30");
            }
        }
    }
}
