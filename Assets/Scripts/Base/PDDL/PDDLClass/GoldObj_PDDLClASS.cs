
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class GoldObj_PDDL:PDDLClass<GoldObj,GoldType>{
public GoldObj_PDDL():base(){
            
}
public override void SetObj(object obj){
            this.obj=(GoldObj)obj;
            ((GoldObj)obj).pddl = this;
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
        
public override List<Pop> GetPredsVal(){var ret= new List<Pop>();
return ret;}
public override List<Pop> GetFuncsVal(){var ret= new List<Pop>();
return ret;}
public override List<PType> GetTypes(){
            var ret=new List<PType>();
ret.Add(obj.GetPtype());
return ret;
     }
}
