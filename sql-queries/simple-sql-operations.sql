select * from users
where role_id = 1;

select * from users
where role_id = 2;

select * from roles;

select * from songs
where title = 'Song 1'

select * from tags
order by id asc

select id, title from albums



alter table roles 
add qwe int default 2;

alter table roles
drop qwe



update tags
set name = 'Romantic' where id = 2



insert into tags (name)
values ('tag 4');

select count(*) from tags;

select avg(duration_secs) from songs

select min(duration_secs) from songs

select max(duration_secs) from songs

select * from songs

delete from tags 
where name = 'tag 4';



create table qwe1 (
	id int primary key,
	name varchar(10)
);


alter table qwe1
alter column name set default 'qweqwe'

alter table qwe1
add column countq int not null;

alter table qwe1
add constraint countq_unique unique (countq)

select * from information_schema.table_constraints
where table_name = 'qwe1'


truncate table qwe1;

delete from table qwe1;

drop table qwe1;




