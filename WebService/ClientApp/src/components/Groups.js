import React, {Component} from 'react';
import { Card, CardBody, CardText } from 'reactstrap';

export class Groups extends Component {
    static displayName = Groups.name;

    constructor(props) {
        super(props);
        this.state = {groups: null, loading: true};
    }

    componentDidMount() {
        this.populateGroupData();
    }
    
    visitGroup = (path) => {
        this.props.history.push(path);
    }

    static render(groups, visitGroup) {
        return (
            <div>
                {groups.map(group =>
                    <Card key={group.id}>
                        <CardBody>
                            <h5 className="card-title">{group.name || "New Group"}</h5>
                            <CardText>{group.state}</CardText>
                            {group.members.map(member => <p>Member: member.name</p>)}
                            <button className="btn btn-primary" onClick={() => visitGroup('/group/' + group.id)}>View Group</button>
                        </CardBody>
                    </Card>
                )}
            </div>
        );
    }

    addGroup = async () => {
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({userId: localStorage.getItem('userId')})
        };
        const response = await fetch('group', requestOptions);
        await this.populateGroupData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Groups.render(this.state.groups, this.visitGroup);

        return (
            <div>
                <h1 id="tabelLabel">Groups</h1>
                <button className="btn btn-primary" onClick={() => this.addGroup()}>Create Group</button>
                {contents}
            </div>
        );
    }

    async populateGroupData() {
        const response = await fetch('group');
        const data = await response.json();
        this.setState({groups: data, loading: false});
    }
}
