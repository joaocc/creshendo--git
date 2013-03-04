(defrule rule-3
  (logical (exists (F ?)))
  =>
  (assert (G)))
  
  
(deffunction test-something ()
	(watch facts)

  (printout t crlf "Logical exists" crlf)
  (reset)
  (assert (F 2))
  (run)
  (retract (fact-id 1))
  )
  
  
(printout t "Testing (logical (not)) :" crlf)
(test-something)
(printout t "Test done." crlf)