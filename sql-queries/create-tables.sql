CREATE TABLE user_info (
	id serial PRIMARY KEY,
	nickname varchar(50) NOT NULL,
	about varchar(300)
);

CREATE TABLE artist_info (
	id serial PRIMARY KEY,
	name varchar(50) not null,
	description text,
	country varchar(50),
	birthday date CHECK (birthday >= '1900-01-01' AND birthday <= CURRENT_DATE)
);

CREATE TABLE roles (
	id serial PRIMARY KEY,
	name varchar(30) not null
);

CREATE TABLE users (
	id serial PRIMARY KEY,
	login varchar(50) NOT NULL UNIQUE,
	password_hash varchar(100) NOT NULL,
	is_blocked bool NOT NULL DEFAULT false,
	role_id int not null references roles(id),
	user_info_id int references user_info(id) on delete set null,
	artist_info_id int references artist_info(id) on delete set null
);

CREATE TABLE events (
	id serial primary key,
	title varchar(50) not null,
	description text,
	datetime timestamp not null check (datetime >= CURRENT_TIMESTAMP),
	location varchar(50) not null,
	artist_id int not null references users(id) on delete cascade
);

CREATE TABLE user_logs (
	id serial primary key,
	action varchar(100) not null,
	datetime timestamp not null,
	user_id int references users(id) on delete set null
);

CREATE TABLE genres (
	id serial primary key,
	name varchar(30) not null,
	description text
);

CREATE TABLE tags (
	id serial primary key,
	name varchar(30) not null
);

CREATE TABLE albums (
	id serial primary key,
	title varchar(30) not null,
	release_year int check (release_year <= EXTRACT(YEAR FROM CURRENT_DATE)),
	artist_id int not null references users(id) on delete cascade
);

CREATE TABLE songs (
	id serial primary key,
	title varchar(30) not null,
	duration_secs int not null check (duration_secs > 0),
	release_year int check (release_year <= EXTRACT(YEAR FROM CURRENT_DATE)),
	plays_count int not null check (plays_count >= 0) default 0,
	artist_id int not null references users(id) on delete cascade,
	genre_id int references genres(id) on delete set null,
	album_id int references albums(id) on delete cascade
);

CREATE TABLE songs_tags (
	id serial primary key,
	song_id int not null references songs(id) on delete cascade,
	tag_id int not null references tags(id) on delete cascade
);

CREATE TABLE favourite_songs (
	id serial primary key,
	date_added date default CURRENT_DATE,
	user_id int not null references users(id) on delete cascade,
	song_id int not null references songs(id) on delete cascade
);

CREATE TABLE playlists (
	id serial primary key,
	title varchar(50) not null,
	description varchar(200),
	is_public bool default false,
	user_id int not null references users(id) on delete cascade
);

CREATE TABLE playlists_songs (
	id serial primary key,
	song_id int not null references songs(id) on delete cascade,
	playlist_id int not null references playlists(id) on delete cascade
);

CREATE TABLE reviews (
	id serial primary key,
	title varchar(50),
	text text,
	rating smallint not null check (rating >= 1 and rating <= 5),
	date date default CURRENT_DATE,
	user_id int references users(id) on delete set null,
	song_id int not null references songs(id) on delete cascade
);








