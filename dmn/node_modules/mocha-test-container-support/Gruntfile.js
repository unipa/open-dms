module.exports = function(grunt) {

  require('load-grunt-tasks')(grunt);

  // project configuration
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    karma: {
      options: {
        configFile: 'test/config/karma.unit.js',
      },
      single: {
        singleRun: true,
        browsers: [ 'PhantomJS' ],
        autoWatch: false,
      },
      unit: {
        singleRun: false,
        browsers: [ 'Chrome' ],
        debug: true
      }
    },

    jshint: {
      src: [ 'lib/**/*.js', 'test/spec/**/*.js' ],

      options: {
        jshintrc: true
      }
    },

    release: {
      options: {
        tagName: 'v<%= version %>',
        commitMessage: 'chore(project): release v<%= version %>',
        tagMessage: 'chore(project): tag v<%= version %>'
      }
    }
  });
  // tasks

  grunt.registerTask('test', [ 'karma:single' ]);

  grunt.registerTask('auto-test', [ 'karma:unit' ]);

  grunt.registerTask('default', [ 'jshint', 'test' ]);
};
