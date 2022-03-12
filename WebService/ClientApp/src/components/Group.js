import React, { Component } from 'react';
import { withRouter } from "react-router";

export class Group extends Component {
    static displayName = Group.name;
    userId = localStorage.getItem('userId');

    constructor(props) {
        super(props);
        this.state = { group: null, loading: true, resolution: "" };
    }

    componentDidMount() {
        this.populateGroupData();
    }

    static render(group) {
        return (
            <div>
                <p>ID: {group.id}</p>
                <p>Name: {group.name}</p>
                <p>Chair: {group.chair.name}</p>
                <p>State: {group.state}</p>
                {group.members.map(member => <p>Member: {member.name}</p>)}
                {group.resolutions.map(text => <p>Resolution: {text}</p>)}
            </div>
        );
    }

    async takeAction(action) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/${action}`, requestOptions);
        await this.populateGroupData();
    }

    async moveElectChair(nomineeName) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nomineeName: nomineeName, userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/electchair`, requestOptions);
        await this.populateGroupData();
    }

    async moveResolution(text) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ text: text, userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/moveresolution`, requestOptions);
        await this.populateGroupData();
    }

    handleChangeResolution = (event) => {
        this.setState({resolution: event.target.value});
    }
    
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Group.render(this.state.group);

        return (
            <div>
                <h1 id="tabelLabel" >Group info</h1>
                <p>You are: {this.userId}</p>
                {contents}
                <button className="btn btn-primary" onClick={() => this.takeAction("calltoorder")}>Call to order</button>
                <button className="btn btn-primary" onClick={() => this.takeAction("adjourn")}>Adjourn</button>
                <button className="btn btn-primary" onClick={() => this.takeAction("declaremotionpassed")}>Declare motion passed</button>
                <button className="btn btn-primary" onClick={() => this.moveElectChair("Rawan Hammoud")}>Elect Roni chair</button>
                <button className="btn btn-primary" onClick={() => this.moveResolution(this.state.resolution)}>Resolution</button>
                <input type="text" value={this.state.resolution} onChange={this.handleChangeResolution} />
            </div>
        );
    }

    async populateGroupData() {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}`);
        const data = await response.json();
        this.setState({ group: data, loading: false });
    }
}
