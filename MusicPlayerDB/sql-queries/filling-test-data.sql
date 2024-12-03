INSERT INTO TAGS (NAME) VALUES 
('Energetic'), 
('Love'), 
('Sad');


insert into genres (name, description) values
('Pop', 'Pop music is characterized by its catchy melodies and widespread appeal. It often features simple lyrics and a focus on themes of love and relationships. The genre draws influences from various styles, including rock, dance, and electronic music, making it highly versatile. Popular artists in this genre include Taylor Swift, Ariana Grande, and Justin Bieber.'),
('Rock', 'Rock music originated in the 1950s and is known for its strong beat and use of electric guitars. It encompasses a wide range of styles, from classic rock to punk and alternative rock. Lyrically, rock songs often express themes of rebellion, love, and social issues. Iconic rock bands include The Beatles, Led Zeppelin, and The Rolling Stones.');


INSERT INTO user_info (nickname, is_blocked) VALUES
('user1', false),
('user2', false);


INSERT INTO artist_info (name, description, country, birthday) VALUES
('Artist1', 'Description of Artist 1', 'USA', '1985-06-15'),
('Artist2', 'Description of Artist 2', 'UK', '1990-08-22');


INSERT INTO roles (name) VALUES
('User'),
('Artist'),
('Admin');


INSERT INTO users (login, password_hash, role_id, user_info_id, artist_info_id) VALUES
('user1_login', 'hash1', 1, 1, NULL),
('user2_login', 'hash2', 1, 2, NULL),
('artist1_login', 'hash3', 2, NULL, 1),
('artist2_login', 'hash4', 2, NULL, 2),
('admin1_login', 'hash5', 3, NULL, NULL);


INSERT INTO events (title, description, datetime, location, artist_id) VALUES
('Event 1', 'Description of Event 1', '2024-12-01 18:00:00', 'New York', 3),
('Event 2', 'Description of Event 2', '2024-11-15 19:00:00', 'London', 4),
('Event 3', 'Description of Event 3', '2024-10-20 20:00:00', 'Toronto', 5);


INSERT INTO user_logs (action, datetime, user_id) VALUES
('Login', '2024-09-01 10:00:00', 1),
('Logout', '2024-09-01 12:00:00', 1),
('Password Change', '2024-09-02 15:30:00', 2);


INSERT INTO albums (title, release_year, artist_id) VALUES
('Album 1', 2020, 3),
('Album 2', 2022, 4),
('Album 3', 2014, 4);


INSERT INTO songs (title, duration_secs, release_year, artist_id, genre_id, album_id) VALUES
('Song 1', 210, 2020, 3, 1, 1),
('Song 2', 180, 2022, 4, 2, 2),
('Song 3', 240, 2014, 4, 2, 3);


INSERT INTO songs_tags (song_id, tag_id) VALUES
(1, 1),
(2, 2),
(3, 3);


INSERT INTO favourite_songs (user_id, song_id) VALUES
(2, 1),
(2, 2),
(2, 3),
(1, 2);


INSERT INTO playlists (title, description, is_public, user_id) VALUES
('Playlist 1', 'Description of Playlist 1', true, 1),
('Playlist 2', 'Description of Playlist 2', false, 2),
('Playlist 3', 'Description of Playlist 3', false, 2);


INSERT INTO playlists_songs (song_id, playlist_id) VALUES
(1, 1),
(2, 2),
(2, 3),
(3, 3);


INSERT INTO reviews (title, text, rating, user_id, song_id) VALUES
('Review 1', 'Great song!', 5, 1, 1),
('Review 2', 'Not bad', 3, 2, 2),
('Review 3', 'Could be better', 2, 2, 3);



