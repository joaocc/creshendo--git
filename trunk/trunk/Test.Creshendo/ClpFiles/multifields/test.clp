(defrule test-omitted-dollar-signs
  (foo $?stuff)
  (bar ?stuff)
  =>
  (printout t ?stuff crlf))

(defrule test-retract-multi-match
  ?f <- (foo A $? ?x)
  =>
  (facts)
  (retract ?f)
  (facts))

(deffunction test-something ()
  ;; ----------------------------------------------------------------------
  (printout t ">>> create$" crlf)
  (printout t (create$ a b c d e) crlf)
  (printout t (create$ a b (create$ c) d e) crlf)
  (printout t (create$ a b (create$ (create$ c)) d e) crlf)
  (printout t (create$ (create$ a b) (create$ (create$ c)) d e) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> multifieldp" crlf)
  (printout t (multifieldp (create$ a b c d e)) " ")
  (printout t (multifieldp a) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> explode$" crlf)
  (printout t (explode$ "a b c d e") crlf)  
  (printout t (explode$ "a b \"c\" d e") crlf)  
  (printout t (explode$ "a b \"c d\" e") crlf)  
  (printout t (explode$ "1. 2 a \"b\"") crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> implode$" crlf)
  (bind ?x (implode$ (create$ 1. 2 a "b")))  
  (printout t ?x " " (stringp ?x) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> first$" crlf)
  (printout t (first$ (create$ a b c)) crlf)  
  (printout t (first$ (create$ a)) crlf)  
  (printout t (first$ (create$)) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> rest$" crlf)
  (printout t (rest$ (create$ a b c)) crlf)  
  (printout t (rest$ (create$ a)) crlf)  
  (printout t (rest$ (create$)) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> nth$" crlf)
  (printout t (nth$ 1 (create$ a b c)) " ")  
  (printout t (nth$ 2 (create$ a b c)) " ")  
  (printout t (nth$ 3 (create$ a b c)) " ")  
  (printout t (nth$ 1 (create$ a)) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> length$" crlf)
  (bind $?x (create$ a b c))
  (printout t (length$ $?x) " ")
  (printout t (nth$ (length$ $?x) $?x) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> union$" crlf)  
  (printout t (union$ (create$ a b c) (create$ d e f)) " ")  
  (printout t (union$ (create$ a b c) (create$ c d e)) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> intersection$" crlf)  
  (printout t (intersection$ (create$ a b c) (create$ d e f)) " ")  
  (printout t (intersection$ (create$ a b c) (create$ b d c)) " ")  
  (printout t (intersection$ (create$ a b c) (create$ c d e)) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> complement$" crlf)  
  (printout t (complement$ (create$ a b c) (create$ d e f)) " ")  
  (printout t (complement$ (create$ a b c) (create$ c d e)) " ")  
  (printout t (complement$ (create$ a b c) (create$ b c d)) " ")  
  (printout t (complement$ (create$ a b c) (create$ a b c)) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> subseq$" crlf)  
  (printout t (subseq$ (create$ a b c) 1 1) " ")  
  (printout t (subseq$ (create$ a b c) 1 2) " ")  
  (printout t (subseq$ (create$ a b c) 1 3) " ")  
  (printout t (subseq$ (create$ a b c) 2 3) " ")  
  (printout t (subseq$ (create$ a b c) 3 3) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> subsetp" crlf)  
  (printout t (subsetp (create$ a b c) (create$ d e f)) " ")  
  (printout t (subsetp (create$ a b c) (create$ b c d)) " ")  
  (printout t (subsetp (create$ a b c) (create$ a b c)) " ")  
  (printout t (subsetp (create$ a) (create$ a b c)) " ")  
  (printout t (subsetp (create$ z) (create$ a b c)) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> member$" crlf)  
  (printout t (member$ a (create$ a b c)) " ")
  (printout t (member$ a (create$ d e f)) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> list-function$" crlf)  
  (printout t (multifieldp (list-function$)) " ")
  (printout t (neq (member$ list-function$ (list-function$) FALSE)) crlf)
  ;; ----------------------------------------------------------------------
  (printout t ">>> delete$" crlf)
  (printout t (delete$ (create$ a b c d e f) 1 6) " ")  
  (printout t (delete$ (create$ a b c d e f) 2 5) " ")  
  (printout t (delete$ (create$ a b c d e f) 1 1) " ")  
  (printout t (delete$ (create$ a b c d e f) 6 6) " ")  
  (printout t (delete$ (create$ a) 1 1) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> insert$" crlf)
  (printout t (insert$ (create$ b c d e f) 1 (create$ a)) " ")  
  (printout t (insert$ (create$ a c d e f) 2 (create$ b)) " ")  
  (printout t (insert$ (create$ a b c d e) 6 (create$ f)) " ")  
  (printout t (insert$ (create$ a) 1 (create$ b)) " ")  
  (printout t (insert$ (create$ a) 2 (create$ b)) crlf)  
  ;; ----------------------------------------------------------------------
  (printout t ">>> replace$" crlf)
  (printout t  (replace$ (create$ a b c) 2 2 (create$ x y z)) " ")  
  (printout t  (replace$ (create$ a b c) 1 3 (create$ x y z)) " ")  
  (printout t  (replace$ (create$ a b c) 1 1 (create$ x y z)) " ")  
  (printout t  (replace$ (create$ a b c) 3 3 (create$ x y z)) crlf)  
  ;; ---------------------------------------------------------------------- 
  (printout t ">>> Multifields in rules" crlf)
  (assert (foo A B C) (bar A B C))
  (run)
  )


(printout t "Testing multifields:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  

























