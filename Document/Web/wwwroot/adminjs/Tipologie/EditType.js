

$(function () {
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
                .addClass("custom-combobox")
                .insertAfter(this.element);

            this.element.hide();
            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
                value = selected.val() ? selected.text() : "";

            this.input = $("<input type='text'>")
                .appendTo(this.wrapper)
                .val(value)
                .attr("title", "")
                .addClass("")
                .autocomplete({
                    delay: 0,
                    minLength: 0,
                    source: $.proxy(this, "_source")
                })
                .tooltip({
                    classes: {
                        "ui-tooltip": "ui-state-highlight"
                    }
                });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this.element.val(ui.item.option.value).change();
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
                wasOpen = false;

            $("<a>")
                .attr("tabIndex", -1)
                .attr("title", "Show All Items")
                .tooltip()
                .appendTo(this.wrapper)
                .removeClass("ui-corner-all")
                .addClass("custom-combobox-toggle ui-corner-right")
                .html("<i class='fa fa-caret-down'></i>")
                .on("mousedown", function () {
                    wasOpen = input.autocomplete("widget").is(":visible");
                })
                .on("click", function () {
                    input.trigger("focus");

                    // Close if already visible
                    if (wasOpen) {
                        return;
                    }

                    // Pass empty string as value to search for, displaying all results
                    input.autocomplete("search", "");
                });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    this.element.val($(this).val());
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
                .val("")
                .attr("title", value + " non valido")
                .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }

    });
});

function key(ID, guid, index, datatype, defaultvalue, preserve, description, mandatory, fromfolder, editable, send, multiple, crypted, cancellato, larghezzaChiave, visibility) {
    this.ID = ID;
    this.New = !ID;
    this.Index = ko.observable(index);
    this.Default = defaultvalue;
    this.KType = ko.observable(datatype);
    this.GUID = guid;
    this.Cs = preserve;
    this.descrizione = description;
    this.LarghezzaChiave = larghezzaChiave;
    this.Required = mandatory;
    this.Multiple = multiple;
    this.Crypted = crypted;
    this.Editable = editable;
    this.Distribuzione = send;
    this.Visibilita = visibility;
    this.Eredita = fromfolder;
    this.Deleted = ko.observable(cancellato);
}

function Document() {
    this.AvvioScadenza = 0;
    this.DataRevisione = 0;
    this.descrizione = "";
    this.Documento = 0;
    this.GiorniScadenza = 0;
    this.Responsabile = "";
    this.Sequenza = 1;
    this.Tipologia = "";
    this.UtenteRevisore = "";
}


