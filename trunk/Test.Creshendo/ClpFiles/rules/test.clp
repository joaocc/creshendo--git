(deftemplate foo (slot bar))

(defrule foo
  (A B 1.0)
  (not (B C D))
  (foo (bar ?X))
  =>
  (printout t ?X crlf))

(defrule bar
  (bar A B C)
  =>
  (printout t "bar fired!" crlf))

(defrule foobar
  (foobar)
  (foobar ?X&:(< ?X 10))
  =>
  (printout t ?X " is less than ten." crlf))

(deffunction test-something ()
  (printout t ">>> errors" crlf)
  (try
   (build "(defrule bad (foo (baz x)) => )")
   catch 
   (printout t (call ?ERROR getMessage) crlf))
  (printout t ">>> ppdefrule" crlf)
  (printout t (ppdefrule foo) crlf)
  (printout t ">>> rules" crlf)
  (rules)
  (printout t ">>> undefrule" crlf)
  (undefrule foo)
  (rules)
  (printout t ">>> watch" crlf)
  (watch all)
  (reset)
  (assert (bar A B C))
  (run)
  (printout t ">>> unwatch" crlf)
  (unwatch all)
  (reset)
  (assert (bar A B C))
  (run)
  (printout t ">>> Zero-length ordered, and Test1 at nonzero index" crlf)
  (reset)
  (assert (foobar) (foobar 1) (foobar 10))
  (run)
  (printout t ">>> matches" crlf)
  (reset)
  (assert (foobar) (foobar 1) (foobar 10))
  (matches foobar)
  (printout t ">>> clear" crlf)
  (clear)
  (rules)
  )


(printout t "Testing rules:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  