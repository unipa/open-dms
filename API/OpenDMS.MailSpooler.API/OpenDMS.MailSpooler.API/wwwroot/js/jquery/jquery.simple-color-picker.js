$.fn.simpleColorPicker = function (options) {
    var defaults = {
        colorsPerLine: 13,
        colors: ['#330000', '#331900', '#333300', '#193300', '#003300', '#003319', '#003333', '#001933', '#000033', '#190033', '#330033', '#330019', '#000000',
            '#660000', '#663300', '#666600', '#336600', '#006600', '#006633', '#006666', '#003366', '#000066', '#330066', '#660066', '#660033', '#202020',
            '#990000', '#994C00', '#999900', '#4C9900', '#009900', '#00994C', '#009999', '#004C99', '#000099', '#4C0099', '#990099', '#99004C', '#404040',
            '#CC0000', '#CC6600', '#CCCC00', '#66CC00', '#00CC00', '#00CC66', '#00CCCC', '#0066CC', '#0000CC', '#6600CC', '#CC00CC', '#CC0066', '#606060',
            '#FF0000', '#FF8000', '#FFFF00', '#80FF00', '#00FF00', '#00FF80', '#00FFFF', '#0080FF', '#0000FF', '#7F00FF', '#FF00FF', '#FF007F', '#808080',
            '#FF3333', '#FF9933', '#FFFF33', '#99FF33', '#33FF33', '#33FF99', '#33FFFF', '#3399FF', '#3333FF', '#9933FF', '#FF33FF', '#FF3399', '#A0A0A0',
            '#FF6666', '#FFB266', '#FFFF66', '#B2FF66', '#66FF66', '#66FFB2', '#66FFFF', '#66B2FF', '#6666FF', '#B266FF', '#FF66FF', '#FF66B2', '#C0C0C0',
            '#FF9999', '#FFCC99', '#FFFF99', '#CCFF99', '#99FF99', '#99FFCC', '#99FFFF', '#99CCFF', '#9999FF', '#CC99FF', '#FF99FF', '#FF99CC', '#E0E0E0',
            '#FFCCCC', '#FFE5CC', '#FFFFCC', '#E5FFCC', '#CCFFCC', '#CCFFE5', '#CCFFFF', '#CCE5FF', '#CCCCFF', '#E5CCFF', '#FFCCFF', '#FFCCE5', '#FFFFFF'],
        showEffect: '',
        hideEffect: '',
        onChangeColor: false,
        includeMargins: false,
    };

    var opts = $.extend(defaults, options);

    return this.each(function () {
        var txt = $(this);

        var colorsMarkup = '';

        var prefix = txt.attr('id').replace(/-/g, '') + '_';

        for (var i = 0; i < opts.colors.length; i++) {
            var item = opts.colors[i];

            var breakLine = '';
            if (i % opts.colorsPerLine == 0)
                breakLine = 'clear: both; ';

            if (i > 0 && breakLine && $.browser && $.browser.msie && $.browser.version <= 7) {
                breakLine = '';
                colorsMarkup += '<li style="float: none; clear: both; overflow: hidden; background-color: #fff; display: block; height: 1px; line-height: 1px; font-size: 1px; margin-bottom: -2px;"></li>';
            }

            colorsMarkup += '<li id="' + prefix + 'color-' + i + '" class="color-box" style="' + breakLine + 'background-color: ' + item + '" title="' + item + '"></li>';
        }

        var box = $('<div id="' + prefix + 'color-picker" class="color-picker" style="position: absolute; left: 0px; top: 0px;"><ul>' + colorsMarkup + '</ul><div style="clear: both;"></div></div>');
        $('body').append(box);

        box.hide();

        box.find('li.color-box').click(function () {
            if (txt.is('input')) {
                txt.val(opts.colors[this.id.substr(this.id.indexOf('-') + 1)]);
                txt.blur();
            }
            if ($.isFunction(defaults.onChangeColor)) {
                defaults.onChangeColor.call(txt, opts.colors[this.id.substr(this.id.indexOf('-') + 1)]);
            }
            hideBox(box);
        });

        $('body').on('click', function () {
            hideBox(box);
        });

        box.click(function (event) {
            event.stopPropagation();
        });

        var positionAndShowBox = function (box) {
            var pos = txt.offset();
            var left = pos.left + txt.outerWidth(opts.includeMargins) - box.outerWidth(opts.includeMargins);
            if (left < pos.left) left = pos.left;
            var top = pos.top + txt.outerHeight(opts.includeMargins);// - box.outerHeight(opts.includeMargins);
            //if (top < pos.top) top = pos.top;
            box.css({ left: left, top: top  });
            showBox(box);
        }

        txt.click(function (event) {
            event.stopPropagation();
            if (!txt.is('input')) {
                // element is not an input so probably a link or div which requires the color box to be shown
                positionAndShowBox(box);
            }
        });

        txt.focus(function (event) {
            event.stopPropagation();
            positionAndShowBox(box);
        });

        function hideBox(box) {
            if (opts.hideEffect == 'fade')
                box.fadeOut();
            else if (opts.hideEffect == 'slide')
                box.slideUp();
            else
                box.hide();
        }

        function showBox(box) {
            if (opts.showEffect == 'fade')
                box.fadeIn();
            else if (opts.showEffect == 'slide')
                box.slideDown();
            else
                box.show();
        }
    });
};