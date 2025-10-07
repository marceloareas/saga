import { useEffect, useState } from "react"
import '../../styles/studentList.scss'
import Table from "../../components/Table/table"
import { getStudents, getStudentsPaged } from "../../api/student_service"
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
    const [q, setQ] = useState("")
    const [page, setPage] = useState(1)
    const [pageSize, setPageSize] = useState(20)
    const [totalPages, setTotalPages] = useState(1)

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

    const fetchPage = ({ page: p = page, pageSize: ps = pageSize, q: query = q } = {}) => {
        setIsLoading(true)
        return getStudentsPaged({ page: p, pageSize: ps, q: query })
            .then(result => {
                let mapped = []
                if (result && result.items) {
                    mapped = result.items.map((student) => {
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
                if (result?.totalPages) setTotalPages(result.totalPages)
                if (result?.page) setPage(result.page)
                setIsLoading(false)

            }).catch(() => setIsLoading(false))
    }

    useEffect(() => { fetchPage({ page: 1 }) }, [])

    const onSearch = () => fetchPage({ page: 1, q })
    const prev = () => page > 1 && fetchPage({ page: page - 1 })
    const next = () => page < totalPages && fetchPage({ page: page + 1 })

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
                        <input
                          type="search"
                          name="search"
                          id="search"
                          value={q}
                          onChange={(e)=>setQ(e.target.value)}
                          placeholder="Buscar por nome/email/matrícula"
                        />
                        <button onClick={onSearch}>Buscar</button>
                    </div>
                    <div className="create-button">
                        <button onClick={() => navigate('/students/add')}>Novo Estudante</button>
                    </div>
                </div>}
            </div>
            <BackButton ></BackButton>
            <Table data={students} useOptions={true} detailsCallback={(id) => navigate(`${id}`)} />
            <div style={{display:'flex', gap:8, justifyContent:'flex-end', marginTop:12}}>
              <button disabled={page<=1} onClick={prev}>Anterior</button>
              <span>Página {page} de {totalPages}</span>
              <button disabled={page>=totalPages} onClick={next}>Próxima</button>
            </div>

        </PageContainer>
    )
}