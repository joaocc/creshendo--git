(clear)
(printout t "Testing return in rules :" crlf)

(defmodule DETECTION)

(defrule MAIN::start
  =>
  (focus DETECTION))

(defrule DETECTION::example-1
  =>
  (return)
  (printout t "No printout!" crlf))

(defrule DETECTION::example-2
  =>
  (printout t "Finally!" crlf))

(watch all)
(reset)
(run)

(focus DETECTION)
(run)

(printout t "Test done." crlf)
(exit)  