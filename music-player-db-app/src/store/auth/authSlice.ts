import { createSlice, PayloadAction } from "@reduxjs/toolkit";


export interface AuthState {
  isAuthenticated: boolean;
  userId: number;
  role: number;
}

const initialState: AuthState = {
  isAuthenticated: false,
  userId: 0,
  role: 0,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setAuthState: (state, action: PayloadAction<AuthState>) => {
      state.isAuthenticated = action.payload.isAuthenticated;
      state.userId = action.payload.userId;
      state.role = action.payload.role;
    },
    logout: (state) => {
      state.isAuthenticated = false;
      state.userId = 0;
      state.role = 0;
    },
  }
});

export const { setAuthState, logout } = authSlice.actions;

export default authSlice.reducer;