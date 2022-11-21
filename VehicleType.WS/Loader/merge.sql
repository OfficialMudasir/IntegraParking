delete from vehicles where veh_plate not in (select veh_plate from vehicles_temp);

merge into vehicles
using vehicles_temp src
on (vehicles.veh_plate = src.veh_plate)
when matched
then
update set veh_type = src.veh_type
when not matched
then
insert (veh_plate,veh_type)
values (src.veh_plate, src.veh_type);

commit;

truncate table vehicles_temp;

exit

