
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class IronObj_PDDL:PDDLClass<IronObj,IronType>{
public PDDLValRef<TableModel_PDDL,TableModel> belong;
public IronObj_PDDL():base(){
            

            belong = new PDDLValRef<TableModel_PDDL,TableModel>(
            (p)=> { return new Predicate("IronObj_belong", obj.GetPtype(), p); },
            () => { return new Predicate("IronObj_belong",obj.GetPtype(),obj.belong.GetPtype()); },
            () => { return (TableModel_PDDL)(obj.belong.GetPDDLClass()); },
            () => { return obj.belong; });
            
}
public override void SetObj(object obj){
            this.obj=(IronObj)obj;
            ((IronObj)obj).pddl = this;
}
public override List<Predicate> GetPreds()
        {
            var ret= new List<Predicate>() {
(Predicate)belong.pop(),
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
public override List<PDDLClass> GetPddls(){
        var ret = new List<PDDLClass>();
ret.Add(this);
ret.Add(belong.pVal());
return ret;}
}
