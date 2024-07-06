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
    public CardInf(string title="", string description="", UnityAction action=null)
    {
        this.title = title;
        this.description = description;
        this.effect = action;
    }
}
public class SelectInf
{
    public string title;
    public string description;
    public UnityAction action;
    public int num;
    public object obj;
    public SelectInf(string title = "", string description = "", UnityAction action = null, object obj = null)
    {
        this.title = title;
        this.description = description;
        this.action = action;
        this.obj = obj;
    }
}
