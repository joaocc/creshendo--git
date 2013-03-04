(deftemplate myfact (slot myslot))

(defrule asserter
  (logical (foo))
  =>
  (assert (myfact (myslot 1))))

(defrule modifier
  ?fact <- (myfact (myslot 1))
  =>
  (modify ?fact (myslot 2)))

(deffacts startup
  (foo))

(deffunction test-something ()
  (reset)
  (run)
  (facts)
  (retract 1)
  (facts)
  )

(printout t "Testing modify under logical support :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  