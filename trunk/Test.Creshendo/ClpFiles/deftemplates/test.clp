(deftemplate foo "This is a comment" (slot bar))
(deftemplate bar extends foo "This is a comment" (slot foo))
(deffunction test-something ()
  (printout t (ppdeftemplate foo) crlf)
  (printout t (ppdeftemplate bar) crlf)
  )


(printout t "Testing deftemplates :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  