export const AREA_ENUM = [
    { key: 0, name: "Default", translation: "Padrão" },
    { key: 1, name: "COMPUTATION", translation: "Computação" },
    { key: 2, name: "EXACT_SCIENCES", translation: "Ciências Exatas" },
    { key: 3, name: "HUMANITIES", translation: "Humanidades" },
    { key: 4, name: "HEALTH", translation: "Saúde" },
    { key: 5, name: "ENGINEERING", translation: "Engenharia" },
  ];
  
  export const STATUS_ENUM = [
    { key: 0, name: "Default", translation: "Padrão" },
    { key: 1, name: "Active", translation: "Ativo" },
    { key: 2, name: "Graduated", translation: "Formado" },
    { key: 3, name: "Disconnected", translation: "Desconectado" },
  ];
  
  export const ROLES_ENUM = [
    { key: 0, name: "Default", translation: "Padrão" },
    { key: 1, name: "Student", translation: "Estudante" },
    { key: 2, name: "Professor", translation: "Professor" },
    { key: 3, name: "Administrator", translation: "Administrador" },
    { key: 4, name: "ExternalResearcher", translation: "Pesquisador Externo" },
    { key: 5, name: "ResetPassword", translation: "Redefinir Senha" },
  ];
  
  export const PROJECT_STATUS_ENUM = [
    { key: 0, name: "Default", translation: "Padrão" },
    { key: 1, name: "Active", translation: "Ativo" },
    { key: 2, name: "Inactive", translation: "Inativo" },
    { key: 3, name: "Closed", translation: "Encerrado" },
  ];
  
  export const INSTITUTION_TYPE_ENUM = [
    { key: 0, name: "Default", translation: "Padrão" },
    { key: 1, name: "Publica", translation: "Pública" },
    { key: 2, name: "Particular", translation: "Particular" },
    { key: 3, name: "CEFET", translation: "CEFET" },
  ];
  
  export const SCHOLARSHIP_TYPE = [
    { key: 0, name: "Default", translation: "Padrão" },
    { key: 1, name: "Cefet", translation: "Cefet" },
    { key: 2, name: "Capes", translation: "Capes" },
    { key: 3, name: "FapeRj", translation: "FapeRj" },
  ];

  export const translateEnumValue = (enumValues, value) => {
    const matchedValue = enumValues.find((item) => item.key === value || item.name === value);
    return matchedValue ? matchedValue.translation : "";
  };
  