var zoomfactor = 1;
var zoombutton = 0;
var ImageUrl;
var Dragging = false;
var ImageObj = 'ImageObj';
var MouseXID = "LoadImage_MouseX";
var MouseYID = "LoadImage_MouseY";
var SignTypeID = "LoadImage_SignType";
var PageObj = '';
var Signing = '';
var SignType = 's';
var PdfSignType = 'p';
var VistoType = 'v';
var TimbroType = 't';
var Timbri = '';
var SignWidth = 0;
var ImageWidth = 0;
var ImageHeight = 0;
var Pages = 0;
var LastSigningButton = null;
var dragtimeout = null;
var IE = document.all ? true : false;

var obj = null;
var x = null;
var dx = null;
var y = null;
var dy = null;

var xtempX = 0;
var xtempY = 0;


function Zoom(factor) {
    var panel = $(".PreviewPanel .ImageViewerContainer");
    if (factor == 2) // miniature
        zoomfactor = .13348388671875;
    else
        if (factor == 3) // altezza
            zoomfactor = 1;
        else
            if (factor == 4) // larghezza
            {
                var ppanel = $(".PreviewPanel");
                var img = ppanel.find(".ImagePage");
                var containerWidth = ppanel[0].clientWidth - 38;
                var imageWidth = img[0].original_Width + 38;// clientWidth;
                zoomfactor = containerWidth / imageWidth;
            }
            else
                zoomfactor = zoomfactor * (factor > 0 ? 3 / 4 : 4 / 3);
    panel.css("zoom", zoomfactor);
    $("#tbZoomValue").html(" " + Math.trunc(zoomfactor * 100) + "% ");

}



function StartSign(Control, SignatureImage) {
    HideDrag();
    obj = document.getElementById(ImageObj);
    // se è già attivo un segnaposto lo elimino
    if (Signing != '') {
        var sobj = document.getElementById("SignMark");
        if (sobj)
            sobj.style.display = 'none';
        Signing = '';

        if (LastSigningButton == Control)
            SignatureImage = '';
        if (LastSigningButton) {
            $(LastSigningButton).removeClass("active");
        }
    }
    LastSigningButton = null;
    if ((Signing == '') && (SignatureImage != '')) {
        LastSigningButton = Control;
        $(".StopSign").removeClass("hidden");
        $(Control).addClass("active");
        Signing = SignatureImage;
    }
}
function HideDrag(im, tempX, tempY, e) {
    {
        $(obj).off("click");
        $(obj).off("mousemove");

        var obj = document.getElementById("SignMark");
        if (obj) {
            obj.parentNode.removeChild(obj);
        }
    }
    return true;
}

