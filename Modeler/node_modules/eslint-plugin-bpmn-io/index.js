'use strict';

var es6 = require('./configs/es6');
var es5 = require('./configs/es5');
var mocha = require('./configs/mocha');
var jsx = require('./configs/jsx');

var recommended = require('./configs/recommended');


module.exports = {
  configs: {
    es6,
    es5,
    mocha,
    jsx,
    recommended
  }
};
