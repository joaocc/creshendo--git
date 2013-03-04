(deffunction test-1 ()
  (printout t "This is a deffunction" crlf))

(deffunction test-2 (?a)
  (printout t "This is a simple argument: " ?a crlf))

(deffunction test-3 ($?a)
  (printout t "This is a wildcard argument: " $?a crlf))

(deffunction test-4 (?a $?b)
  (printout t "This is mixed arguments: " ?a " " $?b crlf))

(deffunction test-5 ()
  (printout t "Testing functional return values: ")
  (+ 3 3))

(deffunction test-6 ()
  (printout t "Testing simple return values: ")
  foo)

(deffunction test-7 ()
  (printout t "Testing return values in the middle of a block: ")
  (return (* 7 3))
  (printout t "Ooops, this shouldn't get called!" crlf))

(deffunction test-8 ()
  (printout t "Testing return values in the middle of a foreach: ")
  (foreach ?i (create$ a b c)
           (return TRUE))
  (printout t "Ooops, this shouldn't get called!" crlf))


(printout t "Testing deffunction:" crlf)
(test-1)
(test-2 foo)
(test-3 foo bar baz)
(try
 (test-3)
 (printout t OK crlf)
 catch
 (printout t "Error - can't call wildcard with no args." crlf))
(test-4 foo bar baz)
(printout t (test-5) crlf)
(printout t (test-6) crlf)
(printout t (test-7) crlf)
(printout t (test-8) crlf)
(printout t "Test done." crlf)
(exit)  
