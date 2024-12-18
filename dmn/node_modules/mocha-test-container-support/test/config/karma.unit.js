module.exports = function(karma) {
  karma.set({

    basePath: '../../',

    frameworks: [ 'mocha',
                  'chai' ],

    files: [
      'lib/MochaTestContainer.js',
      'test/spec/**/*.spec.js'
    ],

    reporters: [ 'dots' ],

    browsers: [ 'Chrome' ],

    browserNoActivityTimeout: 30000,

    singleRun: false,
    autoWatch: true,
  });
};