function DragSign(im, tempX, tempY, e) {
    var ok = false;
    var i = im;
    var p = im.parentNode;
    obj = document.getElementById('SignMark');
    if (Signing != '') {
        if (!obj) {
            SignWidth = -1;
            obj = document.createElement('img');
            obj.id = 'SignMark';
            obj.style.position = 'absolute';
            obj.style.width = '';
            $(obj).load(function () {
                SignWidth = $(obj).width();
                obj.style.position = 'absolute';
                DragSign(im, tempX, tempY, e);
            });
            p.appendChild(obj);
            if (Signing == 'SignMark')
                obj.src = `/internalapi/UserManager/Images/Sign`;
                else
            if (Signing == 'VistoMark')
                obj.src = `/internalapi/UserManager/Images/Visto`;
            else
            if (Signing == 'DigitalMark')
                obj.src = `/internalapi/UserManager/Images/-DigitalSign`;
            else
            //if (Signing == 'TimbroMark')
            //    obj.src = url + TimbroType;
            //    obj.src = url + PdfSignType;
            //if (Signing == 'EvidenceMark')
            //    obj.src = url + EvidenceSignType;
            //if (Signing == 'ObscureMark')
            //    obj.src = url + ObscureSignType;
            //if (Signing == 'NoteMark')
            //    obj.src = url + NoteSignType + "&txt=" + $("#NoteTxt").val().replace(/\n/g, '<br />') + "&ftn=" + $("#fontname").val() + "&fts=" + $("#fontsize").val();
            //if (Signing == 'EvidenceRedMark')
            //    obj.src = url + EvidenceRedSignType;
            //if (Signing == 'EraserMark')
            //    obj.src = url + EraserSignType;
            if (Signing == 'SignField')
                obj.src = `/internalapi/UserManager/Images/-FieldSign`;
            else
                obj.src = Signing;

        } else {
            obj.style.display = '';
            if (tempX >= 3) {
                x = tempX / zoomfactor - 3;
                y = tempY / zoomfactor - 3;
                x = x + im.offsetLeft;
                y = y + im.offsetTop;
                if (x < 0) x = 0;
                var y = y - $(obj).height();
                if (y < 0) y = 0;
                // aggiusto per lo zoom
                //                x = x / zoomfactor;
                //                y = y / zoomfactor;
                if ((Signing != 'EvidenceMark' && Signing != 'EvidenceRedMark' && Signing != 'EraserMark' && Signing != 'ObscureMark')) {
                    obj.style.left = (x + 2) + 'px';
                    obj.style.top = (y + 2) + 'px';
                    dx = Math.round(SignWidth * $(i).width() / im.width * 1); // * 0.7);
                    if (dx > 0)
                        obj.style.width = dx + 'px';
                } else {
                    if (Dragging) {
                        dx = Math.round(((x - parseInt(obj.style.left)))); // * 0.7);
                        if (dx > 0)
                            obj.style.width = dx + 'px';
                        dy = Math.round(((y - parseInt(obj.style.top) + parseInt($(obj).height())))); // * 0.7);
                        if (dy > 0)
                            obj.style.height = dy + 'px';
                    } else {
                        obj.style.left = (x) + 'px';
                        obj.style.top = (y + parseInt($(obj).height())) + 'px';
                        obj.style.width = '1px';
                        obj.style.height = '1px';
                        ok = (e.which == 1);
                    }
                }
            }

            obj.style.zindex = 999;
        }
        $(obj).off("mousemove").on("mousemove", function (event) {
            event = event || window.event;

            var target = event.target || event.srcElement;

            var rect = target.getBoundingClientRect();
            var imrect = im.getBoundingClientRect();

            var x = (event.offsetX || event.clientX - rect.left);
            var y = (event.offsetY || event.clientY - rect.top);
            x = rect.left + event.offsetX - imrect.left;
            y = rect.top + event.offsetY - imrect.top;
            x = Math.round(x);
            y = Math.round(y);
            DragSign(im, x , y, event);

        })
        $(obj).off("mousedown").on("mousedown", function (event) {
            event = event || window.event;
            var target = event.target || event.srcElement, rect = target.getBoundingClientRect(), x = event.clientX - rect.left, y = event.clientY - rect.top;
            DragSign(im, parseInt(target.offsetLeft) + parseInt(x) - im.offsetLeft, parseInt(target.offsetTop) + parseInt(y) - im.offsetTop, event);
        })
        $(obj).off("click").on("click", function (event) {

            var target = event.target || event.srcElement, rect = target.getBoundingClientRect(), x = event.clientX - rect.left, y = event.clientY - rect.top;
            ImageClick(im, parseInt(target.offsetLeft) + parseInt(x) - im.offsetLeft, parseInt(target.offsetTop) + parseInt(y) - im.offsetTop, event);
        })
    } else {
        if (obj)
            obj.style.display = 'none';
    }
    return ok;
}
function AfterDrag(im, tempX, tempY, event) {
    var i = im;
    var p = im.parentNode;
    if (Signing) {
        var obj = document.getElementById('SignMark');
        var $im = $(im);
        $("#ok1").show();
        $("#ok2").show();
        var o = $(obj);
        var i = parseInt(im.FullWidth) / parseInt($im.width());
        obj.Fullleft = parseInt(obj.style.left) * i;
        obj.Fulltop = parseInt(obj.style.top) * i;
        obj.FullWidth = parseInt(o.width()) * i;
        obj.FullHeight = parseInt(o.height()) * i;

        x = parseInt(tempX - 3);
        y = parseInt(tempY - 3);
        if (Signing != "VistoMark" && Signing != "NoteMark") {
            x = x - parseInt(o.width());
        }
        y = y - parseInt(o.height());
        if (y < 0) y = 0;
        if (x < 0) x = 0;

        x = x * 100 / parseInt($im.width());
        y = y * 100 / parseInt($im.height());
        if (y > 100) y = 100;
        if (x > 100) x = 100;

        dx = parseInt(o.width()) * 100 / parseInt($im.width());
        dy = parseInt(o.height()) * 100 / parseInt($im.height());

        var pageIndex = parseInt(document.getElementById("current-page-count").textContent);

        $(obj).addClass("Page_parts").attr("id", "sign_" + pageIndex + "_" + Signing + "_" + x + "_" + y + "_" + dx + "_" + dy + "_" + window.btoa(unescape(encodeURIComponent($("#NoteTxt").val()))) + "_" + $("#fontname").val() + "_" + $("#fontsize").val());
        var p = obj.parentNode;
        o.unbind("click").unbind("mousedown").bind("click", function () {
            p.removeChild(this);
        });
        obj = null;
        //if (Signing == "NoteMark")
        //    StartNote($("#ContentPlaceHolder1_Timbro2")[0])
    }
}
function AbortSign() {
    debugger;
    $("#ok1").hide();
    $("#ok2").hide();
    $(".StopSign").addClass("hidden");
    StartSign(LastSigningButton, Signing);
    //cancello gli elementi rimasti a video
    $(".Page_parts").remove()
}
function StopSign() {
    debugger;
    var coordinates = ""; // recupero gli oggetti componendo una stringa con le coordinate dei timbri dagli id
    $(".Page_parts").each(function (i, e) {
        var id = e.id;
        coordinates = coordinates + id + ",";
    })
    $("#ok1").hide();
    $("#ok2").hide();
    $(".Page_parts").remove();

    $(".PreviewPanel").hide().parent().append(`<div id="loading" class="d-flex align-items-center justify-content-center h-100">
                                                    <div class="progress-spinner progress-spinner-active">
                                                        <span class="visually-hidden">Caricamento...</span>
                                                    </div>
                                                </div>`);
    debugger;

    var stampId = "Visto";

    $.ajax({
        url: `internalapi/preview/AddStamps/${docId}/${stampId}/${coordinates}`,
        type: "GET",
    })
        .done(function (type) {
            debugger;
            window.location.reload(true);
        }).fail(function (err) {
            debugger;
            ShowMessage('alert', err.responseText, 'Message_Target', '/Folders');
        });

    //$.postJSON("/WebServices/Document.asmx/EditPdf", "{Bd: '" + queryString("BD") + "', Doc: '" + queryString("DOC") + "', Operations: '" + a + "'}", function (plist) {
    //    // Carica i canali
    //    //                    $("#ContentPanel").hide();
    //    window.location.reload(true);
    //});
}
function Reject(tipo) {

    $.prompt($("#reject-dialog").html(), {
        buttons: { Ok: true, Cancel: false },
        show: 'show',
        loaded: function (e) {
            $(".jqi").find("#Descrizione").focus();

        },
        submit: function (v, m, f) {
            if (v && f["Descrizione"].length > 0) {
                var params = {
                    "BD": queryString("bd"),
                    "TodoID": queryString("TODO"),
                    "Descrizione": f["Descrizione"]
                }
                var r = false;
                $.postSyncJSON("/WebServices/Todo.asmx/Reject", JSON.stringify(params), function (data) {
                    window.refresh();
                    r = true;
                });
                return r;
            } else {
                $(".jqi").find(".alert").removeClass("hidden");
                return false;
            }
        }
    });

}

function ImageClick(im, tempX, tempY, e) {
    if (Signing == 'SignMark' || Signing == 'DigitalMark' || Signing == 'TimbroMark' || Signing == 'SignField') {

        i = im; //document.getElementById(ImageObj);
        var $i = $(i);
        p = i.parentNode; // document.getElementById("PreviewPanel");
        var obj = document.getElementById("SignMark");

        x = tempX / zoomfactor - 3;
        y = tempY / zoomfactor - 3;
//        x = x + im.offsetLeft;
//        y = y + im.offsetTop;
        if (x < 0) x = 0;

        if (obj) y = y - $(obj).height(); 
        if (y < 0) y = 0;


        console.debug("IMG X=" + x + " - Y=" + y + "i=" + i.id + " i=" + i.offsetTop + " e=" + e.witch);
        sx = 100 * x / $i.width();
        sy = 100 * y / $i.height();

        console.debug("IMG SX=" + sx + " - SY=" + sy);
        if (sy < 0) sy = 0;
        if (sy > 100) sy = 100;
        if (sx < 0) sx = 0;
        if (sx > 100) sx = 100;
        var nomeCampofirma = "";
        //if (Signing == 'VistoMark') {
        //    if (!confirm('Applicare il visto in questo punto  ?')) return true;
        //} else
        var result = false;

        if (Signing == 'SignMark') {
            Confirm(`Applicare la firma autografa in questo punto  ?`, "Message_Target", function () { finishImageClick(); }, function () { result = true });
        } else
            if (Signing == 'SignField') {
            Confirm(`Applicare il campo firma in questo punto  ?`, "Message_Target", function () {
                if ($('#IsFromSignDialog').val()) {
                    // caso di firma con annessa scelta del campo firma
                    debugger
                    nomeCampofirma = "Firma_" + window.parent.$('body > div.MainWindow > main > div.MainMenu > ul.nav.MainPanel > li.user > a')[0].outerText.replace(" ", "_") + "_" + newGuid();
                }
                else {
                   // if (!confirm('Applicare il campo firma in questo punto  ?')) return true;
                    nomeCampofirma = prompt("Inserire un nome per il campo firma.", "");
                }
                finishImageClick();

            }, function () { result = true });
        } else
            Confirm(`Applicare il timbro in questo punto  ?`, "Message_Target", function () { finishImageClick(); }, function () { result = true });
        //if (Signing == 'DigitalMark') {
        //    if (!confirm('Applicare la firma digitale in questo punto  ?')) return true;
        //} else
        //Confirm(`Applicare il timbro in questo punto  ?`, "Message_Target", function () { finishImageClick(); }, function () { result = true });

        if (result) return true

        function finishImageClick() {
            var st = "X";
            var method = "AddStamp";
            if (Signing == 'VistoMark')
                st = "V";
            else if (Signing == 'SignField') {
                st = "Z&nomeCampofirma=" + escape(nomeCampofirma);
                method = "AddSignField";
            }
            else
                if (Signing == 'SignMark')
                    st = "Sign";
                else
                    if (Signing == 'NoteMark')
                        st = "N&txt=" + $("#NoteTxt").val().replace(/\n/g, '<br />');
                    else
                        st = "Label";
            //if (Signing == 'DigitalMark') {
            //    HideDrag();
            //    StartSign(LastSigningButton, "");
            //    Signing = '';
            //    StartDigitalSign(im.PageIndex, sx, sy);
            //    return true;
            //}
            //else
            //st = "Label";

            document.getElementById(SignTypeID).value = st;
            document.getElementById(MouseXID).value = x;
            document.getElementById(MouseYID).value = y;

            StartSign(LastSigningButton, "");

            $(".Page_parts").remove();

            $(".PreviewPanel").hide().parent().append(`<div id="loading" class="d-flex align-items-center justify-content-center h-100">
                                                    <div class="progress-spinner progress-spinner-active">
                                                        <span class="visually-hidden">Caricamento...</span>
                                                    </div>
                                                </div>`);

            var stampId = st;

            var pageIndex = parseInt(document.getElementById("current-page-count").textContent);

            $.ajax({
                url: `internalapi/preview/${method}?documentId=${docId}&stampId=${stampId}&pageIndex=${pageIndex}&xPercentage=${sx}&yPercentage=${sy}`,
                type: "GET",
            })
                .done(function (type) {
                    if ($('#IsFromSignDialog').val()) {
                        // caso di firma con annessa scelta del campo firma
                        debugger;
                        var searchParams = new URLSearchParams(window.location.search);
                        var docId = searchParams.get('id');

                        var iframe = $("body > div.Window iframe");
                        iframe.attr("src", "/AddRemoteSign?documents=[" + docId + "]&signField=" + nomeCampofirma);
                        $('.Window').css("display", "block");
                    } else {
                        debugger;
                        window.location.reload(true);
                    }
                }).fail(function (err) {
                    debugger;
                    ShowMessage('alert', err.responseText, 'Message_Target', '/Folders');
                });

            HideDrag();
            StartSign(LastSigningButton, "");
            Signing = '';

            return true;
        }
    } else {
        document.getElementById(MouseXID).value = "0";
        document.getElementById(MouseYID).value = "0";
        return AfterDrag(im, tempX, tempY, e);
        //return true;
    }
}
function Rotate(senso) {
    Container = $(".PreviewPanel")[0];
    im = $("#" + Container.id + "_Page" + Container.opts.CurrentPage)[0];
    var h = im.style.height;
    im.src = Container.opts.ImageService + Container.opts.FileName + "&pid=" + Container.opts.CurrentPage + "&w=" + Container.opts.LargeImageWidth + "&q=r&d=" + senso + "&guid=" + new Date().getTime().toString();
    im.style.height = im.style.width;
    im.style.width = h;
    return true;
}

