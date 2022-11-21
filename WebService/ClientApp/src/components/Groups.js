import React, {useEffect, useState} from 'react';
import { Card, CardBody, CardText } from 'reactstrap';
import {Link} from "react-router-dom";
import usePerson from "../usePerson";
import {JoinMeetingComponent} from "./JoinMeetingComponent";

export function Groups() {
    const [groups, setGroups] = useState([]);
    const [loading, setLoading] = useState(true);
    const {person, addPerson} = usePerson();

    async function populateGroupData() {
        const response = await fetch('api/group');
        const data = await response.json();
        setLoading(false);
        setGroups(data);
    }

    async function addGroup() {
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({userId: person.id})
        };
        await fetch('api/group', requestOptions);
        await populateGroupData();
    }
    
    useEffect(() => {
        if (loading) {
            populateGroupData()
                .then(() => {});
        }
    });

    if (loading) {
        return <p><em>Loading...</em></p>;
    }

    return (
        <div>
            <h1 id="tabelLabel">Groups</h1>
            {person &&
                <button className="btn btn-primary mb-3" onClick={() => addGroup()}>Create Group</button>
            }
            {!person && <div>
                <p>Sign in to create a group.</p>
                <JoinMeetingComponent person={person} addPerson={addPerson} />
            </div>}
            <div>
                {groups.map(group =>
                    <Card key={group.id}>
                        <CardBody>
                            <Link to={'/group/' + group.id}>
                                <h5 className="card-title">{group.name || "New Group"}</h5>
                            </Link>
                            <CardText>{group.state}</CardText>
                        </CardBody>
                    </Card>
                )}
            </div>
        </div>
    );
}
