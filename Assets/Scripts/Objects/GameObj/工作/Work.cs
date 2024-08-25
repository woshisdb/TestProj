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
/// �Լ�����������
/// </summary>
public enum SelectEnum
{
    /// <summary>
    /// �۸�����
    /// </summary>
    PriceSensitive,
    /// <summary>
    /// ����������
    /// </summary>
    Quality_Oriented,
    /// <summary>
    /// �ݳ���
    /// </summary>
    Luxury_Seeking
}
/// <summary>
/// ʳ�￪��
/// </summary>
public class FoodStyle
{
    public SelectEnum selectEnum;
    /// <summary>
    /// ʳ��Ŀ�ζ
    /// </summary>
    public float foodPoint;

    /// <summary>
    /// npc,����ѡ���ʳ��
    /// </summary>
    /// <param name="npc"></param>
    public FoodObj Decision(NPCObj npc)
    {
        var table = npc.belong;
        ///һϵ�е�����
        foreach(var t in table.objs)
        {
            if(t is BuildingObj)
            {
                var building = (BuildingObj)t;//
                ///�����Ʒ����ʳ�������
                if (building.goodsManager.goods.ContainsKey(ObjEnum.FoodObjE))
                {

                }
            }
        }
        return null;
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
    public FoodStyle foodStyle;
}