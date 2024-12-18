# @bpmn-io/feel-lint

Linting for FEEL expressions.

## Usage

There are 2 ways to use this library:

### Linting string expressions

The `lintExpression` function takes a string expression and returns a list of linting errors.

```javascript
import { lintExpression } from "@bpmn-io/feel-lint"

lintExpression('foo = bar');
```

### Codemirror plugin

The `cmFeelLinter` function returns a codemirror linting source that you can use as a extension
in you codemirror instance.

```javascript
import { cmFeelLinter } from "@bpmn-io/feel-lint"
import { linter } from '@codemirror/lint';

// ...

const myEditor = new EditorView({
    state: EditorState.create({
      doc: '',
      extensions: [
        linter(cmFeelLinter())
      ]
    })
  });
```


## Hacking the Project

To get the development setup make sure to have [NodeJS](https://nodejs.org/en/download/) installed.
As soon as you are set up, clone the project and execute

```
npm install
npm run dev
```

## License

MIT
