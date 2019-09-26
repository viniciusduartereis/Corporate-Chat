import React from 'react';
import { Toast, ToastBody, ToastHeader, Row, Col } from 'reactstrap';

function WelcomeToast({ nick, showMessage, toggle }) {
    return (
        <Row>
            <Col sm="12" md={{ size: 6, offset: 3 }}>
                <Toast className="App-toast" isOpen={showMessage}>
                    <ToastHeader toggle={toggle}>Hi</ToastHeader>
                    <ToastBody>Welcome, {nick}!</ToastBody>
                </Toast>
            </Col>
        </Row>
    )
}

export default WelcomeToast;