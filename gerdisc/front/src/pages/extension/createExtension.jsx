import { useEffect, useState } from "react";
import "../../styles/createExtension.scss";
import { useParams } from "react-router";
import { useNavigate } from "react-router"
import jwt_decode from "jwt-decode";
import Select from "../../components/select";
import BackButton from "../../components/BackButton";
import { postExtensions } from "../../api/extension_service";
import PageContainer from "../../components/PageContainer";


export default function ExtensionForm() {
    const navigate  = useNavigate()
    const { id}  = useParams()
    const [name,] = useState(localStorage.getItem('name'))
    const [error, setError] = useState(null);
    const [role, setRole] = useState(localStorage.getItem('role'))
    const [extension, setExtension] = useState(
        {
            studentId: id,
            numberOfDays: 0,
            type: 1,
            status: 'aprovado'

        }
    );

    const setDays = (numberOfDays)=> {
        setExtension({...extension,...{'numberOfDays':Number(numberOfDays)}});
    }
    const setType = (type)=> {
        type = type === "Defesa"? 1: 2
        setExtension({...extension,...{'type': type}});
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

    const handlepost = async () => {
        postExtensions(extension)
        .then(result => navigate(-1))
        .catch(errors=>setError(true));
    }

    const handleSave = (e)=>{
        e.preventDefault()
        handlepost()
    }

    return (
        <PageContainer name={name} isLoading={false}>
            <BackButton />
            <div className="extensionForm">
                <div className="form-section">
                    <div className="formInput">
                        <label htmlFor="name">Quantidade de Dias</label>
                        <input type="number" name="numberOfDays" value={extension.name} onChange={(e)=>setDays(e.target.value)} id="numberOfDays" />
                    </div>
                    <Select
                        className="formInput"
                        onSelect={setType}
                        options={["Defesa", "Qualificação"].map((option) => ({
                            value: option,
                            label: option,
                        }))}
                        label="Type"
                        name="type"
                        />
                </div>
                <div className="form-section">
                </div>
                <div className="form-section">
                    <div className="formInput">
                        <input type="submit" value={"Submit"} onClick={(e)=> handleSave(e)} />
                    </div>                 
                </div>
            </div>          
        </PageContainer>
);
}
