(define (domain block-world)
  (:requirements :strips :typing)
  (:types block - object)

  (:predicates
    (on ?x - block ?y - block)
    (clear ?x - block)
    (holding ?x - block)
    (ontable ?x - block)
  )

  (:action pick-up
    :parameters (?x - block)
    :precondition (and (clear ?x) (ontable ?x) (not (holding ?x)))
    :effect (and (holding ?x) (not (ontable ?x)) (not (clear ?x)))
  )

  (:action put-down
    :parameters (?x - block)
    :precondition (holding ?x)
    :effect (and (ontable ?x) (clear ?x) (not (holding ?x)))
  )

  (:action stack
    :parameters (?x - block ?y - block)
    :precondition (and (holding ?x) (clear ?y))
    :effect (and (not (holding ?x)) (not (clear ?y)) (on ?x ?y) (clear ?x))
  )

  (:action unstack
    :parameters (?x - block ?y - block)
    :precondition (and (on ?x ?y) (clear ?x) (clear ?y) (not (holding ?x)))
    :effect (and (holding ?x) (clear ?y) (not (clear ?x)) (not (on ?x ?y)))
  )
)
