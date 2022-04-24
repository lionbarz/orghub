import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input } from 'reactstrap';
import {AddMemberButton} from "./AddMemberButton";

export class Group extends Component {
    static displayName = Group.name;
    userId = localStorage.getItem('userId');
    
    // The ID of the interval that is refreshing the state.
    intervalId = null;

    constructor(props) {
        super(props);
        this.state = { group: null, loading: true, resolution: "", groupName: "", actions: [], motions: [], minutes: [],
            showMembershipModal: false,
            allPeople: [],
            selectedPersonId: null};
    }

    componentDidMount = () => {
        this.updateState();
        this.intervalId = setInterval(this.updateState, 5000);
    }

    componentWillUnmount = () => {
        clearInterval(this.intervalId);
    }

    renderGroup = (group, actions) => {
        return (
            <div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Members</div>
                    <div className="card-body">
                        <ul>
                            {group.members.map(member => <li key={member.id} className="card-text">{member.name}</li>)}
                        </ul>
                        <AddMemberButton groupId={group.id} onAdd={() => this.updateState()}/>
                    </div>
                </div>
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
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">What you can do</div>
                    <div className="card-body">
                        {actions}
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Decisions Agreed On</div>
                    <div className="card-body">
                        <ul>
                            {group.resolutions.map(text => <li key={text} className="card-text">{text}</li>)}
                        </ul>
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">History</div>
                    <div className="card-body">
                        <ul>
                            {group.minutes.map(text => <li key={text} className="card-text">{text}</li>)}
                        </ul>
                    </div>
                </div>
            </div>
        );
    }

    renderActions = () => {
        return (
            <div>
                {this.state.actions.includes('Core.Actions.CallMeetingToOrder') &&
                    <Button color="primary" onClick={() => this.takeAction("calltoorder")}>
                        Call to order
                    </Button>
                }
                {this.state.actions.includes('Core.Actions.MoveToAdjourn') &&
                    <Button color="primary" onClick={() => this.takeAction("adjourn")}>
                        Suggest ending the meeting
                    </Button>
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
                {this.state.motions.includes('Core.Motions.GrantMembership') &&
                    <Button color="primary" onClick={() => this.moveGrantMembershipPrompt()}>
                        Suggest adding a member
                    </Button>
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

    async moveGrantMembership(personId) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ personId: personId, userId: this.userId })
        };
        await fetch(`group/${this.props.match.params.id}/action/movegrantmembership`, requestOptions);
        await this.updateState();
    }

    moveGrantMembershipPrompt = async () => {
        this.getAllPeople();
        this.setState({showMembershipModal: true});
    }
    
    closeGrantMembershipPrompt = () => {
        console.log("Close modal.");
        this.setState({showMembershipModal: false});
    }

    handleChangeResolution = (event) => {
        this.setState({resolution: event.target.value});
    }

    handleChangeGroupName = (event) => {
        this.setState({groupName: event.target.value});
    }
    
    handleChangePersonSelect = (event) => {
        let personId = event.target.value;
        console.log("Selected person " + personId);
        this.setState({selectedPersonId: personId});
    }

    render = () => {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderGroup(this.state.group, this.renderActions());

        return (
            <div>
                {!this.state.group &&
                    <h1 id="tabelLabel">Loading...</h1>
                }
                {this.state.group &&
                    <h1 id="tabelLabel">{this.state.group.name}</h1>
                }
                {contents}
                <Modal isOpen={this.state.showMembershipModal} toggle={() => this.closeGrantMembershipPrompt()}>
                    <ModalHeader
                        close={<button className="close" onClick={this.closeGrantMembershipPrompt}>×</button>}
                        toggle={this.closeGrantMembershipPrompt}>
                        Who do you think should join {this.state.group && this.state.group.name}?
                    </ModalHeader>
                    <ModalBody>
                        <FormGroup>
                            <Input
                                id="exampleSelect"
                                name="select"
                                type="select"
                                onChange={this.handleChangePersonSelect}>
                                <option key="default" value="default">-- Select --</option>
                                {this.state.allPeople.map(person =>
                                    <option key={person.id} value={person.id} className="card-text">{person.name}</option>)}
                            </Input>
                        </FormGroup>
                    </ModalBody>
                    <ModalFooter>
                        <Button
                            color="primary"
                            onClick={() => this.moveGrantMembership(this.state.selectedPersonId)}>
                            Suggest membership
                        </Button>
                        {' '}
                        <Button onClick={this.closeGrantMembershipPrompt}>
                            Cancel
                        </Button>
                    </ModalFooter>
                </Modal>
            </div>
        );
    }

    updateState = async () => {
        console.log("Updating state");
        this.populateGroupData();
        this.getAvailableActions();
        this.getAvailableMotions();
        this.getMinutes();
    }

    populateGroupData = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}`);
        const data = await response.json();
        this.setState({ group: data, loading: false });
    }

    getAvailableActions = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}/action?userId=${this.userId}`);
        const data = await response.json();
        this.setState({ actions: data });
    }

    getAvailableMotions = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}/motion?userId=${this.userId}`);
        const data = await response.json();
        this.setState({ motions: data });
    }

    getMinutes = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`group/${id}/minutes`);
        const data = await response.json();
        this.setState({ minutes: data });
    }

    getAllPeople = async () => {
        const response = await fetch(`person`);
        const data = await response.json();
        this.setState({ allPeople: data });
    }
}
