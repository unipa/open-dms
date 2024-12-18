const {
  URL
} = require('url');

const open = require('open');


/**
 * A simple karma launcher that opens the debug view
 * of the test suite at hand with your default browser,
 * via the <open> utility.
 *
 * @param {Function} baseBrowserDecorator
 * @param {Object} args
 */
function DebugBrowser(baseBrowserDecorator, args) {
  baseBrowserDecorator(this);

  this._start = function(url) {

    const debugURL = new URL('/debug.html', url).toString();

    open(debugURL)
      .then(() => this._done())
      .catch(err => this._done(err));
  };
}

DebugBrowser.prototype = {
  name: 'Debug'
};

DebugBrowser.$inject = [ 'baseBrowserDecorator', 'args' ];

// PUBLISH DI MODULE
module.exports = {
  'launcher:Debug': [ 'type', DebugBrowser ]
};