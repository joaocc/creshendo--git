(deffacts foo
  (foo blue red)
  (foo blue blue)
  (foo blue purple)
  (foo red blue))

(deffunction test-something ()
  (build "(defquery foo (declare (variables ?X)) (foo ?X ?Y))")
  (printout t (ppdefquery foo) crlf)
  (reset)  
  (bind ?e (run-query foo blue))

  (while (?e MoveNext)
    (printout t (nth$ 2 (call (call (get ?e Current) fact 1) get 0)) crlf))

  (printout t "There are " (count-query-results foo blue) " results." crlf)

  (build "(defquery bar (declare (variables ?X ?Y)) (foo ?X ?Y))")
  (printout t (ppdefquery bar) crlf)
  (reset)
  (bind ?e (run-query bar blue red))

  (while (?e MoveNext)
    (printout t (call (call (get ?e Current) fact 1) ToString) crlf))

  (printout t "There are " (count-query-results bar blue red) " results." crlf)

  (clear)
  
  (printout t "Testing multipart queries:" crlf)
  (build "(defquery foo (or (a) (b) (c) (d) ))")
  (printout t (ppdefquery foo) crlf)
  (printout t (ppdefquery "foo&1") crlf)
  (printout t (ppdefquery "foo&2") crlf)
  (printout t (ppdefquery "foo&3") crlf)
  (assert (a) (b) (c) (d))
  (printout t "There are " (count-query-results foo) " results." crlf)
)

(printout t "Testing defquery:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  