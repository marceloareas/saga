import { useEffect, useState } from "react";
import "../../styles/researchList.scss";
import Table from "../../components/Table/table";
import { getResearch } from "../../api/research_service";
import { useNavigate } from "react-router";
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import ErrorPage from "../../components/error/Error";
import PageContainer from "../../components/PageContainer";

export default function ResearchList() {
  const navigate = useNavigate();
  const [name] = useState(localStorage.getItem("name"));
  const [role, setRole] = useState(localStorage.getItem("role"));
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(false);
  const [researches, setResearches] = useState([]);

  useEffect(() => {
    const roles = ["Administrator", "Professor"];
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
  }, [setRole, navigate, role]);

  useEffect(() => {
    getResearch()
      .then((result) => {
        let mapped = [];
        console.log(result);
        if (result !== null && result !== undefined) {
          mapped = result.map((research) => {
            return {
              Id: research.id,
              Nome: research.dissertation,
              Professores: `${research.professor?.firstName} ${research.professor?.lastName}`,
              Students: `${research.student?.firstName} ${research.student?.lastName}`,
            };
          });
        }
        setResearches(mapped);
        setIsLoading(false);
      })
      .catch((error) => {
        console.log(error);
        setError(true);
        setIsLoading(false);
      });
  }, [setResearches, setIsLoading]);

  return (
    <PageContainer name={name} isLoading={isLoading}>
      {!error ? (
        <>
          <div className="researchBar">
            <div className="left-bar">
              <div>
                <img
                  src="research.png"
                  alt="A logo representing Researches"
                  height={"100rem"}
                />
              </div>
              <div className="title">Dissertações</div>
            </div>
          </div>
          <BackButton />
          <Table data={researches} />
        </>
      ) : (
        <ErrorPage />
      )}
    </PageContainer>
  );
}
