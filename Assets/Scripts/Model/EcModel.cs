using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

/// <summary>
/// 金融系统
/// </summary>
public class EcModel : AbstractModel
{
    protected override void OnInit()
    {
        
    }
    /// <summary>
    /// 转账的行为传入（Num（Money））a->b
    /// </summary>
    public void MoneyTransfer(Num a,Num b,int money)
    {
        a.val-=money;
        b.val+=money;
    }
}
