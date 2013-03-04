(deffacts facts
  (foo A)
  (foo B)
  (foo C)
  (foo D)
  (bar C))

(defrule exists-1
  (not (not (foo ?X)))
  =>
  (printout t "Found a foo fact" crlf))

(defrule exists-2
  (bar ?X)
  (not (not (foo ?X)))
  =>
  (printout t "Found a foo bar pair" crlf))

(deffunction test-something ()
  (reset)
  (run)
  )


(printout t "Testing exists CE :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  