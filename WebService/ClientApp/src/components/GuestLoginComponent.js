import React, {useState} from 'react';
import {Button, CardBody} from 'reactstrap';
import { Card, CardHeader, FormGroup, Input, Label } from 'reactstrap';

// Allows creating a person from specifying a name.
// It's the temporary substitute for a login.
// Meant for guests.
export function GuestLoginComponent({ person, addPerson }) {
    const [name, setName] = useState(person ? person.name : '');
    
    function handleChangeName(event) {
        setName(event.target.value);
    }

    return (
        <div>
            <Card>
                <CardHeader><h5>What's your name?</h5></CardHeader>
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
                        value={name}
                        onChange={handleChangeName}
                    />
                    </FormGroup>
                    <Button
                        color="primary"
                        onClick={() => {
                            addPerson(name);
                        }}>
                        Save
                    </Button>
                </CardBody>
            </Card>
        </div>
    );
}