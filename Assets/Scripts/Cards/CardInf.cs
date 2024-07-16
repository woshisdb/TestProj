using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardInf
{
    public string title;
    public string description;
    public UnityAction effect;
    public CardControl cardControl;
    public CardInf(string title="", string description="", UnityAction action=null)
    {
        this.title = title;
        this.description = description;
        this.effect = action;
        this.cardControl = null;
    }
}
public class SelectInf
{
    public string title;
    public string description;
    public int num;
    public int maxnNum;
    public object obj;//ŒÔ∆∑
    public SelectInf(string title = "", string description = "", object obj = null,int maxnNum=5000)
    {
        this.title = title;
        this.description = description;
        this.obj = obj;
        this.maxnNum = maxnNum;
    }
}
