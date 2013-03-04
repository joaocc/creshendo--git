(defrule my-rule-1
  (logical (foo) (bar))
  =>
  (assert (A) (B)))

(defrule my-rule-2
  (logical (foz))
  =>
  (assert (A) (D)))

(deffacts initial-facts
  (foo) (bar) (foz))         

(deffunction test-something ()
  (reset)
  (run)
  (bind ?dependents (dependents 1))
  (printout t ?dependents crlf)

  (bind ?dependencies (dependencies 4))
  (printout t ((nth$ 1 ?dependencies) ToString) crlf)
  (printout t ((nth$ 2 ?dependencies) ToString) crlf)
)


(printout t "Testing dependencies/dependents :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  