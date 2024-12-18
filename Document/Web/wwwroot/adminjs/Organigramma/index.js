function PrepareDateFilter(StartDate) {
    //al click di ricerca per data di inizio

    $("#StartISODate").on("change", function () {
        var StartISODate = $("#StartISODate").val();
        if (StartISODate === "") StartISODate = "0";
        else {
            StartISODate = parseInt(StartISODate.replace(/-/g, ""))
        }
        window.location.href = "/Admin/Organigramma/Index?StartISODate=" + StartISODate;
    });
    if (StartDate != 0) {
        try {
            $("#StartISODate").val(convertIntToDate(StartDate));
        } catch (err) {
            ShowMessage("alert", err, 'Message_Target');
        }
    }

    $("#Filter").on("keyup", function () {
        var filtro = $("#Filter").val().toLowerCase();
        var nodes = $("#Organigramma ul li");
        nodes.each((i, e) => {
            var name = $(e).html().toLowerCase();
            if (name.includes(filtro) )
                $(e).show();
            else
                $(e).hide();
        });
    });


}

function AddEventsToNodes() {
    //ogni volta che viene selezionato un gruppo
    $("#Organigramma li a").on("click", function () {
        var UserGroupId = $(this).attr("codice"); //ottiene lo UserGroupId
        Carica("UserGroupId"); //toglie e inserisce la classe active all'elemento selezionato
        var OrganizationNodeTree = breadthFirstSearch(UserGroupId) //ricerca il nodo selezionato nell'organigramma tramite ricerca in ampiezza
        ShowNodeDetail(OrganizationNodeTree); //costruisce la modale
    });
}

function CreateNodes() {
    var loading_spinner = `<div class="d-flex align-items-center justify-content-center h-100">
                                        <div class="progress-spinner progress-spinner-active">
                                            <span class="visually-hidden">Caricamento...</span>
                                        </div>
                                    </div>`;

    $("#Organigramma").empty();
    $("#Organigramma").html(loading_spinner);
    var htmlTree = buildList(Tree);
    $("#Organigramma").empty();
    $("#Organigramma").html(htmlTree);
}

function MoveNodeJS(NodeInfo, NewFather) {
    //questa funzione serve a spostare il nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);
    if (extractIntegerFromString() == 0) {
        var Node = breadthFirstSearch(NodeInfo.Id);
        Node.info.startDate = NodeInfo.StartDate;
        Node.info.endDate = NodeInfo.EndDate;
        var FatherNode = breadthFirstSearch(NewFather);
        FatherNode.nodes.push(Node);
    }
    CreateNodes();//aggiorno la l'organigramma a video
}

function UpdateNodeJS(NodeInfo) {
    //questa funzione serve ad aggiornare il nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);

    var Node = breadthFirstSearch(NodeInfo.Id);

    Node.info.shortName = NodeInfo.Body.shortName;
    Node.info.name = NodeInfo.Body.name;
    Node.info.startDate = (NodeInfo.Body.startDate) ? NodeInfo.Body.startDate : (NodeInfo.info.startDate) ? Node.info.startDate : "";
    Node.info.endDate = (NodeInfo.Body.endDate) ? NodeInfo.Body.endDate : Node.info.endDate;
    Node.info.taskReallocationStrategy = NodeInfo.Body.taskReallocationStrategy;
    Node.info.taskReallocationProfile = NodeInfo.Body.taskReallocationProfile;
    Node.info.externalId = NodeInfo.Body.externalId;
    Node.info.notificationStrategy = NodeInfo.Body.notificationStrategy;
    Node.info.notificationProfile = NodeInfo.Body.notificationProfile;
    Node.info.notificationStrategyCC = NodeInfo.Body.notificationStrategyCC;
    CreateNodes();//aggiorno la l'organigramma a video
}

function AddNodeJS(NodeInfo) {
    //questa funzione serve ad aggiungere il nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);
    var FatherNode = (NodeInfo.Body.parentUserGroupId != '' && NodeInfo.Body.parentUserGroupId) ? breadthFirstSearch(NodeInfo.Body.parentUserGroupId).nodes : Tree;
    FatherNode.push({ info: NodeInfo.Body, users: [], nodes: [] })
    CreateNodes();//aggiorno la l'organigramma a video
}

