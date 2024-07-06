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
public class FoodSaver : ObjSaver
{

}
/// <summary>
/// 1������������һ���غ�
/// </summary>
[Map()]
public class FoodObj : RawObj
{
    public int energy;//ʳ�����ṩ������
    public FoodObj(ObjSaver objAsset = null,int energy=1):base(objAsset)
    {

    }
    public FoodObj(ObjSaver objAsset = null) : base(objAsset)
    {
        energy = 1;
    }
}
