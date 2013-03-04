
(deffunction test-something ()
  (bind ?a 2)
  (bind ?b 2)
  (printout t (eval "(+ ?a ?b)") crlf))


(printout t "Testing eval :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  