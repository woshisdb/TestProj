using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TransationEnum
{
	cook,
	gengZhong,
	shouHuo,
	zaiZhong,
	qieGe
}
public enum SitEnum
{
	bed,
	set
}
/// <summary>
/// ����ת�ƹ�ϵ
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
}

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
	public List<NodeItem> source;//������Դ
}
[System.Serializable]
public class Edge
{
	/// <summary>
	/// һϵ��ת�ƹ���
	/// </summary>
	[SerializeField]
	public List<EdgeItem> tras;
	/// <summary>
	/// ���ѵ�ʱ��
	/// </summary>
	public int time;
}
/// <summary>
/// ���л�����
/// </summary>
[System.Serializable, CreateAssetMenu(fileName = "GameRule", menuName = "ScriptableObjects/RuleAsset")]
public class NodeGraph:SerializedScriptableObject
{
	[SerializeField]
	public List<Trans> trans;

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
				var t1= ((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == xx.Key; });
				var t2=((GameArchitect)GameArchitect.Interface).tableAsset.tableSaver.tables.Find(e => { return e.TableName == x.y; });
				//Debug.Log(t1);
				//Debug.Log(t2);
				activities.Add(new Go(t1,t2 , x.wastTime));
			}
		}
    }
}