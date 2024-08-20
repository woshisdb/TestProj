(define (problem block-world-problem)
  (:domain block-world)
  (:objects A B C - block)

  (:init
    (ontable A)
    (ontable B)
    (ontable C)
    (clear A)
    (clear B)
    (clear C)
    (not (holding A))
    (not (holding B))
    (not (holding C))
  )

  (:goal
    (and
      (on A B)
      (on B C)
      (clear A)
    )
  )
)