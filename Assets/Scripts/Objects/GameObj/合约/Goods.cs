using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods
{
    /// <summary>
    /// ������Ʒ
    /// </summary>
    public ObjEnum buyO;
    /// <summary>
    /// ��Ŀ
    /// </summary>
    public int buyNum;
    /// <summary>
    /// ������Ʒ
    /// </summary>
    public ObjEnum sellO;
    public int sellNum;

    // ���캯��
    public Goods(ObjEnum buyO, int buyNum, ObjEnum sellO, int sellNum)
    {
        this.buyO = buyO;
        this.buyNum = buyNum;
        this.sellO = sellO;
        this.sellNum = sellNum;
    }
}