function RemoveNodeJS(Id, EndDate) {
    //questa funzione serve ad eliminare il nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);
    var Node = breadthFirstSearch(Id);
    Node.info.endDate = EndDate;
    CreateNodes();//aggiorno la l'organigramma a video
}

function AddUserJS(Id, body) {
    //questa funzione serve ad aggiungere l'user al nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);
    var Node = breadthFirstSearch(Id);
    Node.users.push(body);
}

function ModificaRuoloUserJS(Id, body) {
    //questa funzione serve ad aggiornare l'user al nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);
    var Node = breadthFirstSearch(Id);
    Node.users = Node.users.filter(function (obj) {
        if (obj.userId === body.userId) {
            obj.roleName = body.roleName;
            obj.roleId = body.roleId;
            obj.startDate = body.startDate;
        }
        return obj
    });
}

function DataFineUserJS(Id, UserId, EndDate) {
    //questa funzione serve ad aggiornare il nodo solo nel frontend, modificare solo la variabile Tree (vedi inizio tag Script);
    var Node = breadthFirstSearch(Id);
    if (new Date(EndDate) <= new Date()) {
        Node.users = Node.users.filter(function (obj) {
            return obj.userId !== UserId;
        });
    } else {
        Node.users = Node.users.filter(function (obj) {
            if (obj.userId === UserId) obj.endDate = EndDate;
            return obj
        });
    }
}

function ToggleNode(a) {
    var ul = $(a).next();
    ul.toggleClass("hidden");
    $(a).find("i").toggleClass("hidden");
}

function buildList(data) {
    var ul = document.createElement("ul");
    ul.classList.add("link-list");

    data.forEach(function (nodes) {
        var li = document.createElement("li");
        li.style.position = "relative";

        var a = document.createElement("a");
        var spanWrapper = document.createElement("span");
        var ulSublist;
        a.classList.add("list-item");
        a.setAttribute('codice', nodes.info.userGroupId);
        a.setAttribute('href', "#");
//        a.setAttribute('data-bs-toggle', "modal");
//        a.setAttribute('data-bs-target', "#MainModal");

        var spanTitle = document.createElement("span");
        spanTitle.setAttribute("style", "max-width: calc(100% - 260px);text-overflow:ellipsis;white-space:nowrap;overflow-x:hidden");
        spanWrapper.classList.add("list-item-title-icon-wrapper");
        spanTitle.textContent = nodes.info.name;
        var spanShortTitle = document.createElement("span");
        spanShortTitle.textContent = nodes.info.shortName;
        var spanDate = document.createElement("span");
        var spanDate2 = document.createElement("span");
        spanDate.textContent = nodes.info.startDate?.substring(0, 10);
        spanDate2.textContent = nodes.info.endDate?.substring(0, 10);
        var color = "#888";
        if (new Date(nodes.info.endDate).getDate <= new Date().getDate())
            color = "crimson";

        spanShortTitle.setAttribute("style", "text-align:right;color:#888;position:absolute; right:180px;font-size:12px");
        spanDate.setAttribute("style", "text-align:right;color:#888;position:absolute; right:90px;font-size:12px");

        spanDate2.setAttribute("style", "text-align:right;color:"+color+";position:absolute; right:0;font-size:12px");

        spanWrapper.appendChild(spanTitle);
        spanWrapper.appendChild(spanShortTitle);
        spanWrapper.appendChild(spanDate);
        spanWrapper.appendChild(spanDate2);

        a.appendChild(spanWrapper);
        li.appendChild(a);


        if (nodes.nodes) {
            ulSublist = buildList(nodes.nodes);
            //var checkbox = document.createElement("a");
            //checkbox.classList.add("list-item");
            //checkbox.setAttribute('codice', nodes.info.userGroupId);
            //checkbox.setAttribute('href', "#");
            //checkbox.setAttribute('data-bs-toggle', "modal");
            //checkbox.setAttribute('data-bs-target', "#MainModal");
            //checkbox.innerHTML = "<i class='fa fa-plus'><i><i class='fa fa-minus hidden'></i>"
            //checkbox.click += () => { $(ulSublist).toggleClass("hidden") }
            //li.appendChild(checkbox);
            if (ulSublist.children.length > 0) {
                ulSublist.classList.add("link-sublist")
                //ulSublist.classList.add("hidden");
                $(li).append("<a href='#' style='position:absolute;left:-12px;top:0;padding:0 2px' onclick='ToggleNode(this)'><i class='fa fa-chevron-down hidden'></i><i class='fa fa-chevron-up'></i></a>");

                li.appendChild(ulSublist);
            }
        }
        ul.appendChild(li);
    });

    return ul;
}

