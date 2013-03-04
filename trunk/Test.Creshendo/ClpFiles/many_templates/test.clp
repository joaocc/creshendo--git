(deffunction test-something ()
  (bind ?alpha (create$ a b c d e f g h i j k l m n o p q r s t u v w x y z)) 
	(foreach ?i ?alpha
    	(foreach ?j ?alpha
      		(foreach ?k ?alpha
               (bind ?name (sym-cat ?i ?j ?k 1))
               ;;(bind ?name (sym-cat ?i ?j 1))
               (bind ?rule (str-cat "(defrule " ?name " (" ?name ") => )"))
               (eval ?rule)
       		)
     	)
   	)

  	(foreach ?i ?alpha
    	(foreach ?j ?alpha
      		(foreach ?k ?alpha
               (bind ?name (sym-cat ?i ?j ?k 1))
               ;;(bind ?name (sym-cat ?i ?j 1))
               (assert-string (str-cat "(" ?name ")"))
       		)
    	)
	)
  
)

(printout t "Testing many, many templates: :" crlf)
(bind ?time (time))
(test-something)
(if (> (- (time) ?time) 20) then
  (printout t "Test took too long." crlf))
(printout t "Test done." crlf)
(exit)  