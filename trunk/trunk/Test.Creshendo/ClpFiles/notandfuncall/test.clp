
(printout t "Testing that variable-renaming doesn't "
          "affect subsequent patterns :" crlf)

(deftemplate A (slot a))
(deftemplate B (slot b))

(defrule foo
  (initial-fact)
  (not
   (and
    (A (a ?o&:(< ?o 1)))
    (B (b ?p))
    ))
  =>
)   

(defrule boo
  (initial-fact)
  (not
   (and
    (B (b ?p))
    (A (a ?o&:(< ?o 1)))
    ))
  =>
)   

(defrule bar
  (not
   (and
    (B (b ?p))
    (A (a ?o&:(< ?o 1)))
    ))
  =>
)   

(defrule baz
  (not
   (and
    (A (a ?o&:(< ?o 1)))
    (B (b ?p))
    ))
  =>
)   

(reset)
(printout t "Test done." crlf)
;; (exit)  

