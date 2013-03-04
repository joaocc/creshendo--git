;; NetTestJess.clp
;;
;; A simple example to test a complete FuzzyJess program (no Java code at all).
;; Mainly to see the complexity of a pattern/join net for debugging FuzzyJess
;;
;;
;; Note: future versions (beyond 5.0a5) of Jess will allow us to use --
;;
;;             (new FuzzyValue ... )
;;       etc.
;;
;;       will no longer always need to fully qualify the classes!
;;
;; Example as shown will give result ...
;;
;; Jack is tall with degree (similarity) 0.5363321799307958
;; Jack is tall with degree (match) 0.588235294117647
;; Timothy is short with degree (similarity) 0.5363321799307958
;; Timothy is short with degree (match) 0.588235294117647
;; *************************************************************
;; BOB: FuzzyVariable         -> height [ 0.0, 10.0 ] feet
;; Linguistic Expression -> ???
;; FuzzySet              -> { 0/4 0.59/4.47 0.59/5.71 0/6 }
;; DAN: FuzzyVariable         -> height [ 0.0, 10.0 ] feet
;; Linguistic Expression -> ???
;; FuzzySet              -> { 0/4 0.59/4.8 0.59/5.5 0/6 }
;; Randy is tall with degree (similarity) 0.6085526315789473
;; Randy is tall with degree (match) 0.6085526315789473
;; Timothy is short with degree (similarity) 0.6085526315789473
;; Timothy is short with degree (match) 0.6085526315789473
;; *************************************************************
;; BOB: FuzzyVariable         -> height [ 0.0, 10.0 ] feet
;; Linguistic Expression -> ???
;; FuzzySet              -> { 0/4 0.61/4.49 0.61/5.7 0/6 }
;; DAN: FuzzyVariable         -> height [ 0.0, 10.0 ] feet
;; Linguistic Expression -> ???
;; FuzzySet              -> { 0/4 0.61/4.8 0.61/5.5 0/6 }
;; Ralph is tall with degree (similarity) 0.4117647058823532
;; Ralph is tall with degree (match) 0.49999999999999994
;; Timothy is short with degree (similarity) 0.4117647058823532
;; Timothy is short with degree (match) 0.49999999999999994
;; *************************************************************
;; BOB: FuzzyVariable         -> height [ 0.0, 10.0 ] feet
;; Linguistic Expression -> ???
;; FuzzySet              -> { 0/4 0.5/4.4 0.61/4.49 0.61/5.7 0/6 }
;; DAN: FuzzyVariable         -> height [ 0.0, 10.0 ] feet
;; Linguistic Expression -> ???
;; FuzzySet              -> { 0/4 0.61/4.8 0.61/5.5 0/6 }

(defglobal ?*heightFvar* = (new nrc.fuzzy.FuzzyVariable "height" 0.0 10.0 "feet"))

(defglobal ?*rlf* = (new nrc.fuzzy.RightLinearFunction))
(defglobal ?*llf* = (new nrc.fuzzy.LeftLinearFunction))

(deftemplate person
   (slot name)
   (slot height)
)

(defrule init
   (declare (salience 100))
  =>
   (load-package nrc.fuzzy.jess.FuzzyFunctions)
   (?*heightFvar* addTerm "short" (new nrc.fuzzy.RFuzzySet 0.0 5.0 ?*rlf*))
   (?*heightFvar* addTerm "medium" (new nrc.fuzzy.TrapezoidFuzzySet 4.0 4.8 5.5 6.0))
   (?*heightFvar* addTerm "tall" (new nrc.fuzzy.LFuzzySet 5.5 6.0 ?*llf*))

   (assert (person (name "Ralph")
                   (height (new nrc.fuzzy.FuzzyValue ?*heightFvar*
                                (new nrc.fuzzy.PIFuzzySet 5.7 0.1)))
           )
           (person (name "Timothy")
                   (height (new nrc.fuzzy.FuzzyValue ?*heightFvar*
                                (new nrc.fuzzy.PIFuzzySet 2.0 0.1)))
           )
           (person (name "Randy")
                   (height (new nrc.fuzzy.FuzzyValue ?*heightFvar*
                                (new nrc.fuzzy.PIFuzzySet 6.5 0.1)))
           )
           (person (name "Jack")
                   (height (new nrc.fuzzy.FuzzyValue ?*heightFvar*
                                (new nrc.fuzzy.PIFuzzySet 5.75 0.1)))
           )
           (do-pairs)
           (extra-fact 111)
   )
)

(defrule identify-tall-and-short-people-pairs
   (do-pairs)
   (person (name ?n1) (height ?ht1&:(fuzzy-match ?ht1 "tall")))
   (person (name ?n2&~?n1) (height ?ht2&:(fuzzy-match ?ht2 "short")))
   (extra-fact 111)
 =>
   (printout t ?n1 " is tall with degree (similarity) " (fuzzy-rule-similarity) crlf)
   (printout t ?n1 " is tall with degree (match) " (fuzzy-rule-match-score) crlf)
   (printout t ?n2 " is short with degree (similarity) " (fuzzy-rule-similarity) crlf)
   (printout t ?n2 " is short with degree (match) " (fuzzy-rule-match-score) crlf)
   (printout t "*************************************************************" crlf)
   (call nrc.fuzzy.FuzzyRule setDefaultRuleExecutor (new nrc.fuzzy.MamdaniMinMaxMinRuleExecutor))
   (assert (bob (new nrc.fuzzy.FuzzyValue ?*heightFvar* "medium")))
   ;; change inference method to larsen product  ... 
   (call nrc.fuzzy.FuzzyRule setDefaultRuleExecutor (new nrc.fuzzy.LarsenProductMaxMinRuleExecutor))
   (assert (dan (new nrc.fuzzy.FuzzyValue ?*heightFvar* "medium")))
   
)

(defrule pp
  (declare (salience 100))
  (bob ?x)
  (dan ?y)
=>
  (printout t "BOB: " (?x toString) crlf "DAN: " (?y toString) crlf)
)

;;(batch  "e:\\alexis\\fuzzytest.jessonly\\nettestjess.clp")
