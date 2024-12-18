
function CollectCustomProperties() {
    var customProperties = {};

    $("#FormCustomProperties .customProperties")
        .map(function () {

            let name = $(this).attr("id");
            let value = $(this).val();

            customProperties[name] = value;

        });

    return JSON.stringify(customProperties);
}

function LoadCustomProperties(customProperties) {

    customProperties = JSON.parse(customProperties);

    let keys = Object.keys(customProperties);

    for (let i = 0; i < keys.length; i++) {

        let value = customProperties[keys[i]];
        let key = keys[i];

//        if ($("#FormCustomProperties .customProperties#" + key).is("select")) { //caso per i select che va in sinergia con la funzione selectHelper
//            $('<input type="hidden" value="' + value + '" class="selectedInfo" id="' + key + '" />').insertBefore("#FormCustomProperties #" + key);
//        } else {
        $("#FormCustomProperties .customProperties#" + key).val(value);
        $("#FormCustomProperties .customProperties#" + key).attr("default-value", value);
//        }

    }
}

function CollectDefaultProperties(tipo) {

    $("#FormCustomProperties .defaultProperties")
        .map(function () {

            let name = $(this).attr("id");
            let value = $(this).val();

            //tipo.tag = ($("#Tag").val()) ? true : false; 
            if (name === 'tag')
                tipo[name] = (value) ? true : false;
            else
                tipo[name] = value;
        });
    return tipo;
}

function LoadDefaultProperties(defaultProperties) {

    let keys = Object.keys(defaultProperties);

    for (let i = 0; i < keys.length; i++) {

        let value = defaultProperties[keys[i]];
        let key = keys[i];

        if ($("#FormCustomProperties .defaultProperties #" + key).is("select")) { //caso per i select che va in sinergia con la funzione selectHelper

            $('<input type="hidden" value="' + value + '" class="selectedInfo" id="' + key + '" />').insertBefore("#FormCustomProperties #" + key);
        }
        else if (value === true || value === false) {
            $("#FormCustomProperties .defaultProperties#" + key).attr('checked', 'checked');
        }
        else {
            $("#FormCustomProperties .defaultProperties#" + key).val(value);
        }

    }
}



