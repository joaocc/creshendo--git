(import nrc.fuzzy.*)

(deffunction test-something ()  
  (printout t "------------------------------------------------------------------" crlf)
  (printout t "***************************  SimpleRule **************************" crlf)
  (printout t "------------------------------------------------------------------" crlf)
  (batch SimpleRule.clp)
  (reset)
  (run)
  (call FuzzyRule setDefaultRuleExecutor (new LarsenProductMaxMinRuleExecutor))
  (reset)
  (run)

  (printout t "------------------------------------------------------------------" crlf)
  (printout t "***************************  TestJess ****************************" crlf)
  (printout t "------------------------------------------------------------------" crlf)

  (clear)
  (batch TestJess.clp)
  (reset)
  (run)
  

  (printout t "------------------------------------------------------------------" crlf)
  (printout t "***************************  TestJess2 ***************************" crlf)
  (printout t "------------------------------------------------------------------" crlf)

  (clear)
  (batch TestJess2.clp)
  (reset)
  (run)

  (printout t "------------------------------------------------------------------" crlf)
  (printout t "***************************  NetTestJess1 ************************" crlf)
  (printout t "------------------------------------------------------------------" crlf)

  (clear)
  (batch NetTestJess1.clp)
  (reset)
  (run)

  (printout t "------------------------------------------------------------------" crlf)
  (printout t "***************************  NetTestJess2 ************************" crlf)
  (printout t "------------------------------------------------------------------" crlf)

  (clear)
  (batch NetTestJess2.clp)
  (reset)
  (run)

  )


(printout t "Testing fuzzy extensions :" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  
