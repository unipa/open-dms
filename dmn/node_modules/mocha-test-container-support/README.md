# mocha-test-container-support

This [Mocha](https://github.com/mochajs/mocha) plugin provides test
hooks for Mocha that add a container for every test case.
The plugin is inspired by [jasmine-test-container-support](https://github.com/bpmn-io/jasmine-test-container-support) by [Nico Rehwaldt](https://github.com/nikku).


## Install

```bash
npm install --save-dev mocha-test-container-support
```


## Features

* Add one test container for every test to the DOM
* Allows checking the result of your test code visually while TDD
* Full title (suite + test) is displayed for every test case
* Test result is indicated by color and text
* Tests are marked with an anchor element for navigation purposes

### Limits of the plugin

* __No__ 'real' encapsulation of your tests
  * CSS / JS / DOM changes can still break other tests
  * Make sure tests use the test container as DOM root otherwise your DOM is not rendered in the container
  * Test is only rendered into a `<div>`, no `<iframe>` or shadow DOM


## Usage

To get container support for all tests just add a test file to the test root containing the following:

```js
var TestContainer = require('mocha-test-container-support');
var testContentContainer;

beforeEach(function() {
  testContentContainer = TestContainer.get(this);
});
```

The created test container will have this structure:

```html
<div class="test-container passed" id="254">
  <div class="title-row">
    <a href="#254">
      <h3 class="test-titel">testsuite - feature should do</h3>
    </a>
    <div class="test-result">passed</div>
  </div>
  <div class="test-content-container">
    Your content will be added here.
  </div>
</div>
```

The `get()` function returns an instance of the test content container. That's where custom test related DOM content should be added.

```js
var testContentContainer = TestContainer.get(this);

testContentContainer.appendChild(yourDomContent);
```


## License

MIT


## History

* v0.2.0 - Core Refactoring, bug fixes
* v0.1.0 - Changed API
  * Need to explicit call TestContainer.get(this) to create a container for the test
  * Use links based on test name
* v0.0.1 - Initial release, test container
