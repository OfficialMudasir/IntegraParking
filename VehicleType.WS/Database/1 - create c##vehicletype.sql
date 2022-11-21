
drop user C##VEHICLETYPE CASCADE;

create user C##VEHICLETYPE
  identified by vehicletype
  default tablespace USERS
  temporary tablespace TEMP
  profile DEFAULT
  quota unlimited on users;

grant create procedure to C##VEHICLETYPE;
grant create sequence to C##VEHICLETYPE;
grant create session to C##VEHICLETYPE;
grant create table to C##VEHICLETYPE;
grant create trigger to C##VEHICLETYPE;
grant create view to C##VEHICLETYPE;