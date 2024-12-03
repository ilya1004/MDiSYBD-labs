import React, { useEffect } from 'react';
import { LoaderFunction, useLoaderData, useNavigate, useRevalidator, redirect } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { setAuthState, AuthState } from '../../store/auth/authSlice';
import axios from 'axios';
import { BASE_URL } from '../../store/constants';
import { showMessageStc } from '../../services/ResponseErrorHandler';
import { Flex, Button, Image, Card, Typography, List, Tag, Table, Dropdown, Space } from "antd";
import { RootState } from '../../store/store';
import type { MenuProps, TableProps } from 'antd';
import { FavouriteSongDTO, PlaylistEntity, SongInPlaylistDTO } from '../../utils/types';

const { Text, Title } = Typography


interface SongChart {
	id: number,
	title: string,
	artistName: string,
	averageRating: number,
	ratingCategory: string,
}

interface ArtistChart {
	id: number,
	artistName: string,
	playsCount: number
}

export const loaderHomePage: LoaderFunction = async (): Promise<{
	songs: SongChart[], artists: ArtistChart[], favSongs: FavouriteSongDTO[],
	songsInPlaylists: SongInPlaylistDTO[], playlists: PlaylistEntity[]
} | any> => {
	const axiosInstance = axios.create({
		baseURL: BASE_URL,
		withCredentials: true,
	});

	let userId = 0;

	try {
		const res = await axiosInstance.get(`Users/check`);
		console.log(res.data.id);
		userId = res.data.id;
	} catch (err: any) {
		showMessageStc(err, "error");
		// if (err.response.status == 401) {
			return redirect('/login');
		// }
	}
	console.log("qeqwe");
	try {
		const res1 = await axiosInstance.get(`Songs/average-rating?limit=${20}&offset=${0}`);
		const res2 = await axiosInstance.get(`Artists/by-plays`);
		const res3 = await axiosInstance.get(`FavouriteSongs/by-user/${userId}`);
		const res4 = await axiosInstance.get(`Songs/songs-in-playlists-by-user/${userId}`);
		const res5 = await axiosInstance.get(`Playlists/by-user/${userId}`);

		return { songs: res1.data.data, artists: res2.data.data, favSongs: res3.data.data, songsInPlaylists: res4.data.data, playlists: res5.data.data };

	} catch (err: any) {
		showMessageStc("Ошибка при загрузке состояния аутентификации: " + err, "error");
		return redirect('/login');
	}
}

