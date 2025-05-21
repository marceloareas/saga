import { useParams, useNavigate } from "react-router";
import { useEffect, useState } from "react";
import "../../styles/profile.scss";
import { getProfessorById, deleteProfessor } from "../../api/professor_service";
import BackButton from "../../components/BackButton";
import ErrorPage from "../../components/error/Error";
import jwt_decode from "jwt-decode";
import PageContainer from "../../components/PageContainer";

export default function ProfessorProfile() {
    const { id } = useParams()
    const [user, setUser] = useState(undefined)
    const [error, setError] = useState(false)
    const [role, setRole] = useState()

    const navigate = useNavigate()
    const [name,] = useState(localStorage.getItem('name'))
    const [isLoading, setIsLoading] = useState(true)

    console.log(id)
    console.log('user', user)

    const handleDeleteProfessor = async () => {
            deleteProfessor(id)
                .then((user) => navigate("/professors"))
                .catch(error => { setError('Unable to create user'); })
    }

    useEffect(() => {
        const token = localStorage.getItem('token')
        try {
            const decoded = jwt_decode(token)
            setRole(decoded.role)
        } catch (error) {
            navigate('/login', { replace: true })
        }
    }, [navigate]);

    useEffect(() => {
        getProfessorById(id)
            .then(user => {
                console.log(user)
                setUser(user);
                setIsLoading(false);
            })
            .catch(error => setError(true))
    }, [id, setUser, setError, setIsLoading]);

    return (
        <PageContainer name={name} isLoading={isLoading}>
            {!error && <div style={
                { display: "flex", flexDirection: "column", flexWrap: 'wrap' }
            }>
                <div className="bar">
                    <BackButton />
                    {role === "Administrator" && <div className="options">
                        <input type={'button'} className="option" value={'Editar Professor'} onClick={(e) => navigate('edit')} />
                        <input type={'button'} className="option" value={'Excluir Professor'} onClick={handleDeleteProfessor} />
                    </div>}
                </div>
                {!isLoading && <>
                    <div className="card-label">Perfil Professor</div>
                    <div className="studentCard">
                        <p data-label="Nome">{`${user.firstName} ${user.lastName}`}</p>
                        <p data-label="Email">{user.email}</p>
                        <p data-label="Proficiencia">{user.proficiency}</p>
                        <p data-label="Siape">{user.siape}</p>
                        <p data-label="Cpf">{user.cpf}</p>
                    </div>
                </>}
            </div>
            }
            {error && <ErrorPage />}

        </PageContainer>
    );
}
