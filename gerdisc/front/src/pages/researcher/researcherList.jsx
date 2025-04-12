import React, { useEffect, useState } from "react";
import '../../styles/researcherList.scss';
import Table from "../../components/Table/table"
import { getResearchers } from "../../api/researcher_service"
import { useNavigate } from "react-router"
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import PageContainer from "../../components/PageContainer";

export default function ResearcherList() {
    const navigate = useNavigate()
    const [name,] = useState(localStorage.getItem('name'))
    const [role, setRole] = useState(localStorage.getItem('role'))
    const [isLoading, setIsLoading] = useState(true)
    const [researchers, setResearchers] = useState([])

    useEffect(() => {
        const roles = ['Administrator']
        const token = localStorage.getItem('token')
        try {
            const decoded = jwt_decode(token)
            if (!roles.includes(decoded.role)) {
                navigate('/')
            }
            setRole(decoded.role)
        } catch (error) {
            navigate('/login')
        }
    }, [setRole, navigate, role]);

    useEffect(() => {
        getResearchers()
            .then(result => {
                let mapped = []
                if (result !== null && result !== undefined) {
                    mapped = result.map((researcher) => {
                        return {
                            Id: researcher.id,
                            Nome: `${researcher?.firstName} ${researcher?.lastName}`,
                            "E-mail": researcher.email,
                            "Instituição": researcher.institution,
                        }
                    })
                }
                setResearchers(mapped)
                setIsLoading(false)
            })
    }, [setResearchers, setIsLoading])

    return (
        <PageContainer name={name} isLoading={isLoading}>
            <div className="researcherBar">
                <div className="left-bar">
                    <div>
                        <img className="filtered" src="researcher.png" alt="A logo representing researchers" height={"100rem"} />
                    </div>
                    <div className="title">Pesquisadores</div>
                </div>
                <div className="right-bar">
                    <div className="search">
                        <input type="search" name="search" id="search" />
                        <i className="fas fa-" />
                    </div>
                    <div className="create-button">
                        <button onClick={() => navigate('/researches/add')}>Novo Pesquisador</button>
                    </div>
                </div>
            </div>
            <BackButton />
            <Table data={researchers} />
        </PageContainer>
    )
}
