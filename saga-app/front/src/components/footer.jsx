import React from 'react';
import '../styles/footer.scss'
export default function Footer ()
{
    return <div className="footer">
        <img src={process.env.PUBLIC_URL + '/ppcic.jpg'} alt="ppcic's logo"  height={"250rem"}/>
    </div>
}