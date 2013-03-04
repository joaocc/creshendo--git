(printout t "Testing str-cat :" crlf)

(reset)

(defglobal ?*x* = 0)

(printout t  "x is " ?*x* crlf)
(printout t  "increment of x is 2nd argument to str-cat" crlf)
(printout t  (str-cat "incr x by 1 results in " (bind ?*x* (+ ?*x* 1))) crlf)
(printout t  (str-cat "incr x by 1 results in " (bind ?*x* (+ ?*x* 1))) crlf)
(printout t  (str-cat "incr x by 1 results in " (bind ?*x* (+ ?*x* 1))) crlf)
(printout t  (str-cat "incr x by 1 results in " (bind ?*x* (+ ?*x* 1))) crlf)
(printout t  "Set x back to " (bind ?*x* 0) crlf)
(printout t  "increment of x is 1st argument to str-cat" crlf)
(printout t  (str-cat (bind ?*x* (+ ?*x* 1)) " is value of x after incr by 1") crlf)
(printout t  (str-cat (bind ?*x* (+ ?*x* 1)) " is value of x after incr by 1") crlf)
(printout t  (str-cat (bind ?*x* (+ ?*x* 1)) " is value of x after incr by 1") crlf)
(printout t  (str-cat (bind ?*x* (+ ?*x* 1)) " is value of x after incr by 1") crlf)

(printout t "Test done." crlf)
