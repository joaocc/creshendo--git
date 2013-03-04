(defmodule DETECTION)
(defmodule ISOLATION)
(defmodule RECOVERY)

(deffacts control-information
  (MAIN::phase-sequence DETECTION ISOLATION RECOVERY))

(defrule MAIN::change-phase
  ?list <- (MAIN::phase-sequence ?next-phase $?other-phases)
  =>
  (focus ?next-phase)
  (retract ?list)
  (assert (MAIN::phase-sequence ?other-phases ?next-phase)))

(defrule DETECTION::change-phase
  (MAIN::phase-sequence $?phases)
  =>
  (printout t "Detection" crlf))

(defrule ISOLATION::change-phase
  (MAIN::phase-sequence $?phases)
  =>
  (printout t "Isolation" crlf))

(defrule RECOVERY::change-phase
  (MAIN::phase-sequence $?phases)
  =>
  (printout t "Recovery" crlf))




(reset)
(watch focus)
(watch rules)
(run 7)
(list-focus-stack)
(clear-focus-stack)
(list-focus-stack)