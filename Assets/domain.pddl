(define (domain TestDomain)
(:requirements :strips :typing :durative-actions)
(:types
PType
PersonType-AnimalType
ResourceType-PType
TableModelType-ObjType

)

(:predicates

(IsPDDL ?PType_58-PType ?PType_59-PType  )
(HasMap ?TableModelType_60-TableModelType ?TableModelType_61-TableModelType  )
(Person_isPlayer ?PersonType_2-PersonType  )
(Person_resource ?PersonType_2-PersonType ?ResourceType_3-ResourceType  )
(Person_belong ?PersonType_2-PersonType ?TableModelType_1-TableModelType  )

)
(:functions

(MapLen  ?TableModelType_62 ?TableModelType_63)
(Person_money  ?PersonType_2)

)
(:durative-action GoAct
:parameters(?PersonType_109-PersonType ?TableModelType_111-TableModelType ?TableModelType_112-TableModelType )

:duration(=?duration (MapLen  ?TableModelType_111 ?TableModelType_112))
:condition
(and 
(at start (Person_belong  PersonType_109  TableModelType_111 ))
(at start (HasMap  TableModelType_111  TableModelType_112 ))
)


:effect

(and 
(at end (Person_belong  PersonType_109  TableModelType_112 ))
(at end ( not (Person_belong  PersonType_109  TableModelType_111 )) )

)


)
)