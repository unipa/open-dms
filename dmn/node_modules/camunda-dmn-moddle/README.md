# camunda-dmn-moddle

[![CI](https://github.com/camunda/camunda-dmn-moddle/workflows/CI/badge.svg)](https://github.com/camunda/camunda-dmn-moddle/actions?query=workflow%3ACI)

This project defines the [Camunda](https://camunda.org) namespace extensions for DMN as a [moddle](https://github.com/bpmn-io/moddle) descriptor.


## Usage

Use it together with [dmn-moddle](https://github.com/bpmn-io/dmn-moddle) to validate Camunda DMN extensions.

```javascript
var DmnModdle = require('dmn-moddle');

var camundaModdle = require('camunda-dmn-moddle/resources/camunda');

var moddle = new DmnModdle({ camunda: camundaModdle });

var decision = moddle.create('dmn:Decision', {
  'historyTimeToLive': 'foo'
});
```


## Building the Project

To run the test suite that includes XSD schema validation you must have a Java JDK installed and properly exposed through the `JAVA_HOME` variable.

Execute the test via

```
npm test
```

Perform a complete build of the application via

```
npm run all
```


## License

Use under the terms of the [MIT license](http://opensource.org/licenses/MIT).

