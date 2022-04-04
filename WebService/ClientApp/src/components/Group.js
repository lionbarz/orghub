import React, { Component } from 'react';

export class Group extends Component {
    static displayName = Group.name;
    userId = localStorage.getItem('userId');

    constructor(props) {
        super(props);
        this.state = { group: null, loading: true, resolution: "", groupName: "", actions: [], motions: [], minutes: [] };
    }

    componentDidMount() {
        this.refreshLoop();
    }

    static render(group, actions) {
        return (
            <div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Chair</div>
                    <div className="card-body">
                        <p className="card-text">{group.chair.name}</p>
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Happening Now</div>
                    <div className="card-body">
                        <p className="card-text">{group.state}</p>

                    </div>
                    {actions}
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Decisions Agreed On</div>
                    <div className="card-body">
                        <ul>
                            {group.resolutions.map(text => <li className="card-text">{text}</li>)}
                        </ul>
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">History</div>
                    <div className="card-body">
                        <ul>
                            {group.minutes.map(text => <li className="card-text">{text}</li>)}
                        </ul>
                    </div>
                </div>
                {group.members.map(member => <p>Member: {member.name}</p>)}
            </div>
        );
    }

    renderActions = () => {
        return (
            <div>
                {this.state.actions.includes('Core.Actions.CallMeetingToOrder') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("calltoorder")}>Call to
                        order</button>
                }
                {this.state.actions.includes('Core.Actions.MoveToAdjourn') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("adjourn")}>Suggest ending the meeting</button>
                }
                {this.state.actions.includes('Core.Actions.Speak') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("speak")}>Speak</button>
                }
                {this.state.actions.includes('Core.Actions.YieldTheFloor') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("yield")}>Yield</button>
                }
                {this.state.actions.includes('Core.Actions.SecondMotion') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("second")}>Second</button>
                }
                {this.state.motions.includes('Core.Motions.EndDebate') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("moveenddebate")}>Suggest voting now</button>
                }
                {this.state.actions.includes('Core.Actions.DeclareMotionPassed') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("declaremotionpassed")}>Declare
                        motion passed</button>
                }
                {this.state.motions.includes('Core.Motions.ElectChair') &&
                    <button className="btn btn-primary" onClick={() => this.moveElectChair("Rawan Hammoud")}>Elect Roni
                        chair</button>
                }
                {this.state.motions.includes('Core.Motions.Resolve') && <button className="btn btn-primary"
                                                                                onClick={() => this.moveResolution(this.state.resolution)}>Suggest
                    resolution</button>
                }
                {this.state.motions.includes('Core.Motions.Resolve') &&
                    <input type="text" value={this.state.resolution} onChange={this.handleChangeResolution}/>
                }
                {this.state.motions.includes('Core.Motions.ChangeOrgName') &&
                    <button className="btn btn-primary" onClick={() => this.moveGroupName(this.state.groupName)}>Suggest
                        group name</button>
                }
                {this.state.motions.includes('Core.Motions.ChangeOrgName') &&
                    <input type="text" value={this.state.groupName} onChange={this.handleChangeGroupName}/>
                }
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
        await this.updateState();
    }

    async moveElectChair(nomineeName) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nomineeName: nomineeName, userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/electchair`, requestOptions);
        await this.updateState();
    }

    async moveResolution(text) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ text: text, userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/moveresolution`, requestOptions);
        await this.updateState();
    }

    async moveGroupName(text) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: text, userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/movechangegroupname`, requestOptions);
        await this.updateState();
    }

    handleChangeResolution = (event) => {
        this.setState({resolution: event.target.value});
    }

    handleChangeGroupName = (event) => {
        this.setState({groupName: event.target.value});
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Group.render(this.state.group, this.renderActions());

        return (
            <div>
                {!this.state.group &&
                    <h1 id="tabelLabel">Group</h1>
                }
                {this.state.group &&
                    <h1 id="tabelLabel">{this.state.group.name}</h1>
                }
                {contents}
            </div>
        );
    }

    async updateState() {
        this.populateGroupData();
        this.getAvailableActions();
        this.getAvailableMotions();
        this.getMinutes();
    }

    async populateGroupData() {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}`);
        const data = await response.json();
        this.setState({ group: data, loading: false });
    }

    async getAvailableActions() {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}/action?userId=${this.userId}`);
        const data = await response.json();
        this.setState({ actions: data });
    }

    async getAvailableMotions() {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}/motion`);
        const data = await response.json();
        this.setState({ motions: data });
    }

    async getMinutes() {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}/minutes`);
        const data = await response.json();
        this.setState({ minutes: data });
    }

    refreshLoop = () => {
        this.updateState();
        setTimeout(this.refreshLoop, 3000);
    }
}
