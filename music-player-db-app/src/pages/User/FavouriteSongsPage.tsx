import React, { useEffect } from 'react';
import { LoaderFunction, useLoaderData, useRevalidator } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { setAuthState, AuthState } from '../../store/auth/authSlice';
import axios from 'axios';
import { BASE_URL } from '../../store/constants';
import { showMessageStc } from '../../services/ResponseErrorHandler';
import { Flex, Button, Image, Card, Typography, Table } from "antd";
import type { TableProps } from 'antd';
import { RootState, store } from '../../store/store';
import { FavouriteSongDTO } from '../../utils/types';

const { Text, Title } = Typography

export const loaderFavSongsPage: LoaderFunction = async (): Promise<FavouriteSongDTO[] | null> => {
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
		return null;
	}

	try {
		const res = await axiosInstance.get(`FavouriteSongs/by-user/${userId}`);
		console.log(res.data.data);
		return res.data.data;

	} catch (err: any) {
		showMessageStc(err, "error");

		return null;
	}
}

const UserFavouriteSongsPage: React.FC = () => {
	const dispatch = useDispatch();
	// const authStateLoader = useLoaderData() as AuthState;
	const songsData = useLoaderData() as FavouriteSongDTO[];
	const authState = useSelector((state: RootState) => state.auth);

	const revalidator = useRevalidator();

	console.log(authState);

	// const avatarUrl = ;

	// useEffect(() => {
	//   dispatch(setAuthState(authStateLoader));
	// }, [authStateLoader, dispatch]);

	const removeSongFromFavourities = async (songId: number) => {
		const axiosInstance = axios.create({
			baseURL: BASE_URL,
			withCredentials: true,
		});
	
		try {
			const res = await axiosInstance.delete(`FavouriteSongs/${songId}`);
			revalidator.revalidate();
		} catch (err: any) {
			showMessageStc(err, "error");
		}
	}

	const tableColumns: TableProps<FavouriteSongDTO>['columns'] = [
		{
			title: "#",
			dataIndex: "id",
			key: "id",
			width: "50px",
			render: (_, item, index) => (
				<Text>
					{index + 1}
				</Text>
			)
		},
		{
			title: "Title",
			dataIndex: "title",
			key: "title",
			width: "200px",
		},
		{
			title: "Date added",
			dataIndex: "dateAdded",
			key: "dateAdded",
			width: "200px",
		},
		{
			title: "Artist",
			dataIndex: "artistName",
			key: "artistName",
			width: "200px",
		},
		{
			title: "Duration",
			dataIndex: "durationSecs",
			key: "durationSecs",
			width: "130px",
			render: (_, item, index) => (
				<Text>
					{`${Math.floor(item.durationSecs / 60)}:${((item.durationSecs % 60 < 10) ? (item.durationSecs % 60).toString() + "0" : item.durationSecs % 60)}`}
				</Text>
			)
		},
		{
			title: "Favourite Song",
			dataIndex: "",
			key: "",
			width: "200px",
			render: (_, item, index) => (
				<Button danger onClick={() => removeSongFromFavourities(item.id)}>Remove</Button>
			)
		}
	]

	return (
		<>
			<Flex
				justify="center"
				align="flex-start"
				style={{
					margin: "10px 15px",
					minHeight: "80vh",
				}}
			>
				<Flex vertical align='center'>
					<Title>My favourite songs</Title>
					<Table columns={tableColumns} dataSource={songsData} />
				</Flex>
			</Flex>
		</>
	);
};

export default UserFavouriteSongsPage;
