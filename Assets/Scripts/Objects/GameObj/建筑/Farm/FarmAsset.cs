using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmType : BuildingType
{
    public FarmType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class FarmSaver : BuildingSaver
{

}

[Map()]
// ũ��
public class FarmObj : BuildingObj
{
    /// <summary>
    /// ���ֹ���,
    /// </summary>
    public Rate gengZhong;
    /// <summary>
    /// �ջ񹤾�,
    /// </summary>
    public Rate shouHuo;
    /// <summary>
    /// ���ֹ���,
    /// </summary>
    public Rate zaiZhong;
    /// <summary>
    /// �и��
    /// </summary>
    public Rate qieGe;
    ///////////////////////////////////////

    public FarmObj(BuildingSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public FarmSaver GetSaver()
    {
        return (FarmSaver)objSaver;
    }
}
