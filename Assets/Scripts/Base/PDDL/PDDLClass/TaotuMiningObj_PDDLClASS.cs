
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class TaotuMiningObj_PDDL:PDDLClass<TaotuMiningObj,TaotuMiningType>{
public TableModel_PDDL belong;
public TaotuMiningObj_PDDL():base(){
            
belong=  (TableModel_PDDL)PDDLClassGet.Generate(typeof(TableModel));
}
public override void SetObj(object obj){
            this.obj=(TaotuMiningObj)obj;
            ((TaotuMiningObj)obj).pddl = this;
belong.SetObj(((TaotuMiningObj)obj).belong);
  ((TaotuMiningObj)obj).belong.pddl = belong;  
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
