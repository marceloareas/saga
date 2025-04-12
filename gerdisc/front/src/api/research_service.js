import api from './_api'

export async function getResearch(){
    return (await api.get("orientations"))?.data
}

export async function postResearch(data){
    return (await api.post("orientations",data))?.data
}