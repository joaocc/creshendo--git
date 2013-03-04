(defrule MAIN::rule-1 => )
(defrule MAIN::rule-2 => )
(defrule MAIN::rule-3 => )
(defrule MAIN::rule-4 => )


(printout t "Make sure single stepping works" crlf)
(reset)
(printout t "1: " (run 1) crlf)
(printout t "2: " (run 2) crlf)
(printout t "1: " (run 1) crlf)

(printout t "0: " (run 1) crlf)

(printout t "Test done." crlf)