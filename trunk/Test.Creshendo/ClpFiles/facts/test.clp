(deftemplate dtfoo (slot bar) (slot baz))
(deftemplate dyntemp (slot normal) (slot dyn (default-dynamic (+ 2 3))))

(deffunction test-something ()
  ;; ----------------------------------------------------------------------
  (printout t ">>> ppdeftemplate default-dynamic" crlf)
  (printout t (ppdeftemplate dyntemp) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> Duplicate asserts" crlf)
  (reset)
  (assert (foo bar) (bar foo))
  (assert (foo bar))
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> Reset clears" crlf)
  (build "(defrule rule1 (foo bar) => )")
  (agenda)
  (reset)
  (facts)
  (agenda)
  ;; ----------------------------------------------------------------------
  (printout t ">>> Assert-string" crlf)
  (reset)
  (assert-string "( foo bar foo)")
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> Assert-string + default-dynamic" crlf)
  (reset)
  (assert-string "( dyntemp (normal 1) )")
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> Deftemplate" crlf)
  (reset)
  (assert (dtfoo (bar 1) (baz 2)))
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> load facts" crlf)
  (reset)
  (assert (a 2 "3" iv)
          (dtfoo (bar 2))
          (dtfoo (bar "two"))
          (dtfoo (bar ii)))
  (save-facts "factfile.clp")
  (reset)
  (facts)
  (load-facts "factfile.clp")
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> modify" crlf)
  (reset)
  (bind ?fid (assert (dtfoo (bar 1))))
  (facts)
  (modify ?fid (bar 2) (baz 3))
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> fact-slot-value" crlf)
  (reset)
  (assert (dtfoo (bar foobar)))
  (printout t (fact-slot-value (fact-id 1) bar)  crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> retract" crlf)
  (reset)
  (assert (foo bar))
  (facts)
  (retract (fact-id 0))
  (facts)
  (printout t ">>> retract-string" crlf)
  (reset)
  (assert (foo bar))
  (facts)
  (retract-string "(foo bam)")
  (facts)
  (retract-string "(foo bar)")
  (facts)
  ;; ----------------------------------------------------------------------
  (printout t ">>> many facts" crlf)
  (reset)
  (bind ?i 1)
  (while (< ?i 1000) do
         (assert (foo ?i))
         (bind ?i (+ ?i 1))
         )
  (printout t (call (call (engine) retractString "(foo 57)") getFactId) crlf)  
  (printout t (call (call (engine) retractString "(foo 257)") getFactId) crlf)  
  (printout t (call (call (engine) retractString "(foo 357)") getFactId) crlf)  
  (printout t (call (call (engine) retractString "(foo 657)") getFactId) crlf)  
  (printout t (call (call (engine) retractString "(foo 857)") getFactId) crlf)  
)
(printout t "Testing facts:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  