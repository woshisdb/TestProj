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
/// <summary>
/// 物体转移关系
/// </summary>
[System.Serializable]
public class Trans
{
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
	public ObjEnum x=ObjEnum.Restaurant;
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
	[SerializeField]
	public List<Trans> trans;

}
