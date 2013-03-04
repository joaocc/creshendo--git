(deftemplate pet (slot name) (slot type))
(deftemplate toy (slot color) (slot type))
(deftemplate person (slot name) (multislot toys) (multislot pets))

(defrule people-with-dogs
        (person (name ?person-name) (pets $? ?d $?))
        ?d <- (pet (type dog) (name ?dog-name))
        =>
        (printout t ?person-name " has a dog named " ?dog-name crlf))

(deffunction test-something ()

  (bind ?r (assert (pet (name "rover") (type dog))))
  (bind ?f (assert (pet (name "fluffy") (type cat))))
  (bind ?g (assert (toy (color red) (type guitar))))
  
  (assert (person (name "Fred") (toys ?g) (pets ?r ?f)))
  (run)
  )


(printout t "Testing rover :" crlf)
(test-something)
(printout t "Test done." crlf)
