(function ($) {
    $.widget("elmi.List", $.extend({}, { // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   list.js modificato per boostrap italia 
        options: {
            className: "",
            highlight: "active", // "ui-state-highlight",
            idField: null,
            iconClass: "",
            iconField: "",
            imageField: "",
            textField: "",
            rightTextField: "",
            commentField: "",
            model: null,
            items: "",
            onSelect: "Select",
            selectedId: "",
            HTML: "",
            NewItem: ""
        },
        _create: function () {
        },
        _init: function () {
            var that = this;
            var c = that.options.onSelect;
            var n = that.options.items;
            if (that.options.model) {
                that.options.model["Selected" + n + "Value"] = ko.observable();
                that.options.model["Selected" + n + "Object"] = ko.observable();
                // chiamata dalla GUI
                that.options.model["Select" + n] = function (data) {
                    var ok = true;
                    var newValue = data[that.options.idField]();
                    if (that.options.onSelect)
                        (that.options.model[that.options.onSelect].call(this, newValue, function () { that._set(newValue); }));
                    else
                        that._set(newValue);
                };
                c = "Select" + n;
            }
            if (this.options.textField) {
                var $ul = $("<ul class='link-list pt-4 " + (this.options.commentField ? "Lista3" : "Lista1")+"'></ul>").addClass(this.options.className).attr("data-bind", "foreach: " + this.options.items + "").appendTo(this.element);
                var $li = $("<li class='list-item' style='cursor:pointer;'></li>").appendTo($ul);
                var $item = $li;
                var $a = $(`<a class='left-icon p-0' data-bind='visible:  ` + that.options.idField + `() !=\"-\",  click: $root.` + c + `, attr: { itemid: ` + (this.options.idField) + `}'` + (this.options.commentField ? "" : '')+`>
                            </a>`).appendTo($item);

                var $h1 = $("<span class='' data-bind='visible: " + that.options.idField + "()==\"-\", text: " + that.options.textField + "'></span>").appendTo($item);
                var icon = this.options.iconClass;

                //if (icon)
                //    $a.append(`<svg class="icon icon-primary"><use href="/bootstrap-italia/svg/sprites.svg#${this.options.iconClass}"></use></svg>`);
                    //$a.append("<i class='" + this.options.iconClass + "'></i>"); //originale
                    
                if (this.options.iconField)
                    $a.append(`<svg class="icon icon-primary"><use data-bind='attr: { href:${this.options.iconField}}'></use></svg>`);
                if (this.options.imageField)
                    $a.append("<img data-bind='attr: { src: " + this.options.imageField + " }' />");
                $a.append("<span data-bind='text: " + this.options.textField + "'></span>");
                if (this.options.commentField)
                    $a.append("<sub data-bind='text: " + this.options.commentField + "'></sub>");
                if (this.options.rightTextField)
                    $a.append("<small data-bind='text: " + this.options.rightTextField + "'></small>");
                if (this.options.HTML)
                    $a.append(this.options.HTML);

            }
        },
        _set: function (itemid) {
            that = this;
            $(that.element).find("a").removeClass(that.options.highlight);
            that.options.model["Selected" + that.options.items + "Value"](itemid);
            that.options.model["Selected" + that.options.items + "Object"](ko.utils.arrayFirst(that.options.model[that.options.items](), function (item) { return item[that.options.idField]().toUpperCase() === itemid.toUpperCase(); }));

            var s = $(that.element).find("[itemid='" + itemid + "']");
            if (s.length > 0) s.addClass(that.options.highlight); //else alert("elemento non trovato");
        },
        destroy: function () {
        }


    }));
})(jQuery);
