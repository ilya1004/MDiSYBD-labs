import React from 'react';

const UnauthorizedPage: React.FC = () => {
  return (
    <div>
      <h1>Доступ запрещён</h1>
      <p>У вас нет прав для просмотра этой страницы.</p>
    </div>
  );
};

export default UnauthorizedPage;