function breadthFirstSearch(targetId) { // ricerca in ampiezza
    var queue = [...Tree];
    while (queue.length > 0) {
        var current = queue.shift();
        if (current.info.userGroupId === targetId) {
            return current;
        }
        if (current.nodes) {
            queue.push(...current.nodes);
        }
        if (current.nodes && current.nodes.length > 0) {
            queue.push(...current.nodes);
        }
    }
    return null; // Se il targetName non viene trovato
}

function getNodesList(NodesToRemove = "EMPTY") {
    var queue = [...Tree];
    var result = [];
    while (queue.length > 0) {
        var current = queue.shift();
        if (current.info.userGroupId !== NodesToRemove) {
            result.push({ nome: current.info.name, id: current.info.userGroupId });
        }
        if (current.nodes) {
            queue.push(...current.nodes);
        }
    }
    return result;
}

function ShowNodeDetail(OrganizationNodeTree) {
    if (!OrganizationNodeTree) return HidePanels();
    document.getElementById('Organigramma').classList.remove("active");

    var StartISODate = extractIntegerFromString(); //recupero la StartISODate della pagina corrente
    document.getElementById('NodeDetail').classList.add("hidden");
    $.ajax({
        url: "/internalapi/Organization/Users/" + OrganizationNodeTree.info.userGroupId + "/" + StartISODate,
        type: "GET",
        headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
    })
        .done(function (users) {
            users.forEach(n => {
                n.startDateText = n.startDate.split('T')[0],
                n.endDateText = n.endDate.split('T')[0]
            });

            $("#Organigramma[codice='" + OrganizationNodeTree.info.userGroupId +"']").addClass("active");
            try {
                OrganizationNodeTree.parent = breadthFirstSearch(OrganizationNodeTree.info.parentUserGroupId).info;

            } catch (e) {
                OrganizationNodeTree.parent = { userGroupId: OrganizationNodeTree.info.parentUserGroupId, name: "Nessuna" };
            }

            OrganizationNodeTree.users = users;
            switch (OrganizationNodeTree.info.notificationStrategy) {
                case 1: OrganizationNodeTree.info.notificationStrategyText = "Utenti abilitati al ruolo 'ViewInbox'"; break;
                case 2: OrganizationNodeTree.info.notificationStrategyText = "Utenti con ruolo specifico"; break;
                case 3: OrganizationNodeTree.info.notificationStrategyText = "Tutti gli utenti della struttura"; break;
                default: OrganizationNodeTree.info.notificationStrategyText = "Tutti gli utenti e sotto-strutture";
            }
            switch (OrganizationNodeTree.info.taskReallocationStrategy) {
                case 1: OrganizationNodeTree.info.taskReallocationStrategyText = "Riassegnazione struttura superiore"; break;
                case 2: OrganizationNodeTree.info.taskReallocationStrategyText = "Riassegnazione a struttura specifica"; break;
                case 3: OrganizationNodeTree.info.taskReallocationStrategyText = "Riassegnazione a ruolo specifico"; break;
                default: OrganizationNodeTree.info.taskReallocationStrategyText = "Archiviazione automatica"; 
            }
            if (OrganizationNodeTree.info.startDate) {
                OrganizationNodeTree.info.period = new Date(OrganizationNodeTree.info.startDate).toISOString().split('T')[0];
                if (OrganizationNodeTree.info.endDate)
                    OrganizationNodeTree.info.period += " ... " + new Date(OrganizationNodeTree.info.endDate).toISOString().split('T')[0];
                else
                    OrganizationNodeTree.info.period += "...";
            }
            else OrganizationNodeTree.info.period = "Nessuno";

        HidePanels(true);
        const template = document.getElementById('NodeDetail_Template').innerHTML;
        const rendered = Mustache.render(template, { data: OrganizationNodeTree });
        document.getElementById('NodeDetail_Content').innerHTML = rendered;
        document.getElementById('NodeDetail').classList.remove("hidden");
    })
    .fail(function (err) {
        HidePanels();
        ShowMessage("alert", "Non è stato possibile recuperare gli utenti di questa unità. Errore: " + err.responseText, 'Message_Target' );
    });
}

