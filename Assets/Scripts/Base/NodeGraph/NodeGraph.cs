using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TransationEnum
{
	/// <summary>
	/// 烹饪食物
	/// </summary>
	cook,
	/// <summary>
	/// 耕种农作物
	/// </summary>
	gengZhong,
	/// <summary>
	/// 采摘作物
	/// </summary>
	shouHuo,
	/// <summary>
	/// 栽种植物
	/// </summary>
	zaiZhong,
	/// <summary>
	/// 砍树或是拆卸
	/// </summary>
	qieGe,
	/// <summary>
	/// 搭建建筑
	/// </summary>
	daJian,
	/// <summary>
	/// 规划建筑设备
	/// </summary>
	guiHua,
	/// <summary>
	/// 安装设备
	/// </summary>
	anZhuang,
	/// <summary>
	/// 制作工具
	/// </summary>
	zhiZuo,
	/// <summary>
	/// 开采
	/// </summary>
	kaiCai,
}
public enum SitEnum
{
	bed,
	set
}
public enum TransEnum
{
	one,
	conti
}
/// <summary>
/// 物体转移关系
/// </summary>
[System.Serializable]
public class Trans
{
    [SerializeField]
	public string title;
	[SerializeField]
	public Node from;
	[SerializeField]
	public Node to;
	[SerializeField]
	public Edge edge;
	[SerializeField]
	public TransEnum transEnum;
	public virtual Source AddSource(Obj obj,Trans trans)
	{
		if (transEnum == TransEnum.one)
			return new Source((BuildingObj)obj, ((BuildingObj)obj).resource, trans);
		else
			return new IterSource((BuildingObj)obj, ((BuildingObj)obj).resource, trans);
	}
}
///// <summary>
///// 持续需要资源的转移关系
///// </summary>
//[System.Serializable]
//public class IterTrans:Trans
//{
//	public override Source AddSource(Obj obj, Trans trans)
//	{
//		return new IterSource((BuildingObj)obj, ((BuildingObj)obj).resource, trans);
//	}
//}

[Serializable]
public class NodeItem
{
	[SerializeField,EnumPaging]
	public ObjEnum x=ObjEnum.BuildingObjE;
	[SerializeField]
	public int y;
}
/// <summary>
/// 
/// </summary>
[Serializable]
public class EdgeItem
{
	[SerializeField, EnumPaging]
	public TransationEnum x= TransationEnum.cook;
	[SerializeField]
	public int y;
}

[System.Serializable]
public class Node
{
	[SerializeField]
	public List<NodeItem> source;//多少资源
}
[System.Serializable]
public class Edge
{
	/// <summary>
	/// 一系列转移规则
	/// </summary>
	[SerializeField]
	public List<EdgeItem> tras;
	/// <summary>
	/// 花费的时间
	/// </summary>
	public int time;
}
/// <summary>
/// 序列化规则
/// </summary>
[System.Serializable, CreateAssetMenu(fileName = "GameRule", menuName = "ScriptableObjects/RuleAsset")]
public class NodeGraph:SerializedScriptableObject
{
    /// <summary>
    /// 带有人性质的规则的转移
    /// </summary>
    [SerializeField]
	public List<Trans> trans;
    [Button]
	public void AddTransToTrans(Trans t)
	{
		trans.Add(t);
	}
    /// <summary>
    /// 世界固有规则的转移
    /// </summary>
	public List<Trans> worldRule;
    [Button]
	public void AddTransToWorld(Trans t)
	{
		worldRule.Add(t);
	}
}
public class MapTo
{
	public string y;
	public int wastTime;
}

public class WorldMap
{
    [SerializeField]
	public Dictionary<string, List<MapTo>> tos;
    [HideInInspector]
	public List<Activity> activities;
    [Button]
    public void Init()
    {
		activities = new List<Activity>();
		foreach (var xx in tos)
		{
			foreach(var x in xx.Value)
            {
				//var t1= ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == xx.Key; });
				//var t2=((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == x.y; });
				//Debug.Log(t1);
				//Debug.Log(t2);
				activities.Add(new Go(xx.Key, x.y, x.wastTime));
			}
		}
    }
}