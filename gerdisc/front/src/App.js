import React from 'react';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./pages/home";
import Login from "./pages/login";
import ResetPassword from "./pages/changePassword";
import StudentProfile from "./pages/student/profile";
import UserForm from './pages/user/createUser';
import StudentList from './pages/student/StudentList';
import ProfessorList from './pages/professor/professorList';
import ResearcherList from './pages/researcher/researcherList';

import ResearchLineList from './pages/researchLine/researchLineList';
import CreateResearchLine from './pages/researchLine/createResearchLine';

import ProjectList from './pages/projects/projectList';
import ProjectForm from './pages/projects/createProject';
import ExtensionList from './pages/extension/extensionList';
import ResearchForm from './pages/research/createResearch';
import ResearchList from './pages/research/researchList';
import ExtensionForm from './pages/extension/createExtension';
import CsvLoader from './pages/CsvLoader';
import StudentUpdate from './pages/student/studentUpdate';
import ResearcherUpdate from './pages/researcher/researcherUpdate';
import ProfessorUpdate from './pages/professor/professorUpdate';
import StudentForm from './pages/student/CreateStudent';

export default function App() {
  return (
    <Router basename="saga">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/changePassword" element={<ResetPassword />} />
        <Route path="/students/:id" element={<StudentProfile />} />
        <Route path="/students/:id/edit" element={<StudentUpdate/>} />
        <Route path="/students" element={<StudentList />} />
        <Route path="/professors" element={<ProfessorList />} />

        <Route path="/researchLine" element={<ResearchLineList />} />
        <Route path="/researchLine/add" element={<CreateResearchLine />} />
        <Route path="/researchLine/:id" element={<CreateResearchLine Update={true}/>} />

        <Route path="/researchers" element={<ResearcherList />} />
        <Route path="/researches" element={<ResearchList />} />
        <Route path="/extensions" element={<ExtensionList />} />
        <Route path="/projects" element={<ProjectList />} />
        <Route path="/user/add" element={<UserForm />} />
        <Route path="/entities/csv" element={<CsvLoader />} />
        <Route path="/students/:id/researches/add" element={<ResearchForm />} />
        <Route path="/students/add" element={<StudentForm />} />
        <Route path="/professors/add" element={<UserForm type={"Professor"}/>} />
        <Route path="/researches/add" element={<UserForm type={"Externo"}/>} />
        <Route path="/professors/:id" element={<ProfessorUpdate/>} />
        <Route path="/researchers/:id" element={<ResearcherUpdate />} />
        <Route path="/projects/add" element={<ProjectForm />} />
        <Route path="/projects/:id" element={<ProjectForm Update={true} />} />
        <Route path="/students/:id/extensions/add" element={<ExtensionForm />} />

      </Routes>
    </Router>
  );
}
