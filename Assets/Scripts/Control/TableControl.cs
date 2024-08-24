using System.Collections;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableControl : MonoBehaviour, IController,ICanRegisterEvent
{
    public Transform PersonObjCardSlot;
    public Transform cardSlot;
    List<CardControl> cardControls;
    List<CardControl> PersonObjControls;
    public TableModel tableModel;
    public Transform center;
    SimpleObjectPool<GameObject> cardPool;
    public TextMeshPro tableNameUI;
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public void Init()
    {
        tableNameUI.text = tableModel.TableName;
        cardControls = new List<CardControl>();
        PersonObjControls = new List<CardControl>();
        cardPool = new SimpleObjectPool<GameObject>(
        () =>
        {
            var card=Resources.Load<GameObject>("Card");
            var t = GameObject.Instantiate<GameObject>(card);
            return t;
        },
        (e) => {
            e.SetActive(false);
            e.transform.parent = null;
        }
        );
        this.RegisterEvent<TableChangeEvent>((e)=>
        {
            if (tableModel  == e.Model)
            {
                ClearCardSlot();
                ClearPersonObj();
                for(int i=0;i<tableModel.objs.Count;i++)
                {
                    if (tableModel.objs[i] is PersonObj)
                        InsertPersonObj((PersonObj)tableModel.objs[i]);
                    else
                        InsertCardSlot(tableModel.objs[i]);
                }
            }
        });
    }
    public void InsertPersonObj(PersonObj PersonObj)
    {
        var card=cardPool.Allocate();
        card.transform.parent= PersonObjCardSlot;
        int n = PersonObjControls.Count;
        card.transform.localPosition = new Vector3((n%6)*1.5f,-(n/6)*2);
        PersonObjControls.Add(card.GetComponent<CardControl>());
        PersonObj.cardInf.cardControl = card.GetComponent<CardControl>();
        card.GetComponent<CardControl>().UpdateInf(PersonObj.cardInf);
    }
    public void InsertCardSlot(Obj obj)
    {
        var card = cardPool.Allocate();
        card.transform.parent = cardSlot;
        int n = cardControls.Count;
        card.transform.localPosition = new Vector3((n % 6) * 1.5f, -(n / 6) * 2);
        cardControls.Add(card.GetComponent<CardControl>());
        obj.cardInf.cardControl = card.GetComponent<CardControl>();
        card.GetComponent<CardControl>().UpdateInf(obj.cardInf);
    }
    public void ClearPersonObj()
    {
        for(int i=0;i<PersonObjControls.Count;i++)
        {
            PersonObjControls[i].cardInf.cardControl = null;
            cardPool.Recycle(PersonObjControls[i].gameObject);
        }
        cardPool.Clear();
        PersonObjControls.Clear();
    }
    public void ClearCardSlot()
    {
        for (int i = 0; i < cardControls.Count; i++)
        {
            cardControls[i].cardInf.cardControl = null;
            cardPool.Recycle(cardControls[i].gameObject);
        }
        cardPool.Clear();
        cardControls.Clear();
    }
    public Vector3 CenterPos()
    {
        return center.position;
    }
}
