(deffunction test-something ()
  ;; ----------------------------------------------------------------------
  (printout t ">>> foreach" crlf)
  (foreach ?i (create$ a b c d e f g)
    (printout t ?i " "))
  (printout t crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> eval" crlf)
  (printout t (eval "(+ 3 2)") crlf)
  (printout t (eval "(str-cat 123 aaa \"bbb\" )") crlf)
  (reset)
  (printout t (eval "(assert (foo bar))") crlf)
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> build" crlf)
  (build "(defrule foo (a b) => (printout t \"RULE FIRED\" crlf))")
  (reset)
  (assert (a b))
  (run)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> if" crlf)
  (if (> 3 1) then
    (printout t "3 is greater than one" crlf)
    else
    (printout t "3 is less than one??" crlf))
  (if (> 1 3) then
    (printout t "3 is less than one??" crlf)
    else
    (printout t "3 is greater than one" crlf))

  (bind ?myVar 1)
  (bind $?a (create$ A B C))
  (bind $?b (create$ D E F))
  
  (bind $?arrayToUse
        (if (eq ?myVar 0) then $?a else $?b))
  (printout t $?arrayToUse crlf)

  ;; ----------------------------------------------------------------------
  (printout t ">>> halt" crlf)
  (build "(defrule bar => (printout t \"I should not fire!\" crlf))")
  (build "(defrule foo (declare (salience 100)) => (printout t \"FIRE!\" crlf) (halt))")
  (reset)
  (run)
  ;; ----------------------------------------------------------------------
  (printout t ">>> while" crlf)
  (bind ?x 10)
  (while (> ?x 0) do
         (printout t ?x " ")
         (bind ?x (- ?x 1)))
  (printout t crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> system" crlf)
  (system ls test.clp)
  ;; ----------------------------------------------------------------------
  (printout t ">>> batch" crlf)
  (printout t (batch test2.clp) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">> call-on-engine" crlf)
  (reset)
  (bind ?e (new CSharpJess.jess.Rete))
  (call ?e addOutputRouter WSTDOUT (call (engine) getOutputRouter WSTDOUT))
  (call-on-engine ?e (assert (foo bar)))
  (assert (bar foo))
  (call (engine) ppFacts (call (engine) getOutputRouter WSTDOUT))
  (call ?e ppFacts (call ?e getOutputRouter WSTDOUT))
                  

  ;; ----------------------------------------------------------------------
  ;; Should test (run-until-halt) here.
  ;; ----------------------------------------------------------------------

  (return (+ 3 2))
)

(printout t "Testing control structures:" crlf)
(defglobal ?*x* = (test-something))

(printout t ">>> return" crlf ?*x* crlf)
(printout t "Test done." crlf)

(printout t ">>> exit" crlf)
(exit)  
(printout t "This should not appear!" crlf)

