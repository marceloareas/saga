import api from './_api'

export async function getProjects() {
    return (await api.get("projects"))?.data
}
export async function getProjectById(id) {
    return (await api.get(`projects/${id}`))?.data
}

export async function postProjects(data) {
    return (await api.post("projects", data))?.data
}

export async function putProjectsById(id, data) {
    return (await api.put(`projects/${id}`, data))?.data
}