function HidePanels(all) {
    document.getElementById('Organigramma').classList.remove("active");
    document.getElementById('NodeDetail').classList.add("hidden");
    document.getElementById('AggiungiNodoModal').classList.add("hidden");
    document.getElementById('ModificaNodoModal').classList.add("hidden");
    document.getElementById('SpostaNodoModal').classList.add("hidden");
    document.getElementById('RimuoviNodoModal').classList.add("hidden");
    document.getElementById('DataFineUserModal').classList.add("hidden");
    document.getElementById('ModificaRuoloUserModal').classList.add("hidden");
    document.getElementById('AggiungiUserModal').classList.add("hidden");
    //$("#totale_strutture").text(Tree.length);
    if (all)
        document.getElementById('NoDetail').classList.add("hidden");
    else
        document.getElementById('NoDetail').classList.remove("hidden");
}


function AggiungiNodoModal(parentNode) {
    HidePanels(true);
    var Modal_Template = "AggiungiNodoModal_Template";
    var Modal_Content = "AggiungiNodoModal_Content";
    var Modal_Id = "AggiungiNodoModal";
    const template = document.getElementById(Modal_Template).innerHTML;
    try {
        parent = breadthFirstSearch(parentNode).info;
    } catch (e) {
        parent = { userGroupId: "", name: "Nessuna" };
    }

    const rendered = Mustache.render(template, {
        today: new Date().toISOString().split('T')[0],
        ParentUserGroupId: parentNode,
        parent: parent,
        StartDate: new Date().toISOString().split('T')[0]
    });
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");
    GestioneCampiProfile();
}
function AggiungiNodoModal_Save() {
    var Modal_Form = "AggiungiNodoForm";
    var form = document.getElementById(Modal_Form);
    var bodyObj = getFormValues(form) //prendo i valori degli input del form
    bodyObj.parentUserGroupId = (bodyObj.parentUserGroupId === 'null') ? null : bodyObj.parentUserGroupId;
    var body = JSON.stringify(bodyObj);
    ShowRequiredPopup(form);//faccio comparire i popup manualmente in caso di campi non validi

    if (document.forms[Modal_Form].reportValidity()) { //controllo la validità dei campi
        $.ajax({
            url: "/Admin/Organigramma/CreateNode",
            type: "POST",
            headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
            data: body
        })
            .done(function (type) {
                //ShowMessage("success", "Nodo aggiunto con successo.", 'Message_Target');
                bodyObj.userGroupId = type;
                AddNodeJS({ Id: type, Body: bodyObj });
                AddEventsToNodes();
                ShowNodeDetail(breadthFirstSearch(type));
            })
            .fail(function (err) {
                ShowMessage("alert", err.responseText, 'Message_Target');
            });
    }
}


