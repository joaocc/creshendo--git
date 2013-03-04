(deffunction compute-salience (?name)
  (fetch ?name))

(store rule-1 1)
(store rule-2 2)
(store rule-3 3)

(defrule rule-1
  (declare (salience (compute-salience rule-1)))
  =>
  (printout t "rule-1 fired" crlf)
  (store rule-3 10))

(defrule rule-2
  (declare (salience (compute-salience rule-2)))
  =>
  (printout t "rule-2 fired" crlf))

(defrule rule-3
  (declare (salience (compute-salience rule-3)))
  =>
  (printout t "rule-3 fired" crlf))


(deffunction test-something ()  
  (set-salience-evaluation when-defined)
  (printout t ">>> " (get-salience-evaluation) crlf)
  (reset)
  (run)
  (store rule-1 3)
  (store rule-2 2)
  (store rule-3 1)
  (set-salience-evaluation when-activated)
  (printout t ">>> " (get-salience-evaluation) crlf)
  (reset)
  (run)
  (set-salience-evaluation every-cycle)
  (printout t ">>> " (get-salience-evaluation) crlf)
  (reset)
  (run)
  
  )


(printout t "Testing dynamic salience :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  