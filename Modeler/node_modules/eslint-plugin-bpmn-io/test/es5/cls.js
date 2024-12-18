var inherits = require('inherits');

var Bar = require('./bar').Bar;


function Foo(a, b, config) {
  Bar.prototype.call(this, a, b, config.c);

  this.foo = this.foo.bind(this);
}

inherits(Foo, Bar);

/**
 * Do foo
 */
Foo.prototype.foo = function(d) {

  var blub = Blub.blub.apply(Blub, d);

  // do bar
  return Object.assign({}, this.bar, {
    blub: blub,
    d: add(d, 1)
  });
};

Foo.prototype.longArgFn = function(
    hello, world,
    wowo, wooooooooorld) {

};


// helpers

function add(a, b) {
  return a + b;
}
