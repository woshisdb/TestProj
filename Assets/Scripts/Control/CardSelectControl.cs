using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinCon
{

}

public class SelectTex:WinCon
{
    public string title;
    public string description;
    public List<SelectInf> selects;
    public UnityAction effect;
    public SelectTex(string title, string description, List<SelectInf> selects, UnityAction effect)
    {
        this.title = title;
        this.description = description;
        this.selects = selects;
        this.effect = effect;
    }
}


public class CardSelectControl : ItemControlUI<SelectInf>
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public TMP_InputField select;
    public SelectInf selectInf;
    public void Init()
    {
        this.title.text = selectInf.title;
        this.content.text = selectInf.description;
        selectInf.num = 0;
    }
    public override void SetCardInf(SelectInf selectInf)
    {
        this.selectInf = selectInf;
        select.text = "";
        select.onEndEdit.AddListener((e)=> {
            int data = 0;
            if(int.TryParse(e,out data)&&data>=0)
            {
                selectInf.num = data;
            }
            else
            {
                select.text = "0";
            }
        });
        Init();
    }
}
