import {Button, Card, CardBody, FormGroup, Input, Label} from "reactstrap";
import {useState} from "react";
import usePerson from "../usePerson";
import moment from "moment-timezone";

export function AddMassMeeting() {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [location, setLocation] = useState('');
    const [time, setTime] = useState('');
    const timeZone = moment.tz.guess();
    
    let {person, addPerson} = usePerson();
    
    async function submit() {
        if (!person) {
            person = await addPerson(name);
        }
        
        if (!person.id) {
            alert('No person!');
            return;
        }

        const isoTime = (new Date(Date.parse(time))).toISOString();
        
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                personId: person.id,
                nextMeeting: {
                    description: description,
                    startTime: isoTime,
                    location: location
                }
            })
        };
        await fetch('api/group', requestOptions);
    }
    
    return (<div>
        <h1>Create a public meeting</h1>
        <p>Collective action starts with one person calling for a meeting!</p>
        <p>Use the form below to create a meeting with a link that you can send to people.</p>
        <p>Anyone who joins the meeting will be able to vote, debate, make suggestions, lead the meeting, and more. You can vote to create a permanent group with a name, mission, and membership, or just make a few decisions.</p>
        <Card>
            <CardBody>
                {!person &&
                    <FormGroup className="mb-3">
                        <Label
                            className="me-sm-2"
                            for="name"
                        >
                            What's your name?
                        </Label>
                        <Input
                            id="name"
                            name="name"
                            placeholder="Ex: Daniel Khan"
                            type="text"
                            value={name}
                            onChange={(event) => {
                                setName(event.target.value)
                            }}
                        />
                    </FormGroup>
                }
                <FormGroup className="mb-3">
                    <Label
                        className="me-sm-2"
                        for="description"
                    >
                        What's the purpose of this meeting?
                    </Label>
                    <Input
                        id="description"
                        name="description"
                        placeholder="Ex: Discuss neighborhood issues"
                        type="text"
                        value={description}
                        onChange={(event) => {setDescription(event.target.value)}}
                    />
                </FormGroup>
                <FormGroup className="mb-3">
                    <Label
                        className="me-sm-2"
                        for="location"
                    >
                        Where is the meeting?
                    </Label>
                    <Input
                        id="location"
                        name="location"
                        placeholder="Ex: Zoom link or address"
                        type="text"
                        value={location}
                        onChange={(event) => {setLocation(event.target.value)}}
                    />
                </FormGroup>
                <FormGroup className="mb-3">
                    <Label
                        className="me-sm-2"
                        for="time"
                    >
                        When is the meeting?
                    </Label>
                    <Input
                        id="time"
                        name="time"
                        type="datetime-local"
                        value={time}
                        onChange={(event) => {setTime(event.target.value)}}
                    />
                    <small>This is in your current time zone: {timeZone}</small>
                </FormGroup>
                <Button
                    color="primary"
                    onClick={async () => {
                        await submit();
                    }}>
                    Create
                </Button>
            </CardBody>
        </Card>
        
    </div>);
}