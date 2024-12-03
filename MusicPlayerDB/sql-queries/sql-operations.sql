
-- выборка логов по дате
select * from user_logs
where '2024-08-01' <= datetime and datetime < '2024-10-01';


-- выборка логов по дате и юзеру
select * from user_logs
where user_id = 1 and '2024-08-01' <= datetime and datetime < '2024-10-01';



-- выборка события по артисту и дате
select * from events
where artist_id = 
	(select u.id from users u 
	inner join artist_info ai on u.artist_info_id = ai.id 
	where ai.name = 'Artist1' 
	limit 1) 
	and datetime > current_date
order by datetime asc;

-- выборка события по дате и месту
select * from events
where datetime > CURRENT_DATE and location = 'New York'
order by datetime asc;



-- выборка для логина пользователя
select u.id, u.login, u.password_hash, r.name role_name, u.is_blocked from users u
inner join roles r on u.role_id = r.id
where u.login = 'user1_login' and u.password_hash = 'hash1';

-- выбор информации о юзере
select u.id, ui.nickname, ui.about from users u
inner join user_info ui on u.user_info_id = ui.id
where u.id = 1;

-- выборка информации об артисте
select u.id, ai.name, ai.description, ai.country, ai.birthday from users u
inner join artist_info ai on u.artist_info_id = ai.id
where u.id = 3;

-- выборка юзеров
select u.id, u.login, u.password_hash, u.is_blocked, ui.nickname, ui.about from users u
inner join user_info ui on u.user_info_id = ui.id
inner join roles r on u.role_id = r.id
where r.name = 'User'
order by u.id asc
limit 10
offset 0;

-- выборка артистов
select distinct u.id, u.login, u.password_hash, u.is_blocked, ai.name, ai.description, ai.country, ai.birthday 
from users u
inner join artist_info ai on u.artist_info_id = ai.id
inner join roles r on u.role_id = r.id
where r.name = 'Artist'
order by u.id asc
limit 10
offset 0;



-- выборка отзывов по песне
select * from reviews
where song_id = 1
limit 10
offset 0;

-- добавление отзыва
insert into reviews (title, text, rating, user_id, song_id)
values ('', '', 1, 1, 1);



-- выборка средней оценки по песням 
select s.id, s.title, ai.name artist_name, avg(r.rating) average_rating,
	case 
        when avg(r.rating) >= 4 then 'High rating'
        when 4 > avg(r.rating) and avg(r.rating) >= 3 then 'Medium rating'
        else 'Low rating'
    end as rating_category
from songs s
inner join reviews r on r.song_id = s.id
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
group by s.id, ai.name
order by average_rating desc
limit 10 offset 0;

-- выборка количества добавлений в любимые
select s.id, s.title, s.duration_secs, s.plays_count, count(*) favourities_count
from favourite_songs fss
inner join songs s on fss.song_id = s.id
inner join users u on s.artist_id = u.id
where s.artist_id = 4
group by s.id, fss.song_id
having plays_count > 15
order by plays_count desc, favourities_count desc;

-- выборка песен по альбому
select s.id, s.title, ai.name artist_name, a.title album_name, s.duration_secs, plays_count
from songs s
inner join albums a on s.album_id = a.id
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
where s.album_id = 2
order by s.id desc;

-- выборка тегов для песни
select s.id, s.title, ai.name artist_name, t.name tag_name from songs s
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
left outer join songs_tags st on st.song_id = s.id
left outer join tags t on st.tag_id = t.id
where s.id = 3
order by s.id asc;

-- выборка песен по плейлисту
select s.id, s.title, s.duration_secs, ai.name artist_name, p.title playlist_name from songs s
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
inner join playlists_songs ps on ps.song_id = s.id
inner join playlists p on ps.playlist_id = p.id
where p.id = 3
order by s.id asc;

-- выборка песен для который нету отзывов
select s.id, s.title, ai.name from songs s
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
where not exists (
    select 1
    from reviews r
    where r.song_id = s.id
);

-- выборка песен по убывающему рейтингу по исполнителю
SELECT s.id, s.title, ai.name artist_name, plays_count, AVG(r.rating) average_rating, 
	DENSE_RANK() OVER (PARTITION BY ai.id ORDER BY AVG(r.rating) DESC) song_rank_by_artist
FROM songs s
INNER JOIN reviews r ON r.song_id = s.id
INNER JOIN users u ON s.artist_id = u.id
INNER JOIN artist_info ai ON u.artist_info_id = ai.id
where u.id = 4
GROUP BY s.id, ai.id
ORDER BY artist_name, song_rank_by_artist;

-- выборка песен по жанру и среднему рейтингу
SELECT g.name genre, s.title,
	AVG(r.rating) OVER (PARTITION BY g.name) average_rating
FROM songs s
INNER JOIN genres g ON s.genre_id = g.id
INNER JOIN reviews r ON r.song_id = s.id
ORDER BY genre, average_rating DESC;

-- выборка артистов по количеству прослущиваний
select u.id, ai.name, sum(s.plays_count) artist_plays_count from songs s
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
group by u.id, ai.name
order by artist_plays_count desc;

-- выборка песен по жанру
select s.id, s.title, u.id artist_id, ai.name artist_name, g.id genre_id, g.name genre_name from songs s
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
inner join genres g on s.genre_id = g.id
where g.id = 2
order by s.id asc
limit 10
offset 0;

-- выборка песен по тегу
select s.id, s.title, u.id artist_id, ai.name artist_name, t.id tag_id, t.name tag_name from songs s
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
inner join songs_tags st on st.song_id = s.id
inner join tags t on st.tag_id = t.id
where t.id = 1
order by s.id asc
limit 10
offset 0;




-- выборка длительности плейлиста
select p.id, p.title, sum(s.duration_secs) playlist_length_secs
from playlists p
inner join playlists_songs ps on ps.playlist_id = p.id
inner join songs s on ps.song_id = s.id
inner join users u on s.artist_id = u.id
inner join artist_info ai on u.artist_info_id = ai.id
where p.id = 3
group by p.id, p.title;

-- выборка всех публичных плейлистов пользователя
select id, title, description
from playlists
where user_id = 1 and is_public = true;

-- создание плейлиста
insert into table playlists (title, description, is_public, user_id)
values ('', '', true, 1);

-- изменение публичности плейлиста
update playlists
set is_public = false
where id = 1;



-- добавление песни в плейлист
insert into playlists_songs (song_id, playlist_id)
values (1, 1);



-- выборка длительности всех любимых песен
select sum(duration_secs) favourities_length_secs from favourite_songs fss
inner join songs s on fss.song_id = s.id;

-- добавление любимой песни
insert into favourite_songs (user_id, song_id)
values (1, 1);



-- главный поиск по названию
select s.id, s.title name, 'song' type from songs s
where s.title ilike '%s%'
union all
select u.id, ai.name name, 'artist' type from users u
inner join artist_info ai on u.artist_info_id = ai.id 
where ai.name ilike '%s%'
union all
select p.id, p.title name, 'playlist' type from playlists p
where p.title ilike '%s%' and is_public = true
union all
select a.id, a.title name, 'album' type from albums a
where a.title ilike '%s%'
order by type, name;



explain analyze verbose select title, duration_secs from songs
where id = 1;





