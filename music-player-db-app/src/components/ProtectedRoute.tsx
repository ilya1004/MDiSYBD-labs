// components/ProtectedRoute.tsx
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Navigate } from 'react-router-dom';
import { RootState } from '../store/store';

interface ProtectedRouteProps {
  children: React.ReactNode;
  allowedRoles: number[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, allowedRoles }) => {
  
  const authState = useSelector((state: RootState) => state.auth);

  // useEffect(() => {
  //   const fetchRole = async () => {
  //     const fetchedRole = await getUserRole();
  //     setRole(fetchedRole);
  //     setIsLoading(false);
  //   };

  //   fetchRole();
  // }, []);

  console.log(authState);

  if (!authState.role) {
    return <Navigate to="/login" replace />; // Неавторизованных перенаправляем на логин
  }

  return <>{children}</>;
};

export default ProtectedRoute;
