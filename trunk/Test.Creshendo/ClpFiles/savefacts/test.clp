(deffunction test-something ()
  (assert (foo bar baz)
          (foo 1 2 3)
          (foo 1.0 2.0 3.0)
          (foo "A String")
          (foo "A String with \"embedded quotes\""))
  (save-facts facts.clp)
  (clear)
  (load-facts facts.clp)
  (facts)
  (clear)
  (assert (foo bar baz)
          (foo 1 2 3)
          (bar 1.0 2.0 3.0))
  (save-facts facts.clp foo)
  (clear)
  (load-facts facts.clp)
  (facts)
  
  )


(printout t "Testing fact-saving:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  