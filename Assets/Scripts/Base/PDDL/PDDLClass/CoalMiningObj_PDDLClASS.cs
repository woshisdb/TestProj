
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class CoalMiningObj_PDDL:PDDLClass<CoalMiningObj,CoalMiningType>{
public CoalMiningObj_PDDL():base(){
            
}
public override void SetObj(object obj){
            this.obj=(CoalMiningObj)obj;
            ((CoalMiningObj)obj).pddl = this;
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
}
