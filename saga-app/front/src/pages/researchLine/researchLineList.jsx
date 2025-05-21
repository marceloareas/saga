import { useEffect, useState } from "react"
import '../../styles/projectList.scss';
import Table from "../../components/Table/table"
import { getResearchLines } from "../../api/research_line"
import { useNavigate } from "react-router"
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import PageContainer from "../../components/PageContainer";
//import { translateEnumValue } from "../../enum_helpers";
//import { PROJECT_STATUS_ENUM } from "../../enum_helpers";

export default function ResearchLinesList() {
    const navigate = useNavigate()
    const [name,] = useState(localStorage.getItem('name'))
    const [role, setRole] = useState(localStorage.getItem('role'))
    const [isLoading, setIsLoading] = useState(true)
    const [projectLines, setProjectLines] = useState([])

    const detailsCallback = (id)=>
    {
        navigate(id)
    }
    useEffect(() => {
        const roles = ['Administrator', 'Student', 'Professor']
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
        getResearchLines()
            .then(result => {
                let mapped = []
                if (result !== null && result !== undefined) {
                    mapped = result.map((rs) => {
                        return {
                            Id: rs.id,
                            Nome: rs.name
                        }
                    })
                }
                setProjectLines(mapped)
                setIsLoading(false)

            })
    }, [setProjectLines, setIsLoading])


    return (
        <PageContainer name={name} isLoading={isLoading}>
            <div className="projectBar">
                <div className="left-bar">
                    <div>
                        <img src="report.png" alt="A logo representing Project Lines" height={"100rem"} />
                    </div>
                    <div className="title">Linha de Pesquisa</div>
                </div>
                <div className="right-bar">
                    <div className="search">
                        <input type="search" name="search" id="search" />
                        <i className="fas fa-" />
                    </div>
                    {role === 'Administrator' && <div className="create-button">
                        <button onClick={() => navigate('/researchLine/add')}>Nova linha de pesquisa</button>
                    </div>}
                </div>
            </div>
            <BackButton /><Table data={projectLines} useOptions={role === 'Administrator'} detailsCallback={detailsCallback} />
        </PageContainer>
    )
}
