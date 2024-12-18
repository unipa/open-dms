'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

var lezerFeel = require('lezer-feel');
var language = require('@codemirror/language');

/**
 * Create an array of syntax errors in the given tree.
 *
 * @param {Tree} syntaxTree
 * @returns {LintMessage[]} array of syntax errors
 */
function lintSyntax(syntaxTree) {

  const lintMessages = [];

  syntaxTree.iterate({
    enter: node => {
      if (node.type.isError) {

        const error = node.toString();

        /* The error has the pattern [⚠ || ⚠(NodeType)]. The regex extracts the node type from inside the brackets */
        const match = /\((.*?)\)/.exec(error);
        const nodeType = match && match[1];

        let message;

        if (nodeType) {
          message = 'unexpected ' + nodeType;
        } else {
          message = 'expression expected';
        }

        lintMessages.push(
          {
            from: node.from,
            to: node.to,
            severity: 'error',
            message: message,
            type: 'syntaxError'
          }
        );
      }
    }
  });

  return lintMessages;
}

/**
 * Generates lint messages for the given syntax tree.
 *
 * @param {Tree} syntaxTree
 * @returns {LintMessage[]} array of all lint messages
 */
function lintAll(syntaxTree) {

  const lintMessages = [
    ... lintSyntax(syntaxTree)
  ];

  return lintMessages;
}

/**
 * Create an array of syntax errors for the given expression.
 *
 * @param {String} expression
 * @returns {LintMessage[]} array of syntax errors
 */
function lintExpression(expression) {

  const syntaxTree = lezerFeel.parser.parse(expression);

  const lintMessages = lintAll(syntaxTree);

  return lintMessages;
}

/**
 * CodeMirror extension that provides linting for FEEL expressions.
 *
 * @param {EditorView} editorView
 * @returns {Source} CodeMirror linting source
 */
const cmFeelLinter = () => editorView => {

  // don't lint if the Editor is empty
  if (editorView.state.doc.length === 0) {
    return [];
  }

  const tree = language.syntaxTree(editorView.state);

  const messages = lintAll(tree);

  return messages.map(message => ({
    ...message,
    source: 'syntaxError'
  }));
};

exports.cmFeelLinter = cmFeelLinter;
exports.lintExpression = lintExpression;
