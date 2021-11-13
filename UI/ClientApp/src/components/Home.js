import React, { Component } from 'react';
import WordsLookup from '../../src/components/words-lookup';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
        <div>
            <WordsLookup />
      </div>
    );
  }
}
