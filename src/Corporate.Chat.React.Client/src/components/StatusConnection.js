import React from 'react';
import { Row, Col, Badge } from 'reactstrap';

function StatusConnection({ connected }) {
    return (
        <Row>
            <Col>
                <Badge className="App-badge" color={(connected ? 'success' : 'danger')} pill>Connection Status</Badge>
            </Col>
        </Row>
    )
}

export default StatusConnection;