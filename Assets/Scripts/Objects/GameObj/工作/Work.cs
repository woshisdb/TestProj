using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ӷ������Э��
/// </summary>
public class WorkContract : Contract
{
    public WorkContract() : base()
    {
        cardInf.title = "����";
        cardInf.description = "������";
    }
    /// <summary>
    /// Ŀ��ʱ��
    /// </summary>
    public override void Sign(IPerson person)
    {
        base.Sign(person);
    }
    /// <summary>
    /// ��ʼ��Э��
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
/// ���ʽ,�������ʱ��
/// </summary>
public class LifeStyle
{
    /// <summary>
    /// ��ǰǩ��Ĺ���Э��
    /// </summary>
    public WorkContract work;
    /// <summary>
    /// ��ǰ�Ĺ�������
    /// </summary>
    public CodeSystemData code;
}