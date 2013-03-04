(defrule rule-1 (not (x ?x)) (b ?x) => )

(defrule rule-2 (and (not (x ?x)) (b ?x)) => )

;; Note that properly renamed (i.e., (b ?y)) Jess can already
;; match this rule correctly; it just can't do the renaming correctly
;; yet. 
(defrule rule-3 (not (and (not (x ?x)) (b ?x))) => )

(deffunction test-something ()
  (printout t (ppdefrule rule-1) crlf)
  (printout t (ppdefrule rule-2) crlf)
  (printout t (ppdefrule rule-3) crlf)
  (reset)
  )


(printout t "Testing not inside an and inside a not :" crlf)
(printout t (test-something) crlf)
(printout t "Test done." crlf)
;; (exit)  