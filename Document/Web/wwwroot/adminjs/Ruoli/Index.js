

function viewModel(_Container, callback) {

    //funzione per intercettare le call ajax ed eseguire il refresh del token quando necessario

    var self = this;
    self.Container = _Container;
    self.Current = ko.observable();
    self.roles = ko.observableArray();
    self.Users = ko.observableArray([]);
    self.ErrorMessage = ko.observable();
    self.ErrorMessage.subscribe(function () {
        if (self.ErrorMessage() !== "")
            ShowMessage('alert', self.ErrorMessage(), 'Message_Target', '/Admin/Roles/Index');
        self.ErrorMessage("");
    });


    self.Refresh = function (select) {

        $.ajax({
            url: "/internalapi/Role?IncludeDeleted=true",
            type: "GET",
        })
            .done(function (typelist) {
                typelist.reverse();
                t = "Nuovo Ruolo";

                $(typelist).each(function (i, e) {
                    e.icon = "fa fa-user-md";
                });
                typelist.push({ id: "", roleName: t, icon: "fa fa-plus", dataType: "" });
                typelist.reverse();
                self.roles (ko.mapping.fromJS(typelist)());

                if (select) self.Selectroles({ id: ko.observable("") });
                else
                    self.Selectroles({ id: self.Current().id });
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il caricamento delle informazioni. Errore: " + err.status);
            });
    }

    self.Refresh(true);

    $(".List").List({
        model: self,
        items: "roles",
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
                if (!type.id)
                //if (!type.roleName) 
                {
                    type = { id : "", roleName : "" };
                //    type.dataType = null;
                }
                self.Current(ko.mapping.fromJS(type));
                if (callback) callback.call(this);
                $(window).resize();
                AttivazionePopover();

                $.ajax({
                    url: "/Admin/organigramma/GetUsersByRoleId/" + type.id,
                    type: "GET",
                })
                    .done(function (userlist) {
                        userlist.forEach(g => {
                            g.startDate = new Date(g.startISODate / 10000, parseInt(g.startISODate / 100) % 100 - 1, parseInt(g.startISODate) % 100).toLocaleDateString();
                            g.endDate = new Date(g.endISODate / 10000, parseInt(g.endISODate / 100) % 100 -1, parseInt(g.endISODate) % 100).toLocaleDateString ();
                        });
                        self.Users(ko.observableArray(userlist));
                    });


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
                    self.roles.remove(function (item) { return item.id() == data.id() });
                    self.Selectroles({ id: ko.observable("") });
                }).fail(function (err) {
                    self.ErrorMessage("Errore durante l'eliminazione. Errore: " + err.status);
                });
        }, null);

    }

    self.Reset = function (data) {
        self.Selectroles(data);
    }


    self.Create = function (data) {
        var tipo = ko.mapping.toJS(data);
        if (!tipo.id || tipo.id == "") {
            self.ErrorMessage("Specificare l'Identificativo del ruolo.");
            ShowMessage("alert", "Specificare l'Identificativo del ruolo.", 'Message_Target', "/Admin/Roles/Index");
            return;
        }

        if (!tipo.roleName || tipo.roleName == "") {
            self.ErrorMessage("Specificare il nome del ruolo.");
            ShowMessage("alert", "Specificare il nome del ruolo.", 'Message_Target', "/Admin/Roles/Index");
            return;
        }

        $.ajax({
            url: "/internalapi/Role/" + tipo.id,
            type: "GET",
        })
            .done(function (type1) {
                if (!type1) {

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
                                    self.roles(ko.mapping.fromJS(u1)());
                                    self.Selectroles({ id: ko.observable(tipo.id) });
                                    ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                                });

                        }).fail(function (err) {
                            self.ErrorMessage(JSON.parse(err.responseText).title+ "\n\n" + JSON.stringify(JSON.parse(err.responseText).errors));
                        });

                } else {
                    self.ErrorMessage("Errore nella creazione del ruolo.");
                }
            }).fail(function (err) {
                self.ErrorMessage(JSON.parse(err.responseText).title+ "\n\n" + JSON.stringify(JSON.parse(err.responseText).errors));
            });
    }

    self.Save = function (data) {
        var tipo = ko.mapping.toJS(data);

        if (!tipo.roleName || tipo.roleName == "") {
            self.ErrorMessage("Errore: parametro nome vuoto.");
            return;
        }

        $.ajax({
            url: "/internalapi/Role", ///" + escape(tipo.id) + "/" + tipo.roleName,
            type: "PUT",
            headers: { "Content-Type": "application/json" },
            data: JSON.stringify(tipo )
        })
            .done(function (type) {
                if (!type) {
                    self.ErrorMessage("Errore durante il salvataggio dei dati.");
                } else {
                    self.Selectroles({ id: ko.observable(tipo.id) });
                    ShowMessage("success", "Dati salvati correttamente", 'Message_Target', null)
                }
                self.SelectedrolesObject().roleName(tipo.roleName);
            }).fail(function (err) {
                self.ErrorMessage("Errore durante il salvataggio dei dati. Errore: " + err.status);
            });
    }


};

