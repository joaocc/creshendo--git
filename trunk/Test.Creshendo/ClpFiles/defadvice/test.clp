(deffunction test-something ()
  (printout t "*** defadvice before a single function:" crlf)
  (defadvice before + (bind $?argv (create$ $?argv 1)))
  (printout t "2 and 2 is " (+ 2 2) "?" crlf)
  (printout t "*** undefadvice before a single function:" crlf)
  (undefadvice +)
  (printout t "2 and 2 is actually " (+ 2 2) crlf)

  (printout t "*** defadvice before with short-circuit:" crlf)
  (defadvice before + (return 1))
  (printout t "2 and 2 is " (+ 2 2) "?" crlf)
  (undefadvice +)
  (printout t "No, 2 and 2 is really " (+ 2 2) crlf)

  (printout t "*** defadvice after:" crlf)
  (defadvice after + (return (+ ?retval 1)))
  (printout t "2 and 2 is " (+ 2 2) "?" crlf)
  (undefadvice +)
  (printout t "No, 2 and 2 is really " (+ 2 2) crlf)

  (printout t "*** defadvice group:" crlf)
  (defadvice after (create$ + - * /) (printout t arithmetic: " "))
  (printout t "2 and 2 is " (+ 2 2) crlf)
  (printout t "2 times 2 is " (* 2 2) crlf)
  (printout t "2 minus 2 is " (- 2 2) crlf)
  (printout t "2 over 2 is " (/ 2 2) crlf)
  (printout t "*** undefadvice group:" crlf)
  (undefadvice (create$ + - * /))
  (printout t "2 and 2 is " (+ 2 2) crlf)
  (printout t "2 times 2 is " (* 2 2) crlf)
  (printout t "2 minus 2 is " (- 2 2) crlf)
  (printout t "2 over 2 is " (/ 2 2) crlf)

  )



(printout t "Testing defadvice:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  