function ModificaNodoModal(UserGroupId) {

    var Modal_Template = "ModificaNodoModal_Template";
    var Modal_Content = "ModificaNodoModal_Content";
    var Modal_Id = "ModificaNodoModal";
   //elaboro il template mustache dentro la modale ancora nascosta
    const template = document.getElementById(Modal_Template).innerHTML;

    var data = breadthFirstSearch(UserGroupId).info;
    data.startDate = data.startDate.split('T')[0];
    data.endDate = data.endDate.split('T')[0];
    data["taskReallocationStrategy" + data.taskReallocationStrategy] = "Selected";
    data["notificationStrategy" + data.notificationStrategy] = "Selected";
    data["notificationStrategyCC" + data.notificationStrategyCC] = "Selected";
    try {
        data.parent = breadthFirstSearch(data.parentUserGroupId).info;
    } catch (e) {
        data.parent = { userGroupId: "", name: "Nessuna" };
    }

    HidePanels(true);
    const rendered = Mustache.render(template, {
        data: data,
        CanEditStartDate: (new Date(data.startDate) > new Date().addDays(1)), //flag booleano per far apparire o meno il campo StartDate
        CanEditEndDate: (new Date(data.endDate) > new Date()), //flag booleano per far apparire o meno il campo EndDate
        today: new Date().toISOString().split('T')[0]
    });
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");
    GestioneCampiProfile();
}
function ModificaNodoModal_Save()
{
    var Modal_Form = "ModificaNodoForm";
    var form = document.getElementById(Modal_Form);
    var bodyObj = getFormValues(form) //prendo i valori degli input del form

    var body = JSON.stringify(bodyObj);

    ShowRequiredPopup(form);//faccio comparire i popup manualmente in caso di campi non validi

        if (document.forms[Modal_Form].reportValidity()) { //controllo la validità dei campi
            $.ajax({
                url: "/Admin/Organigramma/UpdateNode/" + bodyObj.userGroupId,
                type: "PUT",
                headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
                data: body
            })
                .done(function (type) {
                    ShowMessage("success", "Nodo modificato con successo.", 'Message_Target');
                    UpdateNodeJS({ Id: data.userGroupId, Body: bodyObj });
                    AddEventsToNodes();
                    ShowNodeDetail(breadthFirstSearch(bodyObj.userGroupId)); 
                })
                .fail(function (err) {
                    ShowMessage("alert", err.responseText, 'Message_Target');
                });
        }
    };



function SpostaNodoModal(UserGroupId) {

    var Modal_Template = "SpostaNodoModal_Template";
    var Modal_Content = "SpostaNodoModal_Content";
    var Modal_Id = "SpostaNodoModal";

    HidePanels(true);
    var NodesList = getNodesList(UserGroupId); //ottengo la lista dei nodi disponibile (passo lo UserGroupId attuale per evitare di inserirlo nella lista)

    //creo le chiavi da usare sul template per selezionare l'option corretta
    var data = breadthFirstSearch(UserGroupId).info;
    var selected = {};
    selected["taskReallocationStrategy" + data.taskReallocationStrategy] = "Selected";
    data.selected = selected;
    try {
        data.parent = breadthFirstSearch(data.parentUserGroupId).info;
    } catch (e) {
        data.parent = { userGroupId:"", name: "Nessuna" };
    }

//elaboro il template mustache dentro la modale ancora nascosta
    const template = document.getElementById(Modal_Template).innerHTML;
    const rendered = Mustache.render(template, {
        data: data,
        today: new Date().toISOString().split('T')[0]
    });
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");
    GestioneCampiProfile();

}
function SpostaNodoModal_Save() {
    var Modal_Form = "SpostaNodoForm";
    var form = document.getElementById(Modal_Form);
    var bodyObj = getFormValues(form) //prendo i valori degli input del form

    var body = JSON.stringify(bodyObj);

    ShowRequiredPopup(form);//faccio comparire i popup manualmente in caso di campi non validi

    if (document.forms[Modal_Form].reportValidity()) { //controllo la validità dei campi
        $.ajax({
            url: "/Admin/Organigramma/MoveNode",
            type: "POST",
            headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
            data: body
        })
            .done(function (type) {
                ShowMessage("success", "Nodo spostato con successo.", 'Message_Target');
                MoveNodeJS({ Id: bodyObj.userGroupId, StartDate: bodyObj.startDate, EndDate: bodyObj.endDate }, bodyObj.newParentUserGroupId);
                AddEventsToNodes();
                ShowNodeDetail(breadthFirstSearch(bodyObj.userGroupId)); 
            })
            .fail(function (err) {
                ShowMessage("alert", err.responseText, 'Message_Target');
            });
    }

}


