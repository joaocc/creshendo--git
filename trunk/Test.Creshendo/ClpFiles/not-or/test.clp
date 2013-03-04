(defrule rule-1
  (not (and (x)
            (or (y) (z))))
  =>)


(deffunction test-something ()
  (rules)
  (printout t (ppdefrule "rule-1") crlf)
  (watch all)
  (reset)
  (assert (x) (y) (z))
  (retract 1)
  (assert (x))
  (retract 2)
  (assert (y))
  (retract 3)
  (assert (z))
)


(printout t "Testing (not (and (or))) :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  