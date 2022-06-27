import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input } from 'reactstrap';
import ZoomMtgEmbedded from '@zoomus/websdk/embedded';
import {MoveResolutionButton} from "./MoveResolutionButton";

export class Group extends Component {
    static displayName = Group.name;
    userId = localStorage.getItem('userId');
    
    // The ID of the interval that is refreshing the state.
    intervalId = null;

    constructor(props) {
        super(props);
        this.state = {
            group: null,
            loading: true,
            groupName: "",
            actions: [],
            showMembershipModal: false,
            allPeople: [],
            selectedPersonId: null};
    }

    componentDidMount = () => {
        this.updateState();
        this.intervalId = setInterval(this.updateState, 5000);
        this.markAttendance();
        //this.doZoomStuff();
    }
    
    doZoomStuff = async () => {
        const client = ZoomMtgEmbedded.createClient();
        let meetingSDKElement = document.getElementById('meetingSDKElement')
        await client.init({
            debug: true,
            zoomAppRoot: meetingSDKElement,
            language: 'en-US',
            customize: {
                meetingInfo: [
                    'topic',
                    'host',
                    'mn',
                    'pwd',
                    'telPwd',
                    'invite',
                    'participant',
                    'dc',
                    'enctype',
                ],
                toolbar: {
                    buttons: [
                        {
                            text: 'Custom Button',
                            className: 'CustomButton',
                            onClick: () => {
                                console.log('custom button')
                            }
                        }
                    ]
                }
            }
        });
        const signature = await this.getSignature();
        console.log("Signature: " + signature);
        await client.join({
            sdkKey: 'aTa4etMZF2V1nd1OldVl5FJ6U04kAnAkkoO0',
            signature: signature, // role in SDK Signature needs to be 0
            meetingNumber: '3616488549',
            password: 'jiK14w',
            userName: "Mohamed Fakhreddine"
        });
    }
    
    getSignature = async () => {
        const response = await fetch(`signature/token`);
        return await response.json();
    }

    componentWillUnmount = () => {
        clearInterval(this.intervalId);
    }

    renderGroup = (group, actions) => {
        return (
            <div>
                <h1>{group.state}</h1>
                <div>
                    {actions}
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Group name</div>
                    <div className="card-body">
                        <p className="card-text">{group.name}</p>
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Chair</div>
                    <div className="card-body">
                        <p className="card-text">{group.chair.name}</p>
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Members</div>
                    <div className="card-body">
                        
                            {group.members.map(member => <p key={member.id} className="card-text">{member.name}</p>)}
                        
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Decisions made</div>
                    <div className="card-body">
                        <ul>
                            {group.resolutions.map(text => <li key={text} className="card-text">{text}</li>)}
                        </ul>
                    </div>
                </div>
                <div className="card mb-3" style={{maxWidth: "36rem"}}>
                    <div className="card-header">Minutes</div>
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
                {this.state.actions.includes('CallToOrder') &&
                    <div className="mt-3 mb-3">
                        <Button color="primary" onClick={() => this.takeAction("calltoorder")}>
                            Call to order
                        </Button>
                    </div>
                }
                {this.state.actions.includes('MoveToAdjourn') &&
                    <div className="mt-3 mb-3">
                        <Button color="primary" onClick={() => this.takeAction("adjourn")}>
                            Suggest ending the meeting
                        </Button>
                    </div>
                }
                {this.state.actions.includes('Speak') &&
                    <div className="mt-3 mb-3">
                        <button className="btn btn-primary" onClick={() => this.takeAction("speak")}>Speak</button>
                    </div>
                }
                {this.state.actions.includes('Core.Actions.YieldTheFloor') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("yield")}>Yield</button>
                }
                {this.state.actions.includes('Second') &&
                    <div className="mt-3 mb-3">
                        <button className="btn btn-primary" onClick={() => this.takeAction("second")}>Second</button>
                    </div>
                }
                {this.state.actions.includes('Core.Motions.EndDebate') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("moveenddebate")}>Suggest voting now</button>
                }
                {this.state.actions.includes('Core.Actions.DeclareMotionPassed') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("declaremotionpassed")}>Declare
                        motion passed</button>
                }
                {this.state.actions.includes('Core.Motions.ElectChair') &&
                    <button className="btn btn-primary" onClick={() => this.moveElectChair("Rawan Hammoud")}>Elect Roni
                        chair</button>
                }
                {this.state.actions.includes('MoveMainMotion') &&
                    <MoveResolutionButton groupId={this.props.match.params.id} personId={this.userId} onSuccess={() => this.updateState()} />
                }
                {this.state.actions.includes('Core.Motions.ChangeOrgName') &&
                    <button className="btn btn-primary" onClick={() => this.moveGroupName(this.state.groupName)}>Suggest
                        group name</button>
                }
                {this.state.actions.includes('Core.Motions.ChangeOrgName') &&
                    <input type="text" value={this.state.groupName} onChange={this.handleChangeGroupName}/>
                }
                {this.state.actions.includes('Core.Motions.GrantMembership') &&
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
        await fetch(`api/group/${this.props.match.params.id}/action/${action}`, requestOptions);
        await this.updateState();
    }

    async moveElectChair(nomineeName) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nomineeName: nomineeName, userId: this.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/action/electchair`, requestOptions);
        await this.updateState();
    }

    async moveGroupName(text) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: text, userId: this.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/action/movechangegroupname`, requestOptions);
        await this.updateState();
    }

    async moveGrantMembership(personId) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ personId: personId, userId: this.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/action/movegrantmembership`, requestOptions);
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
                <div id="meetingSDKElement">
                </div>
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
    }

    populateGroupData = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`api/group/${id}`);
        const data = await response.json();
        this.setState({ group: data, loading: false });
    }

    markAttendance = async () => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: this.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/markattendance`, requestOptions);
        await this.updateState();
    }

    getAvailableActions = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`api/group/${id}/action?userId=${this.userId}`);
        const data = await response.json();
        this.setState({ actions: data });
    }

    getAllPeople = async () => {
        const response = await fetch(`person`);
        const data = await response.json();
        this.setState({ allPeople: data });
    }
}
