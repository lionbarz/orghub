import React, {useEffect, useState} from 'react';
import {Button, CardBody} from 'reactstrap';
import { Card, CardHeader, FormGroup, Input, Label } from 'reactstrap';
import usePerson from "../usePerson";

// Has a dialog for specifying resolution text.
export function JoinMeetingComponent() {
    const [name, setName] = useState('');
    const {person, setPerson} = usePerson();
    
    function handleChangeName(event) {
        setName(event.target.value);
    }
    
    useEffect(() => {
       if (person) {
           setName(person.name);
       } 
    }, [person]);

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
                        onClick={() => setPerson(name)}>
                        Save
                    </Button>
                </CardBody>
            </Card>
        </div>
    );
}