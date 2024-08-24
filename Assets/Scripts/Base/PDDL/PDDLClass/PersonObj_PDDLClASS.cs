
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class PersonObj_PDDL:PDDLClass<PersonObj,PersonType>{
public PDDLVal isPlayer;
public PDDLValRef<Resource_PDDL,Resource> resource;
public PDDLValRef<TableModel_PDDL,TableModel> belong;
public PersonObj_PDDL():base(){
            

                isPlayer = new PDDLVal(
                () =>
                {
                    return new Predicate("PersonObj_isPlayer", obj.GetPtype());
                },
                () =>
                {
                   return new Bool(new Predicate("PersonObj_isPlayer", obj.GetPtype()),()=>{return obj.isPlayer;});
                });
            

            resource = new PDDLValRef<Resource_PDDL,Resource>(
            (p)=> { return new Predicate("PersonObj_resource", obj.GetPtype(), p); },
            () => { return new Predicate("PersonObj_resource",obj.GetPtype(),obj.resource.GetPtype()); },
            () => { return (Resource_PDDL)(obj.resource.GetPDDLClass()); },
            () => { return obj.resource; });
            

            belong = new PDDLValRef<TableModel_PDDL,TableModel>(
            (p)=> { return new Predicate("PersonObj_belong", obj.GetPtype(), p); },
            () => { return new Predicate("PersonObj_belong",obj.GetPtype(),obj.belong.GetPtype()); },
            () => { return (TableModel_PDDL)(obj.belong.GetPDDLClass()); },
            () => { return obj.belong; });
            
}
public override void SetObj(object obj){
            this.obj=(PersonObj)obj;
            ((PersonObj)obj).pddl = this;
}
public override List<Predicate> GetPreds()
        {
            var ret= new List<Predicate>() {
(Predicate)isPlayer.pop(),
(Predicate)resource.pop(),
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
ret.Add( (Bool) (isPlayer.val()));
ret.Add( new Bool( resource.pop(),true ) );
ret.Add( new Bool( belong.pop(),true ) );
return ret;}
public override List<Num> GetFuncsVal(){var ret= new List<Num>();
return ret;}
public override List<PType> GetTypes(){
            var ret=new List<PType>();
ret.Add(obj.GetPtype());
ret.Add(resource.GetPType());
ret.Add(belong.GetPType());
return ret;
     }
public override List<PDDLClass> GetPddls(){
        var ret = new List<PDDLClass>();
ret.Add(this);
ret.Add(resource.pVal());
ret.Add(belong.pVal());
return ret;}
}
