using System.Collections;
using System.Collections.Generic;
using QFramework;
//using Unity.VisualScripting;
using UnityEngine;

public class PersonControler : MonoBehaviour,IController
{
    public Person person;

    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
}
