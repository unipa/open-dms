# karma-debug-launcher

[![CI Status](https://img.shields.io/github/workflow/status/nikku/karma-debug-launcher/CI/main)](https://github.com/nikku/karma-debug-launcher/actions?query=workflow%3ACI)

Automatically open your Karma spec in your favorite browser. An imaginary launcher.


## Usage

Install the utility via [npm](https://www.npmjs.com/package/karma-debug-launcher):

```sh
npm install karma-debug-launcher
```

Integrate it as a browser in your `karma.config.js`:

```javascript
module.exports = function(karma) {

  karma.set({
    ...,

    browsers: [ 'Debug' ]
  });
};
```


## License

MIT
