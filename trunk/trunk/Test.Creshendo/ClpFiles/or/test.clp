(printout t "Testing the 'or' CE :" crlf)

;; ----------------------------------------------------------------------
(printout t "*** Testing separate paths:" crlf)
;; ----------------------------------------------------------------------
(defrule foo ?x <- (or (a) (b)) => (printout t (?x getName) crlf))
(assert (a))
(run)
(reset)
(assert (b))
(run)
(reset)

;; ----------------------------------------------------------------------
(printout t "*** Testing multibranch erase:" crlf)
;; ----------------------------------------------------------------------
(defrule foo (c) => (printout t c crlf))
(assert (a) (b) (c) )
(run)

(undefrule foo)
(reset)

;; ----------------------------------------------------------------------
(printout t "*** Testing nesting:" crlf)
;; ----------------------------------------------------------------------
(defrule foo (or (a)
                 (and (b)
                      (or (c)
                          (d))))

  => (printout t "Fire." crlf))

(deffunction run-test (?s ?end) (printout t ?s ?end) (run) (reset))
(assert (a))
(run-test "Example 1: Should... " "")
(assert (b))
(run-test "Example 2: Should not." crlf)
(assert (b) (c))
(run-test "Example 3: Should... " "")
(assert (b) (d))
(run-test "Example 4: Should... " "")
(assert (c) (d))
(run-test "Example 5: Should not." crlf)
(undefrule foo)

(defrule foo
  (not (or (e) (f)))
  =>
  (printout t "Fire." crlf))

(reset)
(assert (e))
(run-test "Example 1: Should not." crlf)
(assert (f))
(run-test "Example 2: Should not." crlf)
(assert (g))
(run-test "Example 3: Should... " "")
(assert (e) (f))
(run-test "Example 4: Should not." crlf)
(undefrule foo)

(defrule foo (not (and (A) (B))) => 
  (printout t "Fire." crlf))

(reset)
(assert (A))
(run-test "Example 1: Should..." "")
(assert (B))
(run-test "Example 2: Should..." "")
(assert (A) (B))
(run-test "Example 3: Should not." crlf)
(assert (C))
(run-test "Example 4: Should..." "")
(undefrule foo)

(undefrule foo)

;; ----------------------------------------------------------------------
(printout t "*** Testing bad binding" crlf)
;; ----------------------------------------------------------------------

(try
 (build "(defrule foo ?x <- (or (a) (and (b) (c))) => )")
 (printout t "Ooops, that shouldn't have compiled!" crlf)
 catch
 (printout t "Good, got compiler error" crlf))
 

;; ----------------------------------------------------------------------
(printout t "*** Tests from Mariusz Nowostawski" crlf)
;; ----------------------------------------------------------------------

(assert (a 1))
(assert (a 2))
(assert (b 1))
(assert (b 2))
(assert (c 1))
(assert (c 2))
(assert (d 1))
(assert (d 2))
(assert (e 1))
(assert (e 2))
(deffunction run-test (?rule ?times)
  (printout t ?rule " Should print 'Good' " ?times " time(s)..")
  (run) (undefrule ?rule))

(defrule f1
 (or (a 1) (e 5)) => (printout t "Good" crlf))
(run-test f1 1)

(defrule f2
 (or (a 5) (e 1)) => (printout t "Good" crlf))
(run-test f2 1)

(defrule f3
 (and (a 1) (a 2)) => (printout t "Good" crlf))
(run-test f3 1)

(defrule f4
 (and (a 1) (a 2) (e 1) (e 2)) => (printout t "Good" crlf))
(run-test f4 1)

(defrule f5
 (or (and (a 1) (a 2) (e 1) (e 2)) (not (e 5)) (and (a 4) (a 1)))
  => (printout t "Good" crlf))
(run-test f5 2)

(defrule f6
 (and (and (a 1) (a 2) (e 1) (d 2)) (not (c 5)) (and (a 2) (b 1)))
  => (printout t "Good" crlf))
(run-test f6 1)

(defrule f7
 (and (or (a 5) (b 2) (e 1) (d 2)) (not (c 5)) (and (a 2) (b 1)))
  => (printout t "Good" crlf))
(run-test f7 3)

(defrule f8
 (and (or (a 5) (b 2) (e 1) (d 2)) (and (a 2) (b 1)) (and (a 1) (d 1)))
  => (printout t "Good" crlf))
(run-test f8 3)

(defrule f9
 (and (or (a 5) (b 2) (e 1) (d 2)) (or (a 2) (b 1)) (and (a 1) (d 1)))
  => (printout t "Good" crlf))
(run-test f9 6)

(defrule f10
 (and (or (a 5) (b 2) (e 1) (d 2)) (and (a 2) (b 1)) (or (a 5) (d 1)))
  => (printout t "Good" crlf))
(run-test f10 3)

(defrule f11
 (and 
   (or (a 5) (b 2) (e 1) (d 2)) 
   (and (a 2) (b 1)) 
   (or (a 5) (d 1))
   (not (c 5))
   (or 
     (and (a 1) (b 1) (c 1))
     (not (a 1))
     (or (a 5) (e 5))
   )
 )=> (printout t "Good" crlf))
(run-test f11 3)



(deffunction run-test (?rule) (printout t ?rule " Should not print anything" crlf) (run) (undefrule ?rule))

(defrule b1
 (or (a 5) (e 5)) => (printout t "Bad" crlf))
(run-test b1)

(defrule b2
 (or (a 5) (b 5) (c 5) (d 5) (e 5)) => (printout t "Bad" crlf))
(run-test b2)

(defrule b3
 (and (b 1) (a 5)) => (printout t "Bad" crlf))
(run-test b3)

(defrule b4
 (and (a 1) (d 5) (e 1) (e 2)) => (printout t "Bad" crlf))
(run-test b4)

(defrule b5
 (or (and (a 1) (a 2) (e 1) (e 5)) (and (a 4) (a 1)))
  => (printout t "Bad" crlf))
(run-test b5)

(defrule b6
 (and (and (a 1) (a 2) (e 1) (d 2)) (not (c 1)) (and (a 2) (b 1)))
  => (printout t "Bad" crlf))
(run-test b6)

(defrule b7
 (and (or (a 5) (b 2) (e 1) (d 2)) (not (c 5)) (and (a 2) (b 5)))
  => (printout t "Bad" crlf))
(run-test b7)

(defrule b8
 (and (or (a 5) (b 2) (e 1) (d 2)) (and (a 2) (b 5)) (and (a 1) (d 1)))
  => (printout t "Bad" crlf))
(run-test b8)

(defrule b9
 (and (or (a 5) (b 5) (e 5) (d 5)) (and (a 2) (b 1)) (and (a 1) (d 1)))
  => (printout t "Bad" crlf))
(run-test b9)

(defrule b10
 (and (or (a 5) (b 2) (e 1) (d 2)) (or (a 5) (b 5)) (and (a 1) (d 1)))
  => (printout t "Bad" crlf))
(run-test b10)

(defrule b11
 (and 
   (or (a 5) (b 2) (e 1) (d 2)) 
   (or (a 1) (b 2)) 
   (and 
     (or (a 1) (d 1))
     (or (a 2) (e 5))
     (or (b 5) (c 5))
   )
 ) => (printout t "Bad" crlf))
(run-test b11)

(printout t "Test done." crlf)
(exit)