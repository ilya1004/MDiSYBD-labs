CREATE OR REPLACE FUNCTION get_artist_songs_rating(artist_id integer)
RETURNS TABLE (
    song_id integer,
    title text,
    artist_name text,
    plays_count integer,
    average_rating numeric,
    song_rank_by_artist integer
) 
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.id, s.title, ai.name AS artist_name, 
           s.plays_count, AVG(r.rating) AS average_rating, 
           DENSE_RANK() OVER (PARTITION BY ai.id ORDER BY AVG(r.rating) DESC) AS song_rank_by_artist
    FROM songs s
    INNER JOIN reviews r ON r.song_id = s.id
    INNER JOIN users u ON s.artist_id = u.id
    INNER JOIN artist_info ai ON u.artist_info_id = ai.id
    WHERE u.id = artist_id
    GROUP BY s.id, ai.id, s.plays_count
    ORDER BY artist_name, song_rank_by_artist;
END;
$$;



CREATE OR REPLACE FUNCTION get_tags_by_song(song_id_val integer)
RETURNS TABLE (
    song_id integer,
    title text,
    artist_name text,
    tag_name text
) LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.id, s.title, ai.name AS artist_name, t.name AS tag_name
    FROM songs s
    INNER JOIN users u ON s.artist_id = u.id
    INNER JOIN artist_info ai ON u.artist_info_id = ai.id
    LEFT OUTER JOIN songs_tags st ON st.song_id = s.id
    LEFT OUTER JOIN tags t ON st.tag_id = t.id
    WHERE s.id = song_id_val
    ORDER BY s.id ASC;
END;
$$;



CREATE OR REPLACE FUNCTION get_playlist_duration(playlist_id_val integer)
RETURNS TABLE (
    playlist_id integer,
    title text,
    playlist_length_secs integer
) 
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
	select p.id, p.title, sum(s.duration_secs) playlist_length_secs
	from playlists p
	inner join playlists_songs ps on ps.playlist_id = p.id
	inner join songs s on ps.song_id = s.id
	inner join users u on s.artist_id = u.id
	inner join artist_info ai on u.artist_info_id = ai.id
	where p.id = playlist_id_val
	group by p.id, p.title;
END;
$$;



CREATE OR REPLACE FUNCTION get_songs_by_playlist(playlist_id_val integer)
RETURNS TABLE (
    song_id integer,
    title text,
    duration_secs integer,
    artist_name text,
    playlist_name text
) 
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.id, s.title, s.duration_secs, ai.name AS artist_name, p.title AS playlist_name
    FROM songs s
    INNER JOIN users u ON s.artist_id = u.id
    INNER JOIN artist_info ai ON u.artist_info_id = ai.id
    INNER JOIN playlists_songs ps ON ps.song_id = s.id
    INNER JOIN playlists p ON ps.playlist_id = p.id
    WHERE p.id = playlist_id_val
    ORDER BY s.id ASC;
END;
$$;



CREATE OR REPLACE FUNCTION main_search(search_query text)
RETURNS TABLE (
    id integer,
    name text,
    type text
) 
LANGUAGE plpgsql AS $$
BEGIN
    RETURN QUERY
    SELECT s.id, s.title AS name, 'song' AS type
    FROM songs s
    WHERE s.title ILIKE CONCAT('%', search_query, '%')
    
    UNION ALL
    
    SELECT u.id, ai.name AS name, 'artist' AS type
    FROM users u
    INNER JOIN artist_info ai ON u.artist_info_id = ai.id 
    WHERE ai.name ILIKE CONCAT('%', search_query, '%')
    
    UNION ALL
    
    SELECT p.id, p.title AS name, 'playlist' AS type
    FROM playlists p
    WHERE p.title ILIKE CONCAT('%', search_query, '%') AND p.is_public = true
    
    ORDER BY type, name;
END;
$$;