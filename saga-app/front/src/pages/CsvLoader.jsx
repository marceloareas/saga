import React, { useEffect, useState } from 'react';
import CSVReader from 'react-csv-reader';
import PageContainer from '../components/PageContainer';
import Table from '../components/Table/table';
import '../styles/form.scss'
import '../styles/csvLoader.scss'
import Select from '../components/select';
import jwt_decode from 'jwt-decode';
import { useNavigate } from 'react-router';
import { postStudentCsv, postStudentCourseCsv } from '../api/student_service';
import ErrorPage from '../components/error/Error';

const CsvLoader = () => {
    const [fileData, setFileData] = useState(null);
    const [file, setFile] = useState(null);
    const [name,] = useState(localStorage.getItem('name'))
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState(false)
    const [entity, setEntity] = useState('Estudantes')

    const handleFileLoaded = (data,file_info, originalFile) => {
        setFileData(data);
        setFile(originalFile)
    };
    const navigate = useNavigate()

    useEffect(() => {
        const roles = ['Administrator']
        const token = localStorage.getItem('token')
        try {
            const decoded = jwt_decode(token)
            if (!roles.includes(decoded.role)) {
                navigate('/')
            }
        } catch (error) {
            navigate('/login')
        }
    }, [navigate]);

    const handleFileUpload = () => {
        const formData = new FormData();
        formData.append('file', file);
        setIsLoading(true);
        if(entity === 'Estudantes')
        {       
            postStudentCsv(formData)
                .then((response) => {
                    navigate('/')
                })
                .catch((error) => {
                    setError(true);
                });
        }
        else if (entity === 'Materias cursados')
        {
            postStudentCourseCsv(formData)
                .then((response) => {
                    navigate('/')
                })
                .catch((error) => {
                    setError(true);
                }); 
        }
        setIsLoading(false)
    };

    const options = {
        header: true,
        preview: 5
    }
    return (
        <PageContainer name={name} isLoading={isLoading}>
            { !error &&
            <div className='csv-loader'>
            <div className="form csv">
                <div className='form-section'>
                    <div className='formInput'>
                    <Select
                        onSelect={setEntity}
                        options={["Estudantes", "Materias cursados"].map((option) => ({
                            value: option,
                            label: option,
                        }))}
                        label={"Entidade a criar"}
                        name={"entity"}
                        />
                    </div>
                </div>
                <div className='form-section'>
                    <CSVReader
                        onFileLoaded={handleFileLoaded}
                        cssClass='reader formInput'
                        label={'Escolher'}
                        cssLabelClass='file-label'
                        inputId='file-input'
                        name='file-input'
                        parserOptions={options} />
                    <div className='form-section'>
                        <div className='formInput'>
                            <input type={'submit'} value="Anexar" onClick={(e) => handleFileUpload()} />
                        </div>
                    </div>
                </div>
            </div>
            {fileData && <Table data={fileData} />}
            </div>}
            {error && <ErrorPage/>}
        </PageContainer>)
};

export default CsvLoader;
