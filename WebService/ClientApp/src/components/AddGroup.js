import React, {useState} from 'react';
import usePerson from "../usePerson";
import {GuestLoginComponent} from "./GuestLoginComponent";
import {Grid, TextField} from "@mui/material";
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import Avatar from '@mui/material/Avatar';
import PersonIcon from '@mui/icons-material/Person';
import Button from "@mui/material/Button";

export function AddGroup() {
    const [groupName, setGroupName] = useState('');
    const [groupMission, setGroupMission] = useState('');
    const [memberEmails, setMemberEmails] = useState([]);
    const [newGroupMember, setNewGroupMember] = useState('');
    const {person, addPerson} = usePerson();

    function handleChangeGroupName(event) {
        setGroupName(event.target.value);
    }

    function handleChangeGroupMission(event) {
        setGroupMission(event.target.value);
    }
    
    function handleAddMember() {
        setMemberEmails([
            ...memberEmails,
            newGroupMember
        ])
        setNewGroupMember('');
    }
    
    async function addGroup() {
        const meetingTime = (new Date()).toISOString();
        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                personId: person.id,
                memberEmails: memberEmails,
                name: groupName,
                mission: groupMission,
                nextMeeting: {
                    description: "Talk about eating pumpkins",
                    startTime: meetingTime
                }
            })
        };
        await fetch('api/group', requestOptions);
    }
    
    if (!person) {
        return (
            <div>
                <h1>Sign in to create a group</h1>
                <GuestLoginComponent person={person} addPerson={addPerson} />
            </div>
        );
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
            <h1 id="add-group-label">New group</h1>
            </Grid>
            <Grid item xs={12}>
                <TextField fullWidth id="outlined-basic" label="Name" variant="outlined" onChange={handleChangeGroupName} />
            </Grid>
            <Grid item xs={12}>
                <TextField multiline fullWidth id="outlined-basic" label="Mission" variant="outlined" onChange={handleChangeGroupMission} />
            </Grid>
            <Grid item xs={12}>
                <TextField fullWidth disabled id="outlined-basic" label="Chairperson" variant="outlined" value={person.name} helperText='As the group creator, you will be the temporary chairperson until the group elects someone else.'/>
            </Grid>
            <Grid item xs={12}>
                <p><strong>Members</strong></p>
                <p>Specify the members of the group. Removing or adding members will require a vote after the group is created.</p>
                <Grid item xs={12}>
                    <TextField fullWidth id="outlined-basic" label="Email" variant="outlined" value={newGroupMember} onChange={e => setNewGroupMember(e.target.value)} />
                </Grid>
                <Button variant="contained" onClick={() => handleAddMember()}>Add member</Button>
                <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
                    {memberEmails.map(member =>
                        <ListItem key={member}>
                            <ListItemAvatar>
                                <Avatar>
                                    <PersonIcon />
                                </Avatar>
                            </ListItemAvatar>
                            <ListItemText primary={member} />
                        </ListItem>
                    )}
                </List>
            </Grid>
            <Button variant="contained" onClick={addGroup}>Create group</Button>
        </Grid>
    );
}
