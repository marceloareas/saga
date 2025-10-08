import { useEffect, useState, useCallback } from "react";
import "../../styles/professorList.scss";
import Table from "../../components/Table/table";
import { getProfessors, getProfessorsPaged } from "../../api/professor_service";
import { useNavigate } from "react-router";
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import PageContainer from "../../components/PageContainer";

export default function ProfessorList() {
  const navigate = useNavigate();
  const [name] = useState(localStorage.getItem("name"));
  const [isLoading, setIsLoading] = useState(true);
  const [professors, setProfessors] = useState([]);

  // q = texto digitado; term = termo efetivo da busca (só muda ao clicar "Buscar")
  const [q, setQ] = useState("");
  const [term, setTerm] = useState("");

  // Modo paginado só após clicar "Buscar"
  const [isPaged, setIsPaged] = useState(false);

  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);
  const [totalPages, setTotalPages] = useState(1);

  const detailsCallback = (id) => navigate(id);

  // Auth gate (somente Administrador)
  useEffect(() => {
    const roles = ["Administrator"];
    const token = localStorage.getItem("token");
    try {
      const decoded = jwt_decode(token);
      if (!roles.includes(decoded.role)) navigate("/");
    } catch {
      navigate("/login");
    }
  }, [navigate]);

  const mapProfessors = (items = []) =>
    items.map((professor) => ({
      Id: professor.id,
      Nome: `${professor.firstName} ${professor.lastName}`,
      "E-mail": professor.email,
      Siape: professor.siape,
    }));

  // Lista NÃO paginada (inicial / após "Limpar")
  const fetchAll = useCallback(async () => {
    setIsLoading(true);
    try {
      const result = await getProfessors(); // GET /professors
      setProfessors(mapProfessors(result || []));
      setTotalPages(1);
      setPage(1);
    } catch {
      // noop
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Lista paginada (somente após "Buscar")
  const fetchPage = useCallback(async () => {
    if (!isPaged) return;
    setIsLoading(true);
    try {
      const result = await getProfessorsPaged({ page, pageSize, q: term }); // GET /professors/paged
      setProfessors(mapProfessors(result?.items || []));
      if (result?.totalPages) setTotalPages(result.totalPages);
      if (result?.page) setPage(result.page);
    } catch {
      // noop
    } finally {
      setIsLoading(false);
    }
  }, [isPaged, page, pageSize, term]);

  useEffect(() => {
    if (isPaged) {
      fetchPage();
    } else {
      fetchAll();
    }
  }, [isPaged, fetchAll, fetchPage]);

  const onSearch = () => {
    setPage(1);
    setTerm(q);
    setIsPaged(true);
  };

  const onClear = () => {
    setQ("");
    setTerm("");
    setIsPaged(false);
  };

  const prev = () => {
    if (page > 1) setPage((p) => p - 1);
  };
  const next = () => {
    if (page < totalPages) setPage((p) => p + 1);
  };

  return (
    <PageContainer name={name} isLoading={isLoading}>
      <div className="bar professorBar">
        <div className="left-bar">
          <div>
            <img
              src="professor.png"
              alt="A logo representing professors"
              height={"100rem"}
            />
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
              onChange={(e) => setQ(e.target.value)}
              placeholder="Buscar por nome/email"
            />
            <button onClick={onSearch}>Buscar</button>
            <button onClick={onClear} style={{ marginLeft: 8 }}>
              Limpar
            </button>
          </div>
          <div className="create-button">
            <button onClick={() => ""}>Mostrar inativos</button>
          </div>
          <div className="create-button">
            <button onClick={() => navigate("/professors/add")}>
              Novo Professor
            </button>
          </div>
        </div>
      </div>
      <BackButton />
      {!isLoading && (
        <Table
          data={professors}
          useOptions={true}
          detailsCallback={detailsCallback}
        />
      )}
      {isPaged && (
        <div
          style={{
            display: "flex",
            gap: 8,
            justifyContent: "flex-end",
            marginTop: 12,
          }}
        >
          <button disabled={page <= 1} onClick={prev}>
            Anterior
          </button>
          <span>
            Página {page} de {totalPages}
          </span>
          <button disabled={page >= totalPages} onClick={next}>
            Próxima
          </button>
        </div>
      )}
    </PageContainer>
  );
}
