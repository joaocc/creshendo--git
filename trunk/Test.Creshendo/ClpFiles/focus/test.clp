(printout t "Testing focus :" crlf)

(reset)
(watch focus)
(defmodule foo)
(defmodule bar)

(printout t "Default: " (get-focus) crlf)

(focus foo)
(printout t "explicit foo: " (get-focus) crlf)
(list-focus-stack)

(focus bar)
(printout t "explicit bar: " (get-focus) crlf)
(list-focus-stack)

(try
 (focus baz)
 catch 
 (printout t "Good, got error with bad module name." crlf))

(run)

(printout t "Back to default: " (get-focus) crlf)
(list-focus-stack)

(printout t "Focus multiple: " (focus bar foo) crlf)
(list-focus-stack)

(printout t "Get-focus-stack: " (get-focus-stack) crlf)

(printout t "Pop-focus: " (pop-focus) crlf)
(list-focus-stack)

(clear-focus-stack)
(printout t "Get-focus-stack after clear: " (get-focus-stack) crlf)

(printout t "Test done." crlf)

(exit)  