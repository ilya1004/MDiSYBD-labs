import React, { ChangeEvent, ReactElement, useEffect, useState } from 'react';
import { LoaderFunction, useLoaderData, useRevalidator } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { setAuthState, AuthState } from '../../store/auth/authSlice';
import axios from 'axios';
import { BASE_URL } from '../../store/constants';
import { showMessageStc } from '../../services/ResponseErrorHandler';
import { Flex, Button, Image, Card, Typography, Input, Space, TableProps, Table, Tag, List } from "antd";
import { SearchOutlined } from '@ant-design/icons';
import { RootState } from '../../store/store';
import { AlbumEntity, ArtistDetailDTO, PlaylistEntity, SongByPlaylistDTO, SongEntity } from '../../utils/types';

const { Search } = Input;

const { Text, Title } = Typography


interface SearchItemDTO {
  id: number,
  name: string,
  type: string,
}

const SearchPage: React.FC = () => {

  const revalidator = useRevalidator();

  const [query, setQuery] = useState<string>("");
  const [searchItems, setSearchItems] = useState<SearchItemDTO[]>([]);
  const [selectedItem, setSelectedItem] = useState<SearchItemDTO | null>(null);
  const [component, setComponent] = useState<ReactElement | null>();

  const authState = useSelector((state: RootState) => state.auth);

  const makeSearch = async (e: ChangeEvent<HTMLInputElement>) => {
    let query = e.target.value;

    setQuery(query);

    if (query == null || query.length == 0) {
      setSearchItems([]);
      return;
    }

    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Search?query=${query}`);
      console.log(res.data.data);
      setSearchItems(res.data.data);
    } catch (err: any) {
      showMessageStc(err, "error");
      return null;
    }
  }

  const renderType = (item: SearchItemDTO) => {
    if (item.type == "song") {
      return (
        <Tag color="blue" style={{ fontSize: "14px", padding: "3px 5px" }}>Song</Tag>
      );
    }
    else if (item.type == "artist") {
      return (
        <Tag color="green" style={{ fontSize: "14px", padding: "3px 5px" }}>Artist</Tag>
      );
    }
    else if (item.type == "playlist") {
      return (
        <Tag color="orange" style={{ fontSize: "14px", padding: "3px 5px" }}>Playlist</Tag>
      );
    }
    else if (item.type == "album") {
      return (
        <Tag color="purple" style={{ fontSize: "14px", padding: "3px 5px" }}>Album</Tag>
      );
    }
    else {
      return (
        <Tag>No data</Tag>
      )
    }

  }

  const searchColumns: TableProps<SearchItemDTO>['columns'] = [
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
      title: "Name",
      dataIndex: "name",
      key: "name",
      width: "300px",
      render: (title: string) => <Text style={{ fontSize: "16px" }}>{title}</Text>,
    },
    {
      title: "Type",
      dataIndex: "type",
      key: "type",
      width: "200px",
      render: (_: any, item: SearchItemDTO, index: number) => renderType(item)
    },
    {
      title: "Actions",
      dataIndex: "",
      key: "",
      width: "200px",
      render: (_: any, item: SearchItemDTO, index: number) => (
        <Button onClick={() => renderSelectedItem(item)}>More</Button>
      )
    }
  ]

  const getSong = async (id: number): Promise<SongEntity | null> => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = axiosInstance.get(`Songs/${id}`);
      return (await res).data.data;
    } catch (err: any) {
      showMessageStc(err, "error");
      return null;
    }
  }

  const getAlbum = async (id: number): Promise<AlbumEntity | null> => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Albums/${id}`);
      return res.data.data;
    } catch (err: any) {
      showMessageStc(err, "error");
      return null;
    }
  }

  const getArtist = async (id: number): Promise<ArtistDetailDTO | null> => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Artists/${id}`);
      return res.data.data;
    } catch (err: any) {
      showMessageStc(err, "error");
      return null;
    }
  }

  const getPlaylist = async (id: number): Promise<PlaylistEntity | null> => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Playlists/${id}`);
      return res.data.data;
    } catch (err: any) {
      showMessageStc(err, "error");
      return null;
    }
  }

  const getSongsByPlaylist = async (id: number): Promise<SongByPlaylistDTO[]> => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Songs/by-playlist/${id}`);
      return res.data.data;
    } catch (err: any) {
      showMessageStc(err, "error");
      return [];
    }
  }

  const getSongsByAlbum = async (id: number): Promise<SongByPlaylistDTO[]> => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });

    try {
      const res = await axiosInstance.get(`Songs/by-album/${id}`);
      return res.data.data;
    } catch (err: any) {
      showMessageStc(err, "error");
      return [];
    }
  }

  const renderSelectedItem = async (item: SearchItemDTO) => {
    setSelectedItem(item);
    if (item.type == "song") {

      const song: SongEntity | null = await getSong(item.id);

      if (song == null) {
        return;
      }

      setComponent(
        <Card style={{ width: "300px", marginTop: "100px" }}>
          <Title level={2}>{song?.title}</Title>
          <div>
            <Text>{"Year: "}</Text>
            <Text>{song?.releaseYear}</Text>
          </div>
          <div>
            <Text>{"Duration: "}</Text>
            <Text>{`${Math.floor(song.durationSecs / 60)}:${((song?.durationSecs % 60 < 10) ? (song.durationSecs % 60).toString() + "0" : song.durationSecs % 60)}`}</Text>
          </div>
        </Card>
      )
    }
    else if (item.type == "artist") {

      const artist: ArtistDetailDTO | null = await getArtist(item.id);

      if (artist == null) {
        return;
      }

      setComponent(
        <Card style={{ width: "300px", marginTop: "100px" }}>
          <Title level={2}>{artist?.name}</Title>
          <div>
            <Text>{"Country: "}</Text>
            <Text>{artist?.country}</Text>
          </div>
          <div>
            <Text>{"Birthday: "}</Text>
            <Text>{artist?.birthday.substring(0, 10)}</Text>
          </div>
          <div>
            <Text>{"Description: "}</Text>
            <Text>{artist?.description}</Text>
          </div>
        </Card>
      )
    }
    else if (item.type == "playlist") {

      const playlist: PlaylistEntity | null = await getPlaylist(item.id);

      if (playlist == null) {
        return;
      }

      let songs: SongByPlaylistDTO[] = await getSongsByPlaylist(playlist.id);

      setComponent(
        <Card style={{ width: "300px", marginTop: "100px" }}>
          <Title level={2}>{playlist?.title}</Title>
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
      )
    }
    else if (item.type == "album") {

      const album: AlbumEntity | null = await getAlbum(item.id);

      if (album == null) {
        return;
      }

      console.log(album);

      const songs: SongByPlaylistDTO[] = await getSongsByAlbum(album.id);

      setComponent(
        <Card style={{ width: "300px", marginTop: "100px" }}>
          <Title level={2}>{album.title}</Title>
          <div>
            <Text>{"Release year: "}</Text>
            <Text>{album.releaseYear}</Text>
          </div>
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
      )
    }
    else {
      return (
        <Text>No data</Text>
      )
    }
  }

  return (
    <>
      <Flex
        justify="center"
        align="flex-start"
        style={{
          margin: "10px 15px",
          minHeight: "80vh",
        }}
        gap={30}
      >
        <Flex vertical align="center" gap={30}>
          <Title>
            Search
          </Title>
          <Space.Compact size="large">
            <Input style={{ width: "300px" }} addonBefore={<SearchOutlined />} placeholder="What you want to find?" value={query} onChange={makeSearch} />
          </Space.Compact>
          <Table
            dataSource={searchItems}
            columns={searchColumns}
          />
        </Flex>
        <Flex>
          {selectedItem == null ?
            (<Title style={{ margin: "200px 0px 0px 0px" }}>Select Item</Title>) : (
              component
            )}
        </Flex>
      </Flex>
    </>
  );
};

export default SearchPage;
