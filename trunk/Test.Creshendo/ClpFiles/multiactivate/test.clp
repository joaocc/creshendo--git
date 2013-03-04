(defrule r1
  (LIST $? ?x $?)
  =>
  (printout t "r1 fired on " ?x crlf))

(deffunction test-something ()
  (reset)
  (assert (LIST (create$ A B C)))
  (run)
  (reset)
  (assert (LIST (create$ A B C)))
  (retract (fact-id 1))
  (run))


(printout t "Testing multiple activations from one fact:" crlf)
(test-something)
(printout t "Test done." crlf)
;;(exit)  