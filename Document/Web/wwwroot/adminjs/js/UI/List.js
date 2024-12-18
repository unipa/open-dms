(function ($) {
    $.widget("elmi.List", $.extend({}, {
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
                var $ul = $("<ul class='nav nav-list " + (this.options.commentField ? "Lista3" : "Lista1")+"'></ul>").addClass(this.options.className).attr("data-bind", "foreach: " + this.options.items + "").appendTo(this.element);
                var $li = $("<li></li>").appendTo($ul);
                var $item = $li;
                var $a = $("<a href='#' data-bind='visible:  " + that.options.idField + "() !=\"-\",  click: $root." + c + ", attr: { itemid: " + (this.options.idField) + " }'" + (this.options.commentField ? "style='min-height:45px'" : '')+"></a>").appendTo($item);
                var $h1 = $("<span class='nav-header' data-bind='visible: " + that.options.idField + "()==\"-\", text: " + that.options.textField + "'></span>").appendTo($item);
                var icon = this.options.iconClass;

                if (icon)
                    $a.append("<i class='" + this.options.iconClass + "'></i>");
                if (this.options.iconField)
                    $a.append("<i data-bind='attr: { class:  " + this.options.iconField + " }'></i>");
                if (this.options.imageField)
                    $a.append("<img data-bind='attr: { src: " + this.options.imageField + " }' />");
                $a.append("<span data-bind='text: " + this.options.textField + "'></span>");
                if (this.options.commentField)
                    $a.append("<code data-bind='text: " + this.options.commentField + "'></code>");
                if (this.options.rightTextField)
                    $a.append("<small data-bind='text: " + this.options.rightTextField + "'></small>");
                if (this.options.HTML)
                    $a.append(this.options.HTML);

            }
        },
        _set: function (itemid) {
            that = this;
            $(that.element).find("li").removeClass(that.options.highlight);
            that.options.model["Selected" + that.options.items + "Value"](itemid);
            that.options.model["Selected" + that.options.items + "Object"](ko.utils.arrayFirst(that.options.model[that.options.items](), function (item) { return item[that.options.idField]().toUpperCase() === itemid.toUpperCase(); }));

            var s = $(that.element).find("[itemid='" + itemid + "']");
            if (s.length > 0) s.parent().addClass(that.options.highlight); //else alert("elemento non trovato");
        },
        destroy: function () {
        }


    }));
})(jQuery);
