
declare global {
  import React, { useState } from 'react';
  import { useDispatch, useSelector } from 'react-redux';
  import axios from 'axios';
  import { BASE_URL } from './store/constants';
  import { showMessageStc } from "./services/ResponseErrorHandler";
  import { AuthState } from "./store/auth/authSlice";
  import { Card, Typography, Flex, Button } from "antd";
}

export {};
