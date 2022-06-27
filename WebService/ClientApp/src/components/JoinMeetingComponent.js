import React, { Component } from 'react';
import {Button, CardBody, CardTitle} from 'reactstrap';
import { Card, CardHeader, FormGroup, Input, Label } from 'reactstrap';

// Has a dialog for specifying resolution text.
export class JoinMeetingComponent extends Component {

    constructor(props) {
        super(props);
        this.state = { name: ''};
    }

    handleChangeName = (event) => {
        this.setState({name: event.target.value});
    }

    async createPerson() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName: this.state.name })
        };
        const response = await fetch('api/person/addPerson', requestOptions);
        const user = await response.json();
        this.props.onPersonCreate(user.id);
        localStorage.setItem('userId', user.id);
    }

    render = () => {
        return (
            <div>
                <Card>
                    <CardHeader><h5>Join the meeting</h5></CardHeader>
                    <CardBody>
                        <FormGroup className="mb-3">
                        <Label
                            className="me-sm-2"
                            for="memberName"
                        >
                            Name
                        </Label>
                        <Input
                            id="name"
                            name="name"
                            placeholder="Ex: Mohamed Khan"
                            type="name"
                            value={this.state.name}
                            onChange={this.handleChangeName}
                        />
                        </FormGroup>
                        <Button
                            color="primary"
                            onClick={() => this.createPerson()}>
                            Join Meeting
                        </Button>
                    </CardBody>
                </Card>
            </div>
        );
    }
}