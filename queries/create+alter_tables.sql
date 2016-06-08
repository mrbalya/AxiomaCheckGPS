-- it's necessary to create table BEFORE bulk insert
create table company (
id int, 
morionid int,
PostCode int,
country varchar(max),
region varchar(max),
city_type varchar(max),
city varchar(max),
street_type varchar(max),
street varchar(max),
building varchar(max),
morion_lat float,
morion_lng float,
name varchar(max)
)
-- add columns to table AFTER bulk insert
alter table company 
add 
google_lat float,
google_lng float,
yandex_lat float,
yandex_lng float,
yahoo_lat float,
yahoo_lng float,
google_error varchar(max),
yandex_error varchar(max),
yahoo_error varchar(max)
