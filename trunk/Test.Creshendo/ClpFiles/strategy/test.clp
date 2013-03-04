(watch activations)

(defrule rule-1
  (declare (salience -100))
  (fact-1)
  =>
  (printout t "RULE 1 FIRED (salience -100)" crlf))

(defrule rule-2
  (declare (salience -100))
  (fact-2)
  =>
  (printout t "RULE 2 FIRED (salience -100)" crlf))

(defrule rule-3
  (fact-3)
  =>
  (printout t "RULE 3 FIRED (salience 0)" crlf))

(defrule rule-4
  (declare (salience 100))
  (fact-4)
  =>
  (printout t "RULE 4 FIRED (salience 100)" crlf))

(defrule rule-6
  (declare (salience 100))
  (fact-5)
  (fact-4)
  =>
  (printout t "RULE 6 FIRED (salience 100)" crlf))

(defrule rule-7
  (declare (salience 100))
  (fact-5)
  (fact-4)
  (fact-3)
  =>
  (printout t "RULE 7 FIRED (salience 100)" crlf))

(defrule rule-5
  (declare (salience 100))
  (fact-5)
  =>
  (printout t "RULE 5 FIRED (salience 100)" crlf))


(deffacts test-data (fact-1) (fact-2) (fact-3) (fact-4) (fact-5))

(deffunction test-something ()  
  (printout t ">>> default" crlf)
  (printout t (get-strategy) crlf)
  (reset)
  (run)
  (printout t ">>> breadth" crlf)
  (set-strategy breadth)
  (printout t (get-strategy) crlf)
  (reset)
  (run)
  (printout t ">>> depth" crlf)
  (set-strategy depth)
  (printout t (get-strategy) crlf)
  (reset)
  (run)  
  )

(printout t "Testing conflict-resolution strategies:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  

