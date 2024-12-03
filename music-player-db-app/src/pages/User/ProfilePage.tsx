import React, { useEffect } from 'react';
import { LoaderFunction, useLoaderData } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { setAuthState, AuthState } from '../../store/auth/authSlice';
import axios from 'axios';
import { BASE_URL } from '../../store/constants';
import { showMessageStc } from '../../services/ResponseErrorHandler';
import { Flex, Button, Image, Card, Typography } from "antd";
import AvatarImage from "../../assets/avatar-img.jpg";
import {
  EnvironmentOutlined,
  PhoneOutlined,
  IdcardOutlined,
  MailOutlined,
  UploadOutlined,
} from "@ant-design/icons";
import { RootState } from '../../store/store';

const { Text, Title } = Typography

interface UserDetail {
  id: number
  login: string,
  passwordHash: string,
  isBlocked: boolean,
  roleId: number,
  userInfoId: number | null,
  artistInfoId: number | null,
  nickname: string,
  about: string | null,
}

export const loaderProfilePage: LoaderFunction = async (): Promise<UserDetail | null> => {
  const axiosInstance = axios.create({
    baseURL: BASE_URL,
    withCredentials: true,
  });

  try {
    const res = await axiosInstance.get(`Users/current-user-full-info`);
    console.log(res.data);
    return res.data;

  } catch (err: any) {
    showMessageStc("Ошибка при загрузке состояния аутентификации: " + err, "error");

    return null;
  }
}

const UserProfilePage: React.FC = () => {
  const dispatch = useDispatch();
  // const authStateLoader = useLoaderData() as AuthState;
  const userData = useLoaderData() as UserDetail;

  const authState = useSelector((state: RootState) => state.auth);

  console.log(authState);

  // const avatarUrl = ;

  // useEffect(() => {
  //   dispatch(setAuthState(authStateLoader));
  // }, [authStateLoader, dispatch]);

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
        <Flex
          gap={20}
          justify="flex-start"
          align="center"
          vertical
          style={{
            width: "35%",
          }}
        >
          <Image
            style={{
              maxHeight: "400px",
              maxWidth: "300px",
              borderRadius: "30px",
            }}
            src={AvatarImage}
            preview={false}
          />
          {/* <Flex gap={15} vertical align="center">
            <Flex gap={15}>
              <Button onClick={handleEditProfile}>Редактировать профиль</Button>
              <Button onClick={handleSettingsProfile}>
                Настройки аккаунта
              </Button>
            </Flex>
          </Flex> */}
        </Flex>
        {/* <Flex
          gap={20}
          vertical
          justify="flex-start"
          align="center"
          style={{
            width: "65%",
            margin: "0px 0px 10px 0px",
          }}
        > */}
        <Card
          style={{
            width: "800px",
            padding: "0px",
          }}
        >
          <Flex vertical gap={15} align="flex-start">
            <Title
              level={2}
              style={{ margin: "0px 0px 0px 0px" }}
            >
              {userData.nickname}
            </Title>
            <Flex gap={7} style={{ margin: "0px 0px 0px 3px" }}>
              <MailOutlined style={{ margin: "4px 0px 0px 0px" }} />
              <Text>{userData.login}</Text>
            </Flex>
            <div>
              <Text>{"Description: "}</Text>
              <Text>{userData.about}</Text>
            </div>
          </Flex>
        </Card>
      </Flex>
    </>
  );
};

export default UserProfilePage;
