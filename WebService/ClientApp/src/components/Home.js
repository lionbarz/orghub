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
                <div className="jumbotron">
                    <h1 className="display-4">Organize. Raise money. Grow. Make a difference.</h1>
                    <p className="lead">Free for groups under 10 members.</p>
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
