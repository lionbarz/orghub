import React, { Component } from 'react';

export class Meeting extends Component {
    static displayName = Meeting.name;

    constructor(props) {
        super(props);
        this.state = { meeting: null, loading: true };
    }

    componentDidMount() {
        this.populateMeetingData();
    }

    static renderMeeting(meeting) {
        return (
            <div>
                <p>Chair: {meeting.chair}</p>
                <p>State: {meeting.state}</p>
                <p>Is Quorum: ??</p>
            </div>
        );
    }

    async takeActionCallToOrder() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ meetingIdString: this.state.meeting.id, actionName: "start" })
        };
        await fetch('meeting', requestOptions);
        await this.populateMeetingData();
    }

    async takeActionSpeak() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ meetingIdString: this.state.meeting.id, actionName: "speak" })
        };
        await fetch('meeting', requestOptions);
        await this.populateMeetingData();
    }

    async takeActionYield() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ meetingIdString: this.state.meeting.id, actionName: "yield" })
        };
        await fetch('meeting', requestOptions);
        await this.populateMeetingData();
    }

    async takeActionElectChair() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ meetingIdString: this.state.meeting.id, actionName: "electChair" })
        };
        await fetch('meeting', requestOptions);
        await this.populateMeetingData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Meeting.renderMeeting(this.state.meeting);

        return (
            <div>
                <h1 id="tabelLabel" >Meeting</h1>
                {contents}
                <button className="btn btn-primary" onClick={() => this.takeActionCallToOrder()}>Call to order</button>
                <button className="btn btn-primary" onClick={() => this.takeActionSpeak()}>Speak</button>
                <button className="btn btn-primary" onClick={() => this.takeActionYield()}>Yield</button>
            </div>
        );
    }

    async populateMeetingData() {
        const response = await fetch('meeting');
        const data = await response.json();
        this.setState({ meeting: data[0], loading: false });
    }
}
