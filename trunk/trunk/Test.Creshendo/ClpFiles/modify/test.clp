(deftemplate ab (slot a))
(defrule tt ?x <- (ab (a ?)) => (modify ?x (a 234)))

(deffunction test-something ()
  (assert (ab (a 123)))
  (run 1)
  (retract 0)      
  (facts)

  (reset)
  (assert (ab (a 234)))
  (run 1)
  (facts)

  (try
   (modify 1 (abc x))
   (printout t "Got no exception" crlf)
   catch
   (printout t (call ?ERROR getMessage) crlf))
  )

(printout t "Testing modify :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  