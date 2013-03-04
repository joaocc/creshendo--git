(deftemplate pattern (slot slot))

(do-backward-chaining pattern)

(defrule rule-1
  (not (pattern (slot 1)))
  =>)
  
(deffunction test-something ()
  (reset)
  (facts)
  )


(printout t "Testing that negated patterns shouldn't trigger backward chaining :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  