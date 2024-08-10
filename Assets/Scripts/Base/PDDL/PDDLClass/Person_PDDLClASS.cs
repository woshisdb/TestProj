
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
public Person_PDDL():base(){
            

                isPlayer = new PDDLVal(
                () =>
                {
                    return new Predicate("Person_isPlayer", obj.GetPtype());
                },
                () =>
                {
                   return new Bool(new Predicate("Person_isPlayer", obj.GetPtype()),obj.isPlayer);
                });
            

                money = new PDDLVal(
                () =>
                {
                    return new Func("Person_money", obj.GetPtype());
                },
                () =>
                {
                   return new Num(new Func("Person_money", obj.GetPtype()),obj.money);
                });
            
resource=new Resource_PDDL();
resource.SetObj(obj.resource);
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
        
public override List<Pop> GetPredsVal(){var ret= new List<Pop>();
ret.Add(isPlayer.val());
ret.Add( P.Belong( GetObj() , resource.GetObj() ) );
return ret;}
public override List<Pop> GetFuncsVal(){var ret= new List<Pop>();
ret.Add(money.val());
return ret;}
}
