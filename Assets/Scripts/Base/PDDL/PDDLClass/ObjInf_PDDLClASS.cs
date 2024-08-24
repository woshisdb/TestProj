
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class ObjInf_PDDL:PDDLClass<ObjInf,ObjInfType>{
public PDDLVal can;
public PDDLVal count;
public ObjInf_PDDL():base(){
            

                can = new PDDLVal(
                () =>
                {
                    return new Predicate("ObjInf_can", obj.GetPtype());
                },
                () =>
                {
                   return new Bool(new Predicate("ObjInf_can", obj.GetPtype()),()=>{return obj.can;});
                });
            

                count = new PDDLVal(
                () =>
                {
                    return new Func("ObjInf_count", obj.GetPtype());
                },
                () =>
                {
                   return new Num(new Func("ObjInf_count", obj.GetPtype()),()=>{return obj.count;});
                });
            
}
public override void SetObj(object obj){
            this.obj=(ObjInf)obj;
            ((ObjInf)obj).pddl = this;
}
public override List<Predicate> GetPreds()
        {
            var ret= new List<Predicate>() {
(Predicate)can.pop(),
};

            return ret;
            }
        

            public override List<Func> GetFuncs()
            {
                var ret= new List<Func>() {
               (Func)count.pop(),
};

                return ret;
            }
        
public override List<Bool> GetPredsVal(){var ret= new List<Bool>();
ret.Add( (Bool) (can.val()));
return ret;}
public override List<Num> GetFuncsVal(){var ret= new List<Num>();
ret.Add( (Num)( count.val() ) );
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
