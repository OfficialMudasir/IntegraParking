-- Create table
create table VEHICLES
(
  VEH_PLATE varchar2(50),
  VEH_TYPE  varchar2(50)
)
;
-- Create/Recreate primary, unique and foreign key constraints 
alter table VEHICLES
  add constraint PK_VEHICLES primary key (VEH_PLATE);
  
  