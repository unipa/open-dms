
export function LoadImage(img) {
    //funzione usata per inizializzare al caricamento del pdf (vedi nel componente ImageViewer\lib\Esteso.js)
    // associa le funzioni di necessarie per le pdfAction (tra cui la firma autografa)
    // NB. messa in un file a parte per poter eseguire l'export

    var p = document.querySelector(".ImageViewerContainer");
    p.style.position = "relative";

    SetImageEvent(img, {
        FileName: ImageUrl,
        ZoomOutCss: "tbZoomOut",
        ZoomInCss: "tbZoomIn",
        ZoomWidthCss: "tbZoom100W",
        ZoomHeightCss: "tbZoom100H",
        PageFirstCss: "tbGotoFirst",
        PagePrevCss: "tbGotoPrev",
        PageNextCss: "tbGotoNext",
        PageLastCss: "tbGotoLast",
        PageIndexCss: "tbPageIndex",
        PreviewCss: "tbZoomAll",
        Zoom100Css: "tbZoom100",
        OnLoadFirst: function () { OnResize(); },
        OnOver: DragSign,
        OnDrag: AfterDrag,
        //OnOut: HideDrag,
        NoImage: "/Images/NoImage.png",
        OnDblClick: function () {
//            Open("VIEW");
        },
        OnClick: ImageClick
    });

    function SetImageEvent(im, options) {
        if (options.OnClick)
            $(im).off('click').click(function (event) {
            var i = im;
            var X = (event.offsetX || event.clientX - $(event.target).offset().left);
            var Y = (event.offsetY || event.clientY - $(event.target).offset().top);
            X = Math.round(X);
                Y = Math.round(Y);
            if (options.Dragging)
                options.OnDrag(i, X, Y, event, options.Dragging);
            else
                options.OnClick(i, X, Y, event, options.Dragging)
            options.Dragging = false;
        });
        if (options.OnDblClick)
            $(im).off('dblclick').dblclick(function (event) {
            var i = im;
            event.bubbles = false;
            event.stopPropagation();
            var X = (event.offsetX || event.clientX - $(event.target).offset().left);
            var Y = (event.offsetY || event.clientY - $(event.target).offset().top);
            X = Math.round(X);
            Y = Math.round(Y);
            options.OnDblClick(i, X, Y, event, options.Dragging);
            return false;
        });
        if (options.OnOver)
            $(im).off('mousemove').mousemove(function (event) {
            event = event || window.event;
            var target = event.target || event.srcElement, rect = target.getBoundingClientRect(), X = event.clientX - rect.left, Y = event.clientY - rect.top;
            X = Math.round(X);
            Y = Math.round(Y);

            var i = im;
            options.OnOver(i, X, Y, event, options.Dragging)
        });
        if (options.OnOver)
            $(im).off('mousedown').mousedown(function (event) {
            event = event || window.event;
            event.preventDefault()
            var target = event.target || event.srcElement, rect = target.getBoundingClientRect(), X = event.clientX - rect.left, Y = event.clientY - rect.top;

            X = Math.round(X);
            Y = Math.round(Y);
            var i = im;
            options.Dragging = options.OnOver(i, X, Y, event, options.Dragging)
        });
        if (options.OnOut) $(im).off('mouseout').mouseout(function (event) {
            var i = im;
            var target = event.target || event.srcElement, rect = target.getBoundingClientRect(), X = event.clientX - rect.left, Y = event.clientY - rect.top;
            X = Math.round(X);
            Y = Math.round(Y);
            options.Dragging = false;
            options.OnOut(i, X, Y, event, options.Dragging)
        });

    }

    $(window).trigger("resize");

}


function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if (d > 0) {//Use timestamp until depleted
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}



