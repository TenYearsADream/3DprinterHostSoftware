  M92 E92

M109 S230
M190 S60
G21           ;set units to mm
G90           ;set to absolute positioning
;M80
M107
G92 E0		;reset extruder 
G28 X0 Y0 Z0
G92 Z138.8
G1 Z0.2 F400
G1 X20 E4 F100
G1 Y3 E6 F100
G1 X0 E10
G92 E0  
M140 S90