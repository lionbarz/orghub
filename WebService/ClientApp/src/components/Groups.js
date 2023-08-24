import React, {useEffect, useState} from 'react';
import { Card, CardBody, CardText } from 'reactstrap';
import {Link} from "react-router-dom";
import usePerson from "../usePerson";
import {GuestLoginComponent} from "./GuestLoginComponent";
import { useHistory } from "react-router-dom";

export function Groups() {
    const history = useHistory();
    const [groups, setGroups] = useState([]);
    const [loading, setLoading] = useState(true);
    const {person, addPerson} = usePerson();

    async function populateGroupData() {
        const response = await fetch('api/group');
        const data = await response.json();
        setLoading(false);
        setGroups(data);
    }

    async function handleAddGroup() {
        history.push('/add-group');
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
            <div>
                {groups.length === 0 && <p>There are no groups yet.</p>}
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
            <button className="btn btn-primary mb-3" onClick={() => handleAddGroup()}>Create a new group</button>
        </div>
    );
}
