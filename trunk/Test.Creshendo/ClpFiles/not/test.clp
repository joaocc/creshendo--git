(defrule rule-1
  (not (foo bar))
  ?f1 <- (foo baz)
  (not (foo biddle))
  ?f2 <- (foo bam)
  (not (foo bozo))
  ?f3 <- (foo big)
  (not (foo bimbo))
  ?f4 <- (foo bad)
  =>
  (retract ?f4))

(defrule rule-2
  (var1 ?x)
  (not (a b c))
  (var2 ?y)
  (var3 ?y)
  =>
  (printout t "The variables are: " ?x " " ?y crlf))

(defrule rule-3
  (not (first ?))
  (second ?x)
  =>
  (printout t "No 'first' facts." crlf))

(defrule rule-4
  (rule4-1 ?x)
  (not (rule4-2 ?x))
  (test (eq ?x X))
  =>
  (printout t "rule-4 fired" crlf))

(defrule rule-5
  (rule4-1 ?x)
  (not (rule4-2 ?x))
  (test (eq ?x Y))
  =>
  (printout t "rule-5 fired" crlf))

(deffacts rule-6-facts (A) (B) (C) (D))

(defrule rule-6
  (not (and (A) (B) (C) (D)))
  =>
  (printout t "rule-6 fired" crlf))

(deffunction test-something ()
  ;; ----------------------------------------------------------------------
  (printout t ">>> rule-1" crlf)
  (watch facts)
  (reset)
  (assert
   (foo bam)
   (foo baz)
   (foo big)
   (foo bad))
  (run)

  ;; ----------------------------------------------------------------------
  (printout t ">>> rule-2" crlf)
  (unwatch facts)
  (reset)
  (assert (var1 X) (var2 Y) (var3 Y))
  (run)  

  ;; ----------------------------------------------------------------------
  (printout t ">>> rule-3" crlf)
  (reset)
  (assert (second X))
  (run)

  ;; ----------------------------------------------------------------------
  (printout t ">>> rule-4, rule-5" crlf)
  (reset)
  (assert (rule4-1 X))
  (run)
  
  ;; ----------------------------------------------------------------------

  (printout t ">>> rule-6" crlf)
  (watch all)
  (reset)
  (retract-string "(A)")
  (assert (A))
  (retract-string "(B)")
  (assert (B))
  (retract-string "(C)")
  (assert (C))
  (retract-string "(D)")
  (assert (D))
  (retract-string "(A)")
  (retract-string "(B)")
  (assert (A) (B))
  (retract-string "(B)")
  (retract-string "(C)")
  (retract-string "(D)")
  (assert (B) (C) (D))
  (retract-string "(A)")
  (retract-string "(B)")
  (retract-string "(C)")
  (retract-string "(D)")
  (run)
)


(printout t "Testing not CE:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  