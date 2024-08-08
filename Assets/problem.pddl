(define (problem sleep)
  (:domain sleepP)
  
  (:objects
    p1 - person
    t1 - num
    t2 - num
  )
  
  (:init
    (= (sleep-time p1) 0)
    (= (select-time t1) 1)
    (= (select-time t2) 1)
  )
  
  (:goal 
    (> (sleep-time p1) 4)
  )
)
