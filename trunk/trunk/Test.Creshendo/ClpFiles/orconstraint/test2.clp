; is there some semantic error in this rule LHS?
(defrule r1
  (declare (salience 1))
 (a ?x ?y)
 (b ?x|?y)
=>
 (printout t "R1 FIRES with " ?x " " ?y crlf)
)


; identical to R1, but a constant as first pattern in OR clause
(defrule r2
  (declare (salience 2))
 (a ?x ?y)
 (b larry|?x|?y)
=>
 (printout t "R2 FIRES with " ?x " " ?y crlf)
)

; identical to R1, but a "variable &"  pattern before OR clause
(defrule r3
  (declare (salience 3))
 (a ?x ?y)
 (b ?c&?x|?y)
=>
 (printout t "R3 FIRES with " ?c " " ?x " " ?y crlf)
)

(defrule r4
  (declare (salience 4))
; identical to R1, but a "variable OR" pattern before (second) OR clause(defrule r4
 (a ?x ?y)
 (b ?d|?x|?y)
=>
 (printout t "R4 FIRES with " ?d " " ?x " " ?y crlf)
)

(deffacts facts
  (a harry moe)
  (b harry)
  (b moe)
  (b george))

(deffunction test-something ()
  (reset)
  (run))

(printout t "Testing the '|' connective constraint:" crlf)
(test-something)
(printout t "Test done." crlf)
