'use strict';

var minDom = require('min-dom');

var OVERLAY_HTML = '<div class="drop-overlay">' +
                     '<div class="box">' +
                        '<div class="label">{label}</div>' +
                     '</div>' +
                   '</div>';

/**
 * Add file drop functionality to the given element,
 * calling fn(files...) on drop.
 *
 * @example
 *
 * var node = document.querySelector('#container');
 *
 * var dropHandler = fileDrop(handleFiles);
 *
 * node.addEventListener('dragover', dropHandler);
 *
 * @param {String} [label='Drop files here']
 * @param {Function} fn
 *
 * @return {Function} drag start callback function
 */
function fileDrop(label, fn) {

  if (typeof label === 'function') {
    fn = label;
    label = 'Drop files here';
  }

  var self;
  var extraArgs;

  // we are bound, if overlay exists
  var overlay;

  function onDrop(event) {

    event.preventDefault();

    asyncMap(event.dataTransfer.files, readFile, function(err, files) {

      if (err) {
        console.warn('file drop failed', err);
      } else {

        var args = extraArgs.concat([ files, event ]);

        // cleanup on drop
        // onEnd(event);

        // call provided fn with extraArgs..., files, event
        fn.apply(self, args);
      }
    });
  }

  function isDragAllowed(dataTransfer) {

    if (!dataTransfer || !dataTransfer.items.length) {
      return false;
    }

    var hasFile = false;

    for (var i = 0; i < dataTransfer.items.length; i++) {
      if (dataTransfer.items[i].type === 'file' || dataTransfer.items[i].kind === 'file') {
        hasFile = true;
      }
    }

    return hasFile;
  }

  function onDragover() {

    // (0) extract extra arguments (extraArgs..., event)
    var args = slice(arguments),
        event = args.pop();

    var dataTransfer = event.dataTransfer,
        target = event.currentTarget || event.target;

    if (!isDragAllowed(dataTransfer)) {
      return;
    }

    // make us a drop zone
    event.preventDefault();

    dataTransfer.dropEffect = 'copy';

    // only register if we do not drag and drop already
    if (overlay) {
      return;
    }

    overlay = createOverlay(label);

    target.appendChild(overlay);

    self = this;
    extraArgs = args;


    // do not register events during testing
    if (!target) {
      return;
    }


    // (2) setup drag listeners

    function onLeave(event) {

      var relatedTarget = event.relatedTarget;

      if (target.contains(relatedTarget)) {
        return;
      }

      onEnd();
    }

    // (2.1) detach on end
    function onEnd(event) {

      document.removeEventListener('drop', onDrop);
      document.removeEventListener('drop', onEnd);
      document.removeEventListener('dragleave', onLeave);
      document.removeEventListener('dragend', onEnd);
      document.removeEventListener('dragover', preventDrop);

      if (overlay) {
        target.removeChild(overlay);
        overlay = null;
      }
    }

    // (2.0) attach drag + cleanup event
    document.addEventListener('drop', onDrop);
    document.addEventListener('drop', onEnd);
    document.addEventListener('dragleave', onLeave);
    document.addEventListener('dragend', onEnd);
    document.addEventListener('dragover', preventDrop);
  }

  onDragover.onDrop = onDrop;

  return onDragover;
}


// helpers ////////////////////////////////////

function readFile(dropFile, done) {

  if (!window.FileReader) {
    return done();
  }

  var reader = new FileReader();

  // Closure to capture the file information.
  reader.onload = function(e) {

    done(null, {
      name: dropFile.name,
      path: dropFile.path,
      contents: e.target.result
    });
  };

  reader.onerror = function(event) {
    done(event.target.error);
  };

  // Read in the image file as a data URL.
  reader.readAsText(dropFile);
}


function asyncMap(elements, iterator, done) {

  var idx = 0,
      results = [];

  function next() {

    if (idx === elements.length) {
      done(null, results);
    } else {

      iterator(elements[idx], function(err, result) {

        if (err) {
          return done(err);
        } else {
          results[idx] = result;
          idx++;

          next();
        }
      });
    }
  }

  next();
}

function slice(arr) {
  return Array.prototype.slice.call(arr);
}

function createOverlay(label) {
  var markup = OVERLAY_HTML.replace('{label}', label);

  return minDom.domify(markup);
}

function preventDrop(event) {
  event.preventDefault();
}

module.exports = fileDrop;
//# sourceMappingURL=index.js.map
