(deftemplate factorial (slot in) (slot out))

(do-backward-chaining factorial)

(deffacts one-value (factorial (in 5) (out 120)))

(defrule print-factorial-10
  (factorial (in 10) (out ?r1))
  =>
  (printout t "The factorial of 10 is " ?r1 crlf))

(defrule print-factorial-10-and-12
  (factorial (in 12) (out ?r1))
  (factorial (in 10) (out ?r2))
  =>
  (printout t "The factorial of 12 is " ?r1
              ", and of 10 is " ?r2 crlf))

(defrule print-factorial-5-10-and-12
  "Shows how chaining won't be done if fact already exists"
  (factorial (in 5) (out ?r0))
  (factorial (in 12) (out ?r1))
  (factorial (in 10) (out ?r2))
  =>
  (printout t "The factorial of 12 is " ?r1
              ", and of 10 is " ?r2 ", and of 5 is " ?r0 crlf))

(defrule print-factorial-1000
  "Shows how explicit modifer can be used to prevent backwards chaining."
  ?a <- (explicit (factorial (in 1000) (out ?r1)))
  =>
  (printout t "The factorial of 1000 is " ?r1 crlf))


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


(printout t "Testing backwards chaining in deftemplates:" crlf)
(reset)
(run)
(printout t "Test part 1 done." crlf)

(clear)
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
(deffacts one-value (factorial 5 120))

(reset)

(run)
(printout t "Test part 2 done." crlf)
(clear)

(deftemplate point
  (slot x (default 0)) (slot y (default 0)))

(do-backward-chaining point)

(defrule plot-point
  ?p1 <- (point (x 0) (y ?y1))
  ?p2 <- (point (x 1) (y ?y2))
  =>
  (printout t "points at 0," ?y1 " and 1," ?y2 crlf)
  )

(defrule generate-points-1
  (need-point (x 0))
  =>
  (assert (point (x 0) (y 0))))

(defrule generate-points-2
  (need-point (x 1))
  =>
  (assert (point (x 1) (y 1))))

(printout t "Testing multiple need-x rules with one x rule" crlf)

(reset)
(run)
(printout t "Test part 3 done." crlf)
(clear)

(do-backward-chaining foo)
(do-backward-chaining bar)

(defrule rule-1
  (foo ?A ?B)
  =>
  (printout t foo crlf))

(defrule create-foo
  (need-foo $?)
  (bar ?X ?Y)
  =>
  (assert (foo A B)))

(defrule create-bar
  (need-bar $?)
  =>
  (assert (bar C D)))

(printout t "Testing multistage chaining" crlf)
(reset)
(run)
(printout t "Test part 4 done." crlf)
(clear)

(printout t "Testing multi-item ordered fact chaining" crlf)

(watch facts)
(do-backward-chaining foo)

(defrule abc
  (bar ?a ?b)
  (foo ?a ?b)
  =>
  )

(assert (bar a b))
(printout t (call (call (engine) findFactByID 1) ToStringWithParens) crlf)
(printout t (nth$ 1 (call (call (engine) findFactByID 1) get 0)) crlf)
(printout t (nth$ 2 (call (call (engine) findFactByID 1) get 0)) crlf)

(printout t "Test part 5 done." crlf)






