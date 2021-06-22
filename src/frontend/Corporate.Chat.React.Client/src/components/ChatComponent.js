import React, { } from 'react';
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
const urlChat = 'http://localhost:5000/chat';


/**
 *  using Component
 */
export class ChatComponent extends React.Component {


    /**
     *
     */
    constructor(props) {
        super(props);
        this.state = {
            nick: '',
            messages: [],
            newMessage: '',
            hubConnection: null,
            connected: false,
            showMessage: false
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.toggle = this.toggle.bind(this);


    }

    

    toggle() {
        this.setState({
            showMessage: !this.state.showMessage
        });
    }

    sendUserName = () => {
        if (this.state.connected) {
            this.state.hubConnection.invoke('onUserConnected', {
                name: this.state.nick,
                text: ''
            });
            this.toggle();
        }
    }

    sendMessage = (text) => {
        const message = {
            text: text,
            name: this.state.nick
        };

        this.state.hubConnection.invoke('send', message)
            .catch(err => console.error(err));
    }

    startConnection = async () => {

        const hubConnect = new HubConnectionBuilder()
            .withUrl(urlChat)
            .build();

        this.setState({ hubConnection: hubConnect });

        try {
            await hubConnect.start()
            console.log('Connection successful!')
            this.setState({ connected: true });
            this.sendUserName();
        }
        catch (err) {
            alert(err);
            this.setState({ connected: false });
        }

        this.state.hubConnection.on('receiveMessage', (message) => {
            console.log(`receive message: ${JSON.stringify(message)}`);
            this.setState({ messages: this.state.messages.concat(message) });
        });

        this.state.hubConnection.on('userDisconnected', (message) => {
            console.log(`user disconnected': ${JSON.stringify(message)}`);
            this.setState({ messages: this.state.messages.concat(message) });
        });

        this.state.hubConnection.on('userConnected', (message) => {
            console.log(`user connected': ${JSON.stringify(message)}`);
            this.setState({ messages: this.state.messages.concat(message) });
        });

        this.hubConnection.connection.onclose = (error) => {
            this.setState({ connected: false });
            console.log(`disconnect: ${error.message}`);
        };

    }

    componentDidMount() {

        var nick = window.prompt('Type your nick:', '');
        while(!nick){
            nick = window.prompt('Type your nick:', '');
        }
        this.setState({ nick });

        this.startConnection()
            .then(() => {
                console.log('Start!')

            }).catch(() => {
                console.log('Error')
            });
    }



    handleSubmit = async (e) => {
        e.preventDefault();

        if (this.state.newMessage) {
            this.sendMessage(this.state.newMessage);
            this.setState({ newMessage: '' })
        }
    }

    handleChange(e) {
        this.setState({ newMessage: e.target.value })
    }

    render() {
        return (
            <div >
                <Container className="container">
                    <div className="App-toast-div">
                        <Row>
                            <Col sm="12" md={{ size: 6, offset: 3 }}>
                                <Toast className="App-toast" isOpen={this.state.showMessage}>
                                    <ToastHeader toggle={this.toggle}>Hi</ToastHeader>
                                    <ToastBody>Welcome, {this.state.nick}!</ToastBody>
                                </Toast>
                            </Col>
                        </Row>
                    </div>
                    <br />
                    <Row>
                        <Col>
                            <Badge className="App-badge" color={(this.state.connected ? 'success' : 'danger')} pill>Connection Status</Badge>
                        </Col>
                    </Row>
                    <hr />
                    <br />
                    <Row>
                        <Col>
                            <ListGroup className="App-list">
                                {
                                    this.state.messages.map(function (message, key) {
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
                    <Form onSubmit={this.handleSubmit} >
                        <FormGroup>
                            <InputGroup>
                                <Input type="text" className="form-control" id="message"
                                    onChange={this.handleChange} value={this.state.newMessage} placeholder="Type message..." />
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
}

export default ChatComponent;