function viewModel(_Container,callback) {

    //funzione per intercettare le call ajax ed eseguire il refresh del token quando necessario

    var self = this;
    self.Container = _Container;
    self.Current = ko.observable();
    self.newId = ko.observable();
    self.ErrorMessage = ko.observable();
    self.ErrorMessage.subscribe(function () {
        if (self.ErrorMessage() !== "")
            ShowMessage('alert', self.ErrorMessage(), 'Message_Target', '/Admin/Metadati/Index');
        self.ErrorMessage("");
    });
    self.types = ko.observableArray();

    self.UnSearchable = ko.observable(true);
    self.customProperties = ko.observable("{}");
    self.defaultValue = ko.observable("");
    self.tag = ko.observable(false);
    self.columnWidth = ko.observable("100px");

    self.FormattazioneTesto = ko.observableArray([
        { Codice: 0, Descrizione: "Nessuna" },
        { Codice: 1, Descrizione: "Codice Fiscale" },
        { Codice: 2, Descrizione: "Partita IVA" },
        { Codice: 5, Descrizione: "Email" },
        { Codice: 5, Descrizione: "Telefono" },
        { Codice: 3, Descrizione: "URL" },
        { Codice: 4, Descrizione: "Tutto Maiuscole" }
    ]);
    self.FormattazioneParagrafo = ko.observableArray([
        { Codice: 0, Descrizione: "Testo Semplice" },
        { Codice: 1, Descrizione: "HTML" },
        { Codice: 2, Descrizione: "XML" }
    ]);

    self.Tabella = ko.observable();
    self.Documents = ko.observable("...");
    self.DocumentTypes = ko.observableArray();

    self.DataTypes = ko.observable();
    self.Connessioni = ko.observable();


    // richiesta delle tipologie di metadato disponibile
    $.ajax({
        url: "/internalapi/CustomField/GetAllTypes",
        type: "GET",
    })
        .done(function (type) {
            self.DataTypes(type);
        }).fail(function (err) {
            self.ErrorMessage("Errore durante il caricamento delle informazioni.")
        })

    self.ResetFormTipologia = function () {

        var dataType = "NotFound";
        try {
            dataType = $("#Tipo").val();
        } catch (ex) { }


        $('#FormCustomProperties').load('/adminjs/customProperties/'+ escape( dataType )+ '.html', function (response, status, xhr) {
            if (status != "error") {
                //                $('#FormCustomProperties').load("customProperties/NotFound.html");
                LoadCustomProperties(self.customProperties());
                // caricamento dati con campi prioritari
                var Tag = (self.tag()) ? true : false;
                var defaultValues = {
                    'tag': Tag, 'defaultValue': self.defaultValue(), 'columnWidth': self.columnWidth()
                };
                LoadDefaultProperties(defaultValues);
                //$("#FormCustomProperties #DefaultValue").val(self.defaultValue());
                //if (self.tag()) { $("#FormCustomProperties #Tag").attr("checked", "checked") }
            } else {
                $('#FormCustomProperties').empty();
            }
        });

    }


    self.Refresh = function (select) {

        $.ajax({
            url: "/internalapi/CustomField",
            type: "GET",
        })
            .done(function (typelist) {
                typelist.reverse();
                t = "Nuovo Metadato";

                $(typelist).each(function (i, e) {
                    e.description = "fa fa-tag";
                });
                typelist.push({ id: "", name: t, description: "fa fa-plus", dataType: "" });
                typelist.reverse();
                self.types(ko.mapping.fromJS(typelist)());
                if (select) self.Selecttypes({ id: ko.observable("") });
                else
                    self.Selecttypes({ id: self.Current().id });
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
    }

    self.Refresh(true);

    $(".List").List({
        model: self,
        items: "types",
        idField: "id",
        textField: "name",
        //iconClass: "",
        iconField: "description",
        commentField: "dataType",
        className: "",
        onSelect: "Select",
        NewItem: "Nuovo Metadato",
    });

    self.XHR = undefined;

    self.Select = function (data, callback) {
        if (self.XHR) self.XHR.abort();
        var c = data;
        if (c === "") { c = "nuova tipologia"; }
        var p = $.find('.pill-pane.active');
        if (p.length > 0) p = p[0].id; else p = "t0";
        self.Documents("...");
        self.DocumentTypes();

        $.ajax({
            url: "/internalapi/CustomField/" + c,
            type: "GET",
        })
            .done(function (type) {

                self.ErrorMessage("");

                if (!type.name) {
                    type.id = null;
                    type.dataType = null;
                }
                self.newId(type.id);
                self.Current(ko.mapping.fromJS(type));

                if (type.searchable) self.UnSearchable(false); else self.UnSearchable(true);
                if (type.customProperties) self.customProperties(type.customProperties); else self.customProperties("{}");
                if (type.defaultValue) self.defaultValue(type.defaultValue); else self.defaultValue("");
                if (type.columnWidth) self.columnWidth(type.columnWidth);
                if (type.tag) self.tag(type.tag); else self.tag(false);

                //$.postJSON("/WebServices/Metadato.asmx/Documents", "{ Bd: '" + self.BancaDati + "', Tipo:'" + c + "'}", function (type) {
                //    self.Documents(type);
                //});
                self.Documents();

                self.ResetFormTipologia();

                if (callback) callback.call(this);

                var a = $("a[href='#" + p + "']");
                if (!a.parent().is(":visible"))
                    p = "t0";
                $("a[href='#" + p + "']").tab('show');
                $(window).resize();

                AttivazionePopover();

            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
        return true;
    }

    self.Remove = function (data) {
        Confirm("Sei sicuro di voler eliminare questo metadato: " + data.name() + "?", "Message_Target", function () {
            $.ajax({
                url: "/internalapi/CustomField/" + data.id(),
                type: "DELETE",
            })
                .done(function (typelist) {
                    self.types.remove(function (item) { return item.id() == data.id() });
                    self.Selecttypes({ id: ko.observable("") });
                }).fail(function (err) {
                    self.ErrorMessage("Errore durante l'eliminazione. Errore: " + err.status);
                });
        }, null);

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
        tipo.id = self.newId();
        if (tipo.id === "" || !tipo.id) {
            tipo.id = uuid4();
        }

        if (!tipo.name || tipo.name == "") {
            self.ErrorMessage("Specificare il nome del metadato.");
            ShowMessage("alert", "Specificare il nome del metadato.", 'Message_Target', "/Admin/Metadati/Index");
            return;
        }

        tipo.title = tipo.name;
        //tipo.defaultValue = $("#DefaultValue").val();
        //tipo.tag = ($("#Tag").val()) ? true : false; 
        tipo = CollectDefaultProperties(tipo);

        tipo.customProperties = CollectCustomProperties();


        $.ajax({
            url: "/internalapi/CustomField/" + tipo.id,
            type: "GET",
        })
            .done(function (type1) {
                if (type1.id.toUpperCase() !== tipo.id.toUpperCase()) {

                    $.ajax({
                        url: "/internalapi/CustomField",
                        type: "POST",
                        headers: { "Content-Type": "application/json"},
                        data: JSON.stringify(tipo)
                    })
                        .done(function (type) {

                            $.ajax({
                                url: "/internalapi/CustomField",
                                type: "GET",
                            })
                                .done(function (u1) {

                                    u1.reverse();
                                    t = "Nuovo Metadato";


                                    $(u1).each(function (i, e) {
                                        //if (e.id !== '') e.description = baseRoot + "/bootstrap-italia/svg/sprites.svg#it-code-circle";
                                        //else e.description = baseRoot + "/bootstrap-italia/svg/sprites.svg#it-plus-circle";
                                        e.description = "fa fa-tag";
                                    });

                                    u1.push({ id: "", name: t, description: "fa fa-plus", dataType: "" });

                                    u1.reverse();
                                    self.types(ko.mapping.fromJS(u1)());
                                    self.Selecttypes({ id: ko.observable(tipo.id) });
                                    ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                                });

                        }).fail(function (err) {
                            self.ErrorMessage("Errore durante il salvataggio dati. Errore: " + err.status);
                        });

                } else {
                    self.ErrorMessage("Errore nella creazione del metadato.");
                }
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dati. Errore: " + err.status);
            });
    }

    self.Save = function (data) {
        var tipo = ko.mapping.toJS(data);

        if (!tipo.name || tipo.name == "") {
            self.ErrorMessage("Errore: parametro nome vuoto.");
            return;
        }

        tipo = CollectDefaultProperties(tipo);

        tipo.customProperties = CollectCustomProperties();

        $.ajax({
            url: "/internalapi/CustomField",
            type: "PUT",
            headers: { "Content-Type": "application/json"},
            data: JSON.stringify(tipo)
        })
            .done(function (type) {
                if (type != 1) {
                    self.ErrorMessage("Errore durante il salvataggio dei dati.");
                } else {
                    self.Selecttypes({ id: ko.observable(tipo.id) });
                    ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                }
                self.SelectedtypesObject().name(tipo.name);
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status);
            });
    }


};

