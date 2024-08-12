
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
public class Person_PDDL:PDDLClass<Person,PersonType>{
public PDDLVal isPlayer;
public PDDLVal money;
public Resource_PDDL resource;
public TableModel_PDDL belong;
public Person_PDDL():base(){
            

                isPlayer = new PDDLVal(
                () =>
                {
                    return new Predicate("Person_isPlayer", obj.GetPtype());
                },
                () =>
                {
                   return new Bool(new Predicate("Person_isPlayer", obj.GetPtype()),()=>{return obj.isPlayer;});
                });
            

                money = new PDDLVal(
                () =>
                {
                    return new Func("Person_money", obj.GetPtype());
                },
                () =>
                {
                   return new Num(new Func("Person_money", obj.GetPtype()),()=>{return obj.money;});
                });
            
resource=  (Resource_PDDL)PDDLClassGet.Generate(typeof(Resource));
belong=  (TableModel_PDDL)PDDLClassGet.Generate(typeof(TableModel));
}
public override void SetObj(object obj){
            this.obj=(Person)obj;
            ((Person)obj).pddl = this;
resource.SetObj(((Person)obj).resource);
  ((Person)obj).resource.pddl = resource;  
belong.SetObj(((Person)obj).belong);
  ((Person)obj).belong.pddl = belong;  
}
public override List<Predicate> GetPreds()
        {
            var ret= new List<Predicate>() {
(Predicate)isPlayer.pop(),
};

            return ret;
            }
        

            public override List<Func> GetFuncs()
            {
                var ret= new List<Func>() {
               (Func)money.pop(),
};

                return ret;
            }
        
public override List<Bool> GetPredsVal(){var ret= new List<Bool>();
ret.Add( (Bool) (isPlayer.val()));
ret.Add( P.Is( GetObj() , resource.GetObj() ) );
ret.Add( P.Is( GetObj() , belong.GetObj() ) );
return ret;}
public override List<Num> GetFuncsVal(){var ret= new List<Num>();
ret.Add( (Num)( money.val() ) );
return ret;}
public override List<PType> GetTypes(){
            var ret=new List<PType>();
ret.Add(obj.GetPtype());
ret.Add(resource.GetPType());
ret.Add(belong.GetPType());
return ret;
     }
}
