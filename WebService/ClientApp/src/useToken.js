import { useState } from 'react';

// Saves and gets a string token for user "auth".
// Right now I'm using it to store user IDs in local storage.
// Later it should be user IDs and an actual auth token.
export default function useToken() {
    const getToken = () => {
        const tokenString = localStorage.getItem('token');
        return JSON.parse(tokenString);
    };

    const [token, setToken] = useState(getToken());

    const saveToken = userToken => {
        localStorage.setItem('token', JSON.stringify(userToken));
        setToken(userToken);
    };

    return {
        setToken: saveToken,
        token
    }
}