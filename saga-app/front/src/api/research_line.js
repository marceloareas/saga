import api from './_api'

export async function getResearchLines(){
    return (await api.get("researchLines"))?.data
}
export async function getResearchLineById(id) {
    return (await api.get(`researchLines/${id}`))?.data
}

export async function postResearchLines(data) {
    return (await api.post("researchLines", data))?.data
}

export async function putResearchLinesById(id, data) {
    console.log("Enviando dados:", data);
    try {
        return (await api.put(`researchLines/${id}`, data))?.data
      } catch (error) {
        console.error("Error fetching research lines:", error);
        throw error; 
      }
}
