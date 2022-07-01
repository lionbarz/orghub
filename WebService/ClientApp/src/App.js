import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { About } from './components/About';
import { Groups } from './components/Groups';
import { Group } from './components/Group';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/about' component={About} />
        <Route path='/groups' component={Groups} />
        <Route path='/group/:id' component={Group} />
      </Layout>
    );
  }
}
