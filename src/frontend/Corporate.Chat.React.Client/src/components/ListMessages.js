import React from 'react';
import { ListGroup, ListGroupItem, Col, Row } from 'reactstrap';

import Moment from 'react-moment';
import 'moment/locale/pt-br';
Moment.globalFormat = 'L LTS';

function ListMessages({ messages }) {
    return (
        <Row>
            <Col>
                <ListGroup className="App-list">
                    {messages.map((message, key) =>
                        <ListGroupItem className="App-list-item" key={`App-list-item-${key}`}>
                            <strong>
                                {message.name}
                            </strong>:&nbsp;&nbsp;
                            {message.text}
                            <br />
                            <Moment className="datetime"
                                element="i" locale="pt-br">{message.createdDate}</Moment>
                        </ListGroupItem>
                    )}
                </ListGroup>
            </Col>
        </Row>
    )
}

export default ListMessages;