
(defmodule A)

(deftemplate A::foo (slot X))

(defmodule B)

(defrule A::rule
  (foo (X 1))
  =>)

(printout t "Test part 1 OK" crlf)

(printout t "ALL" crlf)
(list-deftemplates *)

(printout t "MAIN" crlf)
(list-deftemplates MAIN)

(printout t "A" crlf)
(list-deftemplates A)

(printout t "B" crlf)
(list-deftemplates B)

(printout t "Focus" crlf)

(set-current-module A)

(list-deftemplates A)

(exit)  