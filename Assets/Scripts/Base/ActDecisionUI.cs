using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DecisionTex:WinCon
{
    public string title;
    public string description;
    public List<CardInf> cards;
    public DecisionTex(string title, string description, List<CardInf> cards)
    {
        this.title = title;
        this.description = description;
        this.cards = cards;
        foreach (CardInf card in cards)
        {
            var t = card;
            var temp = t.effect;
            t.effect = () =>
            {
                temp();
                data = new DecData();
                ((DecData)data).selc = t.title;
            };
        }
    }
    public override void Decision(WinData winData)
    {
        var wd = (DecData)winData;
        var x=cards.Find(e => { return e.title == wd.selc; });
        x.effect();
    }
}
/// <summary>
/// 一个决策节点
/// </summary>
public struct DecisionNode
{
    public Person person;
    public string title;
    public string description;
    public List<CardInf> cardInfs;
    public DecisionNode(Person person,string title,string description,List<CardInf> cardInfs)
    {
        this.description=description;
        this.person = person;
        this.title=title;
        this.cardInfs = cardInfs;
    }
}
public class ActDecisionUI:MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public GameObject cards;
    /// <summary>
    /// 卡片池
    /// </summary>
    public CardViewList<CardControlUi,CardInf> cardViewList;
    public void Awake()
    {
        cardViewList = new CardViewList<CardControlUi,CardInf>(cards);
        gameObject.SetActive(false);
        GameArchitect.decisionUI = this;
    }

    ///// <summary>
    ///// 更新
    ///// </summary>
    //public void UpdateDecision()
    //{
    //    if (decisionTexList.Count > 0)
    //    {
    //        gameObject.SetActive(true);
    //        title.text = decisionTexList[0].title;
    //        description.text = decisionTexList[0].description;
    //        cardViewList.UpdataListView(decisionTexList[0].cards);
    //        decisionTexList.RemoveAt(0);
    //    }
    //}

    /// <summary>
    /// 添加决策
    /// </summary>
    public IEnumerator AddDecision(DecisionTex decisionTex)
    {
        bool decisionEnd=false;
        foreach(var card in decisionTex.cards)
        {
            var originEffect = card.effect;
            card.effect = () => 
            {
                originEffect.Invoke();
                decisionEnd = true;
                this.gameObject.SetActive(false);
                GameArchitect.get.CallDecision();
            };
        }
        gameObject.SetActive(true);
        title.text = decisionTex.title;
        description.text = decisionTex.description;
        cardViewList.UpdataListView(decisionTex.cards);
        return new WaitUntil(()=> { return decisionEnd == true; });
    }
}
