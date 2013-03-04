(deffunction try-catch-finally(?t ?c ?f)
  (try
   (printout t t)
   (if (eq ?t r) then
     (return)
     else (if (eq ?t e) then
            (throw new RuntimeException "from try")
            else
            (printout t r)))
   (printout t y)

   catch
   (printout t ca)
   (if (eq ?c r) then
     (return)
     else (if (eq ?c e) then
            (throw new RuntimeException "from catch")
            else
            (printout t t)))
   (printout t ch)

   finally
   (printout t fin)
   
   (if (eq ?f r) then
     (return)
     else (if (eq ?f e) then
            (throw new RuntimeException "from finally")
            else
            (printout t a)))
   (printout t lly))

  (printout t tcf-end))

(deffunction try-catch(?t ?c)
  (try
   (printout t t)
   (if (eq ?t r) then
     (return)
     else (if (eq ?t e) then
            (throw new RuntimeException "from try")
            else
            (printout t r)))
   (printout t y)

   catch
   (printout t ca)
   (if (eq ?c r) then
     (return)
     else (if (eq ?c e) then
            (throw new RuntimeException "from catch")
            else
            (printout t t)))
   (printout t ch))

  (printout t tc-end))

(deffunction eol()
  (printout t crlf))

(printout t "Testing try :" crlf)


(try-catch-finally e c c) (eol)
(try-catch-finally e r c) (eol)
(try-catch-finally c x r) (eol)
(try-catch-finally r x r) (eol)
(try-catch-finally e c r) (eol)
(try-catch-finally c x c) (eol)
(try-catch-finally r x c) (eol)

(try-catch c x) (eol)
(try-catch r x) (eol)
(try-catch e c) (eol)
(try-catch e r) (eol)

(printout t "Test done." crlf)
(exit)  



