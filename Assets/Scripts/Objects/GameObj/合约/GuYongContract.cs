using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuYongContract : Contract
{
    public GuYongContract():base()
    {
    }

    public override bool ContractAllow(Person ap, Person bp)
    {
        throw new System.NotImplementedException();
    }

    public override Contract CopyContract()
    {
        throw new System.NotImplementedException();
    }

    public override void EffectAccept(Person ap, Person bp)
    {
        throw new System.NotImplementedException();
    }

    public override bool EffectAllow(Person ap, Person bp)
    {
        throw new System.NotImplementedException();
    }

    public override float EffectProb(Person ap, Person bp)
    {
        throw new System.NotImplementedException();
    }

    public override void EffectReject(Person ap, Person bp)
    {
        throw new System.NotImplementedException();
    }
}
