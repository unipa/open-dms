# file-drops

A simple in-browser file drop utility.


## Usage

```javascript
import fileDrop from 'file-drops';

const element = document.querySelector('#container');

const dropHandler = fileDrop('Drop a file', function(files) {
  // files = [ { name, contents }, ... ]
});

element.addEventListener('dragover', dropHandler);
```


## Styling

On drop over, the utility will attach the following overlay to the
element for which the drop handler got registered:

```html
<div class="drop-overlay">
  <div class="box">
    <div class="label">{label}</div>
  </div>
</div>
```

Style it as you wish. :heart:


## License

MIT
