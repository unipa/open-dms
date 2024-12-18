module.exports = {
  env: {
    node: true
  },
  parserOptions: {
    ecmaVersion: 5
  },
  extends: [
    'eslint:recommended'
  ],
  rules: {
    'indent': [ 2, 2, {
      VariableDeclarator: { var: 2, let: 2, const: 3 },
      FunctionDeclaration: { body: 1, parameters: 2 },
      FunctionExpression: { body: 1, parameters: 2 }
    } ],
    'keyword-spacing': [ 2 ],
    'lines-around-comment': [ 2, { beforeBlockComment: true, beforeLineComment: true } ],
    'no-bitwise': 2,
    'no-console': 0,
    'no-multi-spaces': 2,
    'no-unused-vars': [ 2, { args: 'none', ignoreRestSiblings: true, varsIgnorePattern: '^_' } ],
    'no-trailing-spaces': [ 2 ],
    'object-curly-spacing': [ 2, 'always' ],
    'semi': [ 2, 'always' ],
    'space-before-blocks': [ 2, 'always' ],
    'space-before-function-paren': [ 'error', {
      'anonymous': 'never',
      'named': 'never',
      'asyncArrow': 'always'
    } ],
    'space-in-parens': [ 'error', 'never' ],
    'spaced-comment': [ 2, 'always' ],
    'quotes': [ 2, 'single', { avoidEscape: true } ]
  }
};
