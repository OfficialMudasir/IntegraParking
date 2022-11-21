-- Create table
create table VEHICLES_TEMP
(
  VEH_PLATE varchar2(50),
  VEH_TYPE  varchar2(50)
)
;
-- Create/Recreate primary, unique and foreign key constraints 
alter table VEHICLES_TEMP
  add constraint PK_VEHICLES_TEMP primary key (VEH_PLATE);
  
  