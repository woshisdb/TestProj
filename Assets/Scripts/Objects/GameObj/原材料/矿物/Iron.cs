using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KuangType : RawType
{
    public KuangType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class KuangSaver : RawSaver
{

}

[Map()]
// ��
public class KuangObj : RawObj
{
    ///////////////////////////////////////

    public KuangObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class KuangMiningType : RawType
{
    public KuangMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class KuangMiningSaver : RawSaver
{

}

[Map()]
// ����
public class KuangMiningObj : RawObj
{
    ///////////////////////////////////////
    /// <summary>
    /// ʣ����Դ����Ŀ
    /// </summary>
    public int source;
    public int starSource;
    public virtual ObjEnum GetObj()
    {
        return ObjEnum.KuangObjE;
    }
    public KuangMiningObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override void LatUpdate()
    {
        str.Clear();
        //str.AppendLine(":");
        //str.AppendLine(source.ToString());
        ////��������
        //this.cardInf.description =
    }
    /// <summary>
    /// �ھ���Դ
    /// </summary>
    /// <param name=""></param>
    public int GetRes(int res)
    {
        if(source>= starSource*0.7)
        {
            int sum = Mathf.Min(res, source);
            source -= sum;
            return sum;
        }
        else if(source >=0.3*starSource)
        {
            int sum = Mathf.Min(res*7/10, source);
            source -= sum;
            return sum;
        }
        else
        {
            int sum = Mathf.Min(res * 3 / 10, source);
            source -= sum;
            return sum;
        }
    }
}


public class IronType : KuangType
{
    public IronType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class IronSaver : KuangSaver
{

}

[Map()]
// ��
public class IronObj : KuangObj
{
    ///////////////////////////////////////

    public IronObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
}

public class IronMiningType : KuangMiningType
{
    public IronMiningType(string name = null) : base(name)
    {

    }
}
[System.Serializable]
public class IronMiningSaver : KuangMiningSaver
{

}

[Map()]
// ����
public class IronMiningObj : KuangMiningObj
{
    public IronMiningObj(RawSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override void LatUpdate()
    {
        str.Clear();
    }
    public override ObjEnum GetObj()
    {
        return ObjEnum.KuangObjE;
    }
}