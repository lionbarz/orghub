import {useParams} from "react-router-dom";
import React, {useCallback, useEffect, useState} from "react";
import usePerson from "../usePerson";
import Button from "@mui/material/Button";
import VoteAyeIcon from "@mui/icons-material/ThumbUp";
import VoteNayIcon from "@mui/icons-material/ThumbDown";
import AbstainIcon from "@mui/icons-material/NotInterested";
import GavelIcon from "@mui/icons-material/Gavel";
import TimeExpiredIcon from "@mui/icons-material/Timer";
import MicIcon from "@mui/icons-material/Mic";
import YieldIcon from "@mui/icons-material/MicOff";
import SecondIcon from "@mui/icons-material/EmojiPeople";
import {NominateChairButton} from "./NominateChairButton";
import {MoveResolutionButton} from "./MoveResolutionButton";
import AdjournIcon from "@mui/icons-material/DirectionsRun";
import {AttendeeRole} from "./AttendeeRole";
import {GuestLoginComponent} from "./GuestLoginComponent";
import moment from "moment/moment";

export function Meeting() {
    const {meetingId} = useParams();
    const [actions, setActions] = useState([]);
    const [meeting, setMeeting] = useState(null);
    const [loading, setLoading] = useState(true);
    const {person, addPerson} = usePerson();

    const refreshMeeting = useCallback(async () => {
        const response = await fetch(`api/meeting/${meetingId}`);
        const data = await response.json();
        setMeeting(data);
        setLoading(false);
    }, [meetingId]);

    const markAttendance = useCallback(async () => {
        if (!person) {
            return;
        }

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: person.id })
        };
        await fetch(`api/meeting/${meetingId}/markattendance`, requestOptions);
    }, [person, meetingId]);

    const getAvailableActions = useCallback(async () => {
        if (!person) {
            return;
        }

        const response = await fetch(`api/meeting/${meetingId}/action?userId=${person.id}`);
        const data = await response.json();
        setActions(data);
    }, [person, meetingId]);

    useEffect(() => {
        markAttendance();
        refreshMeeting();
    }, [meetingId, refreshMeeting, markAttendance]);

    const updateState = useCallback(async () => {
        await refreshMeeting();
        await getAvailableActions();
    }, [refreshMeeting, getAvailableActions]);

    useEffect(() => {
        updateState();
    }, [meetingId, updateState]);

    useEffect(() => {
        // The ID of the interval that is refreshing the state.
        const intervalId = setInterval(updateState, 5000);
        return () => {
            clearInterval(intervalId);
        };
    }, [updateState]);

    async function takeAction(action) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: person.id })
        };
        await fetch(`api/meeting/${meetingId}/${action}`, requestOptions);
        await updateState();
    }

    async function takeActionVote(voteType) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: person.id, voteType: voteType })
        };
        await fetch(`api/meeting/${meetingId}/vote`, requestOptions);
        await updateState();
    }
    
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
                {actions.includes('MoveMainMotion') && false && // This isn't working yet.
                    <NominateChairButton
                        meetingId={meeting.groupId}
                        personId={person.id}
                        onSuccess={() => updateState()} />
                }
                {actions.includes('MoveMainMotion') &&
                    <MoveResolutionButton meetingId={meetingId} personId={person.id} onSuccess={() => updateState()} />
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
    
    if (loading) {
        return (
            <div>Loading...</div>
        );
    }
    
    return (
        <div>
            {!person &&
                <div>
                    <h1>Sign in to attend the meeting.</h1>
                    <GuestLoginComponent person={person} addPerson={addPerson} />
                </div>
            }
            <h1>{meeting.state}</h1>
            <div>
                {renderActions()}
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Agenda</div>
                <div className="card-body">
                    {meeting.agenda
                        .map(item => <p key={item.title} className="card-text">{item.title}{item.isCurrent && " (now)"}</p>)}
                </div>
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Is Quorum Currently Present?</div>
                <div className="card-body">
                    {meeting.hasQuorum ? "Yes" : "No"}
                </div>
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Chair</div>
                <div className="card-body">
                    {meeting.attendees
                        .filter(attendee => attendee.roles & AttendeeRole.chair)
                        .map(attendee => <p key={attendee.person.id} className="card-text">{attendee.person.name ?? attendee.person.email}</p>)}
                </div>
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Members present</div>
                <div className="card-body">
                    {meeting.attendees
                        .filter(attendee => attendee.roles & AttendeeRole.member)
                        .map(attendee => <p key={attendee.person.id} className="card-text">{attendee.person.name ?? attendee.person.email}</p>)}
                </div>
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Guests present</div>
                <div className="card-body">
                    {meeting.attendees
                        .filter(attendee => attendee.roles & AttendeeRole.guest)
                        .map(attendee => <p key={attendee.person.id} className="card-text">{attendee.person.name ?? attendee.person.email}</p>)}
                </div>
            </div>
            <div className="card mb-3" style={{maxWidth: "36rem"}}>
                <div className="card-header">Minutes</div>
                <div className="card-body">
                    <ul>
                        {meeting.minutes.map(minute => {
                            let formattedTime = new Date(Date.parse(minute.time));
                            formattedTime = moment(minute.time).fromNow();
                            return <li key={minute.time} className="card-text">{minute.text} ({formattedTime.toString()})</li>
                        })}
                    </ul>
                </div>
            </div>
        </div>
    );
}