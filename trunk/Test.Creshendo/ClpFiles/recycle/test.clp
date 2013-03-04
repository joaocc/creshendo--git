(deffunction test-something ()
  (reset)
  (eval "(defrule test (not (a)) => (printout t FIRE crlf))")
  (assert (a))
  (retract 1)
  (run)
  )


(printout t "Testing not CE recycle :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  