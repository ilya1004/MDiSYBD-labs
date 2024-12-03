CREATE OR REPLACE PROCEDURE update_song_plays_count(song_id integer, count integer)
LANGUAGE plpgsql AS $$
BEGIN
	if exists(select 1 from songs where id = song_id) then
	    update songs
		set plays_count = plays_count + count
		where id = song_id;
	end if;
END;
$$;


CREATE OR REPLACE PROCEDURE add_song_to_favourite(song_id_val integer, user_id_val integer)
LANGUAGE plpgsql AS $$
BEGIN
	insert into favourite_songs (user_id, song_id)
	values (user_id_val, song_id_val);
END;
$$;


CREATE OR REPLACE PROCEDURE clear_past_events()
LANGUAGE plpgsql AS $$
BEGIN
	delete from events
	where datetime < CURRENT_TIMESTAMP;
END;
$$;


CREATE OR REPLACE PROCEDURE block_user(user_id integer)
LANGUAGE plpgsql AS $$
BEGIN
	update users
	set is_blocked = true
	where id = user_id;
END;
$$;


CREATE OR REPLACE PROCEDURE unblock_user(user_id integer)
LANGUAGE plpgsql AS $$
BEGIN
	update users
	set is_blocked = false
	where id = user_id;
END;
$$;


CREATE OR REPLACE PROCEDURE clear_old_reviews(days_past integer)
LANGUAGE plpgsql AS $$
BEGIN
	delete from reviews
	where CURRENT_DATE - date < days_past;
END;
$$;


CREATE OR REPLACE PROCEDURE set_all_playlists_private(user_id_val integer)
LANGUAGE plpgsql AS $$
BEGIN
	update playlists
	set is_public = false
	where user_id = user_id_val;
END;
$$;


CREATE OR REPLACE PROCEDURE set_all_playlists_public(user_id_val integer)
LANGUAGE plpgsql AS $$
BEGIN
	update playlists
	set is_public = true
	where user_id = user_id_val;
END;
$$;


CREATE OR REPLACE PROCEDURE delete_all_reviews_by_user(user_id_val integer)
LANGUAGE plpgsql AS $$
BEGIN
	delete from reviews
	where user_id = user_id_val;
END;
$$;

