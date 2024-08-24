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
}