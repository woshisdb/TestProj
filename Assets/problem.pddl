(define
        (problem Problem)
        (:domain TestDomain)
        (:objects
        PersonE-ObjEnum
BedObjE-ObjEnum
DeskObjE-ObjEnum
AnimalObjE-ObjEnum
ObjE-ObjEnum
PathObjE-ObjEnum
RawObjE-ObjEnum
FoodObjE-ObjEnum
SeedObjE-ObjEnum
TreeSeedObjE-ObjEnum
TreeObjE-ObjEnum
WoodObjE-ObjEnum
BuildingResObjE-ObjEnum
KuangObjE-ObjEnum
KuangMiningObjE-ObjEnum
BuildingObjE-ObjEnum
RestaurantObjE-ObjEnum
FarmObjE-ObjEnum
WheatObjE-ObjEnum
WheatSeedObjE-ObjEnum
WheatTreeObjE-ObjEnum
PlaceObjE-ObjEnum
WheatPlaceObjE-ObjEnum
FullWheatPlaceObjE-ObjEnum
WheatFlourObjE-ObjEnum
GoldKuangObjE-ObjEnum
GoldObjE-ObjEnum
GoldMiningObjE-ObjEnum
IronKuangObjE-ObjEnum
IronObjE-ObjEnum
IronMiningObjE-ObjEnum
CoalObjE-ObjEnum
CoalMiningObjE-ObjEnum
TaotuObjE-ObjEnum
TaotuMiningObjE-ObjEnum
ShuLinObjE-ObjEnum
GongchangObjE-ObjEnum
MoneyObjE-ObjEnum
ToolObjE-ObjEnum
PersonType_2-PersonType
ResourceType_3-ResourceType
TableModelType_1-TableModelType
TableModelType_57-TableModelType
)

(:init

(HasMap  TableModelType_1  TableModelType_1 )
(HasMap  TableModelType_57  TableModelType_57 )
(MapLen TableModelType_1 TableModelType_1  3)
(MapLen TableModelType_57 TableModelType_57  3)
(Person_isPlayer  PersonType_2 )
(Person_resource  PersonType_2  ResourceType_3 )
(Person_belong  PersonType_2  TableModelType_1 )
(Person_money PersonType_2  0)
(Resource_maxSize ResourceType_3  0)
(Resource_nowSize ResourceType_3  0)

)

)