import { useParams } from "react-router";
import { useEffect, useState } from "react";
import "../../styles/profile.scss";
import { useNavigate } from "react-router";
import { getStudentById } from "../../api/student_service";
import BackButton from "../../components/BackButton";
import ErrorPage from "../../components/error/Error";
import jwt_decode from "jwt-decode";
import PageContainer from "../../components/PageContainer";


export default function StudentProfile() {
    const { id } = useParams()
    const [student, setStudent] = useState(undefined)
    const [error, setError] = useState(false)
    const [role, setRole] = useState()

    const navigate = useNavigate()
    const [name,] = useState(localStorage.getItem('name'))
    const [isLoading, setIsLoading] = useState(true)

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
        getStudentById(id)
            .then(student => {
                console.log(student)
                setStudent(student);
                setIsLoading(false);
            })
            .catch(error => setError(true))
    }, [id, setStudent, setError, setIsLoading]);

    return (
        <PageContainer name={name} isLoading={isLoading}>
            {!error && <div style={
                { display: "flex", flexDirection: "column", flexWrap: 'wrap' }
            }>
                <div className="bar">
                    <BackButton />
                    {role === "Administrator" && <div className="options">
                        <input type={'button'} className="option" value={"Criar Dissertação"} onClick={(e) => navigate(`researches/add`)} />
                        <input type={'button'} className="option" value={'Prorrogação'} onClick={(e) => navigate('extensions/add')} />
                        <input type={'button'} className="option" value={'Editar Estudante'} onClick={(e) => navigate('edit')} />
                    </div>}
                </div>
                {!isLoading && <>
                    <div className="card-label">Perfil estudante</div>
                    <div className="studentCard">
                        <p data-label="Nome">{`${student.firstName} ${student.lastName}`}</p>
                        <p data-label="Email">{student.email}</p>
                        <p data-label="Proficiencia">{student.proficiency}</p>
                        <p data-label="Matricula">{student.registration}</p>
                        <p data-label="Projeto de pesquisa">{student.project?.name}</p>
                        <p data-label="Data de ingresso">{student.entryDate}</p>
                        <p data-label="Previsao de defesa">{student.projectDefenceDate}</p>
                        <p data-label="Previsao de qualificação">{student.projectQualificationDate}</p>
                        <p data-label="Bolsa">{student.scholarship}</p>
                    </div>
                </>}
            </div>
            }
            {error && <ErrorPage />}

        </PageContainer>
    );
}
