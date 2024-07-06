using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PD{
public static IT IT(){return new IT();}
public static Duration Duration(){return new Duration();}
public static NowT NowT(){return new NowT();}
public static Capacity Capacity(ObjType objType){return new Capacity(objType);}
public static NowCapacity NowCapacity(ObjType objType){return new NowCapacity(objType);}
public static ObjSizeF ObjSizeF(ObjType objType){return new ObjSizeF(objType);}
public static IsAliveP IsAliveP(ObjType objType){return new IsAliveP(objType);}
public static Money Money(PersonType objType){return new Money(objType);}
public static SleepValF SleepValF(ObjType objType){return new SleepValF(objType);}
public static FoodValF FoodValF(ObjType objType){return new FoodValF(objType);}
//public static HasSelectP HasSelectP(ObjType objType){return new HasSelectP(objType);}
//public static IsUseObjP IsUseObjP(PersonType personType, ObjType objType){return new IsUseObjP(personType, objType);}
//public static FinancialKnowledge FinancialKnowledge(PersonType personType){return new FinancialKnowledge(personType);}
//public static ArtKnowledge ArtKnowledge(PersonType personType){return new ArtKnowledge(personType);}
//public static LanguageKnowledge LanguageKnowledge(PersonType personType){return new LanguageKnowledge(personType);}
//public static MedicalKnowledge MedicalKnowledge(PersonType personType){return new MedicalKnowledge(personType);}
//public static ManagementKnowledge ManagementKnowledge(PersonType personType){return new ManagementKnowledge(personType);}
//public static AgriculturalKnowledge AgriculturalKnowledge(PersonType personType){return new AgriculturalKnowledge(personType);}
//public static IndustrialKnowledge IndustrialKnowledge(PersonType personType){return new IndustrialKnowledge(personType);}
//public static GeneralHistoryKnowledge GeneralHistoryKnowledge(PersonType personType){return new GeneralHistoryKnowledge(personType);}
}