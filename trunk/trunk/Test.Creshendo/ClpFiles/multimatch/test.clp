(deftemplate foo (multislot bar))
(defrule foo ?f <- (foo (bar ? $?X)) => (modify ?f (bar $?X)))
(defrule bar ?f <- (bar ? $?X) => (retract ?f) (assert (bar $?X)))

(deffunction test-something ()
  (reset)
  (printout t "*** Unordered facts:" crlf)
  (assert (foo (bar A B C)))
  (printout t (run) crlf)
  (facts)
  (reset)

  (printout t "*** Ordered facts:" crlf)
  (assert (bar A B C))
  (printout t (run) crlf)
  (facts)
  (reset))


(printout t "Testing edges of multifield matching:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  