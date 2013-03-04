(defrule forall-test-1
  (not (and (a)
            (not (and (b) (c)))))
  =>
  (printout t ">>> FIRED 1" crlf))


(deffunction test-something ()
  (reset)
  (printout t "No facts" crlf)
  (run)
  (reset)
  (assert (a))
  (printout t "a only" crlf)
  (run)
  (reset)
  (assert (a))
  (assert (b))
  (assert (c))
  (printout t "a, b and c" crlf)
  (run)
  (reset)
  (assert (b))
  (assert (c))
  (printout t "b and c only" crlf)
  (run)
  (reset)         
  (assert (a))
  (assert (b))  
  (printout t "a and b only" crlf)
  (run)
  (reset)
  (assert (a))
  (assert (c))
  (printout t "a and c only" crlf)
  (run)
  (reset)
  (assert (c))
  (printout t "c only" crlf)
  (run)
  (reset)
  (assert (b))
  (printout t "b only" crlf)
  (run)
  (reset)
  )


(printout t "Testing forall CE :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  