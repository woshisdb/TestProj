using System.Collections;
using System.Collections.Generic;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActSelectUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public ButtonManager acceptBtn;
    public GameObject cards;
    /// <summary>
    /// 卡片池
    /// </summary>
    public CardViewList<CardSelectControl,SelectInf> cardViewList;
    public void Awake()
    {
        cardViewList = new CardViewList<CardSelectControl,SelectInf>(cards,"SelectUI");
        gameObject.SetActive(false);
        GameArchitect.selectUI = this;
    }

    ///// <summary>
    ///// 更新
    ///// </summary>
    //public void UpdateDecision()
    //{
    //    if (selectTexList.Count > 0)
    //    {
    //        gameObject.SetActive(true);
    //        title.text = selectTexList[0].title;
    //        description.text = decisionTexList[0].description;
    //        cardViewList.UpdataListView(decisionTexList[0].cards);
    //        decisionTexList.RemoveAt(0);
    //    }
    //}

    /// <summary>
    /// 添加决策
    /// </summary>
    public IEnumerator AddDecision(SelectTex selectTex)
    {
        bool decisionEnd = false;
        title.text = selectTex.title;
        description.text = selectTex.description;
        cardViewList.UpdataListView(selectTex.selects);
        acceptBtn.onClick.AddListener(
            () => {
                if (selectTex.effect())
                {
                    Debug.Log(11234);
                    decisionEnd = true;
                    this.gameObject.SetActive(false);
                    GameArchitect.get.CallDecision();//如果满足效果就。。，否则就不管
                }
                else
                {

                }
            }
        );
        return new WaitUntil(() => { return decisionEnd == true; });
    }
}
