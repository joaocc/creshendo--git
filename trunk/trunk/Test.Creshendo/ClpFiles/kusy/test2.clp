(printout t "Testing incremental reset:" crlf)

(deffacts the-facts
        (point -2)
        (point -1)
        (point 0)
        (point 1)
        (point 2)
        (point 3)
        (point 4)
        (point 5)
        (point 6)
        (point 7)
)

(reset)


(defrule minimum
        (point ?x)
        (not (point ?y&:(< ?y ?x)))
        =>
        (printout t "minimum=" ?x crlf)
)

(defrule maximum
        (point ?x)
        (not (point ?y&:(> ?y ?x)))
        =>
        (printout t "maximum=" ?x crlf)
)


(run)
(printout t "Test done." crlf)

