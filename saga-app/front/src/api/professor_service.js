import api from './_api'

export async function getProfessors(){
    return (await api.get("professors"))?.data
}

export async function getProfessorById(id){
    return (await api.get(`professors/${id}`))?.data
}

export async function getProfessorsPaged({ page = 1, pageSize = 20, q = "" } = {}) {
  return (await api.get("professors/paged", { params: { page, pageSize, q } }))?.data;
}

export async function deleteProfessor(id){
    return (await api.delete(`professors/${id}`))
}

export async function putProfessorById(id, data){
    return (await api.put(`professors/${id}`,data))?.data
}

export async function postProfessors(data){
    return (await api.post("professors",data))?.data
}