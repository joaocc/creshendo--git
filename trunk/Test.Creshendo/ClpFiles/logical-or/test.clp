
(deftemplate person
  (slot name)
  (slot city)
)

(defrule eastcoast-rule
  (logical (person (city boston|newyork)))
  =>
  (assert (someone-on-eastcoast))
)


(deffunction test-something ()
(reset)

(run)
(facts)

(bind ?john-fact (assert (person (name john) (city denver))))
(run)
(facts)

(bind ?judy-fact (assert (person (name judy) (city boston))))
(run)
(facts)

(retract ?judy-fact)
(run)
(facts)

(retract ?john-fact)
(run)
(facts)
  )

(printout t "Testing '|' inside logical :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  