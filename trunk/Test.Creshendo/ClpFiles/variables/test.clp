(defrule test-rule
  (foo ?X)
  =>
  (printout t ?X crlf)
  (bind ?Y foo)
  (printout t ?Y crlf)
  (bind ?X baz)
  (printout t ?X crlf))


(defglobal ?*g1* = "foo")
(defglobal ?*g2* = "bar")

(defrule test-rule-2
  (test (or (eq ?*g1* ?*g2* )
            (eq ?*g1* ?*g2* )))
  =>
  (printout t OK crlf)
  )

(deffunction set-var (?value)
  (bind ?x ?value)
  (printout t ?x " ")
  )

(deffunction set-gvar (?value)
  (bind ?*g* ?value)
  (printout t ?*g* " ")
  )

(defglobal ?*g* = 0)

(deffunction test-something ()
  (printout t ">>> bind" crlf)
  (bind ?x 3)
  (printout t ?x " ")
  (set-var 4)
  (printout t ?x crlf)
  (printout t ">>> defglobal" crlf)
  (bind ?*g* 3)
  (printout t ?*g* " ")
  (set-gvar 4)
  (printout t ?*g* crlf)
  (printout t ">>> rule variable bindings" crlf)
  (assert (foo bar))
  (run)
  (printout t ">>> globals on rule LHS" crlf)
  (bind ?*g1* "bar")
  (set-reset-globals nil)
  (reset)
  (run)
  (printout t ">>> context" crlf)
  (printout t (call (context) getVariable "*g*") crlf)
  (printout t ">>> get/set-reset-globals" crlf)
  (set-reset-globals TRUE)
  (bind ?*g* 37)
  (printout t ?*g* " ")
  (reset)
  (printout t ?*g* " " (get-reset-globals) crlf)
  (set-reset-globals FALSE)
  (bind ?*g* 79)
  (printout t ?*g* " ")
  (reset)
  (printout t ?*g* " " (get-reset-globals) crlf)
  )


(printout t "Testing variables:" crlf)
(test-something)
(bind ?*bogus* 3)
(reset)
(try
 (printout t "?*bogus* = " ?*bogus* crlf)
 catch
 (printout t "Good, bogus globals undefined" crlf))

(printout t "Test done." crlf)
(exit)