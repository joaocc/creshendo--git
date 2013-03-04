(deftemplate factorial (slot in) (slot out))

(do-backward-chaining factorial)

(deffacts one-value (factorial (in 5) (out 120)))

(defquery get-factorial-1
  (declare (variables ?x))
  (factorial (in ?x) (out ?y)))

(defquery get-factorial-2
  (declare (variables ?x)
           (max-background-rules 10))
  (factorial (in ?x) (out ?y)))

(defrule do-factorial
  (need-factorial (in ?x))
  =>
  (printout t "( Calculating the factorial of " ?x "... )" crlf)
  (bind ?r 1)
  (bind ?n ?x)
  (while (> ?n 1)
    (bind ?r (* ?r ?n))
    (bind ?n (- ?n 1)))
  (assert (factorial (in ?x) (out ?r))))

(printout t "Testing backwards chaining in during defqueries:" crlf)

(deffacts facts (factorial (in 5) (out 120)))
(reset)

(printout t "#1, in WM " (count-query-results get-factorial-1 5) crlf)
(printout t "#1, not in WM " (count-query-results get-factorial-1 10) crlf)

(reset)

(printout t "#2, in WM " (count-query-results get-factorial-2 5) crlf)
(printout t "#2, not in WM " (count-query-results get-factorial-2 10) crlf)









