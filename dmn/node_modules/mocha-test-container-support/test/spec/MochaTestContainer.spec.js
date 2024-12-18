var TestContainer = window.MochaTestContainer;

/* global describe, it, beforeEach, expect */
/* jshint expr:true */

describe('mocha-test-container-support', function() {


  describe('definition', function() {

    var container,
        containerDefinition;

    beforeEach(function() {
      container = TestContainer.get(this);
      containerDefinition = TestContainer.getDefinition(this);
    });


    it('should configure', function() {

      // when
      var localContainer = TestContainer.get(this);

      // then
      expect(localContainer).to.exist;
      expect(localContainer).to.equal(container);

      expect(localContainer).to.equal(containerDefinition._contentElement);
    });

  });


  this.timeout(200000);


  describe('on test level', function() {

    var container;

    beforeEach(function() {
      container = TestContainer.get(this);

      // expect initially empty
      expect(container.innerHTML).to.eql('');
    });


    it('should populate in test', function() {

      var localContainer = TestContainer.get(this);

      // given
      var element1 = document.createElement('div');
      var element2 = document.createElement('div');

      // when
      localContainer.appendChild(element1);
      localContainer.appendChild(element2);
    });


    afterEach(function() {

      var localContainer = TestContainer.get(this);

      // then
      expect(localContainer).to.equal(container);

      expect(localContainer.innerHTML).to.equal('<div></div><div></div>');
    });

  });



  describe('on suite level', function() {

  });

});
