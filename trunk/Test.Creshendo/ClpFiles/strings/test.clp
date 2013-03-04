(deffunction test-something ()
  ;;----------------------------------------------------------------------
  (printout t ">>> lowcase" crlf)
  (printout t (lowcase "hello, world") crlf)
  (printout t (lowcase "Hello, World") crlf)
  (printout t (lowcase "HELLO, WORLD") crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> upcase" crlf)
  (printout t (upcase "hello, world") crlf)
  (printout t (upcase "Hello, World") crlf)
  (printout t (upcase "HELLO, WORLD") crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> str-cat" crlf)
  (printout t (str-cat abc 1.0 "def") " ")
  (printout t (stringp (str-cat abc 1.0 "def")) crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> sym-cat" crlf)
  (printout t (sym-cat abc 1.0 "def") " ")
  (printout t (symbolp (sym-cat abc 1.0 "def")) crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> str-compare" crlf)
  (printout t (str-compare "abc" "abc") " ")
  (printout t (str-compare "abc" "def") " ")
  (printout t (str-compare "def" "abc") crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> str-index" crlf)
  (printout t (str-index "abc" "abc") " ")
  (printout t (str-index "abc" "def") " ")
  (printout t (str-index "abc" "defabc") crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> str-length" crlf)
  (printout t (str-length "1234567890") " ")
  (printout t (str-length "") " ")
  (printout t (str-length hello) crlf)
  ;;----------------------------------------------------------------------
  (printout t ">>> sub-string" crlf)
  (printout t (sub-string 1 1 "1234567890") " ")
  (printout t (sub-string 2 1 "1234567890") " ")
  (printout t (sub-string 10 1 "1234567890") " ")
  (printout t (sub-string 1 10 "1234567890") " ")
  (printout t (sub-string 2 8 "1234567890") crlf)
  )


(printout t "Testing strings:" crlf)
(test-something)
(printout t "Test done." crlf)
(exit)  