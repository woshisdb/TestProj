
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class Resource_PDDL:PDDLClass<Resource,ResourceType>{
public PDDLVal maxSize;
public PDDLVal nowSize;
public Resource_PDDL():base(){
            

                maxSize = new PDDLVal(
                () =>
                {
                    return new Func("Resource_maxSize", obj.GetPtype());
                },
                () =>
                {
                   return new Num(new Func("Resource_maxSize", obj.GetPtype()),obj.maxSize);
                });
            

                nowSize = new PDDLVal(
                () =>
                {
                    return new Func("Resource_nowSize", obj.GetPtype());
                },
                () =>
                {
                   return new Num(new Func("Resource_nowSize", obj.GetPtype()),obj.nowSize);
                });
            
}
public override void SetObj(object obj){
            this.obj=(Resource)obj;
            ((Resource)obj).pddl = this;
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
               (Func)maxSize.pop(),
               (Func)nowSize.pop(),
};

                return ret;
            }
        
public override List<Bool> GetPredsVal(){var ret= new List<Bool>();
return ret;}
public override List<Num> GetFuncsVal(){var ret= new List<Num>();
ret.Add( (Num)( maxSize.val() ) );
ret.Add( (Num)( nowSize.val() ) );
return ret;}
public override List<PType> GetTypes(){
            var ret=new List<PType>();
ret.Add(obj.GetPtype());
return ret;
     }
}