function viewModel(_Container) {

    //funzione per intercettare le call ajax ed eseguire il refresh del token quando necessario

    var self = this;
//    self.host = host;
    self.Container = _Container;
    //self.ContentType = ko.observable("0");
    self.Current = ko.observable();
    self.ErrorMessage = ko.observable();
    self.ErrorMessage.subscribe(function () {
        if (self.ErrorMessage() !== "")
            ShowMessage('alert', self.ErrorMessage(), 'Message_Target', '/Admin/Tipologie/Index');
        self.ErrorMessage("");
    });

    self.types = ko.observableArray();
    self.AllTypes = ko.observableArray();
    self.AllAcl = ko.observableArray();
    //self.ProtocolList = ko.observableArray();
    self.ClassList = ko.observableArray();
    self.GroupList = ko.observableArray();
    self.Keys = ko.observableArray();

    self.Tags = ko.observableArray();
    self.Forms = ko.observableArray();
//    self.FormTemplates = ko.observable();
    self.ACL = ko.observableArray();
    self.Users = ko.observableArray();
    self.Groups = ko.observableArray();
    self.HasDirezione = ko.observable();
    self.Pubblica = ko.observable();
    //self.IsBatchScan = ko.observable(false);
    self.IsInternal = ko.observable(false);
    self.ToBeIndexed = ko.observable(false);
    self.IsReserved = ko.observable(false);
    self.ConvertToPDF= ko.observable(false);
    self.NewTitle = ko.observable();
    self.Allegati = ko.observableArray();
    self.Tabelle = ko.observableArray();
    self.Documenti = ko.observable(0);
    self.Anagrafiche = ko.observableArray();
    self.Workflow = ko.observableArray();
    self.ProcessList = ko.observableArray();

    self.Etichetta = ko.observableArray([
        { Codice: 0, Descrizione: "-- Nessuna --" },
        { Codice: 1, Descrizione: "In alto/sinistra" },
        { Codice: 2, Descrizione: "In alto/destra" },
        { Codice: 3, Descrizione: "In basso/sinistra" },
        { Codice: 4, Descrizione: "In basso/destra" },
        { Codice: 5, Descrizione: "Personalizzato..." }
    ]);
    self.DataType = ko.observableArray([
    ]);
    self.NuovoType = ko.observable();
    self.NuovoMeta = ko.observable();


    self.LoadKeys = function (data) { };

    self.OpenTemplate = function (data) {
        window.open("/?uid=" + data.creationFormKey());
    }
    // SOSTITUIRE CON API LOOKUPTABLE  GET
    self.Refresh = function (select) {
        //$.postJSON("/WebServices/Lookup.asmx/GetTypes", "{ Bd: '" + self.BancaDati + "', ct:'" + self.ContentType() + "'}", function (typelist) {
        $.ajax({
            url: "/internalapi/DocumentType/GetTypes/Any",
            type: "GET",
        })
        .done(function (typelist) {
            typelist.reverse();
            t = "Nuova Tipologia";

            var newTypelist = [];
            for (let i = 0; i < typelist.length; i++) {
                icon = typelist[i].annotation;
                newTypelist.push({
                    "codice": typelist[i].id,
                    "descrizione": typelist[i].description,
                    "hint":  icon,
                    "tabella": typelist[i].tableId,
                })
            }

            newTypelist.push({ codice: "", descrizione: t, hint: "fa fa-plus", tabella: "" });
            newTypelist.reverse();

            self.NewTitle(t);
            try {
                self.types(ko.mapping.fromJS(newTypelist)());
            } catch (e) {
            }

            if (select) self.Selecttypes({ codice: ko.observable("") });
            else
                self.Selecttypes({ codice: self.Current().codice });

        }).fail(function (err) {
            ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target',  "/Tipologie/Index");
        });
    }

    $.ajax({
        url: "/Admin/Tipologie/Forms",
        type: "GET",
    })
    .done(function (type) {
        type.push({ id: "", name: "Default", key: "" });
        self.Forms(type);
    }).fail(function (err) {
        ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target', "/Tipologie/Index");
    });

    // richiesta delle tipologie di metadato disponibile
    $.ajax({
        url: "/internalapi/CustomField",
        type: "GET",
    })
    .done(function (type) {
        type.push({ id: "$$t", name: "Testo" });
        type.push({ id: "$$p", name: "Paragrafo" });
        type.push({ id: "$$d", name: "Data" });
        type.push({ id: "$$i", name: "Numero Intero" });
        type.push({ id: "$$f", name: "Numero con Virgola" });
        type.push({ id: "$us", name: "Utente" });
        type.push({ id: "$ro", name: "Ruolo" });
        type.push({ id: "$gr", name: "Struttura" });
        type.push({ id: "$ug", name: "Utente/Ruolo/Struttura" });
        type.push({ id: "$$@", name: "Email" });
        type.push({ id: "$$j", name: "Json" });
        self.Tags(type);
    }).fail(function (err) {
        ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target',  "/Tipologie/Index");
    });


    //$.ajax({
    //    url: "/internalapi/LookupTable/$PROTOCOL$",
    //    type: "GET",
    //})
    //    .done(function (p1) {
    //        p1.push({ codice: "", descrizione: "- Nessuno -" });
    //        self.ProtocolList(p1);
    //    }).fail(function (err) {
    //         ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target', "/Tipologie/Index");
    //    });

    $.ajax({
        url: "/internalapi/LookupTable/$CATEGORIES$",
        type: "GET",
    })
    .done(function (c1) {
        self.ClassList(c1);
    }).fail(function (err) {
            ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target',  "/Tipologie/Index");
    });


    $.ajax({
        url: "/internalapi/DocumentType/GetTypes/Any",
        type: "GET",
    })
    .done(function (u1) {
        self.AllTypes.removeAll();
        self.AllTypes(u1);
    }).fail(function (err) {
            ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target', "/Tipologie/Index");
    });


    $.ajax({
        url: "/internalapi/acl",
        type: "GET",
    })
    .done(function (u1) {
        self.AllAcl.removeAll();
        self.AllAcl(u1);
    }).fail(function (err) {
            ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target',  "/Tipologie/Index");
    });

    $.ajax({
        url: "/internalapi/LookupTable/$TABLES$",
        type: "GET",
    })
    .done(function (u1) {
        u1.reverse();
        u1.push({ codice: "", descrizione: "- Nessuna -" });
        u1.reverse();

        self.Tabelle(u1);
    }).fail(function (err) {
            ShowMessage("alert", "Non è stato possibile reperire le informazioni. Errore: " + err.responseText, 'Message_Target',  "/Tipologie/Index");
    });


    $(".List").List({
        model: self,
        items: "types",
        idField: "codice",
        textField: "descrizione",
        iconField: "hint",
        //iconClass: "it-files",
        //imageField: "hint",
        commentField: "tabella",
        rightTextField: "",
        className: "",
        onSelect: "Select",
        NewItem: "Nuova tipologia",
    });

    self.Refresh(true);

    self.AddKey = function (obj) {
        i = 0;
        ko.utils.arrayForEach(self.Keys(), function (e) { if (e.Index() > i) i = e.Index(); });

        self.Keys.push(new key("", "Metadato_"+(i+1), i + 1, self.NuovoMeta(), '', true, "", false, false, true, false, false, false, false, '',0));
        self.NuovoMeta("");
    }

    self.RemoveKey = function (obj) {
        if (!obj.ID) {
            $(self.Keys()).each(function (index, e) {
                if (e.Index() > obj.Index() && !e.ID)
                    e.Index(e.Index() - 1);
            })
            self.Keys.remove(obj);
        }
        else
            obj.Deleted(true);
    }

    self.RestoreKey = function (obj) {
        obj.Deleted(false);
    }

    self.MoveUpKey = function (index) {
        var i = index();
        self.Keys.splice(i - 1, 2, self.Keys()[i], self.Keys()[i - 1]);
    }

    self.MoveDownKey = function (index) {
        i = index();
        self.Keys.splice(i, 2, self.Keys()[i + 1], self.Keys()[i]);
    }

    self.SetIcon = function (obj, icon) {
        obj.icon(icon.className);
    }


    self.Select = function (data, callback) {
        var c = data;
        if (c === "") { c = "nuova tipologia"; }
        var p = $.find('.pill-pane.active');
        if (p.length > 0) p = p[0].id; else p = "t0";

        $.ajax({
            url: "/internalapi/DocumentType/" + c,
            type: "GET",
        })
            .done(function (type) {
                self.ErrorMessage("");
                self.XHR = undefined;
                if (type.id === '') type.name = "";
                type.codice = type.id

                try {
                    self.Current(ko.mapping.fromJS(type));
                } catch (e) {
                    alert(e);
                }
                self.Keys([]);
                var l = type.fields ? type.fields.length : 0;
                for (i = 0; i < l; i++) {
                    self.Keys.push(new key(type.fields[i].id, type.fields[i].fieldName, type.fields[i].fieldIndex, type.fields[i].fieldTypeId, type.fields[i].DefaultValue, type.fields[i].preservable, type.fields[i].title, type.fields[i].mandatory, type.fields[i].inherited, type.fields[i].editable, type.fields[i].filterInMailing, type.fields[i].tag, type.fields[i].enrypted, type.fields[i].deleted, type.fields[i].width, type.fields[i].visibility));
                }
                //self.IsBatchScan(type.batchScan > 0);
                self.IsInternal(type.internal);
                self.ToBeIndexed(type.toBeIndexed);
                self.IsReserved(type.reserved);
                self.ConvertToPDF(type.convertToPDF);

                //$.postJSON("/WebServices/Types.asmx/Documents", "{ Bd: '" + self.BancaDati + "', cdc: '" + c + "'}", function (a1) {
                //    self.Documenti(a1);
                //});
                self.Documenti(0); // API NON disponibile attualmente
                if (callback) callback.call(this);

                var a = $("a[href='#" + p + "']");
                if (!a.parent().is(":visible"))
                    p = "t0";
                $("a[href='#" + p + "']").tab('show');
                window.setTimeout(function () { $(window).resize(); }, 500);

                AttivazionePopover();

            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
        return true;
    }

    self.Remove = function (data) {
        Confirm("Sei sicuro di voler eliminare questa tipologia (" + data.codice() + ") ?", "Message_Target", function () {

            $.ajax({
                url: "/internalapi/DocumentType/" + data.codice(),
                type: "DELETE",
            })
                .done(function (type) {
                    self.types.remove(function (item) { return item.codice().toUpperCase() === data.codice() });
                    self.Selecttypes({ codice: ko.observable("") });
                    ShowMessage("success", "Tipologia eliminata con successo", "Message_Target", null);
                })
                .fail(function (err) {
                    self.ErrorMessage("Errore durante l'eliminazione. Errore: " + err.status);
                });

        });
    }

    self.Reset = function (data) {
        self.Selecttypes(data);
    }

    function uuid4() {
        function hex(s, b) {
            return s +
                (b >>> 4).toString(16) +  // high nibble
                (b & 0b1111).toString(16);   // low nibble
        }

        let r = crypto.getRandomValues(new Uint8Array(16));

        r[6] = r[6] >>> 4 | 0b01000000; // Set type 4: 0100
        r[8] = r[8] >>> 3 | 0b10000000; // Set variant: 100

        return r.slice(0, 4).reduce(hex, '') +
            r.slice(4, 6).reduce(hex, '-') +
            r.slice(6, 8).reduce(hex, '-') +
            r.slice(8, 10).reduce(hex, '-') +
            r.slice(10, 16).reduce(hex, '-');
    }

    self.Create = function (data) {
        var tipo = ko.mapping.toJS(data);
        if (tipo.codice === "") {
            tipo.codice = uuid4();
            //            self.ErrorMessage("Specificare il campo 'codice'");
            //            return;
        }

        if (!tipo.name || tipo.name === "") {
            self.ErrorMessage("Non hai indicato un nome per la nuova tipologia");
            return;
        }

        //$.postJSON("/WebServices/Types.asmx/GetType", "{ Bd: '" + self.BancaDati + "', cdc: '" + tipo.codice + "'}", function (type1) {
        $.ajax({
            url: "/internalapi/DocumentType/" + tipo.codice,
            type: "GET",
        })
            .done(function (type1) {
                if (type1.id === '') {
                    tipo.id = tipo.codice;
                    //delete tipo.acl;
                    tipo.aclId = null;
                    tipo.contentType = parseInt(tipo.contentType);
                    tipo.initialStatus = parseInt(tipo.initialStatus);
                    tipo.expirationStrategy = parseInt(tipo.expirationStrategy);
                    tipo.internal = self.IsInternal();
                    tipo.toBeIndexed = self.ToBeIndexed();
                    tipo.reserved = self.IsReserved();
                    tipo.convertToPDF = self.ConvertToPDF();

                    //$.postJSON("/WebServices/Types.asmx/SetType", JSON.stringify(params), function (type) {
                    $.ajax({
                        url: "/internalapi/DocumentType",
                        type: "POST",
                        headers: { "Content-Type": "application/json" },
                        data: JSON.stringify(tipo)
                    })
                        .done(function (type) {
                            //$.postJSON("/WebServices/Lookup.asmx/GetTypes", "{ Bd: '" + self.BancaDati + "', ct:'" + self.ContentType() + "'}", function (u1) {
                            $.ajax({
                                url: "/internalapi/DocumentType/GetTypes/Any",
                                type: "GET",
                            })
                                .done(function (u1) {
                                    self.AllTypes.removeAll();
                                    self.AllTypes(u1);
                                    self.types.push({ codice: ko.observable(tipo.id), descrizione: ko.observable(tipo.name), tabella: ko.observable(""), hint: "fa fa-file" });
                                    self.Selecttypes({ codice: ko.observable(tipo.id) });
                                    ShowMessage('success', "Dati salvati correttamente", 'Message_Target', null);
                                }).fail(function (err) {
                                    self.ErrorMessage("Errore durante il caricamento delle informazioni dei dati. Errore: " + err.status);
                                });
                        }).fail(function (err) {
                            self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status);
                        });
                } else {
                    self.ErrorMessage("Errore durante il salvataggio dei dati.");
                }
            })
            .fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni dei dati. Errore: " + err.status);
            });
    }

    self.Save = function (data) {
        var tipo = ko.mapping.toJS(data);

        if (!tipo.name || tipo.name === "") {
            self.ErrorMessage("Non hai indicato un nome per la nuova tipologia");
            return;
        }

        if (/*tipo.MasterData.trim() == "" &&*/ tipo.includeAttachments) {
            self.ErrorMessage("Non hai indicato il filtro sugli allegati da spedire.");
            return;
        }
        delete tipo.acl;
        if (!tipo.aclId) { delete tipo.aclId; }
        tipo.contentType = parseInt(tipo.contentType);
        tipo.expirationStrategy = parseInt(tipo.expirationStrategy);
        if (tipo.iconColor)
            tipo.iconColor = tipo.iconColor;//.substring(1);
        tipo.initialStatus = parseInt(tipo.initialStatus);
        //tipo.batchScan = self.IsBatchScan();
        tipo.internal = self.IsInternal();
        tipo.toBeIndexed = self.ToBeIndexed();
        tipo.reserved = self.IsReserved();
        tipo.convertToPDF = self.ConvertToPDF();

        var p = 0;
        var i = 0;
        tipo.fieldCount = 0;

        tipo.fields = [];
        $(self.Keys()).each(function (index, e) {

            var field = {
                "Id": e.ID,
                "FieldTypeId": e.KType(),
                "Preservable": e.Cs,
                "Title": e.descrizione,
                "Mandatory": e.Required,
                "Editable": e.Editable,
                "Tag": e.Multiple,
                "Inherited": e.Eredita,
                "Deleted": e.Deleted(),
                "KeyField": false,
                "DocumentTypeId": tipo.id,
                "FieldIndex": index+1,/*e.Index(),*/
                "FieldName": e.GUID,
                "Visibility": e.Visibilita
                //DefaultChiave : e.Default;
                //Multiple : e.Multiple;
                //Encrypted : e.Crypted;
                //Width : e.LarghezzaChiave;
                //Distribuzione : e.Distribuzione;
            }

            tipo.fields[i] = field;
            i = i + 1;
            tipo.fieldCount = i;

        });


        //tipo.expirationOwner = $("#" + campo_scadenza + "_Val").val();
        //if (tipo.contentType == 4)
        //    tipo.creationFormKey = self.FormTemplates();
        //else
        //    tipo.templateId = 0;
        //var params = {
        //    "Bd": queryString("bd"),
        //    "Tipo": tipo
        //};
        //$.postJSON("/WebServices/Types.asmx/SetType", JSON.stringify(params), function (type) {


        $.ajax({
            url: "/internalapi/DocumentType",
            type: "PUT",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify( tipo ) 
        })
            .done(function (type) {
                if (type === 0) {
                    self.ErrorMessage("Errore durante il salvataggio dei dati.");
                } else {
                    //self.LoadForms();
                    self.Selecttypes({ codice: ko.observable(tipo.codice) });
                    ShowMessage('success', "Dati salvati correttamente", 'Message_Target', null);
                }
                self.SelectedtypesObject().descrizione(tipo.name);
            })
            .fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status);
            });
    }


};

