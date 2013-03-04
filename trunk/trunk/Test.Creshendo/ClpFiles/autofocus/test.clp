(printout t "Testing auto-focus :" crlf)

(defmodule DETECTION)

(defrule DETECTION::example 
  (declare (auto-focus TRUE))
  =>
  (printout t "Fire!" crlf))

(watch focus)
(reset)
(run)

(printout t "Test done." crlf)
(exit)  