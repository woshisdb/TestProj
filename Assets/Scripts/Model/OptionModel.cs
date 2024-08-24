using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public struct ChangeOptionEvent
{
    public PersonObj PersonObj;
    public Dictionary<Obj, CardInf[]> cardInfs;

    public ChangeOptionEvent(PersonObj PersonObj, Dictionary<Obj, CardInf[]> options) : this()
    {
        this.PersonObj = PersonObj;
        this.cardInfs = options;
    }
}

public class PersonObjsOptionModel : AbstractModel
{
    public Dictionary<PersonObj,Option> cardInfs;
    protected override void OnInit()
    {
        
    }
    public PersonObjsOptionModel(List<PersonObj> people)
    {
        cardInfs = new Dictionary<PersonObj, Option>();
        for (int i = 0; i < people.Count; i++)
        {
            cardInfs.Add(people[i], new Option());
        }
    }
    public void AddPersonObj(PersonObj PersonObj)
    {
        cardInfs.Add(PersonObj, new Option());
    }
    public void SetPersonObjOption(PersonObj PersonObj, Dictionary<Obj, CardInf[]> x)
    {
        cardInfs[PersonObj].options = x;
        this.SendEvent<ChangeOptionEvent>(new ChangeOptionEvent(PersonObj, cardInfs[PersonObj].options));
    }
}
public class Option
{
    public Dictionary<Obj, CardInf[]> options = new Dictionary<Obj, CardInf[]>();
}
