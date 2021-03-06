G91					; Make coordinates relative
G92 E0 				; reset Extruder counter
G1 E-2 F200		;Retract extuder 2mm at 900mm/min
G1 Z2 F400			;Move up tw mm (from current position because it is all relative now) 
G1 Z5 F1000 		; Move Z another 2mm up
G90 				; Use absolute coordinates again
G1 X5 Y100 F3000.0	;go to almost home
M84 				;disable steppers so they dont get hot during idling...
;or you can use this one comment out 
;G1 X10.0 F4000 ;home (almost) x
;G1 Y170 F4000 ; move the print to the front.
M104 S0 ; make sure the extuder is turned off.
M140 S0
;M140 S0 ; make sure the bed is turned off.
;M84 ; shut down motors
M81