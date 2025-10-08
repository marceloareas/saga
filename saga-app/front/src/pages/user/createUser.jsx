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

const isCPFValido = (strCPF) => {
  let Soma = 0;
  let Resto = 0;

  if (strCPF === "00000000000") return false;

  for (let i = 1; i <= 9; i++) {
    Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
  }
  Resto = (Soma * 10) % 11;

  if (Resto === 10 || Resto === 11) Resto = 0;
  if (Resto !== parseInt(strCPF.substring(9, 10))) return false;

  Soma = 0;
  for (let i = 1; i <= 10; i++) {
    Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
  }
  Resto = (Soma * 10) % 11;

  if (Resto === 10 || Resto === 11) Resto = 0;
  if (Resto !== parseInt(strCPF.substring(10, 11))) return false;
  return true;
};

export default function UserForm({ type = undefined, isUpdate = false }) {
  const navigate = useNavigate();
  const { id } = useParams();
  const [userType, setUserType] = useState(type);
  const [name] = useState(localStorage.getItem("name"));
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
  const [role, setRole] = useState(localStorage.getItem("role"));
  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: "",
    cpf: "",
    password: "",
    createdAt: "",
    resetPasswordPath: "https://spica.eic.cefet-rj.br/saga/changePassword",
  });
  const [student, SetStudent] = useState({
    registration: "",
    registrationDate: "",
    status: 1,
    entryDate: "",
    proficiency: false,
    undergraduateInstitution: "",
    institutionType: 1,
    undergraduateCourse: "",
    graduationYear: 2012,
    undergraduateArea: 1,
    dateOfBirth: "",
    scholarship: 0,
    projectIds: [],
  });
  const [professor, setProfessor] = useState({
    siape: "",
    institution: "",
    projectId: "",
  });

  // Deixa o botão sempre clicável (a validade fica com o HTML + reportValidity)
  const [isCpfValid, setIsCpfValid] = useState(true);

  const formatDateForInput = (dateString) => {
    if (!dateString) return "";
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
  };

  // Carrega dados quando é update
  useEffect(() => {
    let projectsId;
    if (isUpdate) {
      if (isStudent) {
        getStudentById(id)
          .then((s) => {
            setUser(s);
            SetStudent({
              ...s,
              undergraduateArea: AREA_ENUM.find((x) => x.name === s.undergraduateArea).key,
              scholarship: SCHOLARSHIP_TYPE.find((x) => x.name === s.scholarship).key,
              institutionType: INSTITUTION_TYPE_ENUM.find((x) => x.name === s.institutionType).key,
              status: STATUS_ENUM.find((x) => x.name === s.status).key,
            });
            projectsId = s.projectId;
            setOldValues((o) => ({
              ...o,
              institutionType: s.institutionType,
              scholarship: s.scholarship,
              undergraduateArea: s.undergraduateArea,
            }));
          })
          .catch((e) => {
            setError(true);
            setErrorMessage(e.message);
          });
      } else if (userType === "Professor") {
        getProfessorById(id)
          .then((p) => {
            setUser({
              firstName: p.firstName,
              lastName: p.lastName,
              email: p.email,
              cpf: p.cpf,
              createdAt: p.createdAt,
            });
            setProfessor({
              siape: p.siape,
              institution: p.institution,
              projectId: p.projectId,
            });
            projectsId = p.projectId;
            setOldValues((o) => ({ ...o, institutionType: 1 }));
          })
          .catch((e) => {
            setError(true);
            setErrorMessage(e.message);
          });
      } else if (userType === "Externo") {
        getResearcherById(id)
          .then((r) => {
            setUser(r);
            setProfessor(r);
          })
          .catch((e) => {
            setError(true);
            setErrorMessage(e.message);
          });
      }

      getProjects()
        .then((result) => {
          let mapped = [];
          if (result !== null && result !== undefined) {
            mapped = result.map((project) => ({
              Id: project.id,
              Nome: project.name,
            }));
            setSelectedProject(mapped.filter((p) => projectsId === p.Id));
            setProjects(mapped);
          }
        })
        .catch((e) => {
          setError(true);
          setErrorMessage(e.message);
        });
    }
  }, [isUpdate, isStudent, id, userType]);

  // Carrega lista de projetos (create/update)
  useEffect(() => {
    setIsLoading(true);
    getProjects()
      .then((result) => {
        let mapped = [];
        if (result !== null && result !== undefined) {
          mapped = result.map((project) => ({
            Id: project.id,
            Nome: project.name,
          }));
          setProjects(mapped);
        }
      })
      .catch(() => setError(true))
      .finally(() => setIsLoading(false));
  }, []);

  const onProjectSelect = (selectedList) => {
    const [selected] = selectedList;
    SetStudent((s) => ({ ...s, projectId: selected?.Id }));
  };

  const changeUserAtribute = (name, value) => {
    if (name === "cpf") {
      const digits = (value || "").replace(/\D/g, "");
      const valid = digits.length === 11 && isCPFValido(digits);
      setIsCpfValid(valid);
    }
    setUser((u) => ({ ...u, [name]: value }));
  };

  const changeStudentAttribute = (name, value) => {
    SetStudent((s) => ({ ...s, [name]: value }));
  };

  const changeProfessorAtribute = (name, value) => {
    setProfessor((p) => ({ ...p, [name]: value }));
  };

  const handleUserTypeSelect = (value) => {
    setUserType(value);
  };

  // Auth gate
  useEffect(() => {
    const roles = ["Administrator"];
    const token = localStorage.getItem("token");
    try {
      const decoded = jwt_decode(token);
      if (!roles.includes(decoded.role)) {
        navigate("/");
      }
      setRole(decoded.role);
    } catch (error) {
      navigate("/login");
    }
  }, [navigate]);

  useEffect(() => {
    setIsStudent(userType === "Estudante");
  }, [userType]);

  const handlepost = async () => {
    if (isStudent) {
      const body = {
        ...user,
        ...student,
        createdAt: new Date(),
        entryDate: new Date(student.entryDate),
        dateOfBirth: new Date(student.dateOfBirth),
        registrationDate: new Date(student.registrationDate),
        projectDefenceDate: new Date("2025-05-11"),
        projectQualificationDate: new Date("2025-05-11"),
      };
      postStudents(body)
        .then(() => navigate("/students"))
        .catch(() => setError("Unable to create student"));
    } else {
      const body = {
        ...user,
        ...professor,
        createdAt: new Date(),
        password: professor.siape,
      };
      if (userType === "Professor") {
        postProfessors(body)
          .then(() => navigate("/professors"))
          .catch(() => setError("Unable to create Professor"));
      } else {
        postResearchers(body)
          .then(() => navigate("/researchers"))
          .catch(() => setError("Unable to create Researcher"));
      }
    }
  };

  const handleUpdate = (studentParam, userParam, professorParam) => {
    if (isStudent) {
      const body = { ...studentParam, ...userParam };
      putStudentById(id, body)
        .then(() => navigate("/students"))
        .catch(() => setError(true));
    } else if (userType === "Professor") {
      const body = { ...userParam, ...professorParam };
      putProfessorById(id, body)
        .then(() => navigate("/professors"))
        .catch(() => setError(true));
    } else {
      const body = { ...userParam, ...professorParam };
      putResearcherById(id, body)
        .then(() => navigate("/researchers"))
        .catch(() => setError(true));
    }
  };

  const handleSave = (e) => {
    e.preventDefault();
    const form = e.currentTarget;
    if (form.reportValidity()) {
      isUpdate ? handleUpdate(student, user, professor) : handlepost();
    }
  };

  return (
    <PageContainer name={name} isLoading={isLoading}>
      {!error && (
        <>
          <BackButton />
          <form className="form" onSubmit={handleSave}>
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
                <input
                  required={true}
                  type="text"
                  name="firstName"
                  value={user.firstName}
                  placeholder="entre com o primeiro nome"
                  onChange={(e) => changeUserAtribute(e.target.name, e.target.value)}
                  id="firstName"
                />
              </div>
              <div className="formInput">
                <label htmlFor="lastName">Sobrenome</label>
                <input
                  required={true}
                  type="text"
                  name="lastName"
                  id="lastName"
                  value={user.lastName}
                  placeholder="entre com o sobrenome"
                  onChange={(e) => changeUserAtribute(e.target.name, e.target.value)}
                />
              </div>
            </div>

            <div className="form-section">
              <div className="formInput">
                <label htmlFor="email">Email</label>
                <input
                  required={true}
                  type="email"
                  name="email"
                  id="email"
                  value={user.email}
                  placeholder="entre com o email"
                  onChange={(e) => changeUserAtribute(e.target.name, e.target.value)}
                />
              </div>
              <div className="formInput">
                <label htmlFor="cpf">CPF</label>
                <input
                  required={true}
                  disabled={isUpdate}
                  type="text"
                  name="cpf"
                  value={user.cpf}
                  id="cpf"
                  placeholder="entre com o CPF (11 dígitos)"
                  inputMode="numeric"
                  pattern="^\d{11}$"
                  maxLength={14}
                  onChange={(e) => changeUserAtribute(e.target.name, e.target.value)}
                />
              </div>
            </div>

            {userType === "Estudante" && (
              <>
                <div className="form-section" id="registration-section">
                  <div className="formInput">
                    <label htmlFor="registration">Matricula</label>
                    <input
                      required={true}
                      disabled={isUpdate}
                      type="text"
                      name="registration"
                      id="registration"
                      value={student.registration}
                      placeholder="entre com a matricula"
                      onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                    />
                  </div>
                  <div className="formInput">
                    <Select
                      required={false}
                      defaultValue={oldValues.scholarship}
                      label={"Bolsa"}
                      onSelect={(value) => changeStudentAttribute("scholarship", Number(value))}
                      name="scholarship"
                      options={SCHOLARSHIP_TYPE.map((item) => ({
                        value: item.key,
                        label: item.translation,
                      }))}
                    />
                  </div>
                  <div className="formInput">
                    <label htmlFor="registrationDate">Data da Matrícula</label>
                    <input
                      required={true}
                      disabled={isUpdate}
                      type="date"
                      name="registrationDate"
                      id="registrationDate"
                      value={formatDateForInput(student.registrationDate)}
                      onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                    />
                  </div>
                  <div className="formInput">
                    <label htmlFor="entryDate">Data de Entrada</label>
                    <input
                      required={true}
                      disabled={isUpdate}
                      type="date"
                      name="entryDate"
                      id="entryDate"
                      value={formatDateForInput(student.entryDate)}
                      onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                    />
                  </div>
                  <div className="formInput">
                    <label htmlFor="dateOfBirth">Data de Nascimento</label>
                    <input
                      required={true}
                      disabled={isUpdate}
                      type="date"
                      name="dateOfBirth"
                      id="dateOfBirth"
                      value={formatDateForInput(student.dateOfBirth)}
                      onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                    />
                  </div>

                  <div className="form-section" id="qualification-section2">
                    <div className="formInput">
                      <label htmlFor="undergraduateInstitution">Institução de graduação</label>
                      <input
                        required={true}
                        minLength={3}
                        type="text"
                        name="undergraduateInstitution"
                        value={student.undergraduateInstitution}
                        id="undergraduateInstitution"
                        placeholder="entre com a instituição de graduação"
                        onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                      />
                    </div>
                    <div className="formInput">
                      <label htmlFor="undergraduateCourse">Curso</label>
                      <input
                        required={true}
                        type="text"
                        name="undergraduateCourse"
                        id="undergraduateCourse"
                        value={student.undergraduateCourse}
                        placeholder="entre com o curso"
                        onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                      />
                    </div>
                    <div className="formInput">
                      <Select
                        required={true}
                        defaultValue={oldValues.undergraduateArea}
                        label="Área de graduação"
                        onSelect={(value) =>
                          changeStudentAttribute("undergraduateArea", Number(value))
                        }
                        name="undergraduateArea"
                        options={AREA_ENUM.map((item) => ({
                          value: item.key,
                          label: item.translation,
                        }))}
                      />
                    </div>
                    <div className="formInput">
                      <label htmlFor="graduationYear">Ano de formação</label>
                      <input
                        required={true}
                        type="number"
                        min={1950}
                        max={3000}
                        name="graduationYear"
                        value={student.graduationYear}
                        id="graduationYear"
                        onChange={(e) => changeStudentAttribute(e.target.name, e.target.value)}
                      />
                    </div>
                  </div>

                  <div className="form-section" id="qualification-section3">
                    <div className="formInput">
                      <Select
                        required={false}
                        defaultValue={oldValues.institutionType}
                        className="formInput"
                        options={INSTITUTION_TYPE_ENUM.map((item) => ({
                          value: item.key,
                          label: item.translation,
                        }))}
                        onSelect={(value) =>
                          changeStudentAttribute("institutionType", Number(value))
                        }
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
                  <input
                    required={userType === "Professor"}
                    disabled={isUpdate}
                    type="text"
                    minLength={3}
                    value={professor.siape}
                    name="siape"
                    id="siape"
                    onChange={(e) => changeProfessorAtribute(e.target.name, e.target.value)}
                  />
                </div>
              )}
              {userType === "Externo" && (
                <div className="formInput">
                  <label htmlFor="institution">Institução</label>
                  <input
                    required={true}
                    type="text"
                    name="institution"
                    value={professor.institution}
                    id="institution"
                    onChange={(e) => changeProfessorAtribute(e.target.name, e.target.value)}
                  />
                </div>
              )}
            </div>

            <div className="form-section">
              <div className="formInput">
                <button type="submit">
                  {isUpdate ? "Atualizar" : "Confirmar"}
                </button>
              </div>
            </div>
          </form>
        </>
      )}
      {error && <ErrorPage errorMessage={errorMessage} />}
    </PageContainer>
  );
}
