(defmodule FOO)
(defquery query (foo))

(defmodule BAR)

(defrule FOO::foo
  =>
  (run-query query))

(deffunction test-something ()
  (reset)
  (focus FOO)
  (printout t (run) " rules fired." crlf)
  )


(printout t "Testing query name resolution :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  