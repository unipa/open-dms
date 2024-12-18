export default class CustomPalette {
  constructor(bpmnFactory, create, elementFactory, palette, translate) {
    this.bpmnFactory = bpmnFactory;
    this.create = create;
    this.elementFactory = elementFactory;
    this.translate = translate;

    palette.registerProvider(this);
  }

  getPaletteEntries(element) {
    const {
      bpmnFactory,
      create,
      elementFactory,
      translate
    } = this;

    function _extends() {
      _extends = Object.assign || function (target) {
        for (var i = 1; i < arguments.length; i++) {
          var source = arguments[i];

          for (var key in source) {
            if (Object.prototype.hasOwnProperty.call(source, key)) {
              target[key] = source[key];
            }
          }
        }

        return target;
      };

      return _extends.apply(this, arguments);
    }

    function assign(target) {
      for (var _len = arguments.length, others = new Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
        others[_key - 1] = arguments[_key];
      }

      return _extends.apply(void 0, [target].concat(others));
    }

    function createCustomTask(type) {
      return function(event) {
        const businessObject = bpmnFactory.create(type);
        const shape = elementFactory.createShape({
          type: type,
          businessObject: businessObject
        });
        create.start(event, shape);
      };
    }

    var actions = {};

    assign(actions, {
      'create.separatore-mattoncini-custom': {
        separator: true
      }
	  });

    var type = "elmi:CustomTask";
    
    //qui creo l'oggetto con i mattoncini da aggiungere all'editor (da mappare con i valori che la chiamata api ritornerà)
    let newActions = {
      'create.elmi:CustomTask': {
        className: 'fas fa-cogs',
        type: "elmi:CustomTask",
        title: translate('Custom Task'),
        action: {
          dragstart: createCustomTask(type),
          click: createCustomTask(type)
        }
      }
    };
      
    // cicla attraverso ogni proprietà dell'oggetto newActions e aggiungile all'oggetto actions
    for (let key in newActions) {
      if (newActions.hasOwnProperty(key)) {
        actions[key] = newActions[key];
      }
    }
    //commento perché l'aggiunta è stata gestita direttamente lato paletteprovider.js
    //return actions;
  }
}

CustomPalette.$inject = [
  'bpmnFactory',
  'create',
  'elementFactory',
  'palette',
  'translate'
];