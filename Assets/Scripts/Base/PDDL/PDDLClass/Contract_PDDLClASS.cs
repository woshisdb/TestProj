
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class Contract_PDDL:PDDLClass<Contract,ContractType>{
public Contract_PDDL():base(){
            
}
public override void SetObj(object obj){
            this.obj=(Contract)obj;
            ((Contract)obj).pddl = this;
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
return ret;}
public override List<Num> GetFuncsVal(){var ret= new List<Num>();
return ret;}
public override List<PType> GetTypes(){
            var ret=new List<PType>();
ret.Add(obj.GetPtype());
return ret;
     }
public override List<PDDLClass> GetPddls(){
        var ret = new List<PDDLClass>();
ret.Add(this);
return ret;}
}
