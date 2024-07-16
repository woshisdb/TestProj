using System.Collections;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;

public class CardControl : MonoBehaviour,ICanRegisterEvent
{
    public TextMeshPro title;
    public TextMeshPro content;
    public CardInf cardInf;
    public void Init()
    {

        this.title.text = cardInf.title;
        this.content.text = cardInf.description;
    }
    public void UpdateInf(CardInf cardInf)
    {
        this.cardInf = cardInf;
        Init();
    }
    public void OnMouseDown()
    {
        cardInf.effect.Invoke();
    }
    public void UpdateInf()
    {
        this.title.text = cardInf.title;
        this.content.text = cardInf.description;
    }
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.get;
    }
}
