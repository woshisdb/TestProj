
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class TreeObj_PDDL:PDDLClass<TreeObj,TreeType>{
public PDDLValRef<TableModel_PDDL,TableModel> belong;
public TreeObj_PDDL():base(){
            

            belong = new PDDLValRef<TableModel_PDDL,TableModel>(
            (p)=> { return new Predicate("TreeObj_belong", obj.GetPtype(), p); },
            () => { return new Predicate("TreeObj_belong",obj.GetPtype(),obj.belong.GetPtype()); },
            () => { return (TableModel_PDDL)(obj.belong.GetPDDLClass()); },
            () => { return obj.belong; });
            
}
public override void SetObj(object obj){
            this.obj=(TreeObj)obj;
            ((TreeObj)obj).pddl = this;
}
public override List<Predicate> GetPreds()
        {
            var ret= new List<Predicate>() {
};

            return ret;
            }
        

            public override List<Func> GetFuncs()
            {
                var ret= new List<Func>() {
};

                return ret;
            }
        
public override List<Bool> GetPredsVal(){var ret= new List<Bool>();
ret.Add( new Bool( belong.pop(),true ) );
return ret;}
public override List<Num> GetFuncsVal(){var ret= new List<Num>();
return ret;}
public override List<PType> GetTypes(){
            var ret=new List<PType>();
ret.Add(obj.GetPtype());
ret.Add(belong.GetPType());
return ret;
     }
}
