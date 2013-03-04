(assert (foo 1))

(try
 (eval "(defrule foo (foo ?x&:(> x 0)) =>)")
 (printout t "Ooops, no exception!")
 catch 
 ;; OK
 )
(rules)