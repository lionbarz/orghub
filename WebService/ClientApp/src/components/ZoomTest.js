import useZoom from "../useZoom";
import usePerson from "../usePerson";
import {GuestLoginComponent} from "./GuestLoginComponent";

export function ZoomTest() {
    const topic = 'meeting02091014';
    const userName = 'Mohamed Fakhreddine';

    const {person, addPerson} = usePerson();
    
    const {startVideo} = useZoom(person ? person.id : null, userName, topic, 'self-view-canvas', 'self-view-video', 'participants-canvas');
 
    return (
        <div>
            {!person &&
                <div className="mb-3">
                    <GuestLoginComponent person={person} addPerson={addPerson} />
                </div>
            }
            <p>Test zoom</p>
            <h1>You</h1>
            <video id="self-view-video" width="1920" height="1080"></video>
            <canvas id="self-view-canvas" width="1920" height="1080"></canvas>
            <h1>Others</h1>
            <canvas id="participants-canvas" width="1920" height="1080"></canvas>
            <button onClick={startVideo}>Start video</button>
        </div>
    );
}
