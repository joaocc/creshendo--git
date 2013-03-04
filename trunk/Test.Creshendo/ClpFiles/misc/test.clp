﻿(deffunction test-something ()
  (printout t ">>> asc" crlf)
  (printout t (asc "a") " " (asc "aardvark") " " (asc "zebra") crlf)
  (printout t ">>> time" crlf)
  (bind ?t (time))
  (call (call System.Threading.Thread get_CurrentThread) Sleep 1500)
  (printout t (> (time) ?t) crlf)  
  (printout t ">>> gensym*" crlf)
  (printout t (gensym*) " " (gensym*) " " (gensym*) crlf)
  (printout t ">>> setgen" crlf)
  (printout t (setgen 27) crlf)
  (printout t (gensym*) " " (gensym*) " " (gensym*) crlf)
  (printout t ">>> version numbers" crlf)
  (printout t (integerp
               (str-index (str-cat (jess-version-number)) (jess-version-string)))
            crlf)
  (printout t ">>> socket" crlf)
  (socket 127.0.0.1 80 gwp)
  (printout gwp "GET / HTTP/1.0" crlf crlf)
  (printout t (readline gwp) crlf)
  (close gwp)
  (printout t ">>> bag" crlf)
  (bag create my-bag)
  (printout t (bag find my-bag) " ")
  (bag delete my-bag)
  (printout t (bag find my-bag) crlf)
  (bag create my-bag)
  (printout t (bag list) crlf)
  (bind ?b (bag find my-bag))
  (bag set ?b  foo 1.0)
  (bag set ?b  bar "one")
  (bag set ?b  baz i)
  (bag props ?b)
  (printout t (bag get ?b foo) " ")
  (printout t (bag get ?b bar) " ")
  (printout t (bag get ?b baz) crlf)
  (printout t ">>> store, fetch, clear-storage" crlf)
  (store FOO 3)
  (printout t (fetch FOO) crlf)
  (clear-storage)
  (printout t (fetch FOO) crlf)
  (printout t ">>> set-factory" crlf)
  (set-factory (new CSharpJess.jess.Factory.FactoryImpl))
  (printout t ">>> fact-id" crlf)
  (clear)
  (reset)
  (printout t (call (fact-id 0) getName) crlf)
)


(printout t "Testing miscellaneous:" crlf)
(test-something)
(printout t "Test done." crlf)
;(exit)  
