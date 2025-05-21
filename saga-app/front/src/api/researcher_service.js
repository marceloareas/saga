import api from './_api'

export async function getResearchers(){
    return (await api.get("externalResearchers"))?.data
}

export async function getResearcherById(id){
    return (await api.get(`externalResearchers/${id}`))?.data
}

export async function postResearchers(data) {
    return (await api.post("externalResearchers", data))?.data
}

export async function putResearcherById(id, data) {
    return (await api.put(`externalResearchers/${id}`, data))?.data
}