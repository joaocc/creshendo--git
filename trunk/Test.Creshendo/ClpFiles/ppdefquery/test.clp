(defquery query
  (declare (variables ?a ?b ?c))
  (foo ?a ?b ?c))

(deffunction test-something ()
  (printout t (ppdefquery query) crlf)
  )


(printout t "Testing ppdefquery :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  