(define (domain sleepP)
  (:requirements :strips :typing :numeric-fluents)
  (:types person num)
  
  (:functions
    (sleep-time ?p - person)
    (select-time ?p - num)
  )
  
  (:action Sleep
    :parameters (?p - person ?t - num)
    :effect (and
      (increase (sleep-time ?p) (select-time ?t) )
    )
  )
)

