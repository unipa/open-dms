# eslint-plugin-bpmn-io

[![CI](https://github.com/bpmn-io/eslint-plugin-bpmn-io/workflows/CI/badge.svg)](https://github.com/bpmn-io/eslint-plugin-bpmn-io/actions?query=workflow%3ACI)

Common lint rules for [bpmn.io](https://bpmn.io) projects.


## Use

Extend `bpmn-io/*` in your base eslint configuration:

```json
{
  "extends": "plugin:bpmn-io/browser"
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

* `bpmn-io/browser`: Use for browser based projects
* `bpmn-io/jsx`: Use `jsx` in conjunction with `browser`
* `bpmn-io/node`: Use for node based projects
* `bpmn-io/mocha`: Use for libraries tested with `mocha` + `chai`
* `bpmn-io/recommended`: Use for libraries where no environment is assumed


## License

MIT
