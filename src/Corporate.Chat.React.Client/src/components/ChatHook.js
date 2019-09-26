import React, { useEffect, useState } from 'react';
import {
    Button,
    Input,
    InputGroupAddon,
    InputGroup,
    Badge,
    ListGroup,
    Form,
    FormGroup,
    Container, Row, Col
} from 'reactstrap';
import { HubConnectionBuilder } from '@aspnet/signalr';
import ListMessages from './ListMessages';
import WelcomeToast from './WelcomeToast';


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
    const [bindConnection, setBindConnection] = useState(false);
    const [bindUsername, setBindUsername] = useState(false);



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
        setConnected(true);
        console.log('Connection successful!');
    }

    const getUserName = () => {

        var nick = window.prompt('Type your nick:', '');
        while (!nick) {
            nick = window.prompt('Type your nick:', '');
        }

        setNick(nick);
    }

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

    useEffect(() => {

        if (!nick) {
            getUserName();
        }

    }, [nick])

    useEffect(() => {

        if (!connected && !bindUsername) {

            startConnection()
                .then(() => {
                    console.log('Start');
                    setConnected(true);
                }).catch((err) => {
                    console.log(err);
                });

        }

    }, [connected, bindUsername])


    useEffect(() => {

        if (connected && !bindUsername) {
            hubConnection.invoke('onUserConnected', {
                name: nick,
                text: ''
            });
            setShowMessage(!showMessage);
            setBindUsername(true);
        }

    }, [connected, bindUsername, hubConnection, nick, showMessage])


    useEffect(() => {

        if (hubConnection && connected && !bindConnection) {

            hubConnection.on('receiveMessage', (message) => {
                console.log(`receive message: ${JSON.stringify(message)}`);
                setMessages(messages => [...messages, message]);
            });

            hubConnection.on('userDisconnected', (message) => {
                console.log(`user disconnected': ${JSON.stringify(message)}`);
                setMessages(messages => [...messages, message]);
            });

            hubConnection.on('userConnected', (message) => {
                console.log(`user connected': ${JSON.stringify(message)}`);
                setMessages(messages => [...messages, message]);
            });

            hubConnection.connection.onclose = (error) => {
                setConnected(false);
                console.log(`disconnect: ${error.message}`);
            };

            setBindConnection(true);
        }

    }, [hubConnection, connected, messages, bindConnection]);



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
                    <WelcomeToast nick={nick} showMessage={showMessage} toggle={toggle} />
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
                            <ListMessages messages={messages} />
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
