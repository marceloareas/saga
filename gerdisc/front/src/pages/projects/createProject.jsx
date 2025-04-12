import { useEffect, useState } from "react";
import "../../styles/createProject.scss";
import { useNavigate, useParams } from "react-router"
import jwt_decode from "jwt-decode";
import { getProfessors } from "../../api/professor_service"
import Select from "../../components/select";
import BackButton from "../../components/BackButton";
import MultiSelect from "../../components/Multiselect";
import ErrorPage from "../../components/error/Error";
import PageContainer from "../../components/PageContainer";
import { postProjects, getProjectById, putProjectsById } from "../../api/project_service";
import { getResearchLines } from "../../api/research_line";
import { PROJECT_STATUS_ENUM, STATUS_ENUM } from "../../enum_helpers";

export default function ProjectForm({ Update = false }) {
  const { id } = useParams();
  const navigate = useNavigate();
  const [name,] = useState(localStorage.getItem('name'));
  const [error, setError] = useState(null);
  const [isUpdate] = useState(Update);
  const [isLoading, setIsLoading] = useState(true);
  const [professors, setProfessors] = useState([]);
  const [role, setRole] = useState(localStorage.getItem('role'));
  const [linesOptions, setLinesOptions] = useState([]);
  const [selectedProfessors, setSelectedProfessors] = useState([]);
  const [oldStatus, setOldStatus] = useState();
  const [project, setProject] = useState({
    name: '',
    status: 0,
    professorIds: [],
    researchLineId: undefined
  });

  useEffect(() => {
    setIsLoading(true);
    getProfessors().then(result => {
      let mapped = [];
      if (result !== null && result !== undefined) {
        mapped = result.map((professor) => ({
          Id: professor.id,
          Name: `${professor.firstName} ${professor.lastName}  - ${professor.siape}`,
        }));
      }
      setProfessors(mapped);
    });

    getResearchLines().then((result) => {
      setLinesOptions(result.map(line => ({
        label: line.name,
        value: line.id
      })));
    });

    setIsLoading(false);
  }, []);

  useEffect(() => {
    if (isUpdate) {
      setIsLoading(true);
      getProjectById(id)
        .then(project => {
          setProject({
            name: project.name,
            status: project.status,
            professorIds: project.professors.map(x => x.id),
            researchLineId: project.researchLineId,
          });
          setOldStatus(project.status);
          setSelectedProfessors(project.professors.map(professor => ({
            Id: professor.id,
            Name: `${professor.firstName} ${professor.lastName}`,
          })));
          setIsLoading(false);
        })
        .catch(err => {
          setError(true);
          setIsLoading(false);
        });
    }
  }, [isUpdate, id]);

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
  }, []);

  const setResearchLine = (id) => {
    setProject({ ...project, researchLineId: id });
  };

  const setName = (name) => {
    setProject({ ...project, name });
  };

  const setStatus = (status) => {
    setProject({ ...project, status });
  };

  const handlePost = () => {
    postProjects(project)
      .then(() => navigate(-1))
      .catch((error) => setError(error));
  };

  const handleUpdate = () => {
    putProjectsById(id, project)
      .then(() => navigate("/projects"))
      .catch((error) => setError(error));
  };

  const handleSave = (e) => {
    e.preventDefault();
    const form = document.querySelector('form');
    if (form.reportValidity()) {
      isUpdate ? handleUpdate() : handlePost();
    }
  };

  const onProfessorSelect = (selectedList) => {
    setProject({ ...project, professorIds: selectedList.map(item => item.Id) });
  };

  return (
    <PageContainer isLoading={isLoading} name={name}>
      <BackButton />
      {!error && (
        <form className="form">
          <div className="form-section">
            <div className="formInput">
              <label htmlFor="name">Nome</label>
              <input required type="text" name="name" value={project.name} onChange={(e) => setName(e.target.value)} id="name" />
            </div>
            <Select
              required
              defaultValue={oldStatus}
              className="formInput"
              options={PROJECT_STATUS_ENUM.map((item) => ({ value: item.name, label: item.translation }))}
              onSelect={(value) => setStatus(value)}
              label="Status"
              name="status"
            />
          </div>
          <div className="form-section">
            <MultiSelect
              selectedValues={selectedProfessors}
              options={professors}
              loading={isLoading}
              placeholder="Selecionar professores"
              onSelect={onProfessorSelect}
              onRemove={onProfessorSelect}
              displayValue="Name"
            />
            {!isUpdate && (
              <Select
                className="formInput"
                defaultValue=""
                onSelect={setResearchLine}
                options={[
                  { value: "", label: "" },
                  ...linesOptions,
                ]}
                label="Linha de Pesquisa"
                name="researchLineId"
              />
            )}
          </div>
          <div className="form-section">
            <div className="formInput">
              <input type="submit" value="Submit" onClick={(e) => handleSave(e)} />
            </div>
          </div>
        </form>
      )}
      {error && <ErrorPage />}
    </PageContainer>
  );
}
