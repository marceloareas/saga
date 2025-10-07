import { useEffect, useState } from "react"
import '../../styles/professorList.scss';
import Table from "../../components/Table/table"
import { getProfessors, getProfessorsPaged } from "../../api/professor_service"
import { useNavigate } from "react-router"
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import PageContainer from "../../components/PageContainer"

export default function ProfessorList() {
    const navigate = useNavigate()
    const [name,] = useState(localStorage.getItem('name'))
    const [role, setRole] = useState(localStorage.getItem('role'))
    const [isLoading, setIsLoading] = useState(true)
    const [professors, setProfessors] = useState([])
    const [q, setQ] = useState("")
    const [page, setPage] = useState(1)
    const [pageSize, setPageSize] = useState(20)
    const [totalPages, setTotalPages] = useState(1)

    const detailsCallback = (id)=>
    {
        navigate(id)
    }

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

    const fetchPage = ({ page: p = page, pageSize: ps = pageSize, q: query = q } = {}) => {
        setIsLoading(true)
        return getProfessorsPaged({ page: p, pageSize: ps, q: query })
            .then(result => {
                console.log(result)
                let mapped = []
                if (result && result.items) {
                    mapped = result.items.map((professor) => {
                        return {
                            Id: professor.id,
                            Nome: `${professor.firstName} ${professor.lastName}`,
                            "E-mail": professor.email,
                            Siape: professor.siape,
                        }
                    })
                }
                setProfessors(mapped)
                if (result?.totalPages) setTotalPages(result.totalPages)
                if (result?.page) setPage(result.page)
                setIsLoading(false)
            }).catch(()=> setIsLoading(false))
    }


    return (<PageContainer name={name} isLoading={isLoading}>
        <div className="bar professorBar">
            <div className="left-bar">
                <div>
                    <img src="professor.png" alt="A logo representing professors" height={"100rem"} />
                </div>
                <div className="title">Professores</div>
            </div>
            <div className="right-bar">
                <div className="search">
                    <input
                      type="search"
                      name="search"
                      id="search"
                      value={q}
                      onChange={(e)=>setQ(e.target.value)}
                      placeholder="Buscar por nome/email"
                    />
                    <button onClick={onSearch}>Buscar</button>
                </div>
                <div className="create-button">
                    <button onClick={() => ''}>Mostrar inativos</button>
                </div>
                <div className="create-button">
                    <button onClick={()=> navigate('/professors/add')}>Novo Professor</button>
                </div>
            </div>
        </div>
        <BackButton />
        {!isLoading && <Table data={professors} useOptions={true} detailsCallback={detailsCallback} />}
        <div style={{display:'flex', gap:8, justifyContent:'flex-end', marginTop:12}}>
          <button disabled={page<=1} onClick={prev}>Anterior</button>
          <span>Página {page} de {totalPages}</span>
          <button disabled={page>=totalPages} onClick={next}>Próxima</button>
        </div>
    </PageContainer>
)
}