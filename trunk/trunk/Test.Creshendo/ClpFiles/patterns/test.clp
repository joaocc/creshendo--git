(defrule test1
  (declare (salience 300))
  ?x <- (foo)
  (bar ?x)
  =>
  (printout t ?x crlf)
  )

(defrule test2
  (declare (salience 200))
  ?x <- (foo)
  =>
  (printout t "X is " (call ?x getFactId) crlf))

(defrule test3
  (declare (salience 100))
  ?x <- (foo)
  (baz ?y&:(eq (call ?x getFactId) ?y))
  =>
  (printout t "Y is " ?y crlf))


(deffunction test-something ()
  (assert (foo) (bar (fact-id 0)) (baz 0))
  (run))


(printout t "Testing pattern bindings :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit) 