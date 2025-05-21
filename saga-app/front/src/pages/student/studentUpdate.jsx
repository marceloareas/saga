import React from 'react';
import UserForm from "../user/createUser";

export default function StudentUpdate()
{
    return <UserForm isUpdate={true} type={"Estudante"}/>
}