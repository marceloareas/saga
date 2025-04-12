import React from 'react'
import { useNavigate } from "react-router"
import '../styles/header.scss'

export default function Header({ name }) {
    const navigate = useNavigate()
    return (
        <>
            <div className={"header"}>
                <div className={"appName"} style={{cursor:'pointer'}} onClick={()=> navigate('/')}>
                    <div className={"bleap"}> </div>
                    <span>SAGA</span>
                </div>
                <div className={"headerOptions"}>
                    <div>Olá, <strong>{name}</strong></div>
                </div>
            </div>
            <div className={"headerBreak"}><span></span></div>
            <br></br>
        </>
    );
}