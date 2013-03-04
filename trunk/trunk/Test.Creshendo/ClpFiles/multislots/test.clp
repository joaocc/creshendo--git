(defrule example-3
    (different ?d ~?d)
    (different ?d1 ?d2&~?d1)
    (same ?s ?s)
    (same ?s1 ?s2&?s1)
    =>
    (printout t ?d " " ?d1 " " ?d2 " " ?s " " ?s1 " " ?s2 crlf))

(deffacts data
  (same 1 2)
  (same 3 3)
  (different A A)
  (different B C))

(deffunction test-something ()
  (reset)
  (run))


(printout t "Testing multislot phase-two matching:" crlf)
(test-something)
(printout t "Test done." crlf)
