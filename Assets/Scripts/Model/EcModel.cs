using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

/// <summary>
/// ����ϵͳ
/// </summary>
public class EcModel : AbstractModel
{
    protected override void OnInit()
    {
        
    }
    /// <summary>
    /// ת�˵���Ϊ���루Num��Money����a->b
    /// </summary>
    public void MoneyTransfer(Num a,Num b,int money)
    {
        a.val-=money;
        b.val+=money;
    }
}
