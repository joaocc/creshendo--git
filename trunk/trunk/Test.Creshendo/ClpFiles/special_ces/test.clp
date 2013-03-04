(deffacts foo-facts
  (foo A)
  (foo B)
  (foo C)
  (foo D))

(deffacts bar-facts
  (bar 1))

(defrule exists
  (declare (salience 100))
  (exists (foo ?X))
  =>
  (printout t "Found a foo fact" crlf))

(defrule unique
  (declare (salience 90))
  (unique (bar ?X&:(< ?X 10)))
  =>
  (printout t "Found a bar fact:"  ?X crlf))

(deffunction test-something ()
  (reset)
  (assert (initiator))
  (run)
  )


(printout t "Testing special CEs :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  