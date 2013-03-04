(defrule r
  (declare (salience 1))
  (not (p ?))
  =>
  (printout t "r" crlf))

(defrule r1 
  (declare (salience -1))
  (not (p ?))
  =>
  (printout t "r1" crlf))

(defrule r2
  (declare (salience -2))
  (not (p ?))
  =>
  (printout t "r2" crlf))


(deffunction test-something ()
  (reset)
  (assert (p 1))
  (run)
  (retract (fact-id 1))
  (run))


(printout t "Testing interference between (not) CEs:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)