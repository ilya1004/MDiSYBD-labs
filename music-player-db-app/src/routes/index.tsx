// routes/index.tsx
import React from 'react';
import { Routes, Route } from 'react-router-dom';
import ProtectedRoute from '../components/ProtectedRoute';
import UserHomePage from '../pages/User/ProfilePage';
import ArtistHomePage from '../pages/Artist/ArtistHomePage';
import AdminHomePage from '../pages/Admin/AdminHomePage';
import LoginPage from '../pages/Auth/LoginPage';
import NotFoundPage from '../pages/NotFoundPage'
import { USER_ROLE_ID, ARTIST_ROLE_ID, ADMIN_ROLE_ID } from '../store/constants';
// import UnauthorizedPage from '../pages/Unauthorized';

// interface AppRoutesProps {
//   currUserRole: number | any;
//   isAuthenticated: boolean | any;
// }

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      {/* <Route path="/unauthorized" element={<UnauthorizedPage />} /> */}

      <Route
        path="/user/*"
        element={
          <ProtectedRoute allowedRoles={[USER_ROLE_ID]}>
            <UserHomePage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/artist/*"
        element={
          <ProtectedRoute allowedRoles={[ARTIST_ROLE_ID]}>
            <ArtistHomePage />
          </ProtectedRoute>
        }
      />
      <Route
        path="/admin/*"
        element={
          <ProtectedRoute allowedRoles={[ADMIN_ROLE_ID]}>
            <AdminHomePage />
          </ProtectedRoute>
        }
      />

      <Route path="*" element={<NotFoundPage />} />
    </Routes>

  );
};

export default AppRoutes;
