OPTIONS (SKIP=1) 
load data
INFILE 'export_distintivo_ambiental.txt'
INTO TABLE vehicles_temp
TRUNCATE
FIELDS TERMINATED BY '|'
(veh_plate,
 veh_type)
