
// test for retraction behavior on logically asserted facts
// chp@info.ucl.ac.be - March 2002

; uncomment following to see retraction
;(watch all)

(deftemplate inc (slot cpt)  (slot in))
(deftemplate in  extends inc)
(deftemplate top (slot cpt))

(deffacts f1
  (in (cpt chp)    (in Staff))
  (in (cpt Staff)  (in Person))
  (in (cpt Person) (in Thing))
)

;(deffacts f1
;  (in (cpt lw)     (in Staff))
;  (in (cpt Staff)  (in Person))
;  (in (cpt Person) (in Thing))
;)

(defrule inc-closure
  (logical (inc (cpt ?cpt) (in ?in1))
           (in  (cpt ?in1) (in ?in2&~?in1)))
  =>
  (assert (inc (cpt ?cpt) (in ?in2)))
)

(printout t "**** INITIAL FACTS ****" crlf)
(facts)

(reset)
(run)
(printout t "**** AFTER RUN ****" crlf)
(facts)

(defrule test
  (logical (inc (in ?cpt))
           (not (inc (cpt ?cpt))))
  =>
  (assert (top (cpt ?cpt)))
)

(printout t "**** AFTER DEFRULE ****" crlf)
(facts)

(reset)
(run)
(printout t "**** AFTER NEW RUN ****" crlf)
(facts)
