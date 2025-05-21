import { useEffect, useState } from "react"
import '../../styles/studentList.scss'
import Table from "../../components/Table/table"
import { getStudents } from "../../api/student_service"
import { useNavigate } from "react-router"
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton"
import PageContainer from "../../components/PageContainer"
import { translateEnumValue } from "../../enum_helpers";
import { STATUS_ENUM } from "../../enum_helpers";

export default function StudentList() {
    const navigate = useNavigate()
    const [name, _] = useState(localStorage.getItem('name'))
    const [role, setRole] = useState(localStorage.getItem('role'))
    const [isLoading, setIsLoading] = useState(true)
    const [students, setStudents] = useState([])

    useEffect(() => {
        const roles = ['Administrator', 'Professor']
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
        getStudents()
            .then(result => {
                let mapped = []
                if (result !== null && result !== undefined) {
                    console.log(result)
                    mapped = result.map((student) => {
                        return {
                            Id: student.id,
                            Nome: `${student.firstName} ${student.lastName}`,
                            Status: translateEnumValue(STATUS_ENUM, student.status),
                            "E-mail": student.email,
                            "Matrícula": student.registration,
                            "Data de defesa": student.projectDefenceDate,
                            "Data de qualificação": student.projectQualificationDate
                        }
                    })
                }
                setStudents(mapped)
                setIsLoading(false)

            })
    }, [setStudents, setIsLoading])

    return (
        <PageContainer name={name} isLoading={isLoading}>
            <div className="studentBar">
                <div className="left-bar">
                    <div>
                        <img src="student.png" alt="A logo representing students" height={"100rem"} />
                    </div>
                    <div className="title">Estudantes</div>
                </div>
                {role === 'Administrator' && <div className="right-bar">
                    <div className="search">
                        <input type="search" name="search" id="search" />
                        <i className="fas fa-" />
                    </div>
                    <div className="create-button">
                        <button onClick={() => navigate('/students/add')}>Novo Estudante</button>
                    </div>
                </div>}
            </div>
            <BackButton ></BackButton>
            <Table data={students} useOptions={true} detailsCallback={(id) => navigate(`${id}`)} />
        </PageContainer>
    )
}