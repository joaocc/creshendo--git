
(printout t "Testing modules :" crlf)

(printout t "Current module is " (get-current-module) crlf)

(defrule rule1 => )

(printout t (ppdefrule MAIN::rule1) crlf)

(defmodule foo)

(printout t "Good, got no error" crlf)

(printout t "Current module is " (get-current-module) crlf)

(defrule rule2 => )

(printout t (ppdefrule MAIN::rule2) crlf)

(printout t (ppdefrule foo::rule2) crlf)

(printout t "Lookup against current module" crlf)
(printout t (ppdefrule rule2) crlf)

(defmodule bar)

(try
 (defmodule)
 catch
 (printout t "Good, got syntax error" crlf))

(try
 (defmodule foo)
 catch
 (printout t "Good, got redefinition error" crlf))

(printout t "Current module is " (get-current-module) crlf)

(printout t "Test change current module to foo:" crlf)

(set-current-module foo)

(printout t "Current module is " (get-current-module) crlf)

(printout t "Test done." crlf)
(exit)  