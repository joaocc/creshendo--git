(defrule rule-4
  (rule4-1 ?x)
  (not (rule4-2 ?x))
  (test (eq ?x X))
  =>
  (printout t "rule-4 fired" crlf))

(defrule rule-5
  (rule4-1 ?x)
  (not (rule4-2 ?x))
  (test (eq ?x Y))
  =>
  (printout t "rule-5 fired" crlf))

  
(deffunction test-something ()
  ;; ----------------------------------------------------------------------
  (printout t ">>> rule-4, rule-5" crlf)
  (reset)
  (assert (rule4-1 X))
  (run)
          
  
)


(printout t "Testing not CE:" crlf)
(test-something)
(printout t "Test done." crlf)
