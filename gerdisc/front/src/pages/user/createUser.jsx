import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import jwt_decode from "jwt-decode";
import Select from "../../components/select";
import { getProjects } from "../../api/project_service";
import { postProfessors, getProfessorById, putProfessorById } from "../../api/professor_service";
import { postStudents, getStudentById, putStudentById } from "../../api/student_service";
import { postResearchers, getResearcherById, putResearcherById } from "../../api/researcher_service";
import BackButton from "../../components/BackButton";
import ErrorPage from "../../components/error/Error";
import PageContainer from "../../components/PageContainer";
import { AREA_ENUM, INSTITUTION_TYPE_ENUM, STATUS_ENUM, SCHOLARSHIP_TYPE } from "../../enum_helpers";
import MultiSelect from "../../components/Multiselect";
import { translateEnumValue } from "../../enum_helpers";


export default function UserForm({ type = undefined, isUpdate = false }) {
    const navigate = useNavigate();
    const { id } = useParams();
    const [userType, setUserType] = useState(type);
    const [name] = useState(localStorage.getItem('name'));
    const [isStudent, setIsStudent] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(false);
    const [selectedProjects, setSelectedProject] = useState([]);
    const [errorMessage, setErrorMessage] = useState(undefined);
    const [oldValues, setOldValues] = useState({
        status: 1,
        undergraduateArea: 1,
        institutionType: 1,
        scholarship: 1,
    });
    const [projects, setProjects] = useState([]);
    const [role, setRole] = useState(localStorage.getItem('role'));
    const [user, setUser] = useState({
        firstName: '',
        lastName: '',
        email: '',
        cpf: '',
        password: '',
        createdAt: '',
        resetPasswordPath: "https://spica.eic.cefet-rj.br/saga/changePassword"
    });
    const [student, SetStudent] = useState({
        registration: "",
        registrationDate: '',
        status: 1,
        entryDate: '',
        proficiency: false,
        undergraduateInstitution: "",
        institutionType: 1,
        undergraduateCourse: "",
        graduationYear: 2012,
        undergraduateArea: 1,
        dateOfBirth: '',
        scholarship: 0,
        projectIds: [],
    });
    const [professor, setProfessor] = useState({
        siape: '',
        institution: '',
        projectId: '',
    });
    const formatDateForInput = (dateString) => {
        if (!dateString) return '';
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0'); // Mês começa do zero
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    };
    useEffect(() => {
        var projectsId = undefined;
        if (isUpdate) {
            if (isStudent) {
                getStudentById(id)
                    .then((student) => {
                        setUser(student);
                        SetStudent({
                            ...student,
                            undergraduateArea: AREA_ENUM.find((x) => x.name === student.undergraduateArea).key,
                            scholarship: SCHOLARSHIP_TYPE.find((x) => x.name === student.scholarship).key,
                            institutionType: INSTITUTION_TYPE_ENUM.find((x) => x.name === student.institutionType).key,
                            status: STATUS_ENUM.find((x) => x.name === student.status).key,
                        });
                        projectsId = student.projectId;
                        setOldValues({
                            ...oldValues,
                            institutionType: student.institutionType,
                            scholarship: student.scholarship,
                            undergraduateArea: student.undergraduateArea,
                        });
                    })
                    .catch((error) => {
                        setError(true);
                        setErrorMessage(error.message);
                    });
            }
            else if (userType === 'Professor') {
                getProfessorById(id)
                    .then(professor => {
                        setUser(professor);
                        setProfessor(professor);
                    })
                    .catch(error => {
                        setError(true);
                        setErrorMessage(error.message);
                    });
            }
            else if (userType === 'Externo') {
                getResearcherById(id)
                    .then(researcher => {
                        setUser(researcher);
                        setProfessor(researcher);
                    })
                    .catch(error => {
                        setError(true);
                        setErrorMessage(error.message);
                    });
            }
            getProjects()
                .then(result => {
                    let mapped = [];
                    if (result !== null && result !== undefined) {
                        mapped = result.map((project) => {
                            return {
                                Id: project.id,
                                Nome: project.name
                            };
                        });
                        setSelectedProject(mapped.filter(project => projectsId === project.Id));
                        setProjects(mapped);
                    }
                })
                .catch(error => {
                    setError(true);
                    setErrorMessage(error.message);
                });
        }
    }, [isUpdate, isStudent, setUser, SetStudent, setProfessor, id, userType]);

    useEffect(() => {
        setIsLoading(true);
        getProjects()
            .then(result => {
                let mapped = [];
                if (result !== null && result !== undefined) {
                    mapped = result.map((project) => {
                        return {
                            Id: project.id,
                            Nome: project.name
                        };
                    });
                    setProjects(mapped);
                }
            })
            .catch(err => setError(true))
        setIsLoading(false);
    }, [isStudent, setProjects]);

    const onProjectSelect = (selectedList, Item) => {
        const [selected] = selectedList;
        SetStudent({ ...student, ...{ projectId: selected.Id } });
    };

    const changeUserAtribute = (name, value) => {
        let newValue = {};
        newValue[name] = value;
        setUser({ ...user, ...newValue });
    };

    const changeStudentAttribute = (name, value) => {
        let newValue = {};
        newValue[name] = value;
        SetStudent({ ...student, ...newValue });
    };

    const changeProfessorAtribute = (name, value) => {
        let newValue = {};
        newValue[name] = value;
        setProfessor({ ...professor, ...newValue });
    };

    const handleUserTypeSelect = (value) => {
        setUserType(value);
    };

    useEffect(() => {
        const roles = ['Administrator'];
        const token = localStorage.getItem('token');
        try {
            const decoded = jwt_decode(token);
            if (!roles.includes(decoded.role)) {
                navigate('/');
            }
            setRole(decoded.role);
        } catch (error) {
            navigate('/login');
        }
    }, [setRole, navigate, role]);

    useEffect(() => {
        setIsStudent(userType === "Estudante");
    }, [userType]);

    const handlepost = async () => {
        if (isStudent) {
            let _user = user;
            _user.createdAt = new Date();
            let body = { ..._user, ...student };
            body.entryDate = new Date(body.entryDate);
            body.dateOfBirth = new Date(body.dateOfBirth);
            body.registrationDate = new Date(body.registrationDate);
            body.projectDefenceDate = new Date("2025-05-11");
            body.projectQualificationDate = new Date("2025-05-11");
            postStudents(body)
                .then((student) => navigate("/students"))
                .catch(error => { setError('Unable to create student'); });
        }
        else {
            let body = { ...user, ...professor };
            body.createdAt = new Date();
            body.password = professor.siape;
            if (userType === "Professor") {
                postProfessors(body)
                    .then((professor) => navigate("/professors"))
                    .catch(error => { setError('Unable to create Professor'); });
            }
            else {
                postResearchers(body)
                    .then((researcher) => navigate("/researchers"))
                    .catch(error => { setError('Unable to create Researcher'); });
            }
        }
    };

    const handleSave = (e) => {
        e.preventDefault();
        const form = document.querySelector('form');
        if (form.reportValidity()) {
            isUpdate ? handleUpdate() : handlepost();
        }
    };

    const handleUpdate = () => {
        if (isStudent) {
            let body = { ...user, ...student };
            putStudentById(id, body)
                .then((student) => {
                    navigate("/students");
                })
                .catch(error => setError(true));
        }
        else if (userType === 'Professor') {
            let body = { ...user, ...professor };
            putProfessorById(id, body)
                .then((student) => navigate("/professors"))
                .catch(error => setError(true));
        }
        else {
            let body = { ...user, ...professor };
            putResearcherById(id, body)
                .then((student) => navigate("/researchers"))
                .catch(error => setError(true));
        }
    };

    return (
        <PageContainer name={name} isLoading={isLoading}>
            {!error && (
                <>
                    <BackButton />
                    <form className="form">
                        {type === undefined && (
                            <div className="form-section">
                                <Select
                                    required={true}
                                    className="formInput"
                                    onSelect={handleUserTypeSelect}
                                    options={["", "Professor", "Estudante", "Externo"].map((option) => ({
                                        value: option,
                                        label: option,
                                    }))}
                                    label="Tipo de Usuario"
                                    name="role"
                                />
                            </div>
                        )}
                        <div className="form-section">
                            <div className="formInput">
                                <label htmlFor="firstName">Primeiro Nome</label>
                                <input required={true} type="text" name="firstName" value={user.firstName} onChange={(e) => changeUserAtribute(e.target.name, e.target.value)} id="firstName" />
                            </div>
                            <div className="formInput">
                                <label htmlFor="lastName">Sobrenome</label>
                                <input required={true} type="text" name="lastName" id="lastName" value={user.lastName} onChange={(e) => changeUserAtribute(e.target.name, e.target.value)} />
                            </div>
                        </div>
                        <div className="form-section">
                            <div className="formInput">
                                <label htmlFor="email">Email</label>
                                <input required={true} type="email" name="email" id="email" value={user.email} onChange={(e) => changeUserAtribute(e.target.name, e.target.value)} />
                            </div>
                            <div className="formInput">
                                <label htmlFor="cpf">CPF</label>
                                <input required={true} disabled={isUpdate} type="text" name="cpf" value={user.cpf} id="cpf" onChange={(e) => changeUserAtribute(e.target.name, e.target.value)} />
                            </div>
                        </div>
                        {userType === "Estudante" && (
                            <>
                                <div className="form-section" id="registration-section">
                                    <div className="formInput">
                                        <label htmlFor="registration">Matricula</label>
                                        <input required={true} disabled={isUpdate} type="text" name="registration" id="registration" value={student.registration} onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                    </div>
                                    <div className="formInput">
                                        <Select
                                            required={false}
                                            defaultValue={oldValues.scholarship}
                                            label={"Bolsa"}
                                            onSelect={(value) => changeStudentAttribute("scholarship", Number(value))}
                                            name="scholarship"
                                            options={SCHOLARSHIP_TYPE.map((item) => ({ value: item.key, label: item.translation }))}
                                        />
                                    </div>
                                    <div className="formInput">
                                        <label htmlFor="registrationDate">Data da Matrícula</label>
                                        <input required={true} disabled={isUpdate} type="date" name="registrationDate" id="registrationDate" value={formatDateForInput(student.registrationDate)} onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                    </div>
                                    <div className="formInput">
                                        <label htmlFor="entryDate">Data de Entrada</label>
                                        <input required={true} disabled={isUpdate} type="date" name="entryDate" id="entryDate" value={formatDateForInput(student.entryDate)} onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                    </div>
                                    <div className="formInput">
                                        <label htmlFor="dateOfBirth">Data de Nascimento</label>
                                        <input required={true} disabled={isUpdate} type="date" name="dateOfBirth" id="dateOfBirth" value={formatDateForInput(student.dateOfBirth)} onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                    </div>
                                    <div className="form-section" id="qualification-section2">
                                        <div className="formInput">
                                            <label htmlFor="undergraduateInstitution">Institução de graduação</label>
                                            <input required={true} minLength={3} type="text" name="undergraduateInstitution" value={student.undergraduateInstitution} id="undergraduateInstitution" onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                        </div>
                                        <div className="formInput">
                                            <label htmlFor="undergraduateCourse">Curso</label>
                                            <input required={true} type="text" name="undergraduateCourse" id="undergraduateCourse" value={student.undergraduateCourse} onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                        </div>
                                        <div className="formInput">
                                            <Select
                                                required={true}
                                                defaultValue={oldValues.undergraduateArea}
                                                label="Área de graduação"
                                                onSelect={(value) => changeStudentAttribute("undergraduateArea", Number(value))}
                                                name="undergraduateArea"
                                                options={AREA_ENUM.map((item) => ({ value: item.key, label: item.translation }))}
                                            />
                                        </div>
                                        <div className="formInput">
                                            <label htmlFor="graduationYear">Ano de formação</label>
                                            <input required={true} type="number" min={1950} max={3000} name="graduationYear" value={student.graduationYear} id="graduationYear" onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)} />
                                        </div>
                                    </div>
                                    <div className="form-section" id="qualification-section3">
                                        <div className="formInput">
                                            <Select
                                                required={false}
                                                defaultValue={oldValues.institutionType}
                                                className="formInput"
                                                options={INSTITUTION_TYPE_ENUM.map((item) => ({ value: item.key, label: item.translation }))}
                                                onSelect={(value) => changeStudentAttribute("institutionType", Number(value))}
                                                label="Tipo de Instituição"
                                                name="institutionType"
                                            />
                                        </div>
                                        <div className="formInput">
                                            <MultiSelect
                                                isDisabled={isUpdate}
                                                selectedValues={selectedProjects}
                                                options={projects}
                                                loading={isLoading}
                                                placeholder="Selecionar Projetos"
                                                onSelect={onProjectSelect}
                                                onRemove={onProjectSelect}
                                                displayValue="Nome"
                                            />
                                        </div>
                                    </div>
                                </div>
                            </>
                        )}
                        <div className="form-section" id="professor-section">
                            {userType === "Professor" && (
                                <div className="formInput">
                                    <label htmlFor="siape">SIAPE</label>
                                    <input required={userType === "Professor"} disabled={isUpdate} type="text" minLength={3} value={professor.siape} name="siape" id="siape" onChange={(e) => changeProfessorAtribute(e.target.name, e.target.value)} />
                                </div>
                            )}
                            {userType === "Externo" && (
                                <div className="formInput">
                                    <label htmlFor="institution">Institução</label>
                                    <input required={true} type="text" name="institution" value={professor.institution} id="institution" onChange={(e) => changeProfessorAtribute(e.target.name, e.target.value)} />
                                </div>
                            )}
                        </div>
                        <div className="form-section">
                            <div className="formInput">
                                <input type="submit" value={isUpdate ? "Update" : "Submit"} onClick={(e) => handleSave(e)} />
                            </div>
                        </div>
                    </form>
                </>
            )}
            {error && <ErrorPage errorMessage={errorMessage} />}
        </PageContainer>
    );
}
