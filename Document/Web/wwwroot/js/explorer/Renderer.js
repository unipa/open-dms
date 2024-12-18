function FileExplorer(container, searchRequest, folderId, Auths) {
    var Model = {};
    if (container[0] != "#" && container[0] != ".")
        container = "." + container;
    Model.SearchRequest = searchRequest;
    Model.Container = container;
    Model.FolderId = folderId;
    Model.Authorizations = Auths.split(',');
    $(Model.Container).html(explorerTemplate);




    var LoadPage = function (step) {
        if (Loading) return;
        Loading = true;
        var StartQueryTime = new Date();
        var $C = $(Model.Container);
        $C.find(".ProgressIndicator").removeClass("hidden");
        $C.find(".ProgressIndicator").css("width", "25%");
        $C.find(".ErrorMessage").hide();

        if (step == -1) {
            Model.SearchRequest.pageIndex = 0;
        }
        else
            if (step == -2) {
                if (Model.SearchRequest.pageIndex > 0)
                    Model.SearchRequest.pageIndex -= 1;
                else {
                    Loading = false; return;
                }
            }
            else
                if (step == -3) {
                    if (Model.SearchRequest.pageIndex < Model.PageCount - 1)
                        Model.SearchRequest.pageIndex += 1;
                    else {
                        Loading = false; return;
                    }
                }
                else
                    if (step == -4)
                        Model.SearchRequest.pageIndex = Model.PageCount - 1;
                    else
                        Model.SearchRequest.pageIndex = step;

        // Cerca la pagina in cache
        var data = Model.Data[Model.SearchRequest.pageIndex];
        if (data) {
            $C.find(".ProgressIndicator").css("width", "75%");
            RenderPage(data);
            $C.find(".ProgressIndicator").css("width", "100%");
        }
        else {
            if (Model.SearchRequest.PageIndex == 0) $G.empty();
            $C.find(".ProgressIndicator").css("width", "50%");
            fetch("/internalapi/ui/search/page",
                {
                    method: 'post',
                    headers: {
                        RequestVerificationToken: _getVerificationCode(),
                        'Content-Type': 'application/json;',
                        Accept: 'application/json;charset=ISO-8859-1'
                    },
                    body: JSON.stringify(Model.SearchRequest)
                })
            .then(r => r.json())
            .then((data) => {
                var StopQueryTime = new Date();
                Model.DeltaQueryTime = StopQueryTime - StartQueryTime;
                Model.Data[Model.SearchRequest.rageIndex] = data;
                $C.find(".ProgressIndicator").css("width", "75%");
                RenderPage(data);
                $C.find(".ProgressIndicator").css("width", "100%");
            })
            .catch((err) => {
                $C.find(".ErrorMessage").show().text(err);
            })
        }
        $C.find(".ProgressIndicator").addClass("hidden");
        Loading = false;

    }
    var Translate = function (a) {
        switch (a) {
            case 0: return "Nessuno";
            case 1: return "Nr.";
            case 2: return "Somma";
            case 4: return "Valore Minimo";
            case 8: return "Valore Massimo";
            case 16: return "Valore Medio";
            default: return "";
        }                       
    }

    function Refresh() {
        Model.SearchRequest.pageIndex = 0;
        var StartQueryTime = new Date();

        var $C = $(Model.Container);
        $C.find(".ProgressIndicator").css("width", "0%");
        $C.find(".ProgressIndicator").removeClass("hidden");
        $C.find(".ErrorMessage").hide();


        var $C = $C.find(".Container");
        $C.find(".ProgressIndicator").css("width", "25%");
        fetch("/internalapi/ui/search/get",
            {
                method: 'post',
                headers: {
                    RequestVerificationToken: _getVerificationCode(),
                    'Content-Type': 'application/json;',
                    Accept: 'application/json;charset=ISO-8859-1'
                },
                body: JSON.stringify(Model.SearchRequest)
            })
            .then(r => r.json())
            .then((data) => {
                var StopQueryTime = new Date();
                Model.DeltaQueryTime = StopQueryTime - StartQueryTime;

                Model.PageCount = data.pages+1;
                Model.Records = data.count;
                Model.Title = data.title;
                Model.Filters = data.filters;
                Model.HideSelection = data.hideSelection;
                Model.View = data.view;
                Model.ViewStyle = Model.View.viewStyle;
                Model.DoubleClick = Model.View.doubleClickAction;
                Model.Data = [];
                Model.Data[0] = data.page;
                Model.HasAggregates = false;
                Model.Columns = Model.View.columns.filter(c => c.settings.visible);
                Model.Aggregates = data.totals;
                $(Model.Columns).each((i, col) => {
                    var minWidth = col.settings.width.replace("100%", "392px");
                    var maxWidth = col.settings.width.replace("100%", "640px");
                    col.Width = "min-width:" + minWidth + ";max-width:" + maxWidth + ";width:" + minWidth+";";
                    col.View = Model.View.viewId;
                    col.AggregateClass = col.settings.aggregateType != 0 ? "aggregate" : "";
                    col.SortingTypeAscending = col.settings.sortType == 1;
                    col.SortingTypeDescending = col.settings.sortType == 2;
                    col.AlignmentClass = col.dataType == 2 || col.dataType == 4 ? "text-align:right;" : "";
                    col.HideSelection = Model.HideSelection;
                    if (col.settings.aggregateType != 0) {
                        Model.HasAggregates = true;
                        Model.Aggregates[i].id = col.id;
                        Model.Aggregates[i].tooltip = Translate(col.settings.aggregateType);
                    }
                });
                $C.find(".ProgressIndicator").css("width", "75%");



                $(".tbRefresh").off("click").on("click", () => { Refresh(); });
                $(".tbListView").off("click").on("click", () => { ChangeView(0) });
                $(".tbGridView").off("click").on("click", () => { ChangeView(1) });

                $(".btnExport").off("click").on("click", () => { Command("EXPORT", Selected) });
                $(".btnDownload").off("click").on("click", () => { Command("DOWNLOAD", Selected) });
                $(".btnPDF").off("click").on("click", () => { Command("PDF", Selected) });
                $(".btnShare").off("click").on("click", () => { Command("SHARE", Selected) });
                $(".btnMail").off("click").on("click", () => { Command("MAIL", Selected) });
                $(".btnSign").off("click").on("click", () => { Command("SIGN", Selected) });
                $(".btnAddToFolder").off("click").on("click", () => { Command("ADDTOFOLDER", Selected) });
                $(".btnUnFold").off("click").on("click", () => { Command("UNFOLD", Selected) });
                $(".btnDelete").off("click").on("click", () => { Command("DELETE", Selected) });
                $(".btnCopy").off("click").on("click", () => { Command("COPY", Selected) });
                $(".btnPermissions").off("click").on("click", () => { Command("PERMISSIONS", Selected) });

                $(Model.Authorizations).each((i, e) => {
                    $(".tb" + e).removeClass("hidden");
                });
                if (!Model.FolderId) {
                    $(".tbUNFOLD").hide();
                }



                ShowExplorer();
                $C.find(".ProgressIndicator").css("width", "100%");
            })
            .catch((err) => {
                $C.find(".ErrorMessage").show().text(err);
            })
    }

    var Loading = false;
    var LoadingTimeOut = undefined;
    var InfiniteScroll = function () {
        var parentPanel = $(Model.Container).find(".InfinityScroll");
        parentPanel.on("scroll", () => {
            if (parentPanel[0].scrollTop + parentPanel[0].clientHeight > parentPanel[0].scrollHeight * .80) {
                if (LoadingTimeOut) clearTimeout(LoadingTimeOut);
                LoadingTimeOut = window.setTimeout(() => {
                    LoadPage(-3);
                }, 250);
            }
        })
    }

    var ShowExplorer = function()
    {
        $(Model.ViewStyle == 0 ? ".tbListView" : ".tbGridView").addClass("checked");
        $(Model.ViewStyle != 0 ? ".tbListView" : ".tbGridView").removeClass("checked");

        if (Model.ViewStyle == 0)
            ShowList()
        else
            ShowGrid();
        InfiniteScroll();
    }

    var ShowList = function () {
        GotoFirstPage();
        var $C = $(Model.Container);
        if (Model.Data.length > 0) {
            var grid = Mustache.render(listTemplate, Model);
            $C.find(".explorer").html(grid);
            RenderPage(Model.Data[0]);
        }
        else {
            NoData();
        }
    }
    var ShowGrid = function () {
        GotoFirstPage();
        var $C = $(Model.Container);
        if (Model.Data.length > 0) {
            var grid = Mustache.render(gridTemplate, Model);
            $C.find(".explorer").html(grid);
            RenderPage(Model.Data[0]);
        }
        else {
            NoData();
        }
    }
    var NoData = function () {
        $C.find(".explorer").html(emptyTemplate);
    }
    var GotoFirstPage = function()
    {
        Model.SearchRequest.pageIndex = 0;
        Model.PageIndex = Model.SearchRequest.pageIndex;
    }
    var RenderPage = function (searchResultPage) {
        var StartRenderTime = new Date();
        Model.SearchRequest.pageIndex = searchResultPage.pageIndex;
        Model.PageIndex = searchResultPage.pageIndex;
        Model.FirstPage = Model.PageIndex == 0;
        Model.LastPage = Model.PageIndex < Model.PageCount;

        var i1 = Model.Columns.findIndex(c => c.id == "Document.Description");
        var i2 = Model.Columns.findIndex(c => c.id == "Document.DocumentType");
        var i3 = Model.Columns.findIndex(c => c.id == "Document.ProtocolNumber");
        var i4 = Model.Columns.findIndex(c => c.id == "Document.ContentType");

        $(searchResultPage.rows).each((i, row) => {
            row.key = row.keys[0];
            row.HideSelection = Model.HideSelection;
            row.IsFolder = row.keys[1] != "2";

            row.title = i1 >= 0 ? row.columns[i1].description : "";
            row.docType = i2 >= 0 ? row.columns[i2].description : "";
            row.protocol = i3 >= 0 ? row.columns[i3].description : "";
            row.icon = i4 >= 0 ? row.columns[i4].description : "";

            var isFirst = true;
            $(row.columns).each((i, col) => {
                col.Prefix = "";
                col.Suffix = "";
                var Head = Model.Columns[i];
                // icona, immagine, azione
                if (Head.dataType > 124 && Head.lookupValues && Head.tooltips) {
                    var index = parseInt(col.value);
                    var tindex = index;
                    if (isNaN(index) || index >= Head.lookupValues.length) index = Head.lookupValues.length - 1;
                    if (isNaN(tindex) || tindex >= Head.tooltips.length) tindex = Head.tooltips.length - 1;
                    col.description = Head.lookupValues[index];
                    col.tooltip = Head.tooltips[tindex];
                }
                // data
                if (Head.dataType == 4) {
                    col.Suffix = col.description.split(" ")[1];
                    col.description = col.description.split(" ")[0];
                }
                col.AggregateClass = Head.settings.aggregateType != 0 ? "aggregate" : "";
                col.IsFirstClass = isFirst ? "font-weight:700" : "";
                col.AlignmentClass = Head.dataType == 2 || Head.dataType == 4 ? "text-align:right;" : "";
                col.IsDateOrNumber = Head.dataType == 2 || Head.dataType == 4;

                col.IsAvatar = Head.dataType == 3
                col.IsAction = Head.dataType == 127
                col.IsText = !col.IsDateOrNumber && !col.IsAvatar && !col.IsAction && !col.IsAction;

                if (isFirst && Head.settings.width && Head.dataType == 0)
                    isFirst = false;
            })

        });
        var aggregates = [];
        // Render
        var $C = $(Model.Container);
        var rows = Mustache.render(Model.ViewStyle == 0 ? rowTemplate : previewTemplate, searchResultPage);

        var top = $C.find(".InfinityScroll")[0].scrollTop;
        $C.find(".explorer .rows").append(rows);
        $C.find(".explorer .selectAll").off("click").on("click", () => { return SelectAll() });
        $C.find(".explorer .selectRow").off("click").on("click", (e) => {
            e.preventDefault = true;
            e.stopPropagation();
            e.cancelBubble = true;
            return SelectRow(e.target.getAttribute("rowid")); });

        $C.find(".InfinityScroll")[0].scrollTop = top;

        // Verifica gli aggregati
        if (Model.HasAggregates && Model.ViewStyle == 0 && searchResultPage.pageIndex == 0) {
            var rows = Mustache.render(aggregateTemplate, Model);
            $C.find(".explorer tfoot").append(rows);
        }

        var StopRenderTime = new Date();
        var DeltaTime = StopRenderTime - StartRenderTime;
        $C.find(".renderTime").text(DeltaTime);
        $C.find(".queryTime").text(Model.DeltaQueryTime);
    }




    var ChangeView = function (viewType) {
        fetch("/internalapi/ui/setting/?name=ViewStyle." + Model.View.viewId + "&value=" + viewType)
            .then(r => r.text)
            .then(data => {
                Refresh();
            })
        return false;
    }


    var Export =  function () {
        fetch("/internalapi/ui/Search/export",
            {
                method: 'post',
                headers: {
                    RequestVerificationToken: _getVerificationCode(),
                    'Content-Type': 'application/json',
                    Accept: 'application/json',
                },
                body: JSON.stringify(Model.searchRequest)
            }
        )
            .then(r => r.json())
            .then(data => {
                DownloadCSV(data.replace(/\\r/g, "\r").replace(/\\n/g, "\n"));
            })
        return false;
    }

    var _getVerificationCode = function () {
        var VerificationCode = "";
        var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
        if (RequestVerificationCode.length > 0) {
            VerificationCode = RequestVerificationCode[0].value;
        }
        return VerificationCode;
    }



    var Selected = [];

    var SelectRow = function (rowid) {
        var add = $(Model.Container).find(".selectRow[rowid=" + rowid + "]").prop("checked") == true;
        var rid = parseInt(rowid)
        var i = Selected.indexOf(rid);
        if (i < 0 && add) {

            Selected.push(rid);
        }
        else {
            if (i >= 0 && !add)
                Selected.splice(i, 1);
        }
        $(Model.Container).find(".selectRow[rowid=" + rowid + "]").prop("checked", add);
        UpdateSelection();
        return true;
    }
    var SelectAll = function () {
        var add = $(Model.Container).find(".selectAll").prop("checked") == true;
        Selected = [];
        if (add) {
            $(Model.Container).find(".selectAll").prop("checked", true);
            $(Model.Container).find(".selectRow").each((i, e) => {
                Selected.push(parseInt($(e).attr("rowid")));
            });
            $(Model.Container).find(".selectRow").prop("checked", true);
        }
        else {
            $(Model.Container).find(".selectRow").prop("checked", false);
            $(Model.Container).find(".selectAll").prop("checked", false);
        }
        UpdateSelection();
        return true;
    }
    var UpdateSelection = function () {
        $(Model.Container).find(".SelectedDocs").addClass("hidden");
        $(Model.Container).find(".UnselectedDocs").addClass("hidden");
        $(Model.Container).find(".FilesToolbar").addClass("hidden");

        $(Model.Container).find(".Selected").html(Selected.length);
        if (Selected.length) {
            $(Model.Container).find(".SelectedDocs").removeClass("hidden");
            $(Model.Container).find(".FilesToolbar").removeClass("hidden");
        }
        else {
            $(Model.Container).find(".UnselectedDocs").removeClass("hidden");

        }
    }

    var Command = function (cmd, documents) {
        if (cmd == "DOWNLOAD" || cmd == "PDF") {
            var extension = (cmd == "PDF" ? ".pdf" : "")
            documents.forEach((e, i) => {
                fetch("/internalapi/action/Download?documentId=" + e + "&extension=" + extension)
                    .then(response => {
                        if (response.status === 200) {
                            var disposition = response.headers.get('Content-Disposition');
                            var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                            var matches = filenameRegex.exec(disposition);
                            if (matches != null && matches[1]) {
                                filename = matches[1].replace(/['"]/g, '');
                            }
                            return response.blob();
                        } else {
                            return;
                        }
                    })
                    .then(blob => {
                        var url = window.URL.createObjectURL(blob);
                        var a = document.createElement('a');
                        a.href = url;
                        a.download = filename;
                        document.body.appendChild(a); // we need to append the element to the dom -> otherwise it will not work in firefox
                        a.click();
                        a.remove();  //afterwards we remove the element again
                    });
            });
        }
        if (cmd == "EXPORT") {
            Export();
        }
        if (cmd == "SIGN") {
            OpenModal("/AddSign?documents=" + JSON.stringify( documents));
        }
        if (cmd == "PERMISSIONS") {
            OpenModal("/Permissions?id=" + JSON.stringify(documents));
        }
        if (cmd == "ADDTOFOLDER") {
            OpenModal("/AddFolder?id=" + JSON.stringify(documents));
        }
        if (cmd == "SHARE") {
            OpenModal("/Share?id=" + JSON.stringify(documents));
        }
        if (cmd == "MAIL") {
            OpenModal("/Mail/SendMail?id=" + JSON.stringify(documents));
        }

        if (cmd == "DELETE") {
            if (documents.length > 1) {
                if (!confirm("Stai per cancellare " + documents.length + " documenti. Sei sicuro ?")) return false;
            }
            var justification = "Cancellazione rapida";
            fetch("/internalapi/action/Delete?documents=" + documents + "&justification=" + encodeURIComponent(justification) + "&recursive=0")
                .then(response => { return response.json(); })
                .then(data => {
                    if (data)
                        alert(`Non è stato possibile cancellare n.${data} documenti per mancanza di autorizzazioni`);
                    window.postMessage({ op: "delete" }, "*");
                });
        }
        if (cmd == "UNFOLD") {
            if (documents.length > 1) {
                if (!confirm("Stai per rimuovere " + documents.length + " documenti da questo fascicolo.\nSei sicuro ?")) return false;
            } else {
                if (!confirm("Sei sicuro di voler rimuovere il documento da questo fascicolo ?")) return false;
            }

            fetch("/internalapi/action/RemoveFolders?documents=" + documents + "&folderId=" + Model.FolderId)
                .then(response => { return response.json(); })
                .then(data => {
                    {
                        window.postMessage({ op: "removefolder", data: data }, "*");
                    }
                })
        }

        if (cmd == "COPY") {
            var clipboardstring = localStorage.getItem("clipboard");
            var clipboard = [];
            try {
                if (clipboardstring) clipboard = JSON.parse(clipboardstring);
            } catch
            {
                localStorage.removeItem("clipboard");
            }
            documents.forEach((e, i) => {

                var i = clipboard.indexOf(e);
                if (i < 0)
                    clipboard.push(e);
            });
            localStorage.setItem("clipboard", JSON.stringify(clipboard));

            var msg = "";
            Added = documents.length;
            if (Added > 1)
                msg = Added + " documenti aggiunti negli appunti.";
            else
                if (Added == 1)
                    msg = Added + " documento aggiunto negli appunti.";
                else
                    msg = "Nessun documento aggiunto negli appunti.";
            alert(msg);
        }


    }



    Refresh();

    // Imposto gli eventi della toolbar principale

}