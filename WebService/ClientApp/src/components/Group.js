import React, {useCallback, useEffect, useState} from 'react';
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
import {MoveResolutionButton} from "./MoveResolutionButton";
import {JoinMeetingComponent} from "./JoinMeetingComponent";
import {NominateChairButton} from "./NominateChairButton";
import {useParams} from "react-router-dom";
import usePerson from "../usePerson";
import moment from "moment";

export function Group() {
    const [group, setGroup] = useState(null);
    const [loading, setLoading] = useState(true);
    const [actions, setActions] = useState([]);
    const [showMembershipModal, setShowMembershipModal] = useState(false);
    const [allPeople] = useState([]);
    const [selectedPersonId, setSelectedPersonId] = useState(null);
    const {groupId} = useParams();
    const {person, addPerson} = usePerson();
    
    const populateGroupData = useCallback(async () => {
        const response = await fetch(`api/group/${groupId}`);
        const data = await response.json();
        setGroup(data);
        setLoading(false);
    }, [groupId]);

    const markAttendance = useCallback(async () => {
        if (!person) {
            return;
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: person.id })
        };
        await fetch(`api/group/${groupId}/markattendance`, requestOptions);
    }, [person, groupId]);

    const getAvailableActions = useCallback(async () => {
        if (!person) {
            return;
        }
        
        const response = await fetch(`api/group/${groupId}/action?userId=${person.id}`);
        const data = await response.json();
        setActions(data);
    }, [person, groupId]);
    
    const updateState = useCallback(async () => {
        await populateGroupData();
        await getAvailableActions();
    }, [populateGroupData, getAvailableActions]);
    
    useEffect(() => {
        markAttendance();
        updateState();
    }, [groupId, updateState, markAttendance]);
    
    useEffect(() => {
        // The ID of the interval that is refreshing the state.
        const intervalId = setInterval(updateState, 5000);
        return () => {
            clearInterval(intervalId);
        };
    }, [updateState]);

    function renderActions() {
        return (
            <div>
                {actions.includes('Vote') &&
                    <div>
                        <div className="mt-3 mb-3">
                            <Button variant="outlined" startIcon={<VoteAyeIcon />} onClick={() => takeActionVote("Aye")}>
                                Vote Aye
                            </Button>
                        </div>
                        <div className="mt-3 mb-3">
                            <Button variant="outlined" startIcon={<VoteNayIcon />} onClick={() => takeActionVote("Nay")}>
                                Vote Nay
                            </Button>
                        </div>
                        <div className="mt-3 mb-3">
                            <Button variant="outlined" startIcon={<AbstainIcon />} onClick={() => takeActionVote("Abstain")}>
                                Abstain from voting
                            </Button>
                        </div>
                    </div>
                }
                {actions.includes('CallToOrder') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<GavelIcon />} onClick={() => takeAction("calltoorder")}>
                            Call to order
                        </Button>
                    </div>
                }
                {actions.includes('DeclareTimeExpired') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<TimeExpiredIcon />} onClick={() => takeAction("DeclareTimeExpired")}>
                            Declare time has expired
                        </Button>
                    </div>
                }
                {actions.includes('Speak') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<MicIcon />} onClick={() => takeAction("speak")}>
                            Speak
                        </Button>
                    </div>
                }
                {actions.includes('Yield') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<YieldIcon />} onClick={() => takeAction("yield")}>
                            Yield
                        </Button>
                    </div>
                }
                {actions.includes('Second') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<SecondIcon />} onClick={() => takeAction("second")}>
                            Second
                        </Button>
                    </div>
                }
                {actions.includes('Core.Motions.EndDebate') &&
                    <button className="btn btn-primary" onClick={() => takeAction("moveenddebate")}>Suggest voting now</button>
                }
                {actions.includes('Core.Actions.DeclareMotionPassed') &&
                    <button className="btn btn-primary" onClick={() => takeAction("declaremotionpassed")}>Declare
                        motion passed</button>
                }
                {actions.includes('MoveMainMotion') &&
                    <NominateChairButton
                        groupId={groupId}
                        personId={person.id}
                        members={group.members}
                        onSuccess={() => updateState()} />
                }
                {actions.includes('MoveMainMotion') &&
                    <MoveResolutionButton groupId={groupId} personId={person.id} onSuccess={() => updateState()} />
                }
                {actions.includes('MoveToAdjourn') &&
                    <div className="mt-3 mb-3">
                        <Button variant="outlined" startIcon={<AdjournIcon />} onClick={() => takeAction("movetoadjourn")}>
                            Move to adjourn
                        </Button>
                    </div>
                }
            </div>
        );
    }

    async function takeAction(action) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: person.id })
        };
        await fetch(`api/group/${groupId}/${action}`, requestOptions);
        await updateState();
    }

    async function takeActionVote(voteType) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: person.id, voteType: voteType })
        };
        await fetch(`api/group/${groupId}/vote`, requestOptions);
        await updateState();
    }

    async function moveGrantMembership(personId) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ personId: personId, userId: person.id })
        };
        await fetch(`api/group/${groupId}/action/movegrantmembership`, requestOptions);
        await updateState();
    }
    
    function closeGrantMembershipPrompt() {
        setShowMembershipModal(false);
    }
    
    function handleChangePersonSelect(event) {
        let personId = event.target.value;
        setSelectedPersonId(personId);
    }

    if (loading) {
        return <p><em>Loading...</em></p>;
    }
        return (
            <div>
                {!person &&
                    <div className="mb-3">
                        <JoinMeetingComponent person={person} addPerson={addPerson} />
                    </div>
                }
        <div>
            <h1>{group.state}</h1>
            <div>
                {renderActions()}
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Meeting</div>
                <div className="card-body">
                    <p className="card-text">{moment(group.currentMeeting.startTime).format("dddd, MMMM Do YYYY, h:mm a")}</p>
                    <p className="card-text">{group.currentMeeting.description}</p>
                </div>
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
                            formattedTime = moment(minute.time).fromNow();
                            return <li key={minute.time} className="card-text">{minute.text} ({formattedTime.toString()})</li>
                        })}
                    </ul>
                </div>
            </div>
        </div>
                <Modal isOpen={showMembershipModal} toggle={() => closeGrantMembershipPrompt()}>
                    <ModalHeader
                        close={<button className="close" onClick={closeGrantMembershipPrompt}>×</button>}
                        toggle={closeGrantMembershipPrompt}>
                        Who do you think should join {group && group.name}?
                    </ModalHeader>
                    <ModalBody>
                        <FormGroup>
                            <Input
                                id="exampleSelect"
                                name="select"
                                type="select"
                                onChange={handleChangePersonSelect}>
                                <option key="default" value="default">-- Select --</option>
                                {allPeople.map(person =>
                                    <option key={person.id} value={person.id} className="card-text">{person.name}</option>)}
                            </Input>
                        </FormGroup>
                    </ModalBody>
                    <ModalFooter>
                        <Button
                            color="primary"
                            onClick={() => moveGrantMembership(selectedPersonId)}>
                            Suggest membership
                        </Button>
                        {' '}
                        <Button onClick={closeGrantMembershipPrompt}>
                            Cancel
                        </Button>
                    </ModalFooter>
                </Modal>
            </div>
        );
}
