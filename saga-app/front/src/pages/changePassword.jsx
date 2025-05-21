import React, { useState, useEffect } from 'react';
import '../styles/login.scss';
import { useLocation, useNavigate } from 'react-router-dom';
import { ResetPassword as resetPassword } from '../api/user_service';

export default function ResetPassword() {
  const navigate = useNavigate();
  const location = useLocation();
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [error, setError] = useState(undefined);
  const token = new URLSearchParams(location.search).get('token');

  useEffect(() => {
    // Redirect to login if token is not provided in the URL
    if (!token) {
      navigate('/login');
    }
  }, [token, navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(undefined);

    if (password.length < 8 || !/\d/.test(password) || !/[a-zA-Z]/.test(password)) {
      setError('Password must be at least 8 characters long and contain letters and numbers.');
      return;
    }

    if (password !== repeatPassword) {
      setError('Passwords do not match.');
      return;
    }

    resetPassword({ token, password })
      .then((response) => {
        if (response.status === 200) {
          navigate('/login');
        } else {
          setError('Failed to reset password');
        }
      })
      .catch((error) => {
        setError('Failed to reset password');
      });
  };

  return (
    <>
      <main className="login">
        <div className={'body'}>
          <div className={'header'}>
            <div className="app-name">SAGA</div>
            <div className={'headerOptions'}>
              <div style={{ marginRight: '2rem' }}>Sobre</div>
              <div>Contato</div>
            </div>
          </div>
          <div className={'form'}>
            <p>Redefinir senha</p>
            <label htmlFor="password">Nova Senha</label>
            <input
              type="password"
              id="password"
              placeholder="Digite sua nova senha"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              pattern="(?=.*\d)(?=.*[a-zA-Z]).{8,}"
              required
            />
            <label htmlFor="repeat-password">Repetir Senha</label>
            <input
              type="password"
              id="repeat-password"
              placeholder="Digite novamente sua nova senha"
              value={repeatPassword}
              onChange={(e) => setRepeatPassword(e.target.value)}
              required
            />
            <input type="submit" id="submit" value={'Resetar Senha'} onClick={handleSubmit} />
            {error && <p>{error}</p>}
          </div>
        </div>
      </main>
    </>
  );
}
