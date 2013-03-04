(defglobal ?*foo* = 1)

(deftemplate x (slot y))

(deffacts foo (foo ?*foo*))
(set-reset-globals nil)

(defrule foo
  (foo ?f)
  =>
  (printout t ?f crlf))

(deffunction test-something ()

  (try
   (build "(deffacts bad (x (z q)))")
   catch 
   (printout t (call ?ERROR getMessage) crlf))

  (reset)
  (run)
  (bind ?*foo* 2)
  (reset)
  (run)
  )


(printout t "Testing defglobals in deffacts:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  