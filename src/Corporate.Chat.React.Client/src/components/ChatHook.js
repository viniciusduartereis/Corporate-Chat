import React, { useEffect, useState } from 'react';
import {
    Button,
    Input,
    InputGroupAddon,
    InputGroup,
    Badge,
    ListGroup,
    ListGroupItem,
    Form,
    FormGroup,
    Toast, ToastBody, ToastHeader,
    Container, Row, Col
} from 'reactstrap';
import { HubConnectionBuilder } from '@aspnet/signalr';

import Moment from 'react-moment';
import 'moment/locale/pt-br';

Moment.globalFormat = 'L LTS';
const urlChat = 'http://localhost:32080/chat';

/**
 *  version using Hooks
 */
export function ChatHook() {

    const [nick, setNick] = useState('');
    const [messages, setMessages] = useState([]);
    const [hubConnection, setHubconnection] = useState(null);
    const [connected, setConnected] = useState(false);
    const [showMessage, setShowMessage] = useState(false);
    const [newMessage, setNewMessage] = useState('');

    const sendUserName = () => {
        if (connected) {
            hubConnection.invoke('onUserConnected', {
                name: nick,
                text: ''
            });
            toggle();
        }
    }


    const startConnection = async () => {

        const hubConnect = new HubConnectionBuilder()
            .withUrl(urlChat)
            .build();

        try {
            await hubConnect.start();
        } catch (err) {
            alert(err);
            setConnected(false);
        }

        setHubconnection(hubConnect);
        console.log('Connection successful!');
    }

    const getUserName = () => {
        var nick = window.prompt('Type your nick:', '');
        while (!nick) {
            nick = window.prompt('Type your nick:', '');
        }

        setNick(nick);
    }


    useEffect(() => {
        let cancel = false;

        if (!nick) {
            getUserName();
        }

        return () => {
            cancel = true;
        }

    }, [nick, setNick])

    useEffect(() => {


        if (!connected) {

            startConnection()
                .then(() => {
                    console.log('Start');
                    setConnected(true);
                    sendUserName();
                }).catch((err) => {
                    console.log(err);
                });

        }

    }, [connected, setConnected, setHubconnection])


    useEffect(() => {

        if (hubConnection) {

            hubConnection.on('receiveMessage', (message) => {
                console.log(`receive message: ${JSON.stringify(message)}`);
                setMessages([...messages, message]);
            });

            hubConnection.on('userDisconnected', (message) => {
                console.log(`user disconnected': ${JSON.stringify(message)}`);
                setMessages([...messages, message]);
            });

            hubConnection.on('userConnected', (message) => {
                console.log(`user connected': ${JSON.stringify(message)}`);
                setMessages([...messages, message]);
            });

            hubConnection.connection.onclose = (error) => {
                setConnected(false);
                console.log(`disconnect: ${error.message}`);
            };
        }

    }, [hubConnection, messages])



    const toggle = () => {
        setShowMessage(!showMessage);
    }

    const sendMessage = (text) => {
        const message = {
            text: text,
            name: nick
        };

        hubConnection.invoke('send', message)
            .catch(err => console.error(err));
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (newMessage) {
            sendMessage(newMessage);
            setNewMessage('');
        }
    }

    const handleChange = (e) => {
        setNewMessage(e.target.value)
    }


    return (
        <div >
            <Container className="container">
                <div className="App-toast-div">
                    <Row>
                        <Col sm="12" md={{ size: 6, offset: 3 }}>
                            <Toast className="App-toast" isOpen={showMessage}>
                                <ToastHeader toggle={toggle}>Hi</ToastHeader>
                                <ToastBody>Welcome, {nick}!</ToastBody>
                            </Toast>
                        </Col>
                    </Row>
                </div>
                <br />
                <Row>
                    <Col>
                        <Badge className="App-badge" color={(connected ? 'success' : 'danger')} pill>Connection Status</Badge>
                    </Col>
                </Row>
                <hr />
                <br />
                <Row>
                    <Col>
                        <ListGroup className="App-list">
                            {
                                messages.map(function (message, key) {
                                    return <ListGroupItem className="App-list-item" key={key}>
                                        <strong>
                                            {message.name}</strong>:&nbsp;&nbsp;
                                                {message.text}
                                        <br />
                                        <Moment className="datetime"
                                            element="i" locale="pt-br">{message.createdDate}</Moment>
                                    </ListGroupItem>
                                })
                            }
                        </ListGroup>
                    </Col>
                </Row>
                <br />
                <Form onSubmit={handleSubmit} >
                    <FormGroup>
                        <InputGroup>
                            <Input type="text" className="form-control" id="message"
                                onChange={handleChange} value={newMessage} placeholder="Type message..." />
                            <InputGroupAddon addonType="append">
                                <Button type="submit" color="primary">Send</Button>
                            </InputGroupAddon>
                        </InputGroup>
                    </FormGroup>
                </Form>
            </Container>
        </div >
    );
}

export default ChatHook;
