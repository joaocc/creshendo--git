;; The correct interpretation: the rule applies if neither of the
;; following are true: There are consistent (a ?x ?y) and (b ?x ?y)
;; facts, or there is an (a ?x ?x) fact.

(defrule rule (not
               (and
                   (a ?x ?y)
                   (or
                        (b ?x ?y)
                        (test (eq ?x ?y))))) =>
                        (printout t "FIRED" crlf)
 )

(deffunction test-something ()
  (printout t "No facts: should fire" crlf)
  (reset)
  (run)
  (printout t "(a 1 2): should fire" crlf)
  (reset)
  (assert (a 1 2))
  (run)
  (printout t "(a 1 1): should not fire" crlf)
  (reset)
  (assert (a 1 1))
  (run)
  (printout t "(b 1 2): should fire" crlf)
  (reset)
  (assert (b 1 2))
  (run)
  (printout t "(a 1 2) and (b 1 2): should not fire" crlf)
  (reset)
  (assert (a 1 2) (b 1 2))
  (run)
  )


(printout t "Testing kopena rule :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  