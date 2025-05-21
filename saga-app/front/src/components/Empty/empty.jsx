import React from 'react';
import './empty.scss'

const Empty = ({ message }) => {
  return (
    <div className='empty-entity-message'>
      <p className='empty-entity-message-text'>{message}</p>
    </div>
  );
};

export default Empty;
