
(deffunction test-something ()
  (printout t "Testing progn: " crlf)
  (printout t (progn 1 2 3 (+ 1 2)) crlf)
  (printout t (progn (bind ?x 27) (+ ?x 8)) crlf)

  (printout t "Testing apply: " crlf)
  (printout t (apply + 1 2 3) crlf)
  (printout t (apply sym-cat a b c) crlf)

  )


(printout t "Testing LISP compatibility functions :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  