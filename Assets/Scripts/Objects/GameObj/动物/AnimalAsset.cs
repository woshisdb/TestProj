using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[P]
public class Sex : Predicate
{
    public Sex(AnimalType personType) : base(personType)
    {

    }
}
[P]
public class Age : Func
{
    public Age(AnimalType personType) : base(personType)
    {

    }
}
public class AnimalType:ObjType
{
    public AnimalType(string name=null):base(name)
    {

    }
}
[System.Serializable]
public class AnimalSaver : ObjSaver
{
    [SerializeField]
    public bool sex;
    [SerializeField]
    public int age;
    [SerializeField]
    public bool hasSelect;
    [SerializeField]
    public bool isUseObj;
    [SerializeField]
    public PersonSVal sleepState;
}
public class AnimalObj : Obj
{
    public Bool sex;//�Ա�
    public Num age;//����
    public Bool hasSelect;//�Ƿ���ѡ��
    /******************************************************/
    public PersonState sleepState;//˯�ߵ�״̬
    public PersonState foodState;//ʳ��ĵ�״̬
    public AnimalObj(ObjSaver objAsset=null):base(objAsset)
    {
        Init();
    }
    public void Init()
    {
        AnimalSaver sv = (AnimalSaver)objSaver;
        sex = new Bool(new Sex((AnimalType)obj),sv.sex);
        age = new Num(new Age((AnimalType)obj),sv.age);
        hasSelect = new Bool(new HasSelectP((AnimalType)obj),sv.hasSelect);
        sleepState = new PersonState(this,sv.sleepState);
        sleepState.val = new Num(SleepValF(obj));
    }
}
