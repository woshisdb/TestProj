using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinCon
{
    public WinData data;
    public virtual void Decision(WinData winData)
    {
        
    }
}

public class SelectTex:WinCon
{
    public string title;
    public string description;
    public List<SelectInf> selects;
    public Func<bool> effect;//效果
    public SelectTex(string title, string description, List<SelectInf> selects, Func<bool> effect)
    {
        this.title = title;
        this.description = description;
        this.selects = selects;
        this.effect = effect;
        effect = () =>
        {
            bool now = effect();
            if(now)//选择数据
            {
                data=new SelData();
                for(int i = 0; i < selects.Count; i++)
                {
                    ((SelData)data).selects.Add(new Tuple<string, int>(selects[i].title, selects[i].num));
                }
            }
            return now;
        };
    }
    public override void Decision(WinData winData)
    {
        var wd = (SelData)winData;
        for(int i = 0; i < wd.selects.Count; i++)
        {
            var sel=selects.Find(e => { return e.title == wd.selects[i].Item1; });
            if(sel != null)
            sel.num = wd.selects[i].Item2;
        }
        effect();
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
            if(int.TryParse(e,out data)&&data>=0&&data<=selectInf.maxnNum)
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
