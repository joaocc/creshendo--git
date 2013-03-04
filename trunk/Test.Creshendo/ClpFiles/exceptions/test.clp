;; ======================================================================
;; @(#) From try.clp 2000/04/16 1.0
;; @author Thomas Barnekow
;; ======================================================================

;;
;; Test try-catch-finally
;;
(deffunction try-catch-finally (?x)
  
  (try

   (printout t "Entering try block, x = " ?x crlf)
   (if (> ?x 0) then (throw (new System.Exception "in try block")))
   (printout t "Exiting try block, x = " ?x crlf)

   catch

   (printout t "Entering catch block, x = " ?x crlf)
   (if (> ?x 9) then (throw (new System.Exception "in catch block")))
   (printout t "Exiting catch block, x = " ?x crlf)
  
   finally

   (printout t "Entering finally block, x = " ?x crlf)
   (if (> ?x 99) then (throw (new System.Exception "in finally block")))
   (printout t "Exiting finally block, x = " ?x crlf))

  (return ?x))

;;
;; Test try-catch
;;
(deffunction try-catch (?x)

  (try

   (printout t "Entering try block, x = " ?x crlf)
   (if (> ?x 0) then (throw (new System.Exception "in try block")))
   (printout t "Exiting try block, x = " ?x crlf)

   catch

   (printout t "Entering catch block, x = " ?x crlf)
   (if (> ?x 9) then (throw (new System.Exception "in catch block")))
   (printout t "Exiting catch block, x = " ?x crlf))

  (return ?x))

;;;
;;; Test try-finally
;;;
(deffunction try-finally (?x)
  (try

   (printout t "Entering try block, x = " ?x crlf)
   (if (> ?x 0) then (throw (new System.Exception "in try block")))
   (printout t "Exiting try block, x = " ?x crlf)

   finally

   (printout t "Entering finally block, x = " ?x crlf)
   (if (> ?x 99) then (throw (new System.Exception "in finally block")))
   (printout t "Exiting finally block, x = " ?x crlf))

  (return ?x))

;;
;; Test return from try
;;
(deffunction return-from-try ()
  (try

   (printout t "Entering try block" crlf)
   (return)
   (printout t "Exiting try block" crlf)

   finally

   (printout t "Entering finally block" crlf)
   (printout t "Exiting finally block" crlf)))

;;
;; Test return from catch
;;
(deffunction return-from-catch ()
  (try

   (printout t "Entering try block" crlf)
   (throw (new System.Exception "in try block"))
   (printout t "Exiting try block" crlf)

   catch 

   (printout t "Entering catch block" crlf)
   (return)
   (printout t "Exiting catch block" crlf)

   finally

   (printout t "Entering finally block" crlf)
   (printout t "Exiting finally block" crlf)))


(deffunction call-try-catch-finally (?x)
  (try
   (printout t "y = " (try-catch-finally ?x) crlf)
   catch
   (printout t "Exception = " (call (call ?ERROR getNextException) ToString) crlf)))


(deffunction call-try-catch (?x)
  (try
   (printout t "y = " (try-catch ?x) crlf)
   catch
   (printout t "Exception = " (call (call ?ERROR getNextException) ToString) crlf)))


(deffunction call-try-finally (?x)
  (try
   (printout t "y = " (try-finally ?x) crlf)
   catch
   (printout t "Exception = " (call (call ?ERROR getNextException) ToString) crlf)))


;;
;; Run tests
;;
(deffunction run-test ()
  (printout t crlf "TRY-CATCH... no exception" crlf)
  (call-try-catch 0)
  (printout t crlf "TRY-CATCH... exception in try block (caught)" crlf)
  (call-try-catch 1)
  (printout t crlf "TRY-CATCH... exception in catch block (uncaught)" crlf)
  (call-try-catch 10)

  (printout t crlf "TRY-FINALLY... no exception" crlf)
  (call-try-finally 0)
  (printout t crlf "TRY-FINALLY... exception in try block (uncaught)" crlf)
  (call-try-finally 1)
  (printout t crlf "TRY-FINALLY... exception in finally block (uncaught)" crlf)
  (call-try-finally 100)

  (printout t crlf "TRY-CATCH-FINALLY... no exception" crlf)
  (call-try-catch-finally 0)
  (printout t crlf "TRY-CATCH-FINALLY... exception in try block (caught)" crlf)
  (call-try-catch-finally 1)
  (printout t crlf "TRY-CATCH-FINALLY... exception in catch block (uncaught)" crlf)
  (call-try-catch-finally 10)
  (printout t crlf "TRY-CATCH-FINALLY... exception in finally block (uncaught)" crlf)
  (call-try-catch-finally 100)

  (printout t crlf "RETURN-FROM-TRY" crlf)
  (return-from-try)

  (printout t crlf "RETURN-FROM-CATCH" crlf)
  (return-from-catch)
  )


(deffunction test-something ()
  (printout t ">>> Exceptions during reflection:" crlf)
  (try
   (new foo.bar.NoSuchClass)
   (printout t "Created a foo.bar.NoSuchClass!" crlf)
   catch
   (printout t "Failed to create a foo.bar.NoSuchClass, as expected" crlf))

  (printout t crlf ">>> Exceptions during jess function calls:" crlf)
  (try
   (printout t "(+ 3 foo) is " (+ 3 foo) crlf)
   catch
   (printout t "(+ 3 foo) generated an error:" crlf (call ?ERROR ToString) crlf))

  (printout t crlf ">>> Using exceptions to test for command presence" crlf)
  (try
   (foobar 1 2 3)
   (printout t "foobar command present!" crlf)
   catch
   (printout t "foobar command not present!" crlf))

  (printout t crlf ">>> Throwing my own exceptions" crlf)
  (try
   (throw (new System.Exception "Whew!"))
   (printout t "OOPS!" crlf)
   catch
   (printout t (call (call ?ERROR getNextException) get_Message) crlf))

  (printout t crlf ">>> Return inside try; empty catch" crlf)
  (try
   (return 42)
   catch)
)


(printout t "Testing Exception:" crlf)
(printout t (test-something) crlf)
(run-test)
(printout t "Test done." crlf)
;(exit)  