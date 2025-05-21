import api from './_api'

export async function getExtensions(){
    return (await api.get("extensions"))?.data
}

export async function postExtensions(data) {
    return (await api.post("extensions", data))?.data
}

export async function deleteExtensions(id) {
    return await api.delete(`extensions/${id}`)
}