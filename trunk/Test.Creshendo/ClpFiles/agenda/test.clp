﻿(defmodule module-1)
(defmodule module-2)

(defrule module-1::rule-1 => )
(defrule module-1::rule-2 => )
(defrule module-2::rule-3 => )
(defrule module-2::rule-4 => )

(deffunction test-something ()
  (reset)
  (printout t "Explicit module 1" crlf)
  (agenda module-1)
  (rules module-1)
  (printout t "Explicit module 2" crlf)
  (agenda module-2)
  (rules module-2)
  (printout t "No qualifier -- should be module-2" crlf)
  (agenda)
  (rules)
  (printout t "Agenda *" crlf)
  (agenda *)
  (rules *)
  )


(printout t "Testing agenda command :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  