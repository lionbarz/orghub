import React, {useState} from 'react';
import {Button, CardBody} from 'reactstrap';
import { Card, CardHeader, FormGroup, Input, Label } from 'reactstrap';

// Allows creating a person from specifying a name.
// It's the temporary substitute for a login.
// Also uses an email to correlate with people
// who are added to groups but aren't yet registered.
export function GuestLoginComponent({ person, addPerson }) {
    const [name, setName] = useState(person ? person.name : '');
    const [email, setEmail] = useState(person ? person.email : '');
    
    function handleChangeName(event) {
        setName(event.target.value);
    }

    function handleChangeEmail(event) {
        setEmail(event.target.value);
    }

    return (
        <div>
            <Card>
                <CardHeader><h5>Register</h5></CardHeader>
                <CardBody>
                    <FormGroup className="mb-3">
                    <Label
                        className="me-sm-2"
                        for="name"
                    >
                        Name
                    </Label>
                    <Input
                        id="name"
                        name="name"
                        placeholder="Mohamed Fakhreddine"
                        value={name}
                        onChange={handleChangeName}
                    />
                    <Label
                        className="me-sm-2"
                        for="email"
                    >
                        Email
                    </Label>
                    <Input
                        id="email"
                        name="email"
                        placeholder="you@domain.com"
                        type="email"
                        value={email}
                        onChange={handleChangeEmail}
                    />
                    </FormGroup>
                    <Button
                        color="primary"
                        onClick={() => {
                            addPerson(name, email);
                        }}>
                        Register
                    </Button>
                </CardBody>
            </Card>
        </div>
    );
}