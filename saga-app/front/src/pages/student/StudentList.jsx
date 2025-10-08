import { useEffect, useState, useCallback } from "react";
import "../../styles/studentList.scss";
import Table from "../../components/Table/table";
import { getStudents, getStudentsPaged } from "../../api/student_service";
import { useNavigate } from "react-router";
import jwt_decode from "jwt-decode";
import BackButton from "../../components/BackButton";
import PageContainer from "../../components/PageContainer";
import { translateEnumValue, STATUS_ENUM } from "../../enum_helpers";

export default function StudentList() {
  const navigate = useNavigate();
  const [name] = useState(localStorage.getItem("name"));
  const [role, setRole] = useState(localStorage.getItem("role"));
  const [isLoading, setIsLoading] = useState(true);
  const [students, setStudents] = useState([]);

  // q = texto digitado; term = termo efetivo de busca (só muda ao clicar "Buscar")
  const [q, setQ] = useState("");
  const [term, setTerm] = useState("");

  // Modo paginado só após clicar "Buscar"
  const [isPaged, setIsPaged] = useState(false);

  const [page, setPage] = useState(1);
  const [pageSize] = useState(20);
  const [totalPages, setTotalPages] = useState(1);

  // Auth gate
  useEffect(() => {
    const roles = ["Administrator", "Professor"];
    const token = localStorage.getItem("token");
    try {
      const decoded = jwt_decode(token);
      if (!roles.includes(decoded.role)) {
        navigate("/");
      }
      setRole(decoded.role);
    } catch {
      navigate("/login");
    }
  }, [navigate, setRole]);

  // Mapeia alunos para a tabela
  const mapStudents = (items = []) =>
    items.map((student) => ({
      Id: student.id,
      Nome: `${student.firstName} ${student.lastName}`,
      Status: translateEnumValue(STATUS_ENUM, student.status),
      "E-mail": student.email,
      "Matrícula": student.registration,
      "Data de defesa": student.projectDefenceDate,
      "Data de qualificação": student.projectQualificationDate,
    }));

  // Carrega lista NÃO paginada (inicial / após "Limpar")
  const fetchAll = useCallback(async () => {
    setIsLoading(true);
    try {
      const result = await getStudents(); // GET /students
      setStudents(mapStudents(result || []));
      setTotalPages(1);
      setPage(1);
    } catch {
      // noop
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Carrega lista paginada (apenas após "Buscar")
  const fetchPage = useCallback(async () => {
    if (!isPaged) return; // segurança
    setIsLoading(true);
    try {
      const result = await getStudentsPaged({ page, pageSize, q: term }); // GET /students/paged
      setStudents(mapStudents(result?.items || []));
      if (result?.totalPages) setTotalPages(result.totalPages);
      if (result?.page) setPage(result.page);
    } catch {
      // noop
    } finally {
      setIsLoading(false);
    }
  }, [isPaged, page, pageSize, term]);

  // Efeito principal:
  // - Se NÃO paginado: carrega tudo só na montagem e quando sair do modo paginado
  // - Se paginado: recarrega a cada mudança de page/term
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
      <div className="studentBar">
        <div className="left-bar">
          <div>
            <img
              src="student.png"
              alt="A logo representing students"
              height={"100rem"}
            />
          </div>
          <div className="title">Estudantes</div>
        </div>
        {role === "Administrator" && (
          <div className="right-bar">
            <div className="search">
              <input
                type="search"
                name="search"
                id="search"
                value={q}
                onChange={(e) => setQ(e.target.value)}
                placeholder="Buscar por nome/email/matrícula"
              />
              <button onClick={onSearch}>Buscar</button>
              <button onClick={onClear} style={{ marginLeft: 8 }}>
                Limpar
              </button>
            </div>
            <div className="create-button">
              <button onClick={() => navigate("/students/add")}>
                Novo Estudante
              </button>
            </div>
          </div>
        )}
      </div>
      <BackButton />
      <Table
        data={students}
        useOptions={true}
        detailsCallback={(id) => navigate(`${id}`)}
      />
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
