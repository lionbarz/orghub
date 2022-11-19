import React, {useEffect, useState} from 'react';
import { Card, CardBody, CardText } from 'reactstrap';
import {Link} from "react-router-dom";

export function Groups({history}) {
    const [groups, setGroups] = useState([]);
    const [loading, setLoading] = useState(true);

    async function populateGroupData() {
        const response = await fetch('api/group');
        const data = await response.json();
        setLoading(false);
        setGroups(data);
    }

    function visitGroup(path) {
        history.push(path);
    }

    async function addGroup() {
        let userId = localStorage.getItem('userId');
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({userId: userId})
        };
        await fetch('api/group', requestOptions);
        await populateGroupData();
    }
    
    useEffect(() => {
        if (loading) {
            populateGroupData();
        }
    });

    if (loading) {
        return <p><em>Loading...</em></p>;
    }

    return (
        <div>
            <h1 id="tabelLabel">Groups</h1>
            <button className="btn btn-primary mb-3" onClick={() => addGroup()}>Create Group</button>
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
