
create table VEHICLE_TYPES
(
  VEHT_TYPE        VARCHAR2(50) not null,
  VEHT_DESCRIPTION VARCHAR2(50) not null
)
;
alter table VEHICLE_TYPES
  add constraint PK_VEHICLE_TYPES primary key (VEHT_TYPE);

insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16ME', 'Eco');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16M0', 'Cero Emisiones');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16TC', 'C');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16T0', 'Cero Emisiones');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16TE', 'Eco');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16MB', 'B');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16MC', 'C');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('16TB', 'B');
insert into VEHICLE_TYPES (VEHT_TYPE, VEHT_DESCRIPTION)
values ('SIN DISTINTIVO', 'Sin Distintivo');
commit;
