CREATE OR REPLACE FUNCTION trigger_after_user_creation_add_info_func()
RETURNS TRIGGER AS $$
DECLARE
	new_user_info_id integer;
BEGIN
	if NEW.role_id = 1 then
	    INSERT INTO user_info (nickname)
	    VALUES (CONCAT('User_', NEW.id + 1000))
		RETURNING id INTO new_user_info_id;
		UPDATE users 
		SET user_info_id = new_user_info_id 
		WHERE id = NEW.id;
	end if;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_after_user_creation_add_info
AFTER INSERT ON users
FOR EACH ROW
EXECUTE FUNCTION trigger_after_user_creation_add_info_func();



CREATE OR REPLACE FUNCTION trigger_after_artist_creation_add_info_func()
RETURNS TRIGGER AS $$
DECLARE
	new_artist_info_id integer;
BEGIN
	if NEW.role_id = 2 then
	    INSERT INTO artist_info (nickname)
	    VALUES (CONCAT('Artist_', NEW.id + 1000))
		RETURNING id INTO new_artist_info_id;
		UPDATE users 
		SET artist_info_id = new_artist_info_id 
		WHERE id = NEW.id;
	end if;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_after_artist_creation_add_info
AFTER INSERT ON users
FOR EACH ROW
EXECUTE FUNCTION trigger_after_artist_creation_add_info_func();



CREATE OR REPLACE FUNCTION trigger_before_set_user_nickname_check_unique_func()
RETURNS TRIGGER AS $$
BEGIN
    if exists(select 1 from user_info where nickname = NEW.nickname) then
        raise exception 'This user nickname already exists';
    end if;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_before_set_user_nickname_check_unique
BEFORE INSERT OR UPDATE ON user_info
FOR EACH ROW
EXECUTE FUNCTION trigger_before_set_user_nickname_check_unique_func();



CREATE OR REPLACE FUNCTION trigger_before_set_artist_name_check_unique_func()
RETURNS TRIGGER AS $$
BEGIN
    if exists(select 1 from artist_info where name = NEW.name) then
        raise exception 'This artist name already exists';
    end if;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_before_set_artist_name_check_unique
BEFORE INSERT OR UPDATE ON artist_info
FOR EACH ROW
EXECUTE FUNCTION trigger_before_set_artist_name_check_unique_func();



CREATE OR REPLACE FUNCTION trigger_user_log_default_func()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('User with login "', NEW.login, '" created'), NOW(), NEW.id);
        RETURN NEW;

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('User with login "', NEW.login, '" updated'), NOW(), NEW.id);
        RETURN NEW;

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('User with login "', OLD.login, '" deleted'), NOW(), OLD.id);
        RETURN OLD;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_user_log_default
AFTER INSERT OR UPDATE OR DELETE ON users
FOR EACH ROW
EXECUTE FUNCTION trigger_user_log_default_func();



CREATE OR REPLACE FUNCTION trigger_album_log_default_func()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Album with title "', NEW.title, '" created'), NOW(), NEW.artist_id);
        RETURN NEW;

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Album with title "', NEW.title, '" updated'), NOW(), NEW.artist_id);
        RETURN NEW;

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Album with title "', OLD.title, '" deleted'), NOW(), OLD.artist_id);
        RETURN OLD;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_album_log
AFTER INSERT OR UPDATE OR DELETE ON albums
FOR EACH ROW
EXECUTE FUNCTION trigger_album_log_default_func();



CREATE OR REPLACE FUNCTION trigger_song_log_default_func()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Song with title "', NEW.title, '" created'), NOW(), NEW.artist_id);
        RETURN NEW;

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Song with title "', NEW.title, '" updated'), NOW(), NEW.artist_id);
        RETURN NEW;

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Song with title "', OLD.title, '" deleted'), NOW(), OLD.artist_id);
        RETURN OLD;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_song_log_default
AFTER INSERT OR UPDATE OR DELETE ON songs
FOR EACH ROW
EXECUTE FUNCTION trigger_song_log_default_func();



CREATE OR REPLACE FUNCTION trigger_event_log_default_func()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Event with title "', NEW.title, '" created'), NOW(), NEW.artist_id);
        RETURN NEW;

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Event with title "', NEW.title, '" updated'), NOW(), NEW.artist_id);
        RETURN NEW;

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Event with title "', OLD.title, '" deleted'), NOW(), OLD.artist_id);
        RETURN OLD;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_event_log_default
AFTER INSERT OR UPDATE OR DELETE ON events
FOR EACH ROW
EXECUTE FUNCTION trigger_event_log_default_func();



CREATE OR REPLACE FUNCTION trigger_review_log_default_func()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Review with title "', NEW.title, '" created'), NOW(), NEW.user_id);
        RETURN NEW;

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Review with title "', NEW.title, '" updated'), NOW(), NEW.user_id);
        RETURN NEW;

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Review with title "', OLD.title, '" deleted'), NOW(), OLD.user_id);
        RETURN OLD;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_review_log_default
AFTER INSERT OR UPDATE OR DELETE ON reviews
FOR EACH ROW
EXECUTE FUNCTION trigger_review_log_default_func();




CREATE OR REPLACE FUNCTION trigger_playlist_log_default_func()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Playlist with title "', NEW.title, '" created'), NOW(), NEW.user_id);
        RETURN NEW;

    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Playlist with title "', NEW.title, '" updated'), NOW(), NEW.user_id);
        RETURN NEW;

    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO logs (action, datetime, user_id)
        VALUES (CONCAT('Playlist with title "', OLD.title, '" deleted'), NOW(), OLD.user_id);
        RETURN OLD;
    END IF;

    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_playlist_log_default
AFTER INSERT OR UPDATE OR DELETE ON playlists
FOR EACH ROW
EXECUTE FUNCTION trigger_playlist_log_default_func();


