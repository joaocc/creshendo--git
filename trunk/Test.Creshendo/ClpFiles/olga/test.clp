(deftemplate node
  (slot type)
  (slot id)
  )

(deftemplate task
  (slot type)
  (slot priority)
  )

(defrule set-direct-next-step
  (task (priority ?p))
  (not (foo))
  (not (and (task (type ?t)  (priority ?y&:(> ?y ?p)))
            (node (type ?t))
            )
       )
  =>
  )

(deffunction test-something ()
  (watch activations)
  (reset)
  (assert (task (type "Type1") (priority 9)))
  (assert (task (type "Type2") (priority 8)))

  ;; assert 2 nodes with highest priority
  (assert (node (type "Type1") (id a)))
  (assert (node (type "Type1") (id b)))

  ;; assert one node with lower priority
  (assert (node (type "Type2") (id c)))

  (assert (foo))
  )


(printout t "Testing Olga's bug :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  