import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input, Label } from 'reactstrap';

// Has a dialog for adding a member to a group.
export class AddMemberButton extends Component {
    
    constructor(props) {
        super(props);
        this.state = { groupId: props.groupId, showModal: false, memberName: ''};
    }

    showModal = async () => {
        this.setState({showModal: true});
    }

    hideModal = () => {
        this.setState({showModal: false});
    }

    handleChangeMemberName = (event) => {
        this.setState({memberName: event.target.value});
    }

    createAndAddMember = async () => {
        var person = await this.createPerson(this.state.memberName);
        console.log(person);
        if (person.id) {
            await this.addMember(person.id);
            this.hideModal();
            this.props.onAdd();
        } else {
            alert('Failed to create person.');
        }
    }
    
    createPerson = async (name) => {
        console.log('create person ' + name);
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName: name })
        };
        const response = await fetch(`person/addperson`, requestOptions);
        return await response.json();
    }
    
    addMember = async (personId) => {
        console.log('add ' + personId + ' to ' + this.props.groupId);
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: personId })
        };
        await fetch(`group/${this.props.groupId}/addmember`, requestOptions);
    }
    
    render = () => {
        return (
            <div>
                <button className="btn btn-primary" onClick={this.showModal}>Add Member</button>
                <Modal isOpen={this.state.showModal} toggle={this.hideModal}>
                    <ModalHeader
                        close={<button className="close" onClick={this.hideModal}>×</button>}
                        toggle={this.hideModal}>
                        Create and add member
                    </ModalHeader>
                    <ModalBody>
                        <FormGroup>
                            <Label
                                className="me-sm-2"
                                for="memberName"
                            >
                                Name
                            </Label>
                            <Input
                                id="memberName"
                                name="name"
                                placeholder="Charbel Nahas"
                                type="text"
                                value={this.state.memberName}
                                onChange={this.handleChangeMemberName}
                            />
                        </FormGroup>
                    </ModalBody>
                    <ModalFooter>
                        <Button
                            color="primary"
                            onClick={() => this.createAndAddMember()}>
                            Create and add member
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