import React, {Component} from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {user: null, name: ""};
    }

    componentDidMount() {
        let userId = localStorage.getItem('userId');
        
        if (userId) {
            this.populateUserName(userId);
        }
    }
    
    populateUserName = async (personId) => {
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        };
        const response = await fetch('api/person/' + personId, requestOptions);
        const user = await response.json();
        localStorage.setItem('userId', user.id);
        this.setState({user: user})
    }

    async saveUser() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName: this.state.name })
        };
        const response = await fetch('api/person/addPerson', requestOptions);
        const user = await response.json();
        localStorage.setItem('userId', user.id);
        this.setState({user: user})
    }

    handleChange = (event) => {
        const value = event.target.value;
        this.setState({name: value});
    };

    render() {
        return (
            <div>
                <div className="px-4 py-5 my-5 text-center">
                        <h1 className="display-5 fw-bold">Democracy without complexity</h1>
                        <div className="col-lg-6 mx-auto">
                            <p className="lead mb-4">Organize, raise money, grow, and make a difference with
                                Parlilly, a quick and easy online service, featuring elections, voting,
                                meeting minutes, agendas, and much more.</p>
                            <div className="d-grid gap-2 d-sm-flex justify-content-sm-center">
                                <button type="button" className="btn btn-primary btn-lg px-4 gap-3">Try it now
                                </button>
                            </div>
                        </div>
                </div>
                <div className="row align-items-md-stretch">
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Simplicity</h2>
                        <p>Bylaws. Parliamentary procedures. These are intimidating words. Luckily, you don't
                            need to know any of them.
                            By using Parlilly, your group will leverage
                            timeless methods used by the most effective organizations,
                            without you knowing it.</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Democracy</h2>
                        <p>Successful clubs have active members. Parlilly makes it easy for all members
                            to participate. Make suggestions, vote, debate, and much more, all with a press
                            of a button.</p>
                    </div>
                </div>
                <div className="col-md-4">
                    <div className="h-100 p-5 bg-light border rounded-3">
                        <h2>Transparency</h2>
                        <p>Knowledge is power, and transparency empowers your members.
                            Parlilly automatically records basic meeting minutes and makes it easy to see
                            the group's officers, members, agenda, decisions, and more.</p>
                    </div>
                </div>
                </div>
                <h2>Who are you?</h2>
                {this.state.user && <p>Hello, {this.state.user.name}</p>}
                {!this.state.user &&
                    <div>
                        <label>
                            Name: <input size="10" name="user" value={this.state.name} onChange={this.handleChange}/>
                        </label>
                        <button className="btn btn-primary" onClick={() => this.saveUser()}>Save</button>
                    </div>
                }
            </div>
        );
    }
}
