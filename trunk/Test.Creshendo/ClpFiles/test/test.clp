(defrule rule-1
  (foo ?X)
  (test (eq ?X 1))
  =>
  (printout t "X is 1" crlf))

(defrule rule-2
  (foo ?X)
  (test (and (numberp ?X) (<> ?X 1)))
  =>
  (printout t "X is not 1" crlf))

(defrule rule-3
  (foo ?X&~blue)
  (test (eq ?X red))
  =>
  (printout t "X is red, not blue" crlf))

(defrule rule-4
  (foo ?X)
  (test (neq ?X 1))
  (bar ?X)
  =>
  (printout t "foo and bar match" crlf))

(defrule rule-5
  (bar ?X)
  (not (test (eq ?X 1)))
  =>
  (printout t "bar OK" crlf))


(deffunction test-something ()
  ;; ----------------------------------------------------------------------
  (printout t ">>> rules 1 and 2" crlf)
  (reset)
  (assert (foo 1))
  (run)
  (reset)
  (assert (foo 2))
  (run)
  ;; ----------------------------------------------------------------------
  (printout t ">>> rule 3" crlf)
  (reset)
  (assert (foo red))
  (run)
  ;; ----------------------------------------------------------------------
  (printout t ">>> rule 4" crlf)
  (reset)
  (assert (foo xyz) (bar xyz))
  (run)
  ;; ----------------------------------------------------------------------
  (printout t ">>> No rules" crlf)
  (reset)
  (assert (bar 1))
  (run)
  ;; ----------------------------------------------------------------------
  (printout t ">>> rule 5" crlf)
  (reset)
  (assert (bar 2))
  (run)
  )


(printout t "Testing test CE:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  