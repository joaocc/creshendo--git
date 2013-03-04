(defrule rule-1
  "An easy one"
  (foo A B C)
  =>
  (printout t "Hi" crlf))

(defrule rule-2
  "Groups"
  (not (and (A) (B)))
  (and (C) (D))
  (and (E) (not (and (F) (G))))
  =>)

(defrule rule-3
  "Bindings 1"
  ?x <- (x)
  (and ?y <- (y) ?z <- (z) (not (a)))
  =>)

(defrule rule-4
  "Bindings 2"
  ?a <- (or (a) (b))
  =>)
  


(deffunction test-something ()
  (printout t (ppdefrule rule-1) crlf crlf)
  (printout t (ppdefrule rule-2) crlf crlf)
  (printout t (ppdefrule rule-3) crlf crlf)
  (printout t (ppdefrule rule-4) crlf crlf)
  (printout t (ppdefrule "rule-4&1") crlf crlf)
  )


(printout t "Testing pretty-printing :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  