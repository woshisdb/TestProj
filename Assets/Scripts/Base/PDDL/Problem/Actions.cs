using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//移动相关的行为

//public class GoTo : PAction
//{
//    public SceneType p1 = new SceneType("p1");//当前场景
//    public SceneType p2 = new SceneType("p2");//前往场景
//    public PersonType ps = new PersonType("person");
//    public GoTo(Pop cond=null,Pop eff=null, params PType[] condType)
//    {
//        objects.Add(p1);
//        objects.Add(p2);
//        objects.Add(ps);
//        objects.AddRange(condType);
//        //condition = And(InScene(p1,ps),Not(InScene(p2,ps)));
//        if(cond!=null)
//        condition = And(condition, cond);
//        //effect = And(InScene(p2,ps),Not(InScene(p1,ps)));
//        if (eff != null)
//            effect = And(effect, eff);
//    }
//}