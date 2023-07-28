import {useEffect, useState} from 'react';
import ZoomVideo from "@zoom/videosdk";

// Creates a client for Zoom Video SDK.
export default function useZoom(authToken, userName, topic, viewCanvasId, viewVideoId, participantsCanvasId) {
    const [stream, setStream] = useState(null);
    const [client, setClient] = useState(null);
    function startVideo() {
        if (!stream) {
            console.error('Stream has not been initialized.');
            return;
        }
        
        // if Desktop Chrome, Edge, and Firefox with SharedArrayBuffer not enabled, Android browsers, and on devices with less than 4 logical processors available
        if((!stream.isSupportMultipleVideos() && (typeof OffscreenCanvas === 'function')) || /android/i.test(navigator.userAgent)) {
            // start video - video will render automatically on HTML Video Element if MediaStreamTrackProcessor is supported
            stream.startVideo({ videoElement: document.querySelector('#' + viewVideoId) }).then(() => {
                // if MediaStreamTrackProcessor is not supported
                if(!(typeof MediaStreamTrackProcessor === 'function')) {
                    // render video on HTML Canvas Element
                    stream.renderVideo(document.querySelector('#' + viewCanvasId), client.getCurrentUserInfo().userId, 1920, 1080, 0, 0, 2).then(() => {
                        // show HTML Canvas Element in DOM
                        document.querySelector('#' + viewCanvasId).style.display = 'block'
                    }).catch((error) => {
                        console.log(error)
                    })
                } else {
                    // show HTML Video Element in DOM
                    document.querySelector('#' + viewVideoId).style.display = 'block'
                }
            }).catch((error) => {
                console.log(error)
            })
// desktop Chrome, Edge, and Firefox with SharedArrayBuffer enabled, and all other browsers
        } else {
            // start video
            stream.startVideo().then(() => {
                // render video on HTML Canvas Element
                stream.renderVideo(document.querySelector('#' + viewCanvasId), client.getCurrentUserInfo().userId, 1920, 1080, 0, 0, 2).then(() => {
                    // show HTML Canvas Element in DOM
                    document.querySelector('#' + viewCanvasId).style.display = 'block'
                }).catch((error) => {
                    console.log(error)
                })
            }).catch((error) => {
                console.log(error)
            })
        }
    }
    
    useEffect(() => {
        async function getToken() {
            const requestOptions = {
                method: 'GET',
                headers: { 'Content-Type': 'application/json' }
            };
            const response = await fetch('api/zoom/token?topic=' + topic + '&authToken=' + authToken, requestOptions);
            return await response.json();
        }
        
        if (!authToken) {
            return;
        }

        getToken().then((token) => {
            console.log('got token: ' + token);
            const client = ZoomVideo.createClient();
            console.log('created client');
            setClient(client);
            client.init('en-US', 'CDN').then(() => {
                console.log('initialized. joining');
                client.join(topic, token, userName, null).then(() => {
                    console.log('success! joined!');
                    console.log('you are user ' + client.getCurrentUserInfo().userId);
                    let stream = client.getMediaStream();
                    setStream(stream);
                    
                    // Listen to people turning video on/off to render them or not.
                    client.on('peer-video-state-change', (payload) => {
                        if (payload.action === 'Start') {
                            const xCord = 0;
                            const yCord = 0;
                            console.log(payload.userId + ' joined! rendering');
                            stream.renderVideo(document.querySelector('#' + participantsCanvasId), payload.userId, 960, 540, xCord, yCord, 2);
                        } else if (payload.action === 'Stop') {
                            console.log(payload.userId + ' left! stopped rendering');
                            stream.stopRenderVideo(document.querySelector('#' + participantsCanvasId), payload.userId);
                        }
                    })
                    
                    // Get all existing users and render their videos.
                    client.getAllUser().forEach((user) => {
                        if (user.bVideoOn) {
                            const xCord = 0;
                            const yCord = 0;
                            console.log('Rendering participant ' + user.userId);
                            stream.renderVideo(document.querySelector('#' + participantsCanvasId), user.userId, 960, 540, xCord, yCord, 2);
                        }
                    });
                }).catch((error) => {
                    console.log("Oh noes", error);
                });
            }).catch((error) => { 
                console.log("init failed.", error);
            });
        }).catch((error) => {
            console.log("failed to get token", error);
        });
    }, [authToken, topic, viewCanvasId, viewVideoId, participantsCanvasId, userName]);

    return {
        startVideo
    }
}