(printout t "Testing multiple '(not (and' with bindings to earlier vars:" crlf)

(defrule test2
   ;; Find a first point where the y < 10
   ;;(and
     (x ?id1 ?x1)
     (y ?id1 ?y1&:(< ?y1 10))
   ;;)

   ;; Find a second point after the first where the y >= 10
   ;;(and
     (x ?id2 ?x2&:(> ?x2 ?x1))
     (y ?id2 ?y2&:(>= ?y2 10))
   ;;)

   ;; There should be no points between the first and second.
   (not
     (and
       (x ?id3 ?x3&:(and (> ?x3 ?x1) (< ?x3 ?x2)))
       (y ?id3 ?y3)
     )
   )

   ;; The next two points following the second point must be >= 10.
   ;; EJFH Binding for _4_x4 is to CE#6, but should be to #5? See
   ; HasLHS line 182
   (not
     (and
       (x ?id4 ?x4&:(and (> ?x4 ?x2) (< ?x4 (+ ?x2 2))))
       (y ?id4 ?y4&:(< ?y4 10))
     )
   )
=>
   (printout t "Latch at point (" ?x2 "," ?y2 ")" crlf)
)

(reset)

;; point 1 at coordinates (1, 7)
(assert (x 1 1))
(assert (y 1 7))


;; point 2 at coordinates (2, 8)
(assert (x 2 2))
(assert (y 2 8))

;; point 3 at coordinates (3, 9)
(assert (x 3 3))
(assert (y 3 9))

;; point 4 at coordinates (4, 10)
(assert (x 4 4))
(assert (y 4 10))

;; point 5 at coordinates (5, 11)
(assert (x 5 5))
(assert (y 5 11))

;; point 6 at coordinates (6, 12)
(assert (x 6 6))
(assert (y 6 12))

;; point 7 at coordinates (7, 12)
(assert (x 7 7))
(assert (y 7 12))

;; point 8 at coordinates (8, 13)
(assert (x 8 8))
(assert (y 8 13))

;; point 9 at coordinates (9, 12)
(assert (x 9 9))
(assert (y 9 12))

;; point 10 at coordinates (10, 11)
(assert (x 10 10))
(assert (y 10 11))

;; point 11 at coordinates (11, 10)
(assert (x 11 11))
(assert (y 11 10))

;; point 12 at coordinates (12, 9)
(assert (x 12 12))
(assert (y 12 9))

;; point 13 at coordinates (13, 8)
(assert (x 13 13))
(assert (y 13 8))

;; point 14 at coordinates (14, 9)
(assert (x 14 14))
(assert (y 14 9))

;; point 15 at coordinates (15, 10)
(assert (x 15 15))
(assert (y 15 10))

;; point 16 at coordinates (16, 9)
(assert (x 16 16))
(assert (y 16 9))

;; point 17 at coordinates (17, 9)
(assert (x 17 17))
(assert (y 17 9))

(run)

(printout t "Test done." crlf)
