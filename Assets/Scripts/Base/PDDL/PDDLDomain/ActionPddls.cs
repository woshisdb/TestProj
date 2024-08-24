using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPddls
{
    //public static List<Tuple<Type, PAction>> GetPDDLActions()
    //{
    //    var ret = new List<Tuple<Type, PAction>>();
    //    return ret;
    //}
    public static List<Tuple<Type,PAction>> GetPDDLActions()
    {
        var ret= new List<Tuple<Type,PAction>>();
        /**********Go***********/
        ret.Add(new Tuple<Type, PAction>(typeof(Go),Go()));
        //ret.Add(new Tuple<Type, PAction>(typeof(UseToolAct),UseToolAct()));
        return ret;
    }
    public static PAction Go()
    {
        PAction action = new PAction();
        action.actionName = "GoAct";
        var PersonObj = new PersonObj();
        var PersonObjP = (PersonObj_PDDL)PersonObj.GetPDDLClass();
        var belong = new TableModel();
        PersonObj.belong = belong;
        var belongP = PersonObjP.belong;
        var table = new TableModel();
        var tableP = (TableModel_PDDL)table.GetPDDLClass();
        action.RegPDDL(PersonObjP);
        action.RegRefVal(belongP);
        action.RegPDDL(tableP);
        action.RegCondition(
            P.AtStart(
            P.HasMap((TableModelType)belongP.GetPType(), (TableModelType)tableP.GetPType())
            )
        ) ;
        action.RegEffect(
            P.AtEnd(
            P.And(belongP.Is(tableP.GetObj()),
            P.Not(belongP.Is(belongP.GetPType())))
            )
        );
        action.duration = P.MapLen((TableModelType)belongP.GetPType(), (TableModelType)tableP.GetPType());
        return action;
    }
    public static PAction UseToolAct()
    {
        PAction action = new PAction();
        action.actionName = "UseToolAct";

        return action;
    }
}
