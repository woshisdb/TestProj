using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ƶ���ص���Ϊ

//public class GoTo : PAction
//{
//    public SceneType p1 = new SceneType("p1");//��ǰ����
//    public SceneType p2 = new SceneType("p2");//ǰ������
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