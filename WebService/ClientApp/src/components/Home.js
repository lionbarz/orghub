import React from 'react';
import { useHistory } from "react-router-dom";

export function Home() {
    const history = useHistory();
    
    function handleTryItNow() {
        history.push('/add-group');
    }
    
    return (
        <div>
            <div className="px-4 py-5 my-5 text-center">
                    <h1 className="display-5 fw-bold">Everything your non-profit needs to start and grow</h1>
                    <div className="col-lg-6 mx-auto">
                        <p className="lead mb-4">Organize, raise money, grow, and make a difference quickly and easily.</p>
                        <div className="d-grid gap-2 d-sm-flex justify-content-sm-center">
                            <button type="button" className="btn btn-primary btn-lg px-4 gap-3" onClick={handleTryItNow}>Try it now
                            </button>
                        </div>
                    </div>
            </div>
            <div className="row align-items-md-stretch">
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>What about WhatsApp?</h2>
                        <p>WhatsApp has group admins who stay admins forever. Inclusive has leaders who are elected and have terms. It's democratic!</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>What about Zoom?</h2>
                        <p>Zoom meetings are started and ended by one person. During the meeting anything goes and there's no record of what happened. Inclusive meetings can be called by members, attendees can only speak when its their turn, a timer limits speaker times, an agenda is displayed, votes are conducted, minutes are automatically recorded, and they are adjourned by a motion. It's parliamentary!</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Simplicity</h2>
                        <p>Bylaws. Parliamentary procedures. These are intimidating words. Luckily, you don't
                            need to know any of them.
                            By using Inclusive, your group will leverage
                            timeless methods used by the most effective organizations,
                            without you knowing it.</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Democracy</h2>
                        <p>Successful clubs have active members. Inclusive makes it easy for all members
                            to participate. Make suggestions, vote, debate, and much more, all with a press
                            of a button.</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Transparency</h2>
                        <p>Knowledge is power, and transparency empowers your members.
                            Inclusive automatically records basic meeting minutes and makes it easy to see
                            the group's officers, members, agenda, decisions, and more.</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Diversity</h2>
                        <p>The strongest organizations are diverse. Make sure that your
                        group is diverse and prove that it's diverse by having a dashboard
                        that shows that your group is made up of people all categories
                        that your organization defines as important.</p>
                    </div>
                </div>
            </div>
        </div>
    );
}
