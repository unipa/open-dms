module.exports = {
  plugins: [
    'mocha'
  ],
  env: {
    mocha: true,
    node: true
  },
  rules: {
    'mocha/no-exclusive-tests': 2
  },
  globals: {
    expect: true
  }
};