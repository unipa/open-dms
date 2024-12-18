# eslint-plugin-bpmn-io

[![CI](https://github.com/bpmn-io/eslint-plugin-bpmn-io/workflows/CI/badge.svg)](https://github.com/bpmn-io/eslint-plugin-bpmn-io/actions?query=workflow%3ACI)

Commont lint rules for [bpmn.io](https://bpmn.io) projects.


## Use

Extend `bpmn-io/*` in your base eslint configuration:

```json
{
  "extends": "plugin:bpmn-io/es6"
}
```

If you use [mocha](https://mochajs.org/) for testing, place another `.eslintrc` into your `test` folder and extend from `bpmn-io/mocha`:

```json
{
  "extends": "plugin:bpmn-io/mocha"
}
```


## Available Recommended Configurations

We ship with a few blue prints:

* `bpmn-io/es5`: Use for ES5 style projects / node libraries
* `bpmn-io/es6`: Use for modern, ES2015 based projects
* `bpmn-io/jsx`: Use `jsx` in conjunction with `es6`
* `bpmn-io/mocha`: Use for libraries tested with `mocha` + `chai`


## License

MIT
