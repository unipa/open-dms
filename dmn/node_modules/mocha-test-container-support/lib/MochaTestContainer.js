
// BEGIN function closure
;(function() {

  'use strict';

  /* global afterEach, beforeEach */


  function MochaTestContainer() { }


  MochaTestContainer.prototype._init = function() {
    var id = 0;

    addStyle(CONTAINER_STYLE);

    this._initilized = true;
    this.setupAfterEach();
  };


  /**
   * Return the test-container for the current test.
   * The container will be created if not already exists.
   *
   * @param context - provide the context of the test i.e. this
   *
   * @return {DOMElement} container DOMElement
   */
   MochaTestContainer.prototype.get = function(context) {

    var containerDefinition = this.getDefinition(context);

    return containerDefinition._contentElement;
  };


  MochaTestContainer.prototype.getDefinition = function(context) {

    if (!this._initilized) {
      this._init();
    }

    var currentTest = context && (context.currentTest || context._runnable);

    if (!currentTest) {
      throw new Error(
        'no test context! ' +
        'make sure you call the test container ' +
        'with this (i.e. the tests context)');
    }

    var containerDefinition = currentTest[CONTAINER_SUPPORT_ATTACH];

    if (!containerDefinition) {
      containerDefinition = this.createDefinition(currentTest);

      currentTest[CONTAINER_SUPPORT_ATTACH] = containerDefinition;
    }

    return containerDefinition;
  };


  MochaTestContainer.prototype.createDefinition = function(test) {

    // create container
    var containerElement = document.createElement('div');
    containerElement.classList.add('test-container');
    containerElement.setAttribute('id', test.fullTitle());

    // title
    var headerElement = document.createElement('div');
    headerElement.classList.add('title-row');

    var titleLink = document.createElement('a');
    titleLink.setAttribute('href', '#' + encodeURIComponent(test.fullTitle()));

    var title = document.createElement('h3');
    title.classList.add('test-titel');
    title.textContent = test.fullTitle();

    titleLink.appendChild(title);
    headerElement.appendChild(titleLink);

    var contentElement = document.createElement('div');
    contentElement.classList.add('test-content-container');

    // setup container content
    containerElement.appendChild(headerElement);
    containerElement.appendChild(contentElement);

    // add container to DOM
    document.querySelector('body').appendChild(containerElement);

    // container definition
    return {
      _id: test.fullTitle(),
      _containerElement: containerElement,
      _headerElement: headerElement,
      _contentElement: contentElement
    };
  };

  MochaTestContainer.prototype.setupAfterEach = function() {

    var self = this;

    afterEach(function() {

      var containerDefinition = self.getDefinition(this);

      if (!containerDefinition) {
        return;
      }

      var result = this.currentTest.state;

      var status = document.createElement('div');
      status.style.float = 'right';
      status.classList.add('test-result');


      if (result === 'passed') {
        containerDefinition._containerElement.classList.add('passed');
        status.textContent = 'passed';
      } else {
        containerDefinition._containerElement.classList.add('failed');
        status.textContent = 'failed';
      }

      containerDefinition._headerElement.appendChild(status);

      // Scroll to test
      if (window.location.hash.endsWith &&
          window.location.hash.endsWith('#' + encodeURIComponent(containerDefinition._id))) {

        containerDefinition._containerElement.scrollIntoView();
      }
    });
  };


  function addStyle(styleText) {

    var head = document.head;

    var styleElement = document.createElement('style');
    styleElement.type = 'text/css';
    styleElement.textContent = styleText;

    head.appendChild(styleElement);
  }


  /////// CONSTANTS /////////////////////////////

  var CONTAINER_SUPPORT_ATTACH = '__test_container_support__';

  var CONTAINER_STYLE =
    '.test-container {' +
    '  border: 1px solid black;' +
    '  height: 600px;' +
    '  margin-bottom: 10px;' +
    '  overflow: hidden;' +
    '}' +

    '.test-container.passed {' +
    '  border: 1px solid green;' +
    '}' +

    '.test-container.failed {' +
    '  border: 1px solid red;' +
    '}' +

    '.test-container.passed > div.title-row {' +
    '  background-color: green;' +
    '  border-color: green;' +
    '}' +

    '.test-container.failed > div.title-row {' +
    '  border-color: red;' +
    '  background-color: red;' +
    '}' +

    '.test-container > div.title-row {' +
    '  height: 24px;' +
    '  background-color: grey;' +
    '}' +

    '.test-container > div.title-row > a {' +
    '  text-decoration: none;' +
    '  display: inline-block;' +
    '}' +

    '.test-container > div.title-row > a > h3 {' +
    '  font-weight: normal;' +
    '  font-family: sans-serif;' +
    '  height: 16px;' +
    '  line-height: 16px;' +
    '  font-size: 14px;' +
    '  margin-top: 0;' +
    '  margin-bottom: 0;' +
    '  color: white;' +
    '  padding: 4px 10px 4px 10px;' +
    '}' +

    '.test-container > div.title-row > div.test-result {' +
    '  font-family: sans-serif;' +
    '  height: 16px;' +
    '  line-height: 16px;' +
    '  font-size: 14px;' +
    '  margin-top: 0;' +
    '  margin-bottom: 0;' +
    '  color: white;' +
    '  padding: 4px 10px 4px 10px;' +
    '}' +

    '.test-content-container {' +
      'height: 100%;' +
      'width: 100%;' +
    '}';


  //////// make publicly available ///////////////////////////

  if (typeof module !== 'undefined' && module.exports) {
    module.exports = new MochaTestContainer();
  } else {
    window.MochaTestContainer = new MochaTestContainer();
  }

// END function closure
})();