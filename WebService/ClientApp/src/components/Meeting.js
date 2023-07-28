import {useParams} from "react-router-dom";

export function Meeting() {
    const {meetingId} = useParams();
    
    return (
        <div>{meetingId}</div>
    );
}