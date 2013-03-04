(assert (MAIN::mortgage-payment 2000))

(defmodule WORK)
(deftemplate job (slot salary))

(defmodule HOME)
(deftemplate hobby (slot name) (slot income))

(deffacts WORK::quit-job
  (job (salary 1000))
  (HOME::hobby (income 1000))
  (mortgage-payment 2000))

(printout t (ppdeffacts WORK::quit-job) crlf)
  
