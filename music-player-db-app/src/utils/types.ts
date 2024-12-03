

export interface FavouriteSongDTO {
	id: number,
	dateAdded: string,
	songId: number,
	title: string,
	durationSecs: number,
	artistName: string
}

export interface SongEntity {
	id: number,
	title: string,
	durationSecs: number,
	releaseYear: number,
	artistId: number | null,
	artist: any | null,
	genreId: number | null,
	genre: any | null,
	albumId: number | null,
	album: any | null,
	playsCount: number,
}

export interface AlbumEntity {
	id: number,
	title: string,
	releaseYear: number,
	artistId: number,
	artist: any | null
}

export interface PlaylistEntity {
	id: number,
	title: string,
	description: string,
	isPublic: boolean,
	userId: number | null,
	user: any | null,
}

export interface ArtistDetailDTO {
	id: number,
	login: string,
	passwordHash: string,
	isBlocked: boolean,
	name: string,
	description: string,
	country: string,
	birthday: string,
}

export interface SongByPlaylistDTO {
	id: number,
	title: string,
	durationSecs: number,
	artistName: string,
	playlistName: string,
}

export interface SongInPlaylistDTO {
	songId: number,
	songTitle: string,
	playlistId: number,
	playlistTitle: string,
	userId: number
}
