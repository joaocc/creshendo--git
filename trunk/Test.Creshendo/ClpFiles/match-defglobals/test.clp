(watch activations)
(watch facts)
(defglobal ?*d* = 0)
(defrule foo-1
  (bar ?*d*)
  =>
  )

(defrule foo-2
  (foo)
  (bar ?*d*)
  =>
  )

(assert (bar 1))
(assert (bar 2))
(assert (bar 0))
(assert (foo))