function RimuoviNodoModal(userGroupId) {

    var Modal_Template = "RimuoviNodoModal_Template";
    var Modal_Content = "RimuoviNodoModal_Content";
    var Modal_Id = "RimuoviNodoModal";

    var data = breadthFirstSearch(userGroupId).info;
    try {
        data.parent = breadthFirstSearch(data.parentUserGroupId).info;
    } catch (e) {
        data.parent = { userGroupId: "", name: "Nessuna" };
    }
   data.endDate = new Date(data.endDate).toISOString().split("T")[0];
    data.startISODate = extractIntegerFromString();
    //elaboro il template mustache dentro la modale ancora nascosta
    const template = document.getElementById(Modal_Template).innerHTML;
    const rendered = Mustache.render(template, {
        data: data,
        today: new Date().toISOString().split('T')[0]
    });
    HidePanels(true);
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");
    GestioneCampiProfile();

}
function RimuoviNodoModal_Save() {
    //mostro la modale
    var Modal_Form = "RimuoviNodoForm";
    var form = document.getElementById(Modal_Form);
    var bodyObj = getFormValues(form) //prendo i valori degli input del form

    ShowRequiredPopup(form); //faccio comparire i popup manualmente in caso di campi non validi
    if (document.forms[Modal_Form].reportValidity()) { //controllo la validità dei campi

        $.ajax({
            url: "/Admin/Organigramma/RemoveNode/" + bodyObj.userGroupId + "/" + bodyObj.startISODate + "/" + bodyObj.endDate,
            type: "DELETE",
            headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
        })
        .done(function (type) {
            ShowMessage("success", "Nodo rimosso con successo.", 'Message_Target');
            RemoveNodeJS(bodyObj.userGroupId, bodyObj.endDate);
            AddEventsToNodes();
            ShowNodeDetail(breadthFirstSearch(bodyObj.parentUserGroupId));
       })
        .fail(function (err) {
            ShowMessage("alert", err.responseText, 'Message_Target');
        });
    };
}



function DataFineUserModal(userGroupId, id) {
    var Modal_Template = "DataFineUserModal_Template";
    var Modal_Content = "DataFineUserModal_Content";
    var Modal_Id = "DataFineUserModal";

    var groupdata = breadthFirstSearch(userGroupId);

    var data = groupdata.users.find(f => f.id == id);
    data.info = groupdata.info;

    //elaboro il template mustache dentro la modale ancora nascosta
    const template = document.getElementById(Modal_Template).innerHTML;

    data.endDate = data.endDate.split('T')[0];
    var today = new Date().toISOString().split('T')[0];
    //dentro "data" ci sono i valori passati nell'onclick del template mustache
    const rendered = Mustache.render(template,
        {
            data: data,
            today: today, /* valore minimo dell'input date */
            minDate: (data.startDate.split('T')[0] > today) ? data.startDate.split('T')[0] : today
        });
    HidePanels(true);
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");
}
function DataFineUserModal_Save() {

    //mostro la modale
    var Modal_Form = "DataFineUserForm";

    var form = document.getElementById(Modal_Form);

        var bodyObj = getFormValues(form) //prendo i valori degli input del form
        var body = JSON.stringify(bodyObj);

        ShowRequiredPopup(form); //faccio comparire i popup manualmente in caso di campi non validi
        if (document.forms['DataFineUserForm'].reportValidity()) { //controllo la validità dei campi

            $.ajax({
                url: "/Admin/Organigramma/RemoveUser",
                type: "DELETE",
                headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
                data: body
            })
                .done(function (type) {
                    ShowMessage("success", "Utente rimosso con successo.", 'Message_Target');
                    DataFineUserJS(bodyObj.userGroupId, bodyObj.userId, bodyObj.endDate)
                    ShowNodeDetail(breadthFirstSearch(bodyObj.userGroupId));
                })
                .fail(function (err) {
                    ShowMessage("alert", err.responseText, 'Message_Target');
                });
        }
}

