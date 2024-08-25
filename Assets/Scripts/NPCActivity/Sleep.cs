using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepNPCAct : NPCAct
{
    NPCObj npc;
    int time;
    public override IEnumerator Run()
    {
        npc.sleep.UpdateFront(time);
        yield return null;
    }
    public SleepNPCAct(NPCObj npc,int time)
    {
        this.time = time;
        this.npc = npc;
    }
    public override int WasterTime()
    {
        return time;
    }
}

