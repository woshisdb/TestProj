using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public abstract class ItemControlUI<T>:MonoBehaviour
{
    public abstract void SetCardInf(T cardInf);
}

public class CardControlUi : ItemControlUI<CardInf>
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public Button button;
    public CardInf cardInf;
    public UnityAction action;
    public void Init()
    {
        this.title.text = cardInf.title;
        this.content.text = cardInf.description;
    }
    public override void SetCardInf(CardInf cardInf)
    {
        this.cardInf = cardInf;
        action = this.cardInf.effect;
        Init();
    }
    public void CallAction()
    {
        action.Invoke();
    }

}
