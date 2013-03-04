(defrule rule-1
?x <- (or (a) (b))
=>)

(printout t (ppdefrule rule-1) crlf)
(printout t (ppdefrule "rule-1&1") crlf)


(defrule rule-2
(or ?x<-(a) ?y<-(b))
=>)

(printout t (ppdefrule rule-2) crlf)
(printout t (ppdefrule "rule-2&1") crlf)

    
(exit)  