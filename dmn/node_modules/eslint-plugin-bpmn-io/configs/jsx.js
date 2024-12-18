module.exports = {
  plugins: [
    'react'
  ],
  rules: {
    'jsx-quotes': [ 2, 'prefer-double' ],
    'react/jsx-curly-spacing': [ 2, { when: 'always' } ],
    'react/jsx-equals-spacing': 2,
    'react/jsx-indent': [ 2, 2 ],
    'react/jsx-indent-props': [ 2, 2 ],
    'react/jsx-no-undef': 2,
    'react/jsx-no-duplicate-props': 2,
    'react/jsx-pascal-case': 2,
    'react/jsx-tag-spacing': [ 2, {
      closingSlash: 'never',
      beforeSelfClosing: 'always',
      afterOpening: 'never',
      beforeClosing: 'never'
    } ],
    'react/jsx-uses-vars': 2
  },
  parserOptions: {
    ecmaFeatures: {
      jsx: true
    }
  }
};