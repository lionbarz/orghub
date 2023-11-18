import React, {useState} from 'react';
import {Button, Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { FormGroup } from 'reactstrap';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import {LocalizationProvider} from "@mui/x-date-pickers";
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';

// Creates a button for creating a new group meeting.
// It pops up a dialog for collecting meeting information.
export function CreateMeetingButtonComponent({ personId, groupId }) {
    const [showModal, setShowModal] = useState(false);
    const [startDateTime, setStartDateTime] = useState(dayjs(new Date()));

    function closeModal() {
        setShowModal(false);
    }
    
    async function createMeeting() {
        console.log('Create meeting at ' + startDateTime);
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                groupId: groupId,
                userId: personId,
                meetingStartTime: startDateTime.toISOString()})
        };
        try {
            let response = await fetch(`api/meeting`, requestOptions);
            setShowModal(false);
            return response.ok;
        } catch (error) {
            console.error(error);
            alert('Failed to create meeting.');
        }
    }

    return (
        <div>
            <Button color="primary" onClick={() => setShowModal(true)}>Create meeting</Button>
            <Modal isOpen={showModal} toggle={closeModal}>
                <ModalHeader
                    close={<button className="close" onClick={closeModal}>×</button>}
                    toggle={closeModal}>
                    Create new meeting
                </ModalHeader>
                <ModalBody>
                    <FormGroup>
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DateTimePicker onChange={setStartDateTime} value={startDateTime} />
                        </LocalizationProvider>
                    </FormGroup>
                </ModalBody>
                <ModalFooter>
                    <Button
                        color="primary"
                        onClick={createMeeting}>
                        Create meeting
                    </Button>
                    {' '}
                    <Button onClick={closeModal}>
                        Cancel
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
}