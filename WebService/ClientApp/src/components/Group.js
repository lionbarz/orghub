import React, {useCallback, useEffect, useState} from 'react';
import Button from '@mui/material/Button';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input } from 'reactstrap';
import {GuestLoginComponent} from "./GuestLoginComponent";
import {Link, useParams} from "react-router-dom";
import usePerson from "../usePerson";
import moment from "moment";
import {CreateMeetingButtonComponent} from "./CreateMeetingButtonComponent";

export function Group() {
    const [group, setGroup] = useState(null);
    const [loading, setLoading] = useState(true);
    const [meetingsLoading, setMeetingsLoading] = useState(true);
    const [meetings, setMeetings] = useState([]);
    const [showMembershipModal, setShowMembershipModal] = useState(false);
    const [allPeople] = useState([]);
    const [selectedPersonId, setSelectedPersonId] = useState(null);
    const {groupId} = useParams();
    const {person, addPerson} = usePerson();
    
    const populateGroupData = useCallback(async () => {
        setLoading(true);
        const response = await fetch(`api/group/${groupId}`);
        const data = await response.json();
        setGroup(data);
        setLoading(false);
    }, [groupId]);

    const populateMeetingData = useCallback(async () => {
        setMeetingsLoading(true);
        const response = await fetch(`api/meeting/group/${groupId}`);
        const data = await response.json();
        setMeetings(data);
        setMeetingsLoading(false);
    }, [groupId]);
    
    const updateState = useCallback(async () => {
        await populateGroupData();
        await populateMeetingData();
    }, [populateGroupData, populateMeetingData]);
    
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
    
    if (loading && !group) {
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
                <div className="card-header">Meetings</div>
                <div className="card-body">
                    {meetingsLoading && !meetings && <p className="card-text">Loading...</p>}
                    {meetings && meetings.length === 0 && <p className="card-text">There are no past or scheduled meetings.</p>}
                    {meetings && meetings.map(m => {
                        const formattedCurrentMeeting = moment(m.startTime).format("dddd, MMMM Do YYYY, h:mm a");
                        return <p key={m.id} className="card-text">
                            <Link to={'/meeting/' + m.id}>
                                {formattedCurrentMeeting}
                            </Link>
                        </p> })}
                </div>
                {person && group && (person.id === group.chair.id) && <CreateMeetingButtonComponent groupId={group.id} personId={person.id} />}
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
                <div className="card-header">Resolutions</div>
                <div className="card-body">
                    <ul>
                        {group.resolutions.map(text => <li key={text} className="card-text">{text}</li>)}
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
