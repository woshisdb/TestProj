using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingNPCAct : NPCAct
{
    NPCObj npc;
    int time;
    public override IEnumerator Run()
    {
        ///����
        npc.lifeStyle.foodStyle.Decision(npc);
        yield return null;
    }
    public EatingNPCAct(NPCObj npc)
    {
        this.time = 1;
        this.npc = npc;
    }
    public override int WasterTime()
    {
        return time;
    }
}