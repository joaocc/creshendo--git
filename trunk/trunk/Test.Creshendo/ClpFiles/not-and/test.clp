
(defrule foo
  (not 
   (and (a ?x)
        (test (eq ?x 1))))
  =>)
  
(defrule bar
  (not
   (and (B ?p)
        (A ?p&:(< ?p 10))))
  =>)

(reset)
(printout t "Test done." crlf)
(exit)  