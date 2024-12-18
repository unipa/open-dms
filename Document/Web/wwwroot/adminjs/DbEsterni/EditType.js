

function viewModel(_Container,callback) {

    //funzione per intercettare le call ajax ed eseguire il refresh del token quando necessario

    var self = this;
    self.Container = _Container;
    self.Current = ko.observable();
    self.ErrorMessage = ko.observable();
    self.ErrorMessage.subscribe(function () {
        if (self.ErrorMessage() !== "")
            ShowMessage('alert', self.ErrorMessage(), 'Message_Target', '/Admin/DatabaseEsterni/Index');
        self.ErrorMessage("");
    });
    self.types = ko.observableArray();
    self.Database = ko.observable();

    self.Driver = ko.observableArray([
        { Codice: "System.Data.SqlClient", Descrizione: "Microsoft SQL Server Nativo" },
        { Codice: "System.Data.Mysql", Descrizione: "MySql" },
        { Codice: "System.Data.OracleClient", Descrizione: "Oracle" },
        { Codice: "npgsql", Descrizione: "PostgreSQL" },
        { Codice: "System.Data.OleDb", Descrizione: "Microsoft OLE DB" },
        { Codice: "System.Data.Odbc", Descrizione: "ODBC" },
        { Codice: "Sap.Data.Hana", Descrizione: "SAP HANA" }
    ]);

    //ProviderList = {
    //    "System.Data.SqlClient": "sqlserver",
    //    "System.Data.Mysql": "mysql",
    //    "System.Data.OracleClient": "oracle"
    //};

    //self.DatabaseList = ko.observableArray([
    //    { Codice: 0, Descrizione: "Scegli un database..." },
    //    { Codice: 1, Descrizione: "Microsoft SQL Server Express" },
    //    { Codice: 2, Descrizione: "Microsoft SQL Server" },
    //    { Codice: 6, Descrizione: "Microsoft Access" },
    //    { Codice: 3, Descrizione: "Oracle Database" },
    //    { Codice: 5, Descrizione: "Oracle MySql" },
    //    { Codice: 4, Descrizione: "DB2 AS/400" },
    //    { Codice: 7, Descrizione: "PostgreSQL" },
    //    { Codice: 8, Descrizione: "SAP Hana" }
    //]);

    self.Refresh = function (select) {

        $.ajax({
            url: "/internalapi/ExternalDatasource",
            type: "GET"
        })
            .done(function (typelist) {

                var lista = new Array();
                t = "Aggiungi Collegamento";
                lista.push({ Codice: "", Descrizione: t, Hint: "fa fa-plus", Tabella: "" });
                $(typelist).each(function (i, e) {
                    lista.push({ Codice: e.id, Descrizione: e.name, Hint: "fa fa-database", Tabella: "" });
                });
                self.types(ko.mapping.fromJS(lista)());
                if (select) {
                    self.Selecttypes({ Codice: ko.observable("") }, callback);
                }
                else
                    self.Selecttypes({ Codice: self.Current().ID });

            }).fail(function (err) {
                self.ErrorMessage( "Errore durante il caricamento delle informazioni. " + err.status)
            })
    }

    self.Refresh(true);

    $(".List").List({
        model: self,
        items: "types",
        idField: "Codice",
        textField: "Descrizione",
        iconField: "Hint",


        //imageField: "Hint",
        commentField: "Tabella",
        rightTextField: "",
        className: "",
        onSelect: "Select",
        NewItem: "Nuova Sorgente",
    });

    self.XHR = undefined;

    self.Select = function (data, callback) {

        $.ajax({
            url: "/internalapi/ExternalDatasource/" + data,
            type: "GET"
        })
            .done(function (type) {

                if (type.length == 0 || type.length) {
                    type = {
                        "id": null,
                        "name": null,
                        "driver": null,
                        "provider": null,
                        "connection": null,
                        "userName": null,
                        "password": null,
                        "isNew": true,
                        "connectionString": ";Integrated Security=SSPI"
                    };
                }

                self.ErrorMessage("");
                self.XHR = undefined;

                var test = ko.mapping.fromJS(type)

                self.Current(test);

                if (callback) callback.call(this);
                $(window).resize();

                AttivazionePopover();

            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento dei dati. " + err.status)
            });
        return true;
    }

    


    self.Remove = function (data) {
        Confirm("Sei sicuro di voler eliminare questa fonte di dati: " + data.name() + " ?", 'Message_Target', function () {

            $.ajax({
                url: "/internalapi/ExternalDatasource/" + data.id(),
                type: "DELETE",
            })
                .done(function (type) {
                    self.types.remove(function (item) {
                        return item.Codice() == data.id()
                    });
                    self.Selecttypes({ Codice: ko.observable("") });
                    ShowMessage("success", "Fonte dati eliminata con successo.", 'Message_Target', null);
                }).fail(function (err) {
                    self.ErrorMessage("Errore durante l'eliminazione della fonte dati. "+ err.status);
                });

        }, null);
    }

    self.Reset = function (data) {
        self.Selecttypes(data.id());
    }

    self.Test = function (data) {
        var tipo = ko.mapping.toJS(data);

        $.ajax({
            url: "/internalapi/ExternalDatasource/Test",
            type: "POST",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify(tipo)
        })
            .done(function (type) {
                if (type) {
                    self.ErrorMessage("Errore: " + type);
                }
                else
                    ShowMessage("success", "Connessione avvenuta con successo.", 'Message_Target', null);
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il test. " + err.status);
            });
    }

    self.Preselect = function (data) {
        if (data.driver() == "System.Data.SqlClient") {
            data.connection("Data Source=SQLSERVER;Database={0};Persist Security Info=true;TrustServerCertificate=true");
        }
        if (data.driver() == "System.Data.SqlClient") {
            data.connection("Data Source=.\\SQLEXPRESS;Database={0};Persist Security Info=true");
        }
        if (data.driver()=="System.Data.OracleClient") {
            data.connection("enlist=false;direct=true;Data Source=localhost;port=1521;sid=orcl");
        }
        if (data.driver()=="System.Data.OleDb") {
            data.connection("Provider=IBMDA400.DataSource.1;Data Source=ISERIES;Catalog Library List={0};Default Collection={0};Persist Security Info=true");
        }
        if (data.driver()=="MySql.Data.MySqlClient") {
            data.connection("server=localhost;Database={0}");
        }
        if (data.driver()=="System.Data.OleDb") {
            data.connection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=true");
        }
        if (data.driver()=="npgsql") {
            data.connection("Data Source=localhost;Data Source={0};Persist Security Info=true");
        }
        if (data.driver()=="Sap.Data.Hana") {
            data.connection("Server=127.0.0.1:30015;UserID=SYSTEM;Password=;Current Schema=DPM");
        }
    }

    self.Create = function (data) {
        var tipo = ko.mapping.toJS(data);

        if (!tipo.name || tipo.name == "") {
            self.ErrorMessage("Specificare il nome della fonte dati");
            return;
        }

        //if (!self.Database() || self.Database() == "") {
        //    self.ErrorMessage("Specificare il tipo di database al quale collegarsi");
        //    return;
        //}

        $.ajax({
            url: "/internalapi/ExternalDatasource/" + tipo.id,
            type: "GET"
        })
            .done(function (type1) {
                if (type1.isNew) {
                    type1.name = tipo.name;
                    type1.driver = tipo.driver;
                    type1.id = tipo.id;
                    //if (tipo.provider)
                    //    type1.provider = self.Database();
                    //else
                    //    type1.provider = "";

                    type1.connection = tipo.connection;
                    type1.username = tipo.userName;
                    type1.password = tipo.password;

                    //if (type1.driver !== 'System.Data.OleDb') {
                    //    type1.provider = "";
                    //}

                    $.ajax({
                        url: "/internalapi/ExternalDatasource",
                        type: "POST",
                        headers: { "Content-Type": "application/json" },
                        data: JSON.stringify(type1)
                    })
                        .done(function (type) {
                            if (!type) {
                                self.ErrorMessage("Errore durante la creazione della fonte dati.");
                                return;
                            }
                            else
                                $.ajax({
                                    url: "/internalapi/ExternalDatasource",
                                    type: "GET"
                                })
                                    .done(function (u1) {
                                        tipo.ID = type;
                                        self.types.push({ Codice: ko.observable(type.id), Descrizione: ko.observable(tipo.name), Tabella: ko.observable(""), Hint: "fa fa-database" });
                                        self.Selecttypes({ Codice: ko.observable(type.id) });
                                        ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                                    }).fail(function (err) {
                                        self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status)
                                    });
                        }).fail(function (err) {
                            self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status)
                        });

                } else {
                    self.ErrorMessage("Errore durante la creazione della fonte dati.")
                }
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.responseText)
            });
    }

    self.Save = function (data) {
        var tipo = ko.mapping.toJS(data);

//        if (!tipo.provider)
//            tipo.provider = "";


        if (!tipo.name || tipo.name == "") {
            self.ErrorMessage("Errore: parametro nome vuoto.");
            return;
        }

        $.ajax({
            url: "/internalapi/ExternalDatasource",
            type: "POST",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify(tipo)
        })
            .done(function (type) {
                if (type.name == "" || type.name == null) {
                    self.ErrorMessage("Errore: parametro nome vuoto.");
                } else {
                    ShowSuccess();
                    self.Selecttypes({ Codice: ko.observable(type.id) });
                }
                self.SelectedtypesObject().Descrizione(type.name);
            })
            .fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status);
            });
    }

};

