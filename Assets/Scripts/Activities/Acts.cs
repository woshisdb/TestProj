//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class UseObjA : Act
//{
//    public UseObjA(Person person, Obj obj) : base(person, obj)
//    {
//        wastTime = false;
//    }

//    public override IEnumerator<object> Run(Action<Act> callback)
//    {
//        TC();
//        Debug.Log("Use");
//        BedObj data = (BedObj)Obj;
//        data.capacity.val++;
//        yield return Ret(new EndAct(Person, Obj), callback);
//    }
//}
//public class ReleaseObjA : Act
//{
//    public ReleaseObjA(Person person, Obj obj) : base(person, obj)
//    {
//        wastTime = false;
//    }

//    public override IEnumerator<object> Run(Action<Act> callback)
//    {
//        TC();
//        Debug.Log("Release");
//        BedObj data = (BedObj)Obj;
//        data.capacity.val--;
//        yield return Ret(new EndAct(Person, Obj), callback);
//    }
//}