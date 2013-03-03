(defrule rule0
  (Account
    (AccountType standard)
    (Status active)
    (AccountId "acc0")
  )
=>
  (printout t "rule0 was fired" crlf)
)
(defrule rule1
  (Account
    (AccountType "standard")
    (Status "active")
    (AccountId "acc1")
  )
=>
  (printout t "rule1 was fired" crlf)
)
(defrule rule2
  (Account
    (AccountType "standard")
    (Status "active")
    (AccountId "acc2")
  )
=>
  (printout t "rule2 was fired" crlf)
)
(defrule rule3
  (Account
    (AccountType "standard")
    (Status "active")
    (AccountId "acc3")
  )
=>
  (printout t "rule3 was fired" crlf)
)
(defrule rule4
  (Account
    (AccountType "standard")
    (Status "active")
    (AccountId "acc4")
  )
=>
  (printout t "rule4 was fired" crlf)
)
