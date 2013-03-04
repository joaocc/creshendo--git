(defrule nots
  (not (not (x ?x)))
  (not (y ?x))
  =>
  (printout t "Rule fired." crlf))


(deffunction test-something ()
  (reset)
  (assert (x 1))
  (run)
  )


(printout t "Testing notnot-not :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  