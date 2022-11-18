import React, { Component } from 'react';
import Button from '@mui/material/Button';
import Icon from '@mui/icons-material/EmojiPeople';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input } from 'reactstrap';

// Has a dialog for specifying resolution text.
export class NominateChairButton extends Component {

    constructor(props) {
        super(props);
        this.state = {
            nomineeId: this.props.members[0].id,
            showModal: false, text: ''};
    }

    showModal = async () => {
        this.setState({showModal: true});
    }

    hideModal = () => {
        this.setState({showModal: false});
    }

    handleChangeNominee = (event) => {
        this.setState({nomineeId: event.target.value});
    }

    nominate = async () => {
        let isSuccess = await this.sendRequest(this.state.text);
        
        if (isSuccess) {
            this.hideModal();
            this.props.onSuccess();
        } else {
            alert('Something went wrong.');
        }
    }

    sendRequest = async (text) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: this.props.personId, personId: this.state.nomineeId})
        };
        let response = await fetch(`api/group/${this.props.groupId}/nominateChair`, requestOptions);
        return response.ok;
    }

    render = () => {
        return (
            <div className="mt-3 mb-3">
                <Button variant="outlined" startIcon={<Icon />} onClick={this.showModal}>
                    Nominate for chair...
                </Button>
                <Modal isOpen={this.state.showModal} toggle={this.hideModal}>
                    <ModalHeader
                        close={<button className="close" onClick={this.hideModal}>×</button>}
                        toggle={this.hideModal}>
                        What would you like to nominate for chair?
                    </ModalHeader>
                    <ModalBody>
                        <FormGroup>
                            <FormGroup>
                                <Input
                                    id="selectNominee"
                                    name="selectNominee"
                                    type="select"
                                    value={this.state.nomineeId}
                                    onChange={this.handleChangeNominee}
                                >
                                    {this.props.members.map(member => <option key={member.id} value={member.id}>{member.name}</option>)}
                                </Input>
                            </FormGroup>
                        </FormGroup>
                    </ModalBody>
                    <ModalFooter>
                        <Button
                            color="primary"
                            onClick={() => this.nominate()}>
                            Nominate
                        </Button>
                        {' '}
                        <Button onClick={this.hideModal}>
                            Cancel
                        </Button>
                    </ModalFooter>
                </Modal>
            </div>
        );
    }
}