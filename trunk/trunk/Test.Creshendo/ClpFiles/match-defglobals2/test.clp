(defglobal ?*x* = 3)

(defrule foo
  (bar ?b&:(eq ?*x* 3))
  =>
  (printout t "Ooops!" crlf))

(deffunction test-something()
  (watch activations)
  (watch facts)
  
  (assert (bar 1))
  (bind ?*x* 2)
  (retract 0)
  (run))

(test-something)
