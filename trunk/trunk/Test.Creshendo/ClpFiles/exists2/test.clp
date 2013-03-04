;; Test from Jack Kerkhof

(deffacts facts
  (a harry moe)
  (a larry moe)
  (b fat harry)
  (b skinny moe)
  (b sidekick george)
  (c fat))

; base case rule. fires as expected.
(defrule r1
  (declare (salience 1))
; establish two variables:
  (a ?x ?y)
  
; use variable ?type in slot 1, (or) function in slot 2
  (b ?type ?d&:(or (eq ?d ?x) (eq ?d ?y)))
  
; match against ?type above. ?type appears to be defined.
  (c ?type)
  =>
  (printout t "R1 FIRES with " ?x " " ?y "  " ?type " and " ?d crlf)
  
  )

; test case: identical to r1, but with (exists). Never fires.
(defrule r2
  (declare (salience 2))
  (a ?x ?y)
  (b ?type ?d&:(or (eq ?d ?x) (eq ?d ?y)))
  
; as per r1, but with (exists)
;  Now it appears as though ?type is not defined.
  (exists (c ?type))
  =>
  (printout t "R2 FIRES with " ?x " " ?y "  " ?type " and " ?d crlf)
  
  )


; test case r3 is the same as r1, except for the 'OR' and it's ?y expression
; fires as expected.
(defrule r3
  (declare (salience 3))
  (a ?x ?y)
  (b ?type ?d&:(eq ?d ?x))
  (exists (c ?type))
  =>
  (printout t "R3 FIRES with " ?x " " ?y "  " ?type " and " ?d crlf)
  
  )

; test case r4. identical to r2, but with (not) instead of (exists).
; fires as expected.
(defrule r4
  (declare (salience 4))
  (a ?x ?y)
  (b ?type ?d&:(or (eq ?d ?x) (eq ?d ?y)))
  
; as per r2, but with (not)
;  Now it appears as though ?type IS defined.
  (not (c ?type))
  =>
  (printout t "R4 FIRES with " ?x " " ?y "  " ?type " and " ?d crlf)
  
  )

(deffunction test-something ()
  (reset)
  (run))

(printout t "Testing exists CE :" crlf)
(test-something)
(printout t "Test done." crlf)
;; (exit)  