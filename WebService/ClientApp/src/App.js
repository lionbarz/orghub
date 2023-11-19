import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { About } from './components/About';
import { Groups } from './components/Groups';
import { Group } from './components/Group';
import { AddMassMeeting } from './components/AddMassMeeting';

import './custom.css'
import {AddGroup} from "./components/AddGroup";
import {ZoomTest} from "./components/ZoomTest";
import {Meeting} from "./components/Meeting";

export default class App extends Component {
  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/about' component={About} />
        <Route path='/groups' component={Groups} />
        <Route path='/group/:groupId' component={Group} />
        <Route path='/add-mass-meeting' component={AddMassMeeting} />
        <Route path='/add-group' component={AddGroup} />
        <Route path='/meeting/:meetingId' component={Meeting} />
        <Route path='/zoom-test' component={ZoomTest} />
      </Layout>
    );
  }
}
