(deffunction foo (?x)
  (if (< ?x 0) then
    (return 0)
    else
    (foo (- ?x 1))
    (printout t ?x crlf)
    )
  )

(deffunction test-something ()
  (foo 10)
  )


(printout t "Testing recursive deffunction call:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  