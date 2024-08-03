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
public class KuangMiningSaver : BuildingSaver
{

}

[Map()]
// ����
public class KuangMiningObj : BuildingObj
{
    ///////////////////////////////////////
    /// <summary>
    /// ��ʼ��Դ����Ŀ
    /// </summary>
    public int starSource;
    public virtual ObjEnum GetObj()
    {
        return ObjEnum.KuangObjE;
    }
    public KuangMiningObj(KuangMiningSaver objAsset = null) : base(objAsset)
    {
        Init();
        starSource = 1000;
        resource = new Resource();
        resource.Add(ObjEnum.TaotuObjE,starSource);
    }
    public override List<Activity> InitActivities()
    {
        var acts = new List<Activity>() { 
        new ArrangeContractAct(),//ǩ��Э��
        new AddContractAct(),//���Э��
        new RemoveContractAct(),//�Ƴ�Э��
        /***************************************/
        new SelPipLineAct(),
        new SetPipLineAct(),
        new UseToolAct(),
        new WaKuangAct()
        };
        Debug.Log(acts.Count);
        return acts;
    }
    public override void LatUpdate()
    {
        str.Clear();
        //str.AppendLine(":");
        //str.AppendLine(source.ToString());
        ////��������
        //this.cardInf.description =
        str.AppendLine(GetObj().ToString());
        if (resource == null)
            resource = new Resource();
        if(resource.resources.ContainsKey(GetObj()))
            str.AppendLine(resource.resources[GetObj()].remain+"");
        else
            str.AppendLine(0 + ":" + starSource);
        if(resource.resources.ContainsKey(ObjEnum.KuangObjE))
        {
            str.AppendLine("ʣ��ԭ��:" + resource.resources[ObjEnum.KuangObjE].remain);
        }
        cardInf.description = str.ToString();
        if (cardInf.cardControl)
            cardInf.cardControl.UpdateInf();
    }
    public int GetResCount()
    {
        return resource.resources[GetObj()].remain;
    }
    /// <summary>
    /// �ھ���Դ
    /// </summary>
    /// <param name=""></param>
    public KeyValuePair<ObjEnum, int> GetRes(int res)
    {
        var resNum= resource.resources[ObjEnum.KuangObjE].remain;
        if (resNum >= starSource*0.7)
        {
            int sum = Mathf.Min(res, resNum);
            resource.Remove(ObjEnum.KuangObjE,sum);
            Debug.Log(sum);
            return new KeyValuePair<ObjEnum, int>(ObjEnum.KuangObjE,sum);
        }
        else if(resNum >= 0.3*starSource)
        {
            int sum = Mathf.Min(res*7/10, resNum);
            resource.Remove(ObjEnum.KuangObjE, sum);
            return new KeyValuePair<ObjEnum, int>(ObjEnum.KuangObjE, sum);
        }
        else
        {
            int sum = Mathf.Min(res * 3 / 10, resNum);
            resource.Remove(ObjEnum.KuangObjE, sum);
            return new KeyValuePair<ObjEnum, int>(ObjEnum.KuangObjE, sum);
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
    public IronMiningObj(IronMiningSaver objAsset = null) : base(objAsset)
    {
        Init();
    }
    public override List<Activity> InitActivities()
    {
        var acts = base.InitActivities();
        return acts;
    }
    public override ObjEnum GetObj()
    {
        return ObjEnum.IronObjE;
    }
}