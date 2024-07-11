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
/// ����ת�ƹ�ϵ
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
