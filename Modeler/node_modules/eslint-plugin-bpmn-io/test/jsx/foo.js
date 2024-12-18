import Nested from './nested';

import { Component } from 'what';

export default class Foo extends Component {

  handleEvent = (a, b) => {
    console.log('event', a, b);
  };

  render() {

    return (
      <Nested
        text={ this.props.text }
        onChange={ this.handleEvent }
        key="value"
      >
        {
          this.props.values.map((v) => {
            return (
              <Nested.Val value={ v } />
            );
          })
        }
      </Nested>
    );
  }
}