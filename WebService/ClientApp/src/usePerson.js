import {useEffect, useState} from 'react';
import useToken from "./useToken";

export default function usePerson() {
    const {token, setToken} = useToken();
    const [person, setPerson] = useState(null);
    
    async function createPerson(name, email) {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userName: name, email: email })
        };
        const response = await fetch('api/person/addPerson', requestOptions);
        const person = await response.json();
        setToken(person.id);
        return person;
    }
    
    useEffect(() => {
        async function getPerson(personId) {
            const requestOptions = {
                method: 'GET',
                headers: { 'Content-Type': 'application/json' }
            };
            const response = await fetch('api/person/' + personId, requestOptions);
            return await response.json();
        }
        
        if (token) {
            getPerson(token)
                .then((person) => {
                    setPerson(person);  
                }).catch(() => {
                    // Right now assuming that if we get an error,
                    // then the token is an old testing user ID and the
                    // user doesn't exist anymore so we clear the token.
                    // TODO: Sign the user out if it's a 404 or some response that indicates the user doesn't exist.
                    setPerson(null);
            });
        }
    }, [token]);

    return {
        addPerson: createPerson,
        person
    }
}