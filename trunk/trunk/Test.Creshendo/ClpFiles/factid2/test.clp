;;; -*- Jess -*-
;;;===++===================================================
;;;            Topological Path Planning
;;;               by Matthew Johnson
;;;
;;;=++=====================================================

;;;*************
;;;* TEMPLATES *
;;;*************

(deftemplate room
  (slot name)
  (multislot exits))

(deftemplate exit
  (slot name)
  (multislot rooms)
  (slot x)
  (slot y)
  (slot heading)
  (slot open (default YES))
  (slot found (default NO)))


(deftemplate robot 
   (slot location)
   (slot x)
   (slot y))

(deftemplate goal 

   (slot location))

(deftemplate path
  (slot cost)
  (multislot nodes)
  (slot lastx)
  (slot lasty))

(deftemplate path-list
  (multislot paths))

(deftemplate path-to-goal
  (multislot path))

(deftemplate gneed-to-add
  (slot paths-not-on-list))

;;;***********
;;;*  RULES  *
;;;***********

(defrule start-path
  ?goal <- (goal (location ?there))
  (robot (location ?here)(x ?x)(y ?y))
  (test (neq ?here ?there))  
  =>   
  (assert (path-list))
  (assert (gneed-to-add (paths-not-on-list 1)))
  (assert (path (nodes (create$ ?here))(lastx ?x)(lasty ?y)(cost 0))))  


(defrule add-path-to-list  
  ?path <- (path (cost ?cost))
  ?pathlist <- (path-list(paths $?list))
  (test(not(member$ ?path $?list)))
  ?need <- (gneed-to-add(paths-not-on-list ?x))
  =>  
  (modify ?pathlist (paths (order-list 0 (create$ $?list ?path))))
  (modify ?need (paths-not-on-list (- ?x 1))))
  

(defrule expand-best-node
  ?path <- (path (nodes $?first ?last)(lastx ?x)(lasty ?y)(cost ?cost))
  (room (name ?last)(exits $?exits))
  ?pathlist <- (path-list (paths ?path $?list))
  ?need <- (gneed-to-add (paths-not-on-list 0))
  =>  
  (modify ?need (paths-not-on-list 
       (length$ (complement$ (create$ ?path $?list) $?exits))))
  (foreach ?exit $?exits
    (if (not(member$ ?exit $?first)) then
      (assert 
        (path (nodes $?first ?last ?exit)(lastx ?x)(lasty ?y)(cost ?cost)))))
  (retract ?path)
  (modify ?pathlist(paths $?list)))


;;;***************
;;;*  Functions  *
;;;*************** 


(deffunction order-list (?min $?exits)
  (bind ?min (nth$ 1 $?exits))
  (foreach ?item (rest$ $?exits)
    (if (< (fact-slot-value ?item cost) (fact-slot-value ?min cost)) 
      then (bind ?min ?item)))
  (if (> (length$ $?exits) 2) 
    then
     (return (create$ ?min
       (order-list 0 (complement$ (create$ ?min) (create$ $?exits)))))
    else
     (return (create$ ?min (complement$ (create$ ?min) (create$ $?exits))))))

;;;**********************
;;;* INITIAL STATE RULE * 
;;;**********************

(defrule startup ""
  =>
  ;Rooms
  (assert (room (name Room-1)(exits Door-7 Door-1)))
  (assert (room (name Room-2)(exits Door-7 Door-2)))
  (assert (room (name Room-3)(exits Door-3 Door-6)))
  (assert (room (name Room-4)(exits Door-6 Door-4)))
  (assert (room (name Room-5)(exits Door-5)))
  (assert (room (name Hall-1)(exits Door-1 Door-3 Open-1)))
  (assert (room (name Hall-2)(exits Open-1 Door-4 Open-2 Door-2)))
  (assert (room (name Hall-3)(exits Open-2 Door-5)))
  ;Exits
  (assert (exit (name Door-1)(x 2)(y 7)(rooms Hall-1 Room-1)))  
  (assert (exit (name Door-2)(x 6)(y 7)(rooms Room-2 Hall-2)))  
  (assert (exit (name Door-3)(x 2)(y 4)(rooms Hall-1 Room-3)))  
  (assert (exit (name Door-4)(x 6)(y 4)(rooms Hall-2 Room-4)))
  (assert (exit (name Door-5)(x 10)(y 4)(rooms Hall-3 Room-5)))
  (assert (exit (name Door-6)(x 4)(y 1)(rooms Room-4 Room-3)))
  (assert (exit (name Door-7)(x 4)(y 9)(rooms Room-1 Room-2)))
  (assert (exit (name Open-1)(x 6)(y 5)(rooms Hall-1 Hall-2)))
  (assert (exit (name Open-2)(x 10)(y 5)(rooms Hall-2 Hall-3)))
  ;Positions
  (assert (robot (location Room-5)(x 10)(y 3)))
  (assert (goal (location Room-3))))

(reset)
(watch rules)
(run)


