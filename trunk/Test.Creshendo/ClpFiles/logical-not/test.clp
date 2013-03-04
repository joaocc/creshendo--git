(defrule rule-1
  (logical (A))
  (logical (not (B)))
  =>
  (assert (C)))

(defrule rule-2
  (logical (not (D)))
  =>
  (assert (E)))

(defrule rule-3
  (logical (exists (F ?)))
  =>
  (assert (G)))



(deffunction test-something ()
  (watch facts)
  (printout t "Logical not, not first logical" crlf)
  (assert (A))
  (run)
  (assert (B))

  (printout t crlf "Logical not, first logical" crlf)
  (reset)
  (run)
  (assert (D))

  (printout t crlf "Logical exists" crlf)
  (reset)
  (assert (D) (F 1) (F 2) (F 3))
  (run)
  (retract (fact-id 2))
  (retract (fact-id 3))  
  (retract (fact-id 4))  
  )


(printout t "Testing (logical (not)) :" crlf)
(test-something)
(printout t "Test done." crlf)
;;(exit)  