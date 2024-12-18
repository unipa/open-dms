

/* FUNZIONI DI SETUP DELLE COLONNE */
function ResizeColumn(view, id, range) {
    var value = $(range).val();
    if (value < 392) value += "px"; else value = "100%";
    fetch("/internalapi/ui/search/resizecolumn?ViewId=" + escape(view) + "&ColumnId=" + escape(id) + "&Width=" + value)
        .then(r => r.text)
        .then(data => {
            var th = $("[col-id='" + id + "']");
            th[0].style.maxWidth = value;
            th[0].style.minWidth = $(range).val() + "px";
        })
    return false;

}
function AggregateColumn(view, id, aggregate) {
    fetch("/internalapi/ui/search/aggregatecolumn?ViewId=" + escape(view) + "&ColumnId=" + escape(id) + "&Aggregate=" + aggregate)
        .then(r => r.text)
        .then(data => {
            Refresh();
        })
    return false;

}
function HideColumn(view, id, visible) {
    if (!visible || visible == 0) visible = false;
    fetch("/internalapi/ui/search/changevisibility?ViewId=" + escape(view) + "&ColumnId=" + escape(id) + "&Show=" + visible)
        .then(r => r.text)
        .then(data => {
            if (!visible) {
                var th = $("[col-id='" + id + "']");
                th.hide();
            }
            else
            {
                var th = $("[col-id='" + id + "']");
                if (th.length > 0) {
                    th.show();

                } else {
                    // Necessario creare colonna dummy
                }

            }
        })
}
function RenameColumn(view, id, control) {
    var name = $(control).val();
    fetch("/internalapi/ui/search/renamecolumn?ViewId=" + escape(view) + "&ColumnId=" + escape(id) + "&Name=" + name)
        .then(r => r.text)
        .then(data => {
            var th = $("[col-id='" + id + "']");
            $(th[0]).find("a").find("span")[0].innerText = name;
        })
}

function MoveColumnLeft(view, fromId, toId) {
    fetch("/internalapi/ui/search/movecolumn?ViewId=" + escape(view) + "&fromColumnId=" + escape(fromId) + "&toColumnId=" + toId)
        .then(r => r.text)
        .then(data => {
            var fromColumn = $("[col-id='" + fromId + "']");
            var toColumn = $("[col-id='" + toId + "']");
            for (i = 0; i < fromColumn.length; i++) {
                $(fromColumn[i]).insertBefore(toColumn[i]);
            }

            var fromRow = $("[cid='" + fromId + "']");
            var toRow = $("[cid='" + toId + "']");
            $(fromRow).insertBefore(toRow);

            $(fromColumn[0]).parent().find("[col-id]").each((i, e) => {
                $(e).find(".prev").removeClass("hidden");
                $(e).find(".next").removeClass("hidden");
                if (i == 0) {
                    $(e).find(".prev").addClass("hidden");
                } else
                    if (i == $(fromColumn[0]).parent().find("[col-id]").length - 1) {
                        $(e).find(".next").addClass("hidden");
                    }
            });

            fromRow.parent().find(".columnId").each((i, e) =>
            {
                $(e).text(i);
                $(e).parent().attr("row", i);
                $(e).parent().find(".prev").removeClass("hidden");
                $(e).parent().find(".next").removeClass("hidden");
                if (i == 0) {
                    $(e).parent().find(".prev").addClass("hidden");
                } else
                    if (i == fromRow.parent().find(".columnId").length - 1)
                { 
                    $(e).parent().find(".next").addClass("hidden");
                }
            });

        })
}
function MoveColumnRight(view, fromId, toId) {
    fetch("/internalapi/ui/search/movecolumn?ViewId=" + escape(view) + "&fromColumnId=" + escape(fromId) + "&toColumnId=" + toId)
        .then(r => r.text)
        .then(data => {
            var fromColumn = $("[col-id='" + fromId + "']");
            var toColumn = $("[col-id='" + toId + "']");
            for (i = 0; i < fromColumn.length; i++) {
                $(fromColumn[i]).insertAfter(toColumn[i]);
            }
            var fromRow = $("[cid='" + fromId + "']");
            var toRow = $("[cid='" + toId + "']");
            $(fromRow).insertAfter(toRow);

            $(fromColumn[0]).parent().find("[col-id]").each((i, e) => {
                $(e).find(".prev").removeClass("hidden");
                $(e).find(".next").removeClass("hidden");
                if (i == 0) {
                    $(e).find(".prev").addClass("hidden");
                } else
                    if (i == $(fromColumn[0]).parent().find("[col-id]").length - 1) {
                        $(e).find(".next").addClass("hidden");
                    }
            });

            fromRow.parent().find(".columnId").each((i, e) => {
                $(e).text(i);
                $(e).parent().attr("row", i);
                $(e).parent().find(".prev").removeClass("hidden");
                $(e).parent().find(".next").removeClass("hidden");
                if (i == 0) {
                    $(e).parent().find(".prev").addClass("hidden");
                } else
                    if (i == fromRow.parent().find(".columnId").length - 1) {
                        $(e).parent().find(".next").addClass("hidden");
                    }
            });

        })
}

