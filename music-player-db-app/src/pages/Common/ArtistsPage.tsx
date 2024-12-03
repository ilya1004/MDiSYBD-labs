import React, { useEffect, useState } from 'react';
import { LoaderFunction, useLoaderData } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { setAuthState, AuthState } from '../../store/auth/authSlice';
import axios from 'axios';
import { BASE_URL } from '../../store/constants';
import { showMessageStc } from '../../services/ResponseErrorHandler';
import { Flex, Button, Image, Card, Typography, List, Tag } from "antd";
import { RootState } from '../../store/store';

const { Text, Title } = Typography


interface ArtistInfo {
	id: number,
	login: string,
	passwordHash: string,
	isBlocked: boolean,
	name: string,
	description: string,
	country: string,
	birthday: string
}

interface SongInfo {
	id: number,
	title: string,
	durationSecs: number,
	releaseYear: number,
	artistId: number | null,
	artist: any | null,
	genreId: number | null,
	genre: any | null,
	albumId: number | null,
	playsCount: number
}

interface AlbumInfo {
	id: number,
	title: string,
	releaseYear: number,
	artistId: number,
	artist: any | null;
}

export const loaderArtistsPage: LoaderFunction = async (): Promise<{ artists: ArtistInfo[] } | null> => {
	const axiosInstance = axios.create({
		baseURL: BASE_URL,
		withCredentials: true,
	});

	try {
		const res = await axiosInstance.get(`Artists/all?limit=${10}&offset=${0}`);
		console.log(res.data.data);
		return { artists: res.data.data };

	} catch (err: any) {
		showMessageStc(err, "error");

		return null;
	}
}

const ArtistsPage: React.FC = () => {
	const dispatch = useDispatch();
	// const authStateLoader = useLoaderData() as AuthState;
	const { artists } = useLoaderData() as { artists: ArtistInfo[] };
	const [currArtist, setCurrArtist] = useState<ArtistInfo | null>(null);
	const [artistSongs, setArtistSongs] = useState<SongInfo[]>([]);
	const [artistAlbums, setArtistAlbums] = useState<AlbumInfo[]>([]);

	const authState = useSelector((state: RootState) => state.auth);

	const showArtistInfo = async (item: ArtistInfo) => {
		const axiosInstance = axios.create({
			baseURL: BASE_URL,
			withCredentials: true,
		});

		try {
			const res = await axiosInstance.get(`Artists/${item.id}`);
			console.log(res.data.data);
			setCurrArtist(res.data.data);
		} catch (err: any) {
			showMessageStc(err, "error");
		}

		try {
			const res = await axiosInstance.get(`Songs/by-artist/${item.id}`);
			console.log(res.data.data);
			setArtistSongs(res.data.data);
		} catch (err: any) {
			showMessageStc(err, "error");
		}

		// let albums: AlbumInfo[] = []

		try {
			const res = await axiosInstance.get(`Albums/by-artist/${item.id}`);
			console.log(res.data.data);
			// albums = res.data.data;
			setArtistAlbums(res.data.data);
		} catch (err: any) {
			showMessageStc(err, "error");
		}
	}

	const albumSong = (item: AlbumInfo, index: number) => {
		let songs: SongInfo[] = [];

		for (let i = 0; i < artistSongs.length; i++) {
			if (artistSongs[i].albumId == item.id) {
				songs.push(artistSongs[i])
			}
		}


		return (
			<List.Item>
				<Card
					title={item.title}
					style={{ width: "400px" }}	>
					<List
						itemLayout="horizontal"
						dataSource={songs}
						renderItem={(item, index) => (
							<List.Item style={{ padding: "5px" }}>
								<Flex gap={5}>
									<Text style={{ fontSize: "20px" }}>
										{`${index + 1}. `}
									</Text>
									<Flex vertical
										align='start'>
										<Text style={{ fontSize: "20px" }}>
											{` ${item.title}`}
										</Text>
									</Flex>
								</Flex>
							</List.Item>
						)}
					/>
				</Card>
			</List.Item>
		)
	}

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
					vertical>
					<Title level={2}>
						Popular Artists
					</Title>
					<Card>
						<List
							itemLayout="horizontal"
							dataSource={artists}
							renderItem={(item, index) => (
								<List.Item style={{ padding: "5px" }}>
									<Flex gap={5}>
										<Text style={{ fontSize: "20px" }}>
											{`${index + 1}. `}
										</Text>
										<Flex vertical
											align='start'>
											<Text style={{ fontSize: "20px" }}>
												{` ${item.name}`}
											</Text>
										</Flex>
									</Flex>
									<Button style={{ margin: "0px 0px 0px 10px" }} onClick={() => showArtistInfo(item)}>
										<Text>Show</Text>
									</Button>
								</List.Item>
							)}
						/>
					</Card>
				</Flex>
				{currArtist != null ? (
					<Flex gap={40} style={{ width: "900px" }}>
						<Flex vertical align='start' gap={20}>
							<Card style={{ width: "400px" }}>
								<Title level={1} style={{ marginTop: "5px"}}>
									{currArtist.name}
								</Title>
								<div>
									<Text style={{ fontSize: "18px" }}>{"Country: "}</Text>
									<Text>{currArtist.country}</Text>
								</div>
								<div>
									<Text style={{ fontSize: "18px" }}>{"Birthday: "}</Text>
									<Text>{currArtist.birthday.slice(0, 10)}</Text>
								</div>
								<div>
									<Text style={{ fontSize: "18px" }}>{"Description: "}</Text>
									<Text>{currArtist.description}</Text>
								</div>
							</Card>
							<Card style={{ width: "300px" }}>
								<List
									itemLayout="horizontal"
									dataSource={artistSongs}
									renderItem={(item, index) => (
										<List.Item style={{ padding: "5px" }}>
											<Flex gap={5}>
												<Text style={{ fontSize: "20px" }}>
													{`${index + 1}. `}
												</Text>
												<Flex vertical
													align='start'>
													<Text style={{ fontSize: "20px" }}>
														{` ${item.title}`}
													</Text>
												</Flex>
											</Flex>
										</List.Item>
									)}
								/>
							</Card>
						</Flex>
						<Flex>
							<List
								itemLayout='horizontal'
								dataSource={artistAlbums}
								renderItem={(item, index) => albumSong(item, index)}
							/>
						</Flex>
					</Flex>) : (
					<Flex style={{ width: "900px" }}>
						<Title level={2}>Select Artist</Title>
					</Flex>
				)}
			</Flex>
		</>
	);
};

export default ArtistsPage;
