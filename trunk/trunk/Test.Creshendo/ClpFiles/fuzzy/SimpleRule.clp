;; SimpleRule.clp
;;
;; A simple example to test a complete FuzzyJess program (no Java code at all).
;;
;;
;; Note: future versions (beyond 5.0a5) of Jess will allow us to use --
;;
;;             (new FuzzyValue ... )
;;       etc.
;;
;;       will no longer always need to fully qualify the classes!
;;
;; The rule can be written in english as:
;;
;;     if   temperature is hot
;;     then pressure    is low or medium
;;
;; Example as shown will give result ... 
;;
;;      NOTE: the program must be run twice ... once with default (MamdaniMinMaxMin)
;;            fuzzy inferencing and then again with rule inferencing set to
;;            LarsenProduct rule inferencing to get each of the last 2 plots
;;
;;            sequence of commands is:
;;            
;;            1. (reset)
;;            2. (run)    .... results for MamdaniMinMaxMin
;;            3. (call nrc.fuzzy.FuzzyRule setDefaultRuleExecutor (new nrc.fuzzy.LarsenProductMaxMinRuleExecutor))
;;            4. (reset)
;;            5. (run)
;;
;;     Fuzzy Value: temperature
;;     Linguistic Value: hot (*),  very medium (+)
;;
;;     1.00               +++++++++++         ****************
;;     0.95
;;     0.90                                  *
;;     0.85
;;     0.80              +           +      *
;;     0.75
;;     0.70                                *
;;     0.65             +             +
;;     0.60                               *
;;     0.55
;;     0.50            +               + *
;;     0.45
;;     0.40                             *
;;     0.35           +                 +
;;     0.30                            *
;;     0.25          +                   +
;;     0.20                           *
;;     0.15         +                     +
;;     0.10        +                 *     +
;;     0.05       +                         +
;;     0.00+++++++*******************        +++++++++++++++++
;;         |----|----|----|----|----|----|----|----|----|----|
;;        0.00     10.00     20.00     30.00     40.00     50.00
;;
;;     The conclusion fuzzy value.
;;
;;     Fuzzy Value: pressure
;;     Linguistic Value: low or medium (*)
;;
;;     1.00************            ***
;;     0.95            *          *   *
;;     0.90             *        *     *
;;     0.85              *
;;     0.80                     *       *
;;     0.75               *
;;     0.70                    *
;;     0.65                *             *
;;     0.60
;;     0.55                 * *           *
;;     0.50
;;     0.45                  *
;;     0.40                   *            *
;;     0.35
;;     0.30
;;     0.25                                 *
;;     0.20
;;     0.15                                  *
;;     0.10                                   *
;;     0.05                                    *
;;     0.00                                     **************
;;         |----|----|----|----|----|----|----|----|----|----|
;;        0.00      2.00      4.00      6.00      8.00     10.00
;;
;;     The output using MamdaniMinMaxMinRuleExecutor
;;
;;     Fuzzy Value: pressure
;;     Linguistic Value: ??? (*)
;;
;;     1.00
;;     0.95
;;     0.90
;;     0.85
;;     0.80
;;     0.75
;;     0.70
;;     0.65
;;     0.60
;;     0.55
;;     0.50
;;     0.45
;;     0.40*********************************
;;     0.35
;;     0.30
;;     0.25                                 *
;;     0.20
;;     0.15                                  *
;;     0.10                                   *
;;     0.05                                    *
;;     0.00                                     **************
;;         |----|----|----|----|----|----|----|----|----|----|
;;        0.00      2.00      4.00      6.00      8.00     10.00
;;
;;     The conclusion using LarsenProductMaxMinRuleExecutor.
;;
;;     Fuzzy Value: pressure
;;     Linguistic Value: ??? (*)
;;
;;     1.00
;;     0.95
;;     0.90
;;     0.85
;;     0.80
;;     0.75
;;     0.70
;;     0.65
;;     0.60
;;     0.55
;;     0.50
;;     0.45
;;     0.40************             *
;;     0.35            ***       *** ***
;;     0.30               *     *       *
;;     0.25                *   *         *
;;     0.20                 * *           *
;;     0.15                  **            *
;;     0.10                                 *
;;     0.05                                  **
;;     0.00                                    ***************
;;         |----|----|----|----|----|----|----|----|----|----|
;;        0.00      2.00      4.00      6.00      8.00     10.00


(defglobal ?*tempFvar* = (new nrc.fuzzy.FuzzyVariable "temperature" 0.0 100.0 "C"))
(defglobal ?*pressFvar* = (new nrc.fuzzy.FuzzyVariable "pressure" 0.0 10.0 "kilo-pascals"))

(defrule init
   (declare (salience 100))
  =>
   (load-package nrc.fuzzy.jess.FuzzyFunctions)
   (bind ?xHot  (create$ 25.0 35.0))
   (bind ?yHot  (create$ 0.0 1.0))
   (bind ?xCold (create$ 5.0 15.0))
   (bind ?yCold (create$ 1.0 0.0))
   ;; terms for the temperature Fuzzy Variable
   (?*tempFvar* addTerm "hot" ?xHot ?yHot 2)
   (?*tempFvar* addTerm "cold" ?xCold ?yCold 2)
   (?*tempFvar* addTerm "veryHot" "very hot")
   (?*tempFvar* addTerm "medium" "not hot and (not cold)")
   ;; terms for the pressure Fuzzy Variable
   (?*pressFvar* addTerm "low" (new nrc.fuzzy.ZFuzzySet 2.0 5.0))
   (?*pressFvar* addTerm "medium" (new nrc.fuzzy.PIFuzzySet 5.0 2.5))
   (?*pressFvar* addTerm "high" (new nrc.fuzzy.SFuzzySet 2.0 5.0))

   ;; add the fuzzy input -- temperature is very medium
   (assert (theTemp (new nrc.fuzzy.FuzzyValue ?*tempFvar* "very medium")))
)   


(defrule temp-hot-press-lowOrMedium
   (theTemp ?t&:(fuzzy-match ?t "hot"))
 =>
   (assert (thePress (new nrc.fuzzy.FuzzyValue ?*pressFvar* "low or medium")))

)


(defrule do-the-printing
   (theTemp ?t)
   (thePress ?p)
 =>
   (bind ?theFzVals 
        (create$ (new nrc.fuzzy.FuzzyValue ?*tempFvar* "hot") ?t)
   ) 
   (printout t (call nrc.fuzzy.FuzzyValue plotFuzzyValues "*+" 0.0 50.0 ?theFzVals) crlf)
   (printout t (call (new nrc.fuzzy.FuzzyValue ?*pressFvar* "low or medium") plotFuzzyValue "*") crlf)
   (printout t (?p plotFuzzyValue "*") crlf)
)

;; (batch "f:/Alexis.Eller/Examples/ShowerHtml/fuzzyJavaDocs/exampleFuzzyJavaCode/SimpleRuleJess/SimpleRule.clp")

