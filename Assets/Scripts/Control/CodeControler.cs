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
public class CodeControler : MonoBehaviour, IController, ICanRegisterEvent
{
    public CustomDropdown modeDropdown;
    public CustomDropdown dataDropdown;
    public TextMeshProUGUI nowAct;
    public TextMeshProUGUI nowData;
    public Button saveBtn;
    public Button loadBtn;
    public Button newBtn;
    public bool viewMode = false;
    public bool hasSave=false;
    public List<GameObject> timeLine;
    public GameObject dayList;
    public GameObject timeLineObj;
    public CodeSystemData systemData;
    public CodeSystemEnum systemEnum;
    /// <summary>
    /// 拖拽
    /// </summary>
    public WindowDragger openScene;
    public WindowDragger addScene;
    /// <summary>
    /// 系统数据根节点
    /// </summary>
    public Transform systemDataRoot;
    public GameObject timeList;
    public CardViewList<CardControlUi, CardInf> systemDataViewList;
    public CardViewList<CardControlUi, CardInf> dayDataViewList;
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }

    public void Awake()
    {
        systemDataViewList = new CardViewList<CardControlUi, CardInf>(systemDataRoot.gameObject);
        dayDataViewList = new CardViewList<CardControlUi, CardInf>(timeList);
        openScene.gameObject.SetActive(false);
        addScene.gameObject.SetActive(false);
        if (hasSave == false)
        {
            saveBtn.gameObject.SetActive(false);
            timeLineObj.gameObject.SetActive(false);
            loadBtn.gameObject.SetActive(false);
        }
        else
        {
            saveBtn.gameObject.SetActive(true);
            timeLineObj.gameObject.SetActive(true);
            loadBtn.gameObject.SetActive(true);
        }
        newBtn.onClick.AddListener(
            () => { addScene.gameObject.SetActive(true); }
        );
        saveBtn.onClick.AddListener(() =>
        {
            if (GameArchitect.get.tableAsset.tableSaver.codeDatas.Contains(systemData))
            {
                GameArchitect.get.tableAsset.tableSaver.codeDatas.Add(systemData);
            }
        });
        loadBtn.onClick.AddListener(() =>
        {
            openScene.gameObject.SetActive(true);
            List<CardInf> cardInfs = new List<CardInf>();
            foreach (var x in GameArchitect.get.tableAsset.tableSaver.codeDatas)
            {
                var data = x;
                cardInfs.Add(new CardInf(x.name, "",
                () => {
                    SetData(data);
                }
                ));
            }
            systemDataViewList.UpdataListView(cardInfs);
            List<CardInf> dayInfs = new List<CardInf>();
        });

    }

    public void AddData()
    {
        if (systemEnum == CodeSystemEnum.week)
        {
            systemData = new CodeSystemDataWeek();
        }
        else if (systemEnum == CodeSystemEnum.month)
        {
            systemData = new CodeSystemDataMonth();
        }
        else
        {
            systemData = new CodeSystemDataYear();
        }
        Debug.Log(systemData.week.Count);
        SetData(systemData);
        hasSave = true;
        addScene.gameObject.SetActive(false);
        saveBtn.gameObject.SetActive(true);
        timeLineObj.gameObject.SetActive(true);
        loadBtn.gameObject.SetActive(true);
    }

    public void ConcealLoad()
    {
        openScene.gameObject.SetActive(false);
    }

    public void ConcealAdd()
    {
        addScene.gameObject.SetActive(false);
    }

    public void SetData(CodeSystemData codeSystemData)
    {
        systemData = codeSystemData;
        for(int i=0;i<48;i++)
        {
            var btn=timeLine[i].GetComponent<Button>();
            var title = timeLine[i].GetComponentInChildren<TextMeshProUGUI>();
            title.text = i+"";
        }
        List<CardInf> cardInfs=new List<CardInf>();
        Debug.Log("????" + systemData.week.Count);
        for(int x=0;x<systemData.week.Count;x++)
        {
            var setTime=false;
            cardInfs.Add(new CardInf(x+"",systemData.code.ToString(),
                () => {
                    setTime = !setTime;
                }
            ));
        }
        dayDataViewList.UpdataListView(cardInfs);
        nowData.text = codeSystemData.name;
    }
}
