(deftemplate foo (slot bar))
(deftemplate foo-2 (multislot bar))

(deffunction test-something ()
  (printout t "*** Simple" crlf)
  (build "(defrule foo (foo (bar ?X&red | blue)) => (printout t ?X crlf))")
  (assert (foo (bar red)) (foo (bar green)) (foo (bar purple)) (foo (bar blue)))
  (run)
  (reset)

  (printout t "*** Functions" crlf)
  (assert (foo (bar 0)) (foo (bar 1)) (foo (bar 2)) (foo (bar 3)))
  (build "(defrule foo (foo (bar ?X&0 | :(oddp ?X))) => (printout t ?X crlf))")
  (run)
  (reset)

  (printout t "*** Compound" crlf)
  (build "(defrule foo (foo (bar ?X&:(oddp ?X)&:(< ?X 99) | 100)) => (printout t ?X crlf))")
  (assert (foo (bar 0)) (foo (bar 1)) (foo (bar 111)) (foo (bar 2)) (foo (bar 100)))
  (run)
  (reset)

  (printout t "*** Ordered multi single" crlf)
  (build "(defrule foo (bar ?X&red|blue) => (printout t ?X crlf))")
  (assert (bar red) (bar blue) (bar green))
  (run)
  (reset)

  (printout t "*** Ordered multi multi" crlf)
  (build "(defrule foo ?f <- (bar pink|purple puce|orange) => (printout t ?f crlf))")
  (assert (bar pink orange) (bar purple puce) (bar green) (bar red blue))
  (run)
  (reset)

  (printout t "*** Ordered multi skip" crlf)
  (build "(defrule foo ?f <- (bar pink|purple $?X puce|orange) => (printout t ?f crlf))")
  (assert (bar pink orange) (bar purple blue red) (bar pink green) (bar pink red blue puce))
  (run)
  (reset)

  (printout t "*** Unordered multi single" crlf)
  (build "(defrule foo (foo-2 (bar ?X&red|blue)) => (printout t ?X crlf))")
  (assert (foo-2 (bar red)) (foo-2 (bar blue)) (foo-2 (bar green)))
  (run)
  (reset)

  (printout t "*** Unordered multi multi" crlf)
  (build "(defrule foo ?f <- (foo-2 (bar pink|purple puce|orange)) => (printout t ?f crlf))")
  (assert (foo-2 (bar pink orange)) (foo-2 (bar purple puce)) (foo-2 (bar green)) (foo-2 (bar red blue)))
  (run)
  (reset)

  (printout t "*** Unordered multi skip" crlf)
  (build "(defrule foo ?f <- (foo-2 (bar $? pink|purple puce|orange)) => (printout t ?f crlf))")
  (assert (foo-2 (bar pink orange)) (foo-2 (bar purple blue red)) (foo-2 (bar pink puce green)) (foo-2 (bar red blue pink puce)))
  (run)
  (reset)


  )
         

(printout t "Testing the '|' connective constraint:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit) 