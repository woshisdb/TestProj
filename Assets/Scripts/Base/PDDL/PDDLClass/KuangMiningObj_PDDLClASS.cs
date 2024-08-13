
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class KuangMiningObj_PDDL:PDDLClass<KuangMiningObj,KuangMiningType>{
public TableModel_PDDL belong;
public KuangMiningObj_PDDL():base(){
            
}
public override void SetObj(object obj){
            this.obj=(KuangMiningObj)obj;
            ((KuangMiningObj)obj).pddl = this;
belong.SetObj(((KuangMiningObj)obj).belong);
  ((KuangMiningObj)obj).belong.pddl = belong;  
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
