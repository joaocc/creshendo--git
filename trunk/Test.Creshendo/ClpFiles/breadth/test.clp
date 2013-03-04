(defrule rule-1
  (A ?X)
  (B ?X)
  => 
  (printout t A " " ?X crlf))

(defrule rule-2
  (C ?X)
  =>
  (printout t C " " ?X crlf))

(deffunction test-something ()
  (reset)
  (assert (padding)) ;; fact 1
  (assert (A 1)) ;; fact 2
  (assert (B 1)) ;; fact 3
  (assert (C 1)) ;; fact 4
  (run)
  )


(printout t "Testing strategies :" crlf)
(set-strategy breadth)
(test-something)
(set-strategy depth)
(test-something)
(printout t "Test done." crlf)
(exit)  