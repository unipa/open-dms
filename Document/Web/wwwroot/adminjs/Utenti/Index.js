/// Gestione Utenti

function viewModel(_Container, callback) {

    //funzione per intercettare le call ajax ed eseguire il refresh del token quando necessario

    var self = this;
    self.Container = _Container;
    self.Current = ko.observable();
    self.users = ko.observableArray();
    self.roles = ko.observableArray();
    self.groups = ko.observableArray();
    self.ErrorMessage = ko.observable();
    self.ErrorMessage.subscribe(function () {
        if (self.ErrorMessage() !== "")
            ShowMessage('alert', self.ErrorMessage(), 'Message_Target', '/Admin/Users/Index');
        self.ErrorMessage("");
    });

    self.GetAllRoles = function () {

        $.ajax({
            url: "/internalapi/Roles",
            type: "GET",
        })
            .done(function (rolelist) {
                self.roles(ko.mapping.fromJS(rolelist)());
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
    }
    self.GetAllGroups = function () {

        $.ajax({
            url: "/internalapi/Roles",
            type: "GET",
        })
            .done(function (rolelist) {
                self.roles(ko.mapping.fromJS(rolelist)());
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
    }


    self.GetByRoles = function (select) {

        $.ajax({
            url: "/internalapi/UserManager?GetAllDeleted",
            type: "GET",
        })
            .done(function (typelist) {
                typelist.reverse();
                t = "Nuovo Ruolo";

                $(typelist).each(function (i, e) {
                    e.icon = "fa fa-user-md";
                });
                typelist.push({ id: null, roleName: t, icon: "fa fa-plus", dataType: "" });
                typelist.reverse();
                self.users(ko.mapping.fromJS(typelist)());

                if (select) self.Selectusers({ id: ko.observable("") });
                else
                    self.Selectusers({ id: self.Current().id });
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
    }
    self.Refresh = function (select) {

        $.ajax({
            url: "/internalapi/UserManager?GetAllDeleted",
            type: "GET",
        })
            .done(function (typelist) {
                typelist.reverse();
                t = "Nuovo Ruolo";

                $(typelist).each(function (i, e) {
                    e.icon = "fa fa-user-md";
                });
                typelist.push({ id: null, roleName: t, icon: "fa fa-plus", dataType: "" });
                typelist.reverse();
                self.users(ko.mapping.fromJS(typelist)());

                if (select) self.Selectusers({ id: ko.observable("") });
                else
                    self.Selectusers({ id: self.Current().id });
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
    }

    self.Refresh(true);

    $(".List").List({
        model: self,
        items: "users",
        idField: "id",
        textField: "roleName",
        //iconClass: "",
        iconField: "icon",
        commentField: "",
        className: "",
        onSelect: "Select",
        NewItem: "Nuovo Ruolo",
    });

    self.XHR = undefined;

    self.Select = function (data, callback) {
        if (self.XHR) self.XHR.abort();
        var c = data;
        if (c === "") { c = "Nuovo Ruolo"; }
        var p = $.find('.pill-pane.active');
        if (p.length > 0) p = p[0].id; else p = "t0";

        $.ajax({
            url: "/internalapi/Role/" + c,
            type: "GET",
        })
            .done(function (type) {

                self.ErrorMessage("");

                if (!type.roleName) {
                    type.id = null;
                    type.dataType = null;
                }

                self.Current(ko.mapping.fromJS(type));
                if (callback) callback.call(this);
                $(window).resize();
                AttivazionePopover();
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
        return true;
    }

    self.Remove = function (data) {
        Confirm("Sei sicuro di voler eliminare questo ruolo: " + data.roleName() + "?", "Message_Target", function () {
            $.ajax({
                url: "/internalapi/Role/" + data.id(),
                type: "DELETE",
            })
                .done(function (typelist) {
                    self.users.remove(function (item) { return item.id() == data.id() });
                    self.Selectusers({ id: ko.observable("") });
                }).fail(function (err) {
                    self.ErrorMessage("Errore durante l'eliminazione. Errore: " + err.status);
                });
        }, null);

    }

    self.Reset = function (data) {
        self.Selectusers(data);
    }


    self.Create = function (data) {
        var tipo = ko.mapping.toJS(data);
        if (!tipo.id || tipo.id == "") {
            self.ErrorMessage("Specificare l'Identificativo del ruolo.");
            ShowMessage("alert", "Specificare l'Identificativo del ruolo.", 'Message_Target', "/Admin/users/Index");
            return;
        }

        if (!tipo.roleName || tipo.roleName == "") {
            self.ErrorMessage("Specificare il nome del ruolo.");
            ShowMessage("alert", "Specificare il nome del ruolo.", 'Message_Target', "/Admin/users/Index");
            return;
        }

        $.ajax({
            url: "/internalapi/Role/" + tipo.id,
            type: "GET",
        })
            .done(function (type1) {
                if (type1.id.toUpperCase() !== tipo.id.toUpperCase()) {

                    $.ajax({
                        url: "/internalapi/Role",
                        type: "POST",
                        headers: { "Content-Type": "application/json" },
                        data: JSON.stringify(tipo)
                    })
                        .done(function (type) {

                            $.ajax({
                                url: "/internalapi/Role",
                                type: "GET",
                            })
                                .done(function (u1) {

                                    u1.reverse();
                                    t = "Nuovo Ruolo";


                                    $(u1).each(function (i, e) {
                                        e.icon = "fa fa-user-md";
                                    });

                                    u1.push({ id: "", roleName: t, icon: "fa fa-plus", dataType: "" });

                                    u1.reverse();
                                    self.users(ko.mapping.fromJS(u1)());
                                    self.Selectusers({ id: ko.observable(tipo.id) });
                                    ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                                });

                        }).fail(function (err) {
                            self.ErrorMessage("Errore durante il salvataggio dati. Errore: " + err.status);
                        });

                } else {
                    self.ErrorMessage("Errore nella creazione del ruolo.");
                }
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dati. Errore: " + err.status);
            });
    }

    self.Save = function (data) {
        var tipo = ko.mapping.toJS(data);

        if (!tipo.roleName || tipo.roleName == "") {
            self.ErrorMessage("Errore: parametro nome vuoto.");
            return;
        }

        $.ajax({
            url: "/internalapi/Role",
            type: "PUT",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify(tipo)
        })
            .done(function (type) {
                if (type != 1) {
                    self.ErrorMessage("Errore durante il salvataggio dei dati.");
                } else {
                    self.Selectusers({ id: ko.observable(tipo.id) });
                    ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                }
                self.SelectedusersObject().roleName(tipo.roleName);
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status);
            });
    }


};

