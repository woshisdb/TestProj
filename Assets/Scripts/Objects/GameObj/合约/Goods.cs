using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods
{
    /// <summary>
    /// ������Ʒ
    /// </summary>
    public Resource buyO;
    /// <summary>
    /// ��Ҫ�������׵Ĳ�Ʒ
    /// </summary>
    public Resource sellO;

    // ���캯��
    public Goods()
    {
        this.buyO = new Resource();
        this.sellO = new Resource();
    }
    public Goods(KeyValuePair<ObjEnum, ObjContBase> buy, KeyValuePair<ObjEnum, ObjContBase> sell)
    {
        buyO.Add(buy);
        sellO.Add(sell);
    }
}