const HomePage: React.FC = () => {
	const dispatch = useDispatch();
	const { songs, artists, favSongs, songsInPlaylists, playlists } = useLoaderData() as {
		songs: SongChart[], artists: ArtistChart[], favSongs: FavouriteSongDTO[],
		songsInPlaylists: SongInPlaylistDTO[], playlists: PlaylistEntity[]
	};

	const authState = useSelector((state: RootState) => state.auth);

	const navigate = useNavigate();
	const revalidator = useRevalidator();

	console.log(authState);

	const addSongToFavourities = async (songId: number | undefined) => {
		const axiosInstance = axios.create({
			baseURL: BASE_URL,
			withCredentials: true,
		});

		let userId = 0;

		if (authState.userId == null) {
			try {
				const res = await axiosInstance.get(`Users/check`);
				console.log(res.data);

			} catch (err: any) {
				showMessageStc(err, "error");
				navigate("/login");
				return null;
			}
		}

		const data = {
			userId: authState.userId,
			songId: songId
		}

		console.log(data);

		try {
			const res = await axiosInstance.post(`FavouriteSongs`, data);
			console.log(res.data);
			revalidator.revalidate();
		} catch (err: any) {
			showMessageStc(err, "error");
			return null;
		}
	}

	const removeSongFromFavourities = async (songId: number | undefined) => {
		const axiosInstance = axios.create({
			baseURL: BASE_URL,
			withCredentials: true,
		});

		try {
			const res = await axiosInstance.delete(`FavouriteSongs/${songId}`);
			console.log(res.data);
			revalidator.revalidate();
		} catch (err: any) {
			showMessageStc(err, "error");
			return null;
		}
	}

	const songsColumns: TableProps<SongChart>['columns'] = [
		{
			title: "#",
			dataIndex: "index",
			key: "index",
			width: "70px",
			render: (_: any, __: any, index: number) => (
				<Text style={{ fontSize: "16px" }}>{index + 1}</Text>
			),
		},
		{
			title: "Title",
			dataIndex: "title",
			key: "title",
			width: "150px",
			render: (title: string) => <Text style={{ fontSize: "16px" }}>{title}</Text>,
		},
		{
			title: "Artist",
			dataIndex: "artistName",
			key: "artistName",
			width: "150px",
			render: (artistName: string) => (
				<Text style={{ fontSize: "16px" }}>{artistName}</Text>
			),
		},
		{
			title: "Average Rating",
			dataIndex: "averageRating",
			key: "averageRating",
			width: "100px",
			render: (averageRating: number) => (
				<Tag color="blue" style={{ fontSize: "14px" }}>
					{averageRating}
				</Tag>
			),
		},
		{
			title: "Favourite Song",
			dataIndex: "",
			key: "",
			width: "150px",
			render: (_: any, item: SongChart, index: number) => renderFavSong(item),
		},
		{
			title: "Playlists",
			dataIndex: "",
			key: "",
			width: "200px",
			render: (_: any, item: SongChart, index: number) => renderPlaylists(item),
		},
	];

	const renderFavSong = (song: SongChart) => {
		var listSongInFavIds: number[] = [];

		favSongs.forEach(item => {
			listSongInFavIds.push(item.songId)
		});

		if (listSongInFavIds.includes(song.id)) {
			return (
				<Button danger onClick={() => removeSongFromFavourities(favSongs.find(item => item.songId == song.id)?.id)}>Remove</Button>
			)
		}
		else {
			return (
				<Button type="primary" onClick={() => addSongToFavourities(song.id)}>Add</Button>
			)
		}
	}

	const handleAddSongToPlaylist = async (key: string, songId: number) => {
		const axiosInstance = axios.create({
			baseURL: BASE_URL,
			withCredentials: true,
		});

		try {
			const res = await axiosInstance.post(`Playlists/${key}/songs/${songId}`);
			revalidator.revalidate();
		} catch (err: any) {
			showMessageStc(err, "error");
			return null;
		}

	}


	const renderPlaylists = (song: SongChart) => {
		const playlistsItems: MenuProps['items'] = [];

		playlists.forEach(playlist => {
			let dis = false;
			if (songsInPlaylists.find(itemSong => itemSong.songId === song.id && itemSong.playlistId === playlist.id)) {
				dis = true;
			}
			playlistsItems.push({
				key: playlist.id,
				label: playlist.title,
				disabled: dis,
				onClick: (e) => handleAddSongToPlaylist(e.key, song.id)
			});
		});


		return (
			<Dropdown menu={{ items: playlistsItems }} trigger={['click']}>
				<Space>
					<Button>Add to playlist</Button>
				</Space>
			</Dropdown>
		);
	}

	const artistColumns: TableProps<ArtistChart>['columns'] = [
		{
			title: "#",
			dataIndex: "index",
			key: "index",
			width: "70px",
			render: (_: any, __: any, index: number) => (
				<Text style={{ fontSize: "16px" }}>{index + 1}</Text>
			),
		},
		{
			title: "Artist Name",
			dataIndex: "artistName",
			key: "artistName",
			width: "200px",
			render: (artistName: string) => (
				<Text style={{ fontSize: "16px" }}>{artistName}</Text>
			),
		},
		{
			title: "Plays Count",
			dataIndex: "playsCount",
			key: "playsCount",
			width: "150px",
			render: (playsCount: number) => (
				<Tag color="orange" style={{ fontSize: "14px" }}>
					{playsCount}
				</Tag>
			),
		},
	]

	return (
		<>
			<Flex
				justify="center"
				align="flex-start"
				gap={50}
				style={{
					margin: "10px 15px",
					minHeight: "80vh",
				}}
			>
				<Flex
					align='center'
					vertical>
					<Title level={2}>
						Popular Songs
					</Title>
					<Table
						dataSource={songs}
						columns={songsColumns}
						pagination={false}
						bordered
					/>
				</Flex>

				<Flex
					align='center'
					vertical>
					<Title level={2}>
						Popular Artists
					</Title>
					<Table
						dataSource={artists}
						columns={artistColumns}
						pagination={false}
						bordered />
				</Flex>
			</Flex>
		</>
	);
};

export default HomePage;
