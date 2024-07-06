using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class CardViewList<T,F> where T : ItemControlUI<F>
{
    public SimpleObjectPool<GameObject> pool;
    public List<T> cards;
    public CardViewList(GameObject root,string assetName= "CardUI")
    {
        cards = new List<T>();
        pool = new SimpleObjectPool<GameObject>(
            () =>
            {
                var gameobject = Resources.Load<GameObject>(assetName);
                var y = GameObject.Instantiate(gameobject, root.transform);
                return y;
            },
             (e) =>
             {
                 e.SetActive(false);
             }
        );
    }
    public void UpdataListView(List<F> cardInfs)
    {
        foreach (var obj in cards)
        {
            pool.Recycle(obj.gameObject);
        }
        pool.Clear();
        foreach (var obj in cardInfs)
        {
            var data = pool.Allocate();
            data.gameObject.SetActive(true);
            data.GetComponent<T>().SetCardInf(obj);
            cards.Add(data.GetComponent<T>());
        }
    }
}
