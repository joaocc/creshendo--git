(deftemplate myfact (slot myslot))

(defrule modifier
  (logical (foo))
  ?fact <- (myfact (myslot 1))
  =>
  (modify ?fact (myslot 2)))

(deffacts startup
  (foo)
  (myfact (myslot 1)))

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