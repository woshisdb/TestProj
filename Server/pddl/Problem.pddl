(define (problem simple-problem)
  (:domain simple-domain)
  (:objects
    room1 room2 - room
  )

  (:init
    (at room1)  ;; The robot starts in room1
  )

  (:goal
    (at room2)  ;; The goal is to be in room2
  )
)
