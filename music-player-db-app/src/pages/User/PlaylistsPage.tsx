import React, { useEffect, useState } from 'react';
import { LoaderFunction, useLoaderData, useRevalidator } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { setAuthState, AuthState } from '../../store/auth/authSlice';
import axios from 'axios';
import { BASE_URL } from '../../store/constants';
import { showMessageStc } from '../../services/ResponseErrorHandler';
import { Flex, Button, Image, Card, Typography, List, Tag, Row, Col, Input, Checkbox } from "antd";
import { RootState } from '../../store/store';
import { PlaylistEntity, SongEntity } from '../../utils/types';

const { Text, Title } = Typography

export const loaderPlaylistsPage: LoaderFunction = async (): Promise<{ playlists: PlaylistEntity[] } | null> => {
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
    const res = await axiosInstance.get(`Playlists/by-user/${userId}`);
    console.log(res.data.data);
    return { playlists: res.data.data };

  } catch (err: any) {
    showMessageStc(err, "error");

    return { playlists: [] };
  }
}

const UserPlaylistsPage: React.FC = () => {
  const dispatch = useDispatch();

  const revalidator = useRevalidator();

  const { playlists } = useLoaderData() as { playlists: PlaylistEntity[] };
  const [currPlaylist, setCurrPlaylist] = useState<PlaylistEntity | null>(null);
  const [playlistSongs, setPlaylistSongs] = useState<SongEntity[]>([]);
  const [creatingPlaylist, setCreatingPlaylist] = useState<boolean>(false);

  const [newPlaylistTitle, setNewPlaylistTitle] = useState<string>("")
  const [newPlaylistDescription, setNewPlaylistDescription] = useState<string>("")
  const [newPlaylistPublic, setNewPlaylistPublic] = useState<boolean>(true)

  // const [artistAlbums, setArtistAlbums] = useState<AlbumInfo[]>([]);

  const authState = useSelector((state: RootState) => state.auth);

  const showPlaylistInfo = async (item: PlaylistEntity) => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Playlists/${item.id}`);
      console.log(res.data.data);
      setCurrPlaylist(res.data.data);
    } catch (err: any) {
      showMessageStc(err, "error");
    }

    try {
      const res = await axiosInstance.get(`Songs/by-playlist/${item.id}`);
      console.log(res.data.data);
      setPlaylistSongs(res.data.data);
    } catch (err: any) {
      showMessageStc(err, "error");
    }

  }

  const handleCheckbox = (e: any) => {
    setNewPlaylistPublic(e.target.checked);
  }

  const handlePlaylistTitle = (e: any) => {
    setNewPlaylistTitle(e.target.value);
  }

  const handlePlaylistDescription = (e: any) => {
    setNewPlaylistDescription(e.target.value);
  }

  const handleCreatePlaylist = async () => {

    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    const data = {
      id: 0,
      title: newPlaylistTitle,
      description: newPlaylistDescription,
      isPublic: newPlaylistPublic,
      userId: authState.userId,
      user: null
    }

    try {
      const res = await axiosInstance.post(`Playlists`, data);
      console.log(res);
      setCreatingPlaylist(false);
      revalidator.revalidate();
    } catch (err: any) {
      showMessageStc(err, "error");
    }
  }

  const handleRemoveSongFromPlaylist = async (playlistId: any, songId: number) => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.delete(`Playlists/${playlistId}/songs/${songId}`);
      revalidator.revalidate();
      setPlaylistSongs(playlistSongs.filter(song => song.id !== songId))
    } catch (err: any) {
      showMessageStc(err, "error");
    }
  }

  const handleDeletePlaylist = async () => {
    if (currPlaylist == null) {
      return;
    }
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.delete(`Playlists/${currPlaylist.id}/all-songs`);
    } catch (err: any) {
      showMessageStc(err, "error");
    }

    try {
      const res = await axiosInstance.delete(`Playlists/${currPlaylist.id}`);
    } catch (err: any) {
      showMessageStc(err, "error");
    }

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
          align="center"
          gap={20}
          vertical>
          <Title level={2}>
            My Playlists
          </Title>
          <Card>
            <List
              itemLayout="horizontal"
              dataSource={playlists}
              style={{ width: "300px" }}
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
                  <Button style={{ margin: "0px 0px 0px 10px" }} onClick={() => showPlaylistInfo(item)}>
                    <Text>Show</Text>
                  </Button>
                </List.Item>
              )}
            />
          </Card>
          <Button type="primary" onClick={() => setCreatingPlaylist(!creatingPlaylist)}>Create Playlist</Button>
          {creatingPlaylist == true ? (
            <Card title="Create Playlist">
              <Flex vertical gap={16} style={{ width: "100%" }}>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: "100px" }}>
                    <Text style={{ fontSize: "16px" }}>Title:</Text>
                  </Col>
                  <Col style={{ width: "200px" }}>
                    <Input onChange={handlePlaylistTitle} value={newPlaylistTitle} />
                  </Col>
                </Row>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: "100px" }}>
                    <Text style={{ fontSize: "16px" }}>Description:</Text>
                  </Col>
                  <Col style={{ width: "200px" }}>
                    <Input onChange={handlePlaylistDescription} value={newPlaylistDescription} />
                  </Col>
                </Row>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: "100px" }}>
                    <Text style={{ fontSize: "16px" }}>Is public:</Text>
                  </Col>
                  <Col style={{ width: "200px" }}>
                    <Checkbox onChange={handleCheckbox} value={newPlaylistPublic}></Checkbox>
                  </Col>
                </Row>
                <Row gutter={[16, 16]}>
                  <Col style={{ width: "100px" }}>
                  </Col>
                  <Col style={{ width: "200px" }}>
                    <Button onClick={handleCreatePlaylist}>Create</Button>
                  </Col>
                </Row>
              </Flex>
            </Card>
          ) : null}
        </Flex>
        {currPlaylist != null ? (
          <Flex gap={40} style={{ width: "900px" }}>
            <Flex vertical align='start' gap={20}>
              <Card style={{ width: "400px" }}>
                <Title level={1} style={{ marginTop: "5px" }}>
                  {currPlaylist.title}
                </Title>
                {currPlaylist.isPublic ? <Tag color="green">Public</Tag> : <Tag color="orange">Private</Tag>}
                <div style={{margin: "5px 0px"}}>
                  <Text style={{ fontSize: "18px" }}>{"Description: "}</Text>
                  <Text>{currPlaylist.description}</Text>
                </div>
                <div style={{margin: "10px 0px"}}>
                  <Button danger onClick={() => handleDeletePlaylist()}>
                    Delete
                  </Button>
                </div>
              </Card>
              <Card style={{ width: "400px" }}>
                <List
                  itemLayout="horizontal"
                  dataSource={playlistSongs}
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
                        <Button style={{marginLeft: "160px"}} danger onClick={() => handleRemoveSongFromPlaylist(currPlaylist.id, item.id)} >Remove</Button>
                      </Flex>
                    </List.Item>
                  )}
                />
              </Card>
            </Flex>
          </Flex>) : (
          <Flex style={{ width: "900px" }}>
            <Title level={2}>Select Playlist</Title>
          </Flex>
        )}
      </Flex>
    </>
  );
};

export default UserPlaylistsPage;
