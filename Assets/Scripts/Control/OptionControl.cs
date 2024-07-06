using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class FinishOptionEvent
{
    Person person;
    public FinishOptionEvent(Person person)
    {
        this.person = person;
    }
}

public class OptionControl : MonoBehaviour, IController,ICanSendEvent
{
    public Person person;
    // Start is called before the first frame update
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public void Awake()
    {
        this.RegisterEvent<ChangeOptionEvent>(//Ñ¡ÔñÐÐÎª
         e => {
             this.SendEvent<FinishOptionEvent>(new FinishOptionEvent(person));
         }
         );
    }
}
