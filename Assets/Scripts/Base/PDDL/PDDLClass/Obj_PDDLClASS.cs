
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class Obj_PDDL:PDDLClass<Obj,PType>{
public PDDLValRef<TableModel_PDDL,TableModel> belong;
public Obj_PDDL():base(){
            

            belong = new PDDLValRef<TableModel_PDDL,TableModel>(
            () => { return new Predicate("Obj_belong",obj.GetPtype(),obj.belong.GetPtype()); },
            () => { return (TableModel_PDDL)(obj.belong.GetPDDLClass()); },
            () => { return obj.belong; });
            
}
public override void SetObj(object obj){
            this.obj=(Obj)obj;
            ((Obj)obj).pddl = this;
belong.SetObj(((Obj)obj).belong);
  ((Obj)obj).belong.pddl = belong;  
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
ret.Add( P.Is( GetObj() , belong.GetObj() ) );
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
