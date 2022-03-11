import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Meeting } from './components/Meeting';
import { Groups } from './components/Groups';
import { Group } from './components/Group';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/meeting' component={Meeting} />
        <Route path='/groups' component={Groups} />
        <Route path='/group/:id' component={Group} />
      </Layout>
    );
  }
}
