import React from 'react';
import UserForm from "../user/createUser";

export default function ProfessorUpdate()
{
    return <UserForm isUpdate={true} type={"Professor"}/>
}