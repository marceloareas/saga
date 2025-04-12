import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import '../styles/backButton.scss'
import { useNavigate } from 'react-router';


export default function BackButton() {
    const navigate = useNavigate();

    function handleClick(e) {
        e.preventDefault();
        navigate(-1)
    }

    return <div className="back-button" onClick={(e) => handleClick(e)}>
        <FontAwesomeIcon icon={faArrowLeft} color="white" height={"1rem"} width="1rem" />
    </div>
}

