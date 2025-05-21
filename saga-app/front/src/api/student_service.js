import api from './_api'

export async function getStudents(){
    return (await api.get("api/Students"))?.data
}

export async function postStudents(data){
    return (await api.post("api/Students",data))?.data
}

export async function deleteStudent(id){
    return (await api.delete(`api/Students/${id}`))
}
export async function getStudentById(id){
    return (await api.get(`api/Students/${id}`))?.data
}

export async function putStudentById(id, data){
    return (await api.put(`api/Students/${id}`,data))?.data
}
export async function postStudentCsv(formData){
    return (await api.post(`api/Students/csv`,formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }))?.data
}

export async function postStudentCourseCsv(formData){
    return (await api.post(`api/Students/course/csv`,formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }))?.data
}
