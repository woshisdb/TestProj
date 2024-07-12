using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods
{
    /// <summary>
    /// 购买物品
    /// </summary>
    public ObjEnum buyO;
    /// <summary>
    /// 数目
    /// </summary>
    public int buyNum;
    /// <summary>
    /// 出售物品
    /// </summary>
    public ObjEnum sellO;
    public int sellNum;

    // 构造函数
    public Goods(ObjEnum buyO, int buyNum, ObjEnum sellO, int sellNum)
    {
        this.buyO = buyO;
        this.buyNum = buyNum;
        this.sellO = sellO;
        this.sellNum = sellNum;
    }
}

