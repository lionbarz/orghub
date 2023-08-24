import React, {useCallback, useEffect, useState} from 'react';
import Button from '@mui/material/Button';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input } from 'reactstrap';
import {GuestLoginComponent} from "./GuestLoginComponent";
import {Link, useParams} from "react-router-dom";
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
    
    const updateState = useCallback(async () => {
        await populateGroupData();
    }, [populateGroupData]);
    
    useEffect(() => {
        updateState();
    }, [groupId, updateState]);
    
    useEffect(() => {
        // The ID of the interval that is refreshing the state.
        const intervalId = setInterval(updateState, 5000);
        return () => {
            clearInterval(intervalId);
        };
    }, [updateState]);

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

    const meetingTimeInFuture = true;
    
    let meetingCardBody = (
        <div className="card-body">
            <p className="card-text">No meeting scheduled.</p>
        </div>);
    
    if (group && group.currentMeeting) {
        const formattedCurrentMeeting = moment(group.currentMeeting.startTime).format("dddd, MMMM Do YYYY, h:mm a");
        meetingCardBody = <h1>Next meeting is at {formattedCurrentMeeting}</h1>;
        meetingCardBody = (
            <div className="card-body">
                <p className="card-text">{formattedCurrentMeeting}</p>
                <p className="card-text">{group.currentMeeting.description}</p>
                <p className="card-text"><Link to={"/meeting/" + group.currentMeeting.id}>Join Meeting</Link></p>
            </div>);
    }
    
    if (loading) {
        return <p><em>Loading...</em></p>;
    }
        return (
            <div>
                {!person &&
                    <div className="mb-3">
                        <GuestLoginComponent person={person} addPerson={addPerson} />
                    </div>
                }
        <div>
            {group && group.name && <h1>{group.name}</h1>}
            <h1>{group.state}</h1>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Next Meeting</div>
                {meetingCardBody}
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
                    {group.members.map(member => <p key={member.id} className="card-text">{member.name ?? member.email}</p>)}
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