function ModificaRuoloUserModal(userGroupId, id) {

    var Modal_Template = "ModificaRuoloUserModal_Template";
    var Modal_Content = "ModificaRuoloUserModal_Content";
    var Modal_Id = "ModificaRuoloUserModal";

    var groupdata = breadthFirstSearch(userGroupId);
    var data = groupdata.users.find(f => f.id == id);

    //elaboro il template mustache dentro la modale ancora nascosta
    const template = document.getElementById(Modal_Template).innerHTML;
    //eseguo un controllo se StartDate non è vuota e in tal caso verrà usata la data attuale
    data.startDate = (data.startDate !== '') ? data.startDate.split('T')[0] : null;
    data.info = groupdata.info;

    //dentro "data" ci sono i valori passati nell'onclick del template mustache
    const rendered = Mustache.render(template, {
        data: data,
        CanEditStartDate: (new Date(data.startDate) > new Date()), //flag booleano per far apparire o meno il campo Start Date
        today: new Date().toISOString().split('T')[0],
        value: (new Date(data.startDate) ?? new Date()).toISOString().split('T')[0]
    });
    HidePanels(true);
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");


}
function ModificaRuoloUserModal_Save() {

    var Modal_Form = "ModificaRuoloUserForm";

    var form = document.getElementById(Modal_Form);
    var bodyObj = getFormValues(form) //prendo i valori degli input del form
    var body = JSON.stringify(bodyObj);

    ShowRequiredPopup(form); //faccio comparire i popup manualmente in caso di campi non validi
    if (document.forms['ModificaRuoloUserForm'].reportValidity()) { //controllo la validità dei campi

        $.ajax({
            url: "/Admin/Organigramma/ChangeRoleUser",
            type: "PUT",
            headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
            data: body
        })
            .done(function (type) {
                ShowNodeDetail(breadthFirstSearch(bodyObj.userGroupId));
                ModificaRuoloUserJS(bodyObj.userGroupId, bodyObj);
            })
            .fail(function (err) {
                ShowMessage("alert", err.responseText, 'Message_Target');
            });
    }
}

function AggiungiUserModal(userGroupId) {
    var Modal_Template = "AggiungiUserModal_Template";
    var Modal_Content = "AggiungiUserModal_Content";
    var Modal_Id = "AggiungiUserModal";

    var data = breadthFirstSearch(userGroupId);

    HidePanels(true);
    //elaboro il template mustache dentro la modale ancora nascosta
    const template = document.getElementById(Modal_Template).innerHTML;
    //dentro "data" ci sono i valori passati nell'onclick del template mustache
    const rendered = Mustache.render(template, {
        info: data.info,
        today: new Date().toISOString().split('T')[0]
    });
    document.getElementById(Modal_Content).innerHTML = rendered;
    document.getElementById(Modal_Id).classList.remove("hidden");
    GestioneCampiProfile();
}
function AggiungiUserModal_Save() {
    var Modal_Form = "AggiungiUserForm";

    var form = document.getElementById(Modal_Form);
    var bodyObj = getFormValues(form) //prendo i valori degli input del form
    if (bodyObj["endDate"] == "") bodyObj["endDate"] = null;
    var body = JSON.stringify(bodyObj);

    ShowRequiredPopup(form); //faccio comparire i popup manualmente in caso di campi non validi
    if (document.forms[Modal_Form].reportValidity()) { //controllo la validità dei campi
        $.ajax({
            url: "/Admin/Organigramma/AddUser",
            type: "PUT",
            headers: { "Content-Type": "application/json", 'accept': 'text/plain' },
            data: body
        })
        .done(function (type) {
            ShowNodeDetail(breadthFirstSearch(bodyObj.userGroupId));
            AddUserJS(bodyObj.userGroupId, bodyObj);
        })
        .fail(function (err) {
            ShowMessage("alert", err.responseText, 'Message_Target');
        });
    }

}

function FilterUsers(column, el) {
    var text = $(el).val().toLowerCase();
    var rows = $("#GroupBody").find("tbody tr");
    rows.each((i, e) => {
        var cell = e.children[column].innerHTML.toLowerCase();
        if (cell.includes(text))
            $(e).show()
        else
            $(e).hide()
    });
}


