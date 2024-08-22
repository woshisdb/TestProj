using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods:IPDDL
{
    /// <summary>
    /// 购买物品
    /// </summary>
    public Resource buyO;
    /// <summary>
    /// 需要用来交易的产品
    /// </summary>
    public Resource sellO;

    // 构造函数
    public Goods()
    {
        this.buyO = new Resource();
        this.sellO = new Resource();
    }
    public Goods(KeyValuePair<Enum<ObjEnum>, ObjContBase> buy, KeyValuePair<Enum<ObjEnum>, ObjContBase> sell)
    {
        buyO.Add(buy);
        sellO.Add(sell);
    }

	public PDDLClass GetPDDLClass()
	{
		throw new System.NotImplementedException();
	}

	public PType GetPtype()
	{
		throw new System.NotImplementedException();
	}

	public void InitPDDLClass()
	{
		throw new System.NotImplementedException();
	}
}

