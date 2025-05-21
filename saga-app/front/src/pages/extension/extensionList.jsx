import { useEffect, useState } from "react"
import '../../styles/extensionList.scss';
import Table from "../../components/Table/table"
import { getExtensions } from "../../api/extension_service"
import { useNavigate } from "react-router"
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import ErrorPage from "../../components/error/Error";
import PageContainer from "../../components/PageContainer";


export default function ExtensionList() {
    const navigate = useNavigate()
    const [name,] = useState(localStorage.getItem('name'))
    const [role, setRole] = useState(localStorage.getItem('role'))
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState(false)
    const [extensions, setextensions] = useState([])

    useEffect(() => {
        const roles = ['Administrator', 'Student']
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
        setIsLoading(true)
        getExtensions()
            .then(result => {
                let mapped = []
                if (result !== null && result !== undefined) {
                    mapped = result.map((extension) => {
                        return {
                            Id: extension.id,
                            Status: extension.status,
                            Estudante: extension?.student?.email,
                            Dias: extension?.numberOfDays,
                            Tipo: extension?.type === 1? "Defesa": "Qualificação" 
                        }
                    })
                }
                setextensions(mapped)
                setIsLoading(false)

            })
    }, [setextensions, setIsLoading, ])


    return (<PageContainer isLoading={isLoading} name={name} >
                <div className="extensionBar">
                    <div className="left-bar">
                        <div style={{margin: "0.5rem"}}>
                            <img className="filtered" src="calender.png" alt="A logo representing extension" height={"100rem"} />
                        </div>
                        <div className="title">Extensões</div>
                    </div>
                    <div className="right-bar">
                        <div className="search">
                            <input type="search" name="search" id="search" />
                            <i className="fas fa-" />
                        </div>
                    </div>
                </div>
                <BackButton />
                <Table data={extensions} useOptions={false} />
                {error && < ErrorPage/>}
    </PageContainer>)
}