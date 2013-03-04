(deffunction test-something ()
  (printout t ">>> and, not and or" crlf)
  (printout t (and 1 1 2)  crlf)
  (printout t (not (and 1 1 FALSE))  crlf)
  (printout t (or 1 1 FALSE)  crlf)
  (printout t (and (or 1 FALSE) (or 2 FALSE)) crlf)
  (printout t (not (or (and 1 FALSE) (and 2 FALSE))) crlf)
  (printout t (not (not TRUE)) crlf)
  (printout t ">>> bit-and, bit-not and bit-or" crlf)
  (printout t (bit-and 1 2) crlf)
  (printout t (bit-or 1 2) crlf)
  (printout t (bit-not 255) crlf)
  )


(printout t "Testing logic:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  