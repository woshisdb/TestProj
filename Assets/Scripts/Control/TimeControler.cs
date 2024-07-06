using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using QFramework;

public class TimeControler : MonoBehaviour,IController
{
    public TextMeshProUGUI text;
    public void Awake()
    {
        this.RegisterEvent<TimeUpdateEvent>(e => {
            text.text ="day:"+((e.Time/(TimeModel.timeStep))+1).ToString()+"|"+(e.Time%TimeModel.timeStep)/2+":"+ (((e.Time % TimeModel.timeStep)%2)==0?"00":"30");
        });
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitect.Interface;
    }
}
