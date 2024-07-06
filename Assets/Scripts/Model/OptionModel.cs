using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public struct ChangeOptionEvent
{
    public Person person;
    public Dictionary<Obj, CardInf[]> cardInfs;

    public ChangeOptionEvent(Person person, Dictionary<Obj, CardInf[]> options) : this()
    {
        this.person = person;
        this.cardInfs = options;
    }
}

public class PersonsOptionModel : AbstractModel
{
    public Dictionary<Person,Option> cardInfs;
    protected override void OnInit()
    {
        
    }
    public PersonsOptionModel(List<Person> people)
    {
        cardInfs = new Dictionary<Person, Option>();
        for (int i = 0; i < people.Count; i++)
        {
            cardInfs.Add(people[i], new Option());
        }
    }
    public void AddPerson(Person person)
    {
        cardInfs.Add(person, new Option());
    }
    public void SetPersonOption(Person person, Dictionary<Obj, CardInf[]> x)
    {
        cardInfs[person].options = x;
        this.SendEvent<ChangeOptionEvent>(new ChangeOptionEvent(person, cardInfs[person].options));
    }
}
public class Option
{
    public Dictionary<Obj, CardInf[]> options = new Dictionary<Obj, CardInf[]>();
}
