import React from 'react';
import {
    Button,
    Input,
    InputGroupAddon,
    InputGroup,
    Form,
    FormGroup
} from 'reactstrap';

function FormChat({ handleSubmit, handleChange, newMessage }) {
    return (
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
    )
}

export default FormChat;