import './styles/App.css';
import { createBrowserRouter } from 'react-router-dom';
import ProtectedRoute from './components/ProtectedRoute';
import ArtistHomePage from './pages/Artist/ArtistHomePage';
import AdminHomePage from './pages/Admin/AdminHomePage';
import LoginPage from './pages/Auth/LoginPage';
import NotFoundPage from './pages/NotFoundPage';
import { USER_ROLE_ID, ARTIST_ROLE_ID, ADMIN_ROLE_ID } from './store/constants';
import UserApp, { appRoleCheckLoader } from './pages/UserApp';
import HomePage, { loaderHomePage } from './pages/Common/HomePage';
import UserProfilePage, { loaderProfilePage } from './pages/User/ProfilePage';
import ArtistsPage, { loaderArtistsPage } from './pages/Common/ArtistsPage';
import UserFavouriteSongsPage, { loaderFavSongsPage } from './pages/User/FavouriteSongsPage';
import SearchPage from './pages/Common/SearchPage';
import UserPlaylistsPage, { loaderPlaylistsPage } from './pages/User/PlaylistsPage';

export const router = createBrowserRouter([
  {
    path: "/",
    element: <UserApp />,
    loader: appRoleCheckLoader,
    errorElement: <NotFoundPage />,
    children: [
      {
        path: "/",
        loader: loaderHomePage,
        element: (
          <HomePage />
        )
      },
      {
        path: "/search",
        // loader: ,
        element: (
          <SearchPage />
        )
      },
      {
        path: "/artists",
        loader: loaderArtistsPage,
        element: (
          <ArtistsPage />
        )
      },
      {
        path: "/user/favourite-songs",
        loader: loaderFavSongsPage,
        element: (
          <UserFavouriteSongsPage />
        )
      },
      {
        path: "/user/playlists",
        loader: loaderPlaylistsPage,
        element: (
          <UserPlaylistsPage />
        )
      },
      {
        path: '/user/profile',
        loader: loaderProfilePage,
        element: (
          <UserProfilePage />
        ),
      },
    ]
  },
  {
    path: '/artist/*',
    element: (
      // <ProtectedRoute allowedRoles={[ARTIST_ROLE_ID]}>
      <ArtistHomePage />
      // </ProtectedRoute>
    ),
  },
  {
    path: '/admin/*',
    element: (
      <ProtectedRoute allowedRoles={[ADMIN_ROLE_ID]}>
        <AdminHomePage />
      </ProtectedRoute>
    ),
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    path: '*',
    element: <NotFoundPage />,
  },
]);

// const App: React.FC = () => {
//   return <RouterProvider router={router} />;
// };

// export default App;
