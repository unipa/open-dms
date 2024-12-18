'use strict';

var browser = require('./configs/browser');
var node = require('./configs/node');
var mocha = require('./configs/mocha');
var jsx = require('./configs/jsx');

var recommended = require('./configs/recommended');


module.exports = {
  configs: {
    browser,
    node,
    mocha,
    jsx,
    recommended
  }
};
