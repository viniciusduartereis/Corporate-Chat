import React from 'react';
import {
    ListGroupItem,
} from 'reactstrap';
import Moment from 'react-moment';

import 'moment/locale/pt-br';

Moment.globalFormat = 'L LTS';

function GetMessages({ messages }) {
    return (
        messages.map((message, key) =>
            <ListGroupItem className="App-list-item" key={`App-list-item-${key}`}>
                <strong>
                    {message.name}</strong>:&nbsp;&nbsp;
                    {message.text}
                <br />
                <Moment className="datetime"
                    element="i" locale="pt-br">{message.createdDate}</Moment>
            </ListGroupItem>
        )
    )
}

export default GetMessages;