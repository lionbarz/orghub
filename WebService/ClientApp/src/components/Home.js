import React, {Component} from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {name: ""};
    }

    componentDidMount() {
        this.populateUserName();
    }
    
    populateUserName() {
        
    }

    async saveUser() {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName: this.state.name })
        };
        const response = await fetch('person/addPerson', requestOptions);
        const data = await response.json();
        localStorage.setItem('userId', data.id);
    }

    handleChange = (event) => {
        const value = event.target.value;
        this.setState({name: value});
    };

    render() {
        return (
            <div>
                <div className="jumbotron">
                    <h1 className="display-4">Group. Raise money. Grow. Make a difference.</h1>
                    <p className="lead">Free for groups under 10 members.</p>
                </div>
                <h2>Who are you?</h2>
                <label>
                    Name: <input size="50" name="user" value={this.state.name} onChange={this.handleChange}/>
                </label>
                <button className="btn btn-primary" onClick={() => this.saveUser()}>Save</button>
            </div>
        );
    }
}
