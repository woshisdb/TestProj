(define (domain simple-domain)
  (:requirements :strips :typing)
  (:types room - object)

  (:predicates
    (at ?r - room)  ;; The robot is in room ?r
  )

  (:action move
    :parameters (?from - room ?to - room)
    :precondition (at ?from)
    :effect (and
      (not (at ?from))
      (at ?to)
    )
  )
)
