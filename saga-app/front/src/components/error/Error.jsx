import React from 'react';
import './error.scss';

const ErrorPage = ({ errorMessage='Error while processing request' }) => {
  return (
    <div className="error-page" style={{backgroundImage: `url(/saga/warning-rafiki.svg)`, backgroundPosition:'center', backgroundSize:'50%', backgroundRepeat:'no-repeat',width:'100%'}}>
      <div className="error-content">
        <p className="error-message">{errorMessage}</p>
      </div>
    </div>
  );
};

export default ErrorPage;
