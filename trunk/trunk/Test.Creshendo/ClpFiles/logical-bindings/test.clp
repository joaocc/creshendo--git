(deftemplate Super (slot ref))
(deftemplate A extends Super)
(deftemplate B extends Super)
(deftemplate C extends Super)
(deftemplate D extends Super)


( defrule ARule
    (logical
        ( A (ref ?a))
        ( B (ref ?b))
    )
    ( C (ref ?c))
=>
    ( printout t "-->?a = " ?a crlf )
    ( printout t "-->?b = " ?b crlf )
    ( printout t "-->?c = " ?c crlf )
    (assert
        (D (ref ?c))
    )
)


(assert (A (ref "a")))
(assert (B (ref "b")))
(assert (C (ref "c")))

(run)