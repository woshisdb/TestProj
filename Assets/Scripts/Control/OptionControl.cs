using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class FinishOptionEvent
{
    PersonObj PersonObj;
    public FinishOptionEvent(PersonObj PersonObj)
    {
        this.PersonObj = PersonObj;
    }
}

public class OptionControl : MonoBehaviour, IController,ICanRegisterEvent
{
    public PersonObj PersonObj;
    // Start is called before the first frame update
    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
    public void Awake()
    {
        this.RegisterEvent<ChangeOptionEvent>(//Ñ¡ÔñÐÐÎª
         e => {
             this.SendEvent<FinishOptionEvent>(new FinishOptionEvent(PersonObj));
         }
         );
    }
}
