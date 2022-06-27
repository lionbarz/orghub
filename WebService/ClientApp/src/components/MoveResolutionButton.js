import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { Modal, ModalBody, ModalHeader, ModalFooter, FormGroup, Input, Label } from 'reactstrap';

// Has a dialog for specifying resolution text.
export class MoveResolutionButton extends Component {

    constructor(props) {
        super(props);
        this.state = { groupId: props.groupId, showModal: false, text: ''};
    }

    showModal = async () => {
        this.setState({showModal: true});
    }

    hideModal = () => {
        this.setState({showModal: false});
    }

    handleChangeText = (event) => {
        this.setState({text: event.target.value});
    }

    moveResolution = async () => {
        let isSuccess = await this.sendRequest(this.state.text);
        
        if (isSuccess) {
            this.hideModal();
            this.props.onSuccess();
        } else {
            alert('Something went wrong.');
        }
    }

    sendRequest = async (text) => {
        console.log('move resolution ' + text);
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ text: text, userId: this.props.personId })
        };
        let response = await fetch(`group/${this.props.groupId}/moveresolution`, requestOptions);
        return response.ok;
    }

    render = () => {
        return (
            <div>
                <button className="btn btn-primary" onClick={this.showModal}>Suggest something...</button>
                <Modal isOpen={this.state.showModal} toggle={this.hideModal}>
                    <ModalHeader
                        close={<button className="close" onClick={this.hideModal}>×</button>}
                        toggle={this.hideModal}>
                        What would you like to suggest to the group?
                    </ModalHeader>
                    <ModalBody>
                        <FormGroup>
                            <Label
                                className="me-sm-2"
                                for="memberName"
                            >
                                Suggestion
                            </Label>
                            <Input
                                id="text"
                                name="text"
                                placeholder="Ex: Move meetings to Thursday"
                                type="text"
                                value={this.state.text}
                                onChange={this.handleChangeText}
                            />
                        </FormGroup>
                    </ModalBody>
                    <ModalFooter>
                        <Button
                            color="primary"
                            onClick={() => this.moveResolution()}>
                            Make suggestion
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