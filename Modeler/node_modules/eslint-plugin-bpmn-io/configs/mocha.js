module.exports = {
  plugins: [
    'mocha'
  ],
  env: {
    mocha: true
  },
  rules: {
    'mocha/no-exclusive-tests': 2
  },
  globals: {
    expect: true
  }
};