function SortColumn(view, id) {
    fetch("/internalapi/ui/search/sortcolumn?ViewId=" + escape(view) + "&ColumnId=" + escape(id))
        .then(r => r.text)
        .then(data => {
            Refresh();
        })
    return false;
}
function SetDefault(view) {
    if (!confirm("Impostare questa configurazione di colonne per tutti gli utenti che non hanno una configurazione personalizzata ?")) return false;
    fetch("/internalapi/ui/search/setdefaultcolumn?ViewId=" + escape(view) )
        .then(r => r.text)
        .then(data => {
            Refresh();
        })
    HideCustomize();
    return false;
}

function ResetColumns(view) {
    if (!confirm("Reimpostare le colonne alla configurazione predefinita ?")) return false;
    fetch("/internalapi/ui/search/resetcolumns?ViewId=" + escape(view))
        .then(r => r.text)
        .then(data => {
            Refresh();
        })
    HideCustomize();
    return false;
}

function ShowCustomize() {
    $(".Window").show();
}
function HideCustomize() {
    //$(".Window").hide();
    Refresh();
}


function changeView(view, e) {
    fetch("/internalapi/ui/setting/?name=ViewStyle." + view + "&value=" + e)
        .then(r => r.text)
        .then(data => {
            Refresh();
        })
    return false;
}


/* FUNZIONI DI EXPORT */

function Export(search) {
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
    }
    fetch("/internalapi/ui/Search/export",
        {
            method: 'post',
            headers: {
                RequestVerificationToken: VerificationCode,
                'Content-Type': 'application/json',
                Accept: 'application/json',
            },
            body: JSON.stringify(search)
        }
    )
        .then(r => r.json())
        .then(data => {
            DownloadCSV(data.replace(/\\r/g, "\r").replace(/\\n/g, "\n"));
        })
    return false;
}

function DownloadCSV(data) {
    var filename = "export.csv";
    var file = new Blob([data], { type: "text/plain", endings: "transparent" });
    if (window.navigator.msSaveOrOpenBlob)
        window.navigator.msSaveOrOpenBlob(file, filename);
    else { // Others
        var a = document.createElement("a"),
            url = URL.createObjectURL(file);
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        setTimeout(function () {
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        }, 0);
    }
}



function Refresh() {
    window.location.reload(true);
}



function Goto(pageIndex) {
    var queryParams = new URLSearchParams(window.location.search);

    // Set new or modify existing parameter value.
    queryParams.set("pageIndex", pageIndex);
    window.location.href = window.location.pathname + "?" + queryParams.toString()

    //var page = pages[pageIndex];
    //if (!page) {
    //    LoadPage(pageIndex);
    //} else {
    //    Draw(pageIndex)
    //}
}


function Draw(pageIndex) {

}


/* ROW SELECTION */

var Selected = [];

function SelectRow(rowid) {
    var add = $("tr[rowid=" + rowid + "] td .checkbox").prop("checked") == true;
    var i = Selected.indexOf(rowid);
    if (i < 0 && add)
        Selected.push(parseInt(rowid));
    else
        if (i >= 0 && !add)
            Selected.splice(i, 1);
    $("tr[rowid=" + rowid + "] td .checkbox").prop("checked", add);

    event.stopPropagation();
    event.cancelBubble = true;
    UpdateSelection();
    return true;
}
function SelectAll() {
    var add = $("th .checkbox").prop("checked") == true;
    Selected = [];
    if (add) {
        $("td .checkbox").prop("checked", true);
        $("tr[rowid]").each((i, e) => {
            Selected.push(parseInt($(e).attr("rowid")));
        });
        $("th .checkbox").prop("checked", true);
    }
    else {
        $("td .checkbox").prop("checked", false);
        $("th .checkbox").prop("checked", false);
    }
    UpdateSelection();
    event.cancelBubble = true;
    return true;
}
function UpdateSelection() {
    $(".SelectedDocs").addClass("hidden");
    $(".UnselectedDocs").addClass("hidden");
    $(".FilesToolbar").addClass("hidden");

    $(".Selected").html(Selected.length);
    if (Selected.length) {
        $(".SelectedDocs").removeClass("hidden");
        $(".FilesToolbar").removeClass("hidden");
    }
    else {
        $(".UnselectedDocs").removeClass("hidden");

    }
}