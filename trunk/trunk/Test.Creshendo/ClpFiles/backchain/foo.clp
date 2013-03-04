(do-backward-chaining factorial)

(defrule print-factorial-10
  (factorial 10 ?r1)
  =>
  (printout t "The factorial of 10 is " ?r1 crlf))

(defrule print-factorial-10-and-12
  (factorial 12 ?r1)
  (factorial 10 ?r2)
  =>
  (printout t "The factorial of 12 is " ?r1
              ", and of 10 is " ?r2 crlf))

(defrule print-factorial-5-10-and-12
  "Shows how chaining won't be done if fact already exists"
  (factorial 5 ?r0)
  (factorial 12 ?r1)
  (factorial 10 ?r2)
  =>
  (printout t "The factorial of 12 is " ?r1
              ", and of 10 is " ?r2 ", and of 5 is " ?r0 crlf))

(defrule print-factorial-1000
  "Shows how explicit modifer can be used to prevent backwards chaining."
  ?a <- (explicit (factorial 1000 ?r1))
  =>
  (printout t "The factorial of 1000 is " ?r1 crlf))


(defrule do-factorial
  (need-factorial ?x ?)
  =>
  (printout t "( Calculating the factorial of " ?x "... )" crlf)
  (bind ?r 1)
  (bind ?n ?x)
  (while (> ?n 1)
    (bind ?r (* ?r ?n))
    (bind ?n (- ?n 1)))
  (assert (factorial ?x ?r)))

(printout t "Testing backwards chaining in ordered facts:" crlf)
(reset)
(assert (factorial 5 120))

(run)
(printout t "Test part 2 done." crlf)