function getFormValues(form) {
    var inputs = form.querySelectorAll('input, select, custom-input');
    var formData = {};

    for (var i = 0; i < inputs.length; i++) {
        var input = inputs[i];
        var name = input.getAttribute('name');
        if (name) {
            // if (input.tagName === 'CUSTOM-INPUT')
            //     var value = input.getAttribute('value');
            //else
            var value = input.value;

            //if (name == "EndDate" && value === "") value = "2222-12-31";

            // Verifica se l'elemento è una select
            if (input.tagName === 'SELECT') {
                value = input.options[input.selectedIndex].value;
            }

            // Trasforma la prima lettera della chiave in minuscolo
            var lowercaseKey = name.charAt(0).toLowerCase() + name.slice(1);
            formData[lowercaseKey] = value;
        }
    }
    if (formData.userId) {
        formData.userName = formData.userId;
    }
    if (formData.roleId) {
        formData.roleName = formData.roleId;
    }
    if (formData.notificationStrategy) formData.notificationStrategy = parseInt(formData.notificationStrategy);
    if (formData.notificationStrategyCC) formData.notificationStrategyCC = parseInt(formData.notificationStrategyCC);
    if (formData.visible || formData.visible === "") formData.visible = (formData.visible === "true") ? true : false;
    if (formData.taskReallocationStrategy) formData.taskReallocationStrategy = parseInt(formData.taskReallocationStrategy);

    return formData;
}
function ShowRequiredPopup(formHTML) {
    //funzione per far comparire i popup manualmente in caso di campi non validi

    // Seleziona tutti gli elementi input richiesti all'interno del div temporaneo
    var requiredInputs = $(formHTML).find('input[required]:not([disabled]):visible');

    // Itera sui campi di input richiesti
    requiredInputs.each((i, inputElement) =>{
        // Verifica la validità del campo di input
        if (!inputElement.checkValidity()) {
            // Triggera l'evento "invalid" sull'elemento input non valido
            inputElement.dispatchEvent(new Event('invalid'));

            // Imposta il valore vuoto per l'elemento input
            inputElement.value = '';
        }
    });
}

function extractIntegerFromString() {
    //funzione per estrarre dalla query string (cioé window.location.search) l'integer che indica la StartISODate
    try {
        return parseInt(window.location.search.match(/=(\d+)/)[1]);
    } catch (err) { return 0; }
}

function GestioneCampiProfile() {
    // questa funzione serve a modificare il tipo di input necessario alle varie opzioni dei campi "TaskReallocationStrategy" , "NotificationStrategy"
    // a seconda del loro valore il corrispettivo campo profile varierà.

    var TaskReallocationStrategy = document.getElementById("TaskReallocationStrategy");
    if (TaskReallocationStrategy) {
        TaskReallocationStrategyBuildProfileInput(TaskReallocationStrategy);
        TaskReallocationStrategy.onchange = (e) => { TaskReallocationStrategyBuildProfileInput(TaskReallocationStrategy); };
    }

    var NotificationStrategy = document.getElementById("NotificationStrategy");
    if (NotificationStrategy) {
        NotificationStrategyBuildProfileInput(NotificationStrategy)
        NotificationStrategy.onchange = (e) => { NotificationStrategyBuildProfileInput(NotificationStrategy) };
    }
    var NotificationStrategyCC = document.getElementById("NotificationStrategyCC");
    if (NotificationStrategyCC) {
        NotificationStrategyBuildProfileCCInput(NotificationStrategyCC)
        NotificationStrategyCC.onchange = (e) => { NotificationStrategyBuildProfileCCInput(NotificationStrategyCC) };
    }

    function TaskReallocationStrategyBuildProfileInput(TaskReallocationStrategyInput) {
        if (TaskReallocationStrategyInput.value == 2)
            $("#TaskReallocationProfileGroup").removeAttr("disabled")
        else
            $("#TaskReallocationProfileGroup").attr("disabled", null)

        if (TaskReallocationStrategyInput.value == 3) 
            $("#TaskReallocationProfile").show()
        else
            $("#TaskReallocationProfile").hide();
    }


    function NotificationStrategyBuildProfileInput(NotificationStrategyInput) {
        if (NotificationStrategyInput.value == 2) {
            $("#NotificationStrategyProfile").removeAttr("disabled")
            $("#NotificationStrategyProfilePanel").show()
        }
        else { 
            $("#NotificationStrategyProfilePanel").hide();
            $("#NotificationStrategyProfile").attr("disabled", null)

        }
    }
    function NotificationStrategyBuildProfileCCInput(NotificationStrategyInputCC) {
        if (NotificationStrategyInputCC.value == 2) {
            $("#NotificationStrategyProfileCC").removeAttr("disabled")
            $("#NotificationStrategyProfileCCPanel").show()
        }
        else {
            $("#NotificationStrategyProfileCCPanel").hide();
            $("#NotificationStrategyProfileCC").attr("disabled", null)

        }
    }

}
