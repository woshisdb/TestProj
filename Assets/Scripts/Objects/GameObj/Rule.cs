using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �����еĹ��
/// </summary>
public class Rule
{
    public List<PAction> rules;
    public Rule(TableModel table)
    {
        rules = new List<PAction>();
    }
    public List<PAction> GetRules()
    {
        return rules;
    }
}
