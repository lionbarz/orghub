import React, { Component } from 'react';
import {Link} from "react-router-dom";
import {NavLink} from "reactstrap";

export class Groups extends Component {
    static displayName = Groups.name;

    constructor(props) {
        super(props);
        this.state = { groups: null, loading: true };
    }

    componentDidMount() {
        this.populateGroupData();
    }

    static render(groups) {
        return (
            <div>
                {groups.map(group =>
                    <div key={group.id}>
                        <NavLink tag={Link} className="text-dark" to={"/group/" + group.id}>Link</NavLink>
                        <p>ID: {group.id}</p>
                        <p>Name: {group.name}</p>
                        <p>Chair: {group.chair.name}</p>
                        <p>State: {group.state}</p>
                        {group.members.map(member => <p>Member: member.name</p>)}
                    </div>
                )}
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Groups.render(this.state.groups);

        return (
            <div>
                <h1 id="tabelLabel" >Groups</h1>
                {contents}
            </div>
        );
    }

    async populateGroupData() {
        const response = await fetch('group');
        const data = await response.json();
        this.setState({ groups: data, loading: false });
    }
}
