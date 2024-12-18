(function ($) {
    // guid
    $.widget("elmi.dynamicTree", $.extend({}, $.mjs.nestedSortable.prototype, {

        options: {
            forcePlaceholderSize: true,
            placeholder: "active",
            activeclass: "active",
            handle: 'span',
            helper: 'clone',
            items: 'li',
            opacity: .6,
            revert: 250,
            tolerance: 'pointer',
            toleranceElement: 'div',
            protectRoot: true,
            prefix: "",
            formText: '',
            acceptDropFrom: '',
            renderText: function (node) {
                return "";
            },
            raiseNodeSelected: function (NodeId, itemId, item, ctx) {
                if (this["onNodeSelected"])
                    return this["onNodeSelected"](NodeId, itemId, item, ctx);
                else
                    return false;
            },
            raiseLeafCreated: function (NodeId, node, ctx) {
                if (this["onLeafCreated"])
                    return this["onLeafCreated"](NodeId, node, ctx);
                else
                    return false;
            },
            raiseNodeMoved: function (srcId, destId, ctx) {
                if (this["onNodeMoved"])
                    return this["onNodeMoved"](srcId, destId, ctx);
                else
                    return false;
            },
            raiseNodeChanged: function (NodeId, node, ctx) {
                if (this["onNodeChanged"])
                    return this["onNodeChanged"](NodeId, node, ctx);
                else
                    return false;
            },
            raiseLeafDeleted: function (NodeId, ctx) {
                if (this["onLeafDeleted"])
                    return this["onLeafDeleted"](NodeId, ctx);
                else
                    return false;
            },
            raiseItemDroppedOnNode: function (node, droppedItem, ctx) {
                if (this["onItemDroppedOnNode"])
                    return this["onItemDroppedOnNode"](node, droppedItem, ctx);
                else
                    return false;
            }
        },
        getSelectedItem: function () {
            return ctx.selectedItem;
        },
        _create: function () {

            this.element.data('nestedSortable', this.element.data('dynamicTree'));

            if (!this.element.is(this.options.listType))
                throw new Error('dynamicTree: Please check the listType option is set to your actual list type');

            this.selectedItem = null;
            this.selectedItemId = '';
            this.selectedNodeId = '';

            return $.mjs.nestedSortable.prototype._create.apply(this, arguments);
        },

        destroy: function () {
            this.element
				.removeData("dynamicTree")
				.unbind(".dynamicTree");
            return $.mjs.nestedSortable.prototype.destroy.apply(this, arguments);
        },

        _resetSelection: function () {
            $(this.element).find("li div").removeClass(this.options.activeclass); //.addClass("ui-state-default");
            //$(this.element).find("li div").removeClass(this.options.placeholder); //.addClass("ui-state-default");
            //$(this.element.find("li div a")).removeClass("ui-state-highlight").addClass("ui-state-default");
        },

        _mouseStop: function (event, noPropagation) {
            var ctx = this;
            srcId = $(event.target).parents("div").attr("NodeId");
            destId = $(ctx.placeholder).parents("li").children("div").attr("NodeId"); //.parents("li").children("div").children("span").get(0).id.replace(this.options.prefix, "");
            if (!srcId) return;
            if (!destId) return;

            $.mjs.nestedSortable.prototype._mouseStop.apply(this, arguments);
            //console.log("mousestop - Src: " + event.srcElement.id + " | Dest: " + $(this.placeholder).parents("li").children("div").children("span").get(0).id);
            ctx.options.raiseNodeMoved(srcId, destId);
        },

        _buildTree: function (root) {

            var htmlText = "";
            var ctx = this;

            htmlText += '<li><div NodeId="' + root.NodeId + '">';
            htmlText += '<a data-toggle="xcollapse" data-parent="#accordion" class="TreeNode " href="#oxl' + root.NodeId + '">';
            htmlText += '<span name="Descrizione">' + this.options.renderText(root) + '</span></a>';
            if (root.Children.length) {
                a = ("<a class='switch' href='#' onclick=\"$(this).parent().toggleClass('closed');return false;\"></a>");
                htmlText += a;
            }
            htmlText += '</div>';
            if (root.Children.length)
            {

                ol = '<ol id="ol' + root.NodeId + '">';
                $(root.Children).each(function () {
                    ol += ctx._buildTree(this);
                });
                ol += '</ol>';
                htmlText += ol;

            }
            htmlText += '</li>';

            return htmlText;
        },

        RemoveNode: function (item) {

            var emptyList = $(item).next(this.options.listType);
            if (!emptyList.length) {
                var NodeId = $(item).attr('NodeId');
                if (this.options.raiseLeafDeleted(NodeId)) {
                    var n = $(item).parent().parent().prev().attr("NodeId");
                    $(item).parent().remove();
                    this.performNodeSelection(n);
                    emptyList = $(ctx.selectedItem).next(this.options.listType);
                    if (!emptyList.children().length)
                        $(ctx.selectedItem).next(this.options.listType).remove();

                }
            } else {
                alert("Non è possibile rimuovere l'elemento");
            }

        },

        _initNode: function (item) {
            var ctx = this; //div
            item.bind('click', function (e) {
                ctx.performNodeSelection($(this).attr('NodeId'));
                //ctx.selectedItem = this;
                //ctx.selectedItemId = $(this).attr('NodeId');
                //ctx.selectedNodeId = $(this).attr('NodeId');
                //ctx._resetSelection();
                //$(this).addClass(ctx.options.placeholder);
                ////$(this).children("A").addClass("ui-state-highlight");
                e.stopPropagation();

                //ctx.options.raiseNodeSelected(ctx.selectedNodeId, ctx.selectedItemId, ctx.selectedItem, ctx);

            });

            item.droppable({

                accept: ctx.options.acceptDropFrom,
                hoverClass: 'hovered',
                drop: function (event, ui) {

                    NodeId = $(this).attr('NodeId');
                    if (!NodeId) return 
                    ui.draggable.draggable('option', 'revert', false);
                    ctx.options.raiseItemDroppedOnNode(NodeId, ui, ctx);
                }
            });
        },

        performNodeCreation: function (node, item, ctx) {
          //  ctx = this;
            if (!item) item = this.selectedItem;
            var destNode = $(item).next(this.options.listType); //.children(ctx.options.listType).first();
            if (!destNode.length)
                destNode = $('<ol id="ol' + node.NodeId + '">').insertAfter($(item));
            var newLeaf = $('<li><div NodeId="' + node.NodeId + '"><a data-toggle="xcollapse" data-parent="#accordion" class="TreeNode" href="#ol' + node.NodeId + '"><span name="Descrizione">' + this.options.renderText(node) + '</span></a></a></div></li>').appendTo(destNode);
            this._initNode(newLeaf.find("div"));
        },
        performNodeSelection: function (itemId) {
            ctx = this;
            ctx._resetSelection();
            var item = $(this.element).find("[NodeId='" + itemId + "']");
            $(item).addClass(ctx.options.activeclass);
            //$(item).addClass(ctx.options.placeholder);
            ctx.selectedItem = item;
            ctx.selectedItemId = itemId;
            ctx.selectedNodeId = itemId;

            //$(item).children("A").addClass("ui-state-highlight");

            ctx.options.raiseNodeSelected(ctx.selectedNodeId, ctx.selectedItemId, ctx.selectedItem, ctx);
        },

        performNodeUpdate: function (node, item, ctx) {

            if (!item) item = this.selectedItem;

            item.find("[name='Descrizione']").html(this.options.renderText(node));
            this._initNode($(this.selectedItem));
        },

        renderTree: function (data, prefix) {

            //var currentDepth = 0;
            var ctx = this;
            ctx.options.prefix = prefix;
            ctx.options.rootID = prefix + data.NodeId;

            var htmlText = "";

            $(data).each(function () {
                htmlText += ctx._buildTree(this);
            });

            this.element.append(htmlText);

            $(this.element.find("li div")).each(function () {
                ctx._initNode(
                    $(this).removeClass(ctx.options.activeclass)
                    //$(this).removeClass(ctx.options.placeholder)
                );
            });

            //$('.sortable li div span')

            return $.elmi.dynamicTree.prototype;
            //return ctx;
        }

    }));

    $.elmi.dynamicTree.prototype.options = $.extend({}, $.mjs.nestedSortable.prototype.options, $.elmi.dynamicTree.prototype.options);

})(jQuery);


