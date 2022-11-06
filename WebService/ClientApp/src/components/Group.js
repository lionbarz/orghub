import React, { Component } from 'react';
import Button from '@mui/material/Button';
import GavelIcon from '@mui/icons-material/Gavel';
import MicIcon from '@mui/icons-material/Mic';
import AdjournIcon from '@mui/icons-material/DirectionsRun';
import YieldIcon from '@mui/icons-material/MicOff';
import VoteAyeIcon from '@mui/icons-material/ThumbUp';
import VoteNayIcon from '@mui/icons-material/ThumbDown';
import TimeExpiredIcon from '@mui/icons-material/Timer';
import SecondIcon from '@mui/icons-material/EmojiPeople';
import AbstainIcon from '@mui/icons-material/NotInterested';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input } from 'reactstrap';
import ZoomMtgEmbedded from '@zoomus/websdk/embedded';
import {MoveResolutionButton} from "./MoveResolutionButton";
import {JoinMeetingComponent} from "./JoinMeetingComponent";
import {NominateChairButton} from "./NominateChairButton";

export class Group extends Component {
    static displayName = Group.name;
    
    // The ID of the interval that is refreshing the state.
    intervalId = null;

    constructor(props) {
        super(props);
        this.state = {
            userId: localStorage.getItem('userId'),
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
                            {group.minutes.map(minute => {
                                let formattedTime = new Date(Date.parse(minute.time));
                                return <li key={minute.time} className="card-text">{formattedTime.toString()}: {minute.text}</li>
                            })}
                        </ul>
                    </div>
                </div>
            </div>
        );
    }

    renderActions = () => {
        return (
            <div>
                {this.state.actions.includes('Vote') &&
                    <div>
                        <div className="mt-3 mb-3">
                            <Button variant="outlined" startIcon={<VoteAyeIcon />} onClick={() => this.takeActionVote("Aye")}>
                                Vote Aye
                            </Button>
                        </div>
                        <div className="mt-3 mb-3">
                            <Button variant="outlined" startIcon={<VoteNayIcon />} onClick={() => this.takeActionVote("Nay")}>
                                Vote Nay
                            </Button>
                        </div>
                        <div className="mt-3 mb-3">
                            <Button variant="outlined" startIcon={<AbstainIcon />} onClick={() => this.takeActionVote("Abstain")}>
                                Abstain from voting
                            </Button>
                        </div>
                    </div>
                }
                {this.state.actions.includes('CallToOrder') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<GavelIcon />} onClick={() => this.takeAction("calltoorder")}>
                            Call to order
                        </Button>
                    </div>
                }
                {this.state.actions.includes('DeclareTimeExpired') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<TimeExpiredIcon />} onClick={() => this.takeAction("DeclareTimeExpired")}>
                            Declare time has expired
                        </Button>
                    </div>
                }
                {this.state.actions.includes('Speak') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<MicIcon />} onClick={() => this.takeAction("speak")}>
                            Speak
                        </Button>
                    </div>
                }
                {this.state.actions.includes('Yield') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<YieldIcon />} onClick={() => this.takeAction("yield")}>
                            Yield
                        </Button>
                    </div>
                }
                {this.state.actions.includes('Second') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<SecondIcon />} onClick={() => this.takeAction("second")}>
                            Second
                        </Button>
                    </div>
                }
                {this.state.actions.includes('Core.Motions.EndDebate') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("moveenddebate")}>Suggest voting now</button>
                }
                {this.state.actions.includes('Core.Actions.DeclareMotionPassed') &&
                    <button className="btn btn-primary" onClick={() => this.takeAction("declaremotionpassed")}>Declare
                        motion passed</button>
                }
                {this.state.actions.includes('MoveMainMotion') &&
                    <NominateChairButton
                        groupId={this.props.match.params.id}
                        personId={this.state.userId}
                        members={this.state.group.members}
                        onSuccess={() => this.updateState()} />
                }
                {this.state.actions.includes('MoveMainMotion') &&
                    <MoveResolutionButton groupId={this.props.match.params.id} personId={this.state.userId} onSuccess={() => this.updateState()} />
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
                {this.state.actions.includes('MoveToAdjourn') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<AdjournIcon />} onClick={() => this.takeAction("movetoadjourn")}>
                            Move to adjourn
                        </Button>
                    </div>
                }
            </div>
        );
    }

    async takeAction(action) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: this.state.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/${action}`, requestOptions);
        await this.updateState();
    }

    async takeActionVote (voteType) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: this.state.userId, voteType: voteType })
        };
        await fetch(`api/group/${this.props.match.params.id}/vote`, requestOptions);
        await this.updateState();
    }

    async moveElectChair(nomineeName) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nomineeName: nomineeName, userId: this.state.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/action/electchair`, requestOptions);
        await this.updateState();
    }

    async moveGroupName(text) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: text, userId: this.state.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/action/movechangegroupname`, requestOptions);
        await this.updateState();
    }

    async moveGrantMembership(personId) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ personId: personId, userId: this.state.userId })
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
                {!this.state.userId &&
                    <div className="mb-3">
                        <JoinMeetingComponent onPersonCreate={(userId) => {
                            localStorage.setItem('userId', userId);
                            this.setState({userId: userId});
                            this.markAttendance();
                        }} />
                    </div>
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
    }

    populateGroupData = async () => {
        const id = this.props.match.params.id;
        const response = await fetch(`api/group/${id}`);
        const data = await response.json();
        this.setState({ group: data, loading: false });
    }

    markAttendance = async () => {
        if (!this.state.userId) {
            return;
        }
        
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: this.state.userId })
        };
        await fetch(`api/group/${this.props.match.params.id}/markattendance`, requestOptions);
        await this.updateState();
    }

    getAvailableActions = async () => {
        if (!this.state.userId) {
            return;
        }
        
        const id = this.props.match.params.id;
        const response = await fetch(`api/group/${id}/action?userId=${this.state.userId}`);
        const data = await response.json();
        this.setState({ actions: data });
    }

    getAllPeople = async () => {
        const response = await fetch(`person`);
        const data = await response.json();
        this.setState({ allPeople: data });
    }
}
