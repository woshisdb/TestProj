using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ʳ��
/// </summary>
public class FoodType : RawType
{
    public FoodType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class FoodSaver : RawSaver
{
    /// <summary>
    /// ԭ����Ŀ
    /// </summary>
    public Resource rawObj;
    /// <summary>
    /// ʳ�����ṩ������
    /// </summary>  
    public int energy;
    /// <summary>
    /// ���������ѵĹ�ʱ
    /// </summary>
    public int wastTime;
}
/// <summary>
/// 1������������һ���غ�
/// </summary>
[Map()]
public class FoodObj : RawObj
{

    public FoodObj(ObjSaver objAsset = null) : base(objAsset)
    {
    }
    public FoodSaver GetSaver()
    {
        return (FoodSaver)objSaver;
    }
}

public class MoneyType : RawType
{
    public MoneyType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class MoneySaver : RawSaver
{
}
[Map()]
public class MoneyObj : RawObj
{
    public MoneyObj(MoneySaver objAsset = null) : base(objAsset)
    {
    }
    public MoneySaver GetSaver()
    {
        return (MoneySaver)objSaver;
    }
}
