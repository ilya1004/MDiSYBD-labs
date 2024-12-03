import { Form, Button, Input, Card, Flex, Typography } from "antd";
import React, { useState } from "react";
import { Link, useNavigate, useOutletContext } from "react-router-dom";
import axios from "axios";
import { useSelector } from "react-redux";
import { showMessageStc } from "../../services/ResponseErrorHandler";
import { ADMIN_ROLE_ID, ARTIST_ROLE_ID, BASE_URL, USER_ROLE_ID } from "../../store/constants";
import { RootState } from "../../store/store";

const { Title, Text } = Typography;

const LoginPage: React.FC = () => {

  const authState = useSelector((state: RootState) => state.auth);

  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();


  const handleChangeEmail = (e: any) => {
    setLogin(e.target.value);
  };

  const handleChangePassword = (e: any) => {
    setPassword(e.target.value);
  };

  const loginUser = async () => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });
    try {
      const res = await axiosInstance.post(`Users/login`, {
        login: login.trim(),
        password: password.trim(),
      });
      let roleId = res.data.roleId;
      console.log(res.data);
      switch (roleId) {
        case USER_ROLE_ID:
          navigate("/", { replace: true });
          break;

        case ARTIST_ROLE_ID:
          navigate("/", { replace: true });
          break;

        case ADMIN_ROLE_ID:
          navigate("/", { replace: true });
          break;

        default:
          break;
      }

      
    } catch (err: any) {
      if (err.response.status === 401) {
        showMessageStc(err.response.data.message, "error");
      } else {
        showMessageStc("Произошла неизвестная ошибка.", "error");
      }
      console.error(err);
    }
  };

  const loginModerator = async () => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });
    try {
      const res = await axiosInstance.post(`Moderators/Login`, {
        login: login.trim(),
        password: password.trim(),
      });
      //   var date = new Date();
      //   date.setTime(date.getTime() + 24 * 60 * 60 * 1000);
      //   var expires = date.toUTCString();
      //   document.cookie = `login-cookie=${"moderator"}; expires=${expires}; path=/; SameSite=Lax`;
      navigate("/moderator/profile", { replace: true });
    } catch (err: any) {
      if (err.response.status === 401) {
        showMessageStc("Введена неверная электронная почта или пароль", "error");
      } else {
        showMessageStc("Произошла неизвестная ошибка.", "error");
      }
      console.error(err);
    }
  };

  const loginAdmin = async () => {
    const axiosInstance = axios.create({
      baseURL: BASE_URL,
      withCredentials: true,
    });
    try {
      const res = await axiosInstance.post(`Admin/Login`, {
        login: login.trim(),
        password: password.trim(),
      });
      //   var date = new Date();
      //   date.setTime(date.getTime() + 24 * 60 * 60 * 1000);
      //   var expires = date.toUTCString();
      //   document.cookie = `login-cookie=${"admin"}; expires=${expires}; path=/; SameSite=Lax`;
      navigate("/admin/profile", { replace: true });
    } catch (err: any) {
      if (err.response.status === 401) {
        showMessageStc("Введена неверная электронная почта или пароль", "error");
      } else {
        showMessageStc("Произошла неизвестная ошибка.", "error");
      }
      console.error(err);
    }
  };

  const handleSubmit = () => {
    loginUser();
    // if (email.trim().startsWith("#admin#") && !email.trim().includes("@")) {
    //   loginAdmin();
    // } else if (
    //   email.trim().startsWith("#moderator#") &&
    //   !email.trim().includes("@")
    // ) {
    //   loginModerator();
    // } else {
    //   loginUser();
    // }
  };

  return (
    <>
      <Flex
        justify="center"
        align="flex-start"
        style={{
          minHeight: "80vh",
          height: "fit-content",
        }}
      >
        <Card
          style={{
            marginTop: "100px",
            width: "450px",
          }}
          title={
            <Title style={{ margin: "0px" }} level={4}>
              Введите свои данные
            </Title>
          }
        >
          <Form
            labelCol={{ span: 10, offset: 0 }}
            wrapperCol={{ offset: 0 }}
            layout="horizontal"
            onFinish={handleSubmit}
          >
            <Form.Item
              label="Электронная почта:"
              name="email"
              rules={[
                {
                  required: true,
                  message: "Введите электронную почту!",
                },
              ]}
            >
              <Input
                // type="email"
                name="email"
                onChange={handleChangeEmail}
                value={login}
              />
            </Form.Item>

            <Form.Item
              label="Пароль"
              name="password"
              rules={[
                {
                  required: true,
                  message: "Введите пароль!",
                },
              ]}
            >
              <Input
                name="password"
                onChange={handleChangePassword}
                value={password}
              />
            </Form.Item>

            <Form.Item>
              <Flex justify="center" align="center">
                <Button
                  type="primary"
                  htmlType="submit"
                  style={{
                    margin: "0px",
                    width: "75px",
                  }}
                >
                  Войти
                </Button>
              </Flex>
            </Form.Item>
          </Form>
          <Flex align="center" justify="center">
            <Text style={{ marginRight: "5px" }}>Еще нет аккаунта?</Text>
            <Link to="/register">Зарегистрироваться</Link>
          </Flex>
        </Card>
      </Flex>
    </>
  );
}

export default LoginPage;