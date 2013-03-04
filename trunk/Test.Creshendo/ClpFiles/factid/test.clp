(defrule foo
  ?f1 <- (foo ?A)
  ?f2 <- (bar ?B&:(> ?f2 ?f1))
  =>
  (printout t ?f2 " is greater than " ?f1 crlf))

(deffunction test-something ()
  (assert (bar A))
  (assert (foo B))
  (assert (bar C))
  (run))


(printout t "Testing fact-id access on rule LHSs:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  