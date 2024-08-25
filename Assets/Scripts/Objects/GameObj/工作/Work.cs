using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雇佣工作的协议
/// </summary>
public class WorkContract : Contract
{
    public WorkContract() : base()
    {
        cardInf.title = "工作";
        cardInf.description = "做工作";
    }
    /// <summary>
    /// 目标时间
    /// </summary>
    public override void Sign(IPerson person)
    {
        base.Sign(person);
    }
    /// <summary>
    /// 初始化协议
    /// </summary>
    /// <param name="person"></param>
    public override void Init(IPerson person,int num)
    {
        base.Init(person,num);
    }
    public override bool CanSign(IPerson PersonObj)
    {
        return true;
    }

    public override CodeSystemData GetCodeData()
    {
        throw new NotImplementedException();
    }

    public override void RegisterContract(IPerson person)
    {
        throw new NotImplementedException();
    }

    public override void UnRegisterContract(IPerson person)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// 自己的体验类型
/// </summary>
public enum SelectEnum
{
    /// <summary>
    /// 价格敏感
    /// </summary>
    PriceSensitive,
    /// <summary>
    /// 质量导向型
    /// </summary>
    Quality_Oriented,
    /// <summary>
    /// 奢侈型
    /// </summary>
    Luxury_Seeking
}
/// <summary>
/// 食物开销
/// </summary>
public class FoodStyle
{
    public SelectEnum selectEnum;
    /// <summary>
    /// 食物的口味
    /// </summary>
    public float foodPoint;

    /// <summary>
    /// npc,返回选择的食物
    /// </summary>
    /// <param name="npc"></param>
    public FoodObj Decision(NPCObj npc)
    {
        var table = npc.belong;
        ///一系列的物体
        foreach(var t in table.objs)
        {
            if(t is BuildingObj)
            {
                var building = (BuildingObj)t;//
                ///如果商品里有食物则计算
                if (building.goodsManager.goods.ContainsKey(ObjEnum.FoodObjE))
                {

                }
            }
        }
        return null;
    }
}

/// <summary>
/// 生活方式,例如空闲时间
/// </summary>
public class LifeStyle
{
    /// <summary>
    /// 当前签署的工作协议
    /// </summary>
    public WorkContract work;
    /// <summary>
    /// 当前的工作流程
    /// </summary>
    public CodeSystemData code;
    public FoodStyle foodStyle;
}