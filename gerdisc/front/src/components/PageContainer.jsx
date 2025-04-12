import React from 'react';
import Footer from './footer';
import Header from './header';
import Spinner from './spinner';
import '../styles/form.scss';
import '../styles/pageContainer.scss';
import { useEffect} from "react";
import { useNavigate} from "react-router"
import jwt_decode from "jwt-decode";

export default function PageContainer({ children, name, isLoading }) {
    const navigate = useNavigate()
    useEffect(() => {
        const token = localStorage.getItem('token')
        try {
            const decoded = jwt_decode(token)
            if (decoded.exp < (Date.now()/1000)) {
                navigate('/login')
            }
        } catch (error) {
            navigate('/login')
        }
    }, [navigate,isLoading]);

    return (<div className="container">
        <main className="main">
            <div className="body">
                <Header name={name} />
                {!isLoading && children}
                {isLoading && <Spinner />}
                <Footer></Footer>
            </div>
        </main>
    </div>
    )

}