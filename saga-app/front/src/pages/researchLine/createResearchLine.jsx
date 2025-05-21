import { useEffect, useState } from "react";
import "../../styles/createProject.scss";
import { useNavigate, useParams } from "react-router";
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import ErrorPage from "../../components/error/Error";
import PageContainer from "../../components/PageContainer";
import { postResearchLines, getResearchLineById, putResearchLinesById } from "../../api/research_line";

export default function ResearchLineForm({ Update = false }) {
  const { id } = useParams();
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [error, setError] = useState(null);
  const [isUpdate] = useState(Update);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setIsLoading(true);

    if (isUpdate) {
      getResearchLineById(id)
        .then((researchLine) => {
          setName(researchLine.name);
          setIsLoading(false);
        })
        .catch((err) => {
          setError(true);
          setIsLoading(false);
        });
    } else {
      setIsLoading(false);
    }
  }, [isUpdate, id]);

  useEffect(() => {
    const roles = ["Administrator"];
    const token = localStorage.getItem("token");
    try {
      const decoded = jwt_decode(token);
      if (!roles.includes(decoded.role)) {
        navigate("/");
      }
    } catch (error) {
      navigate("/login");
    }
  }, []);

  const handlePost = () => {
    postResearchLines({ name })
      .then(() => navigate(-1))
      .catch((error) => setError(error));
  };

  const handleUpdate = () => {
    putResearchLinesById(id, { name })
    .then(() => navigate(-1))
    .catch((error) => {
      getResearchLineById(id)
      .then((resLine) => name == resLine.name ? navigate(-1) : setError(error))
      .catch((error) => setError(error))
    })
  };

  const handleSave = (e) => {
    e.preventDefault();
    const form = document.querySelector("form");
    if (form.reportValidity()) {
      isUpdate ? handleUpdate() : handlePost();
    }
  };

  return (
    <PageContainer isLoading={isLoading} name="Gerenciar Linhas de Pesquisa">
      <BackButton />
      {!error && (
        <form className="form">
          <div className="form-section">
            <div className="formInput">
              <label htmlFor="name">Nome da Linha de Pesquisa</label>
              <input
                required
                type="text"
                name="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                id="name"
              />
            </div>
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