function Refresh() {
    window.location.reload(true);
    return true;
}

function Open(cmd) {
    var doc = queryString("DOC");

    var s = "/client.ashx?BD=" + queryString("bd") + "&CMD=" + cmd + "&DOCS=" + doc;
    var $f = $("<iframe style='position:absolute; left:0px;top:0px;frameborder=0;width:0px;height:0px'></iframe>");
    $f.appendTo('body');
    $f.ready(function () {
        window.setTimeout(function () {
            $f.remove();
        }, 1000);
    });
    $f[0].src = s;
    return false;
}


function StartDigitalSign(PageIndex, X, Y) {

    BancaDati = queryString("BD");
    var DOC = queryString('DOC');
    var a = '/client.ashx?CMD=PDFSIGN&BD=' + queryString('BD') + '&DOC=' + DOC + '&PID=' + PageIndex + '&X=' + X + '&Y=' + Y;
    var $f = $("<iframe style='position:absolute; left:0px;top:0px;frameborder=0;width:0px;height:0px'></iframe>");
    $f.appendTo('body');
    $f.ready(function () {
        window.setTimeout(function () {
            $f.remove();
        }, 1000);
    });
    $f[0].src = a;
    // Cmd (action, queryString('DOC'));
    return false;
}

function StartNote(el) {
    if ($(el).hasClass("active")) {
        $(el).removeClass("active");
        $("#NotePanel").removeClass("open");
        OnResize();
        StartSign(el, "");
    }
    else {
        $(el).addClass("active");
        $("#NoteTxt").val("");
        $("#NotePanel").addClass("open");
    }
    $("#NoteOk").unbind("click");
    //$(window).bind('resize',  function () {
    //    $("#NoteTxt").css("font-size", $("#NoteTxt").width() / 65);
    //});
    $("#fontname").on("change", function () {
        $('#NoteTxt').css("font-family", $("#fontname").val());
    })
    $("#fontsize").on("change", function () {
        $('#NoteTxt').css("font-size", $("#fontsize").val() + "pt");
        $("#NoteTxt").css("height", ($("#fontsize").val() * 3 + 8) + "pt");
        $("#NoteTxt").css("line-height", ($("#fontsize").val() * 1.5) + "pt");
    })

    $("#NoteOk").bind("click", function () {
        // chiamare startsign
        StartSign(el, "NoteMark");
        $("#NotePanel").removeClass("open");

    });
    $(window).resize();
}
function EndNote(el) {
    $(el).removeClass("active");
    $("#NotePanel").removeClass("open");
    StartSign(el, "");
    $(window).resize();
}

function StartRemoteSign(el) {
    if ($(el).hasClass("active")) {
        $(el).removeClass("active");
        $("#RemoteSignPanel").removeClass("open");
        RemoteSiteElement = undefined;
    }
    else {
        RemoteSiteElement = el;
        $(el).addClass("active");
        $("#RemoteSignPanel").addClass("open");
    }
    OnResize();
}
var RemoteSiteElement = undefined;

function HideRemoteSign() {
    $(RemoteSiteElement).removeClass("active");
    $("#RemoteSignPanel").removeClass("open");
    OnResize();
}

function FirmaPadesRemotaAggiungendoCampoFirma(self) {
    // funzione di firma che prima di avviare la firma pades remota fa aggiungere un campo di firma all'utente

    window.parent.$('body div.Window').css("display", "none");
    window.parent.$(".modalbackground").removeClass("modalbackground");

    var IsFromSignDialog = $('<input>', {
        type: 'hidden',
        id: 'IsFromSignDialog',
        value: 'true'
    });

    window.parent.$('body').append(IsFromSignDialog);

    window.setTimeout(function () {
        window.parent.$('#CreateSign').click();
    }, 500);

}

function newGuid() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c => (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16));
}


