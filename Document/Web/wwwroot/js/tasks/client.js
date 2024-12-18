import { GetModel, RenderForm, Save } from "../forms/client.js";

// TASKS
var task = undefined;

export function LoadTask(id, container, template)
{
    task = undefined;
    if (!container)
        container = "#Task";
    if (!template)
        template = "#Item";
    $(".loader").hide();

    var $container = $(container);
    $container.attr("task_id", id);

    fetch("/internalapi/tasklist/" + id)
        .then(response => {
            return response.json();
        })
        .then(data =>
        {
            task = data;
            task.taskTypeName = (task.taskItemInfo.taskType == 2 || task.taskItemInfo.taskType == 1)
                ? "Attivita'"
                : task.taskItemInfo.taskType == 0
                    ? "Messaggio"
                    : task.taskItemInfo.taskType == 3
                        ? "Avviso"
                        : "Errore"
            if (task.isCC) task.taskTypeName += " in CC";
            task.taskIcon = (task.taskItemInfo.taskType == 2 || task.taskItemInfo.taskType == 1)
                ? "fa fa-list"
                : task.taskItemInfo.taskType == 0
                    ? "fa fa-comment"
                    : task.taskItemInfo.taskType == 3
                        ? "fa fa-warning"
                        : "fa fa-bug";
            task.taskStatusName = task.status < 250 ? "Da Gestire" : "Gestita";
            task.expirationDate = task.expirationDate ? new Date(Date.parse(task.expirationDate)).toLocaleDateString() : "nessuna scadenza";
            task.creationDate = new Date(Date.parse(task.creationDate)).toLocaleDateString() + " " + new Date(Date.parse(task.creationDate)).toLocaleTimeString()
            task.Executable = (task.received) && (task.status == 1 || task.status == 0);
            task.archivia = (task.taskItemInfo.taskType == 1 || task.taskItemInfo.taskType == 2) ? "Completa" : "Archivia"
            task.Claimed = (task.status == 1)
            task.Claimable = (task.received) && ((task.status == 0) && (task.taskItemInfo.taskType == 1 || task.taskItemInfo.taskType == 2));
            task.Activity = (task.taskItemInfo.taskType == 1 || task.taskItemInfo.taskType == 2);
            task.Releasable = (task.received) && ((task.status == 1) && (task.taskItemInfo.taskType == 1 || task.taskItemInfo.taskType == 2));
            task.MinPercentage = 0;
            task.taskItemInfo.progress.forEach(p => {
                if (p.percentage > task.MinPercentage) task.MinPercentage = p.percentage;
                p.creationDate = new Date(Date.parse(p.creationDate)).toLocaleDateString() + " " + new Date(Date.parse(p.creationDate)).toLocaleTimeString();
            });
            task.activeSubTasks = task.subTasks.reduce((n, val) => { return n + (val.status < 250) }, 0);
            task.subTasks.forEach(p => {
                p.status = p.status < 250 ? "yellow" : "limegreen";
                p.creationDate = new Date(Date.parse(p.creationDate)).toLocaleDateString() + " " + new Date(Date.parse(p.creationDate)).toLocaleTimeString();
                p.expirationDate = p.expirationDate ? new Date(Date.parse(p.expirationDate)).toLocaleDateString() : "nessuna scadenza";
                p.taskItemInfo.progress.forEach(pp => {
                    pp.creationDate = new Date(Date.parse(pp.creationDate)).toLocaleDateString() + " " + new Date(Date.parse(pp.creationDate)).toLocaleTimeString();
                });

            });
            //task.container = container;
            //task.form = task.taskItemInfo.form;
            //task.taskItemInfo.form = undefined;
            //task.form.template = template;
            //task.form.renderer = () => {
            //    if (task.status >= 250 || !task.received) {
            //        DisableFormComponents();
            //    }
            //};
            //RenderForm(task.form, task.container, task);

            var form = task.taskItemInfo.form;
            form.template = template;
            form.renderer = () => {
                if (task.status >= 250 || !task.received) {
                    DisableFormComponents();
                }
            };

            task.container = container;
            //task.taskItemInfo.form = undefined;
            RenderForm(form, container, task);
   
        });
}

export function Claim() {
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
    }
    fetch("/internalapi/tasklist/Claim/" + task.id,
        {
            method: "PATCH",
            headers: {
                RequestVerificationToken: VerificationCode,
                'Content-Type': 'application/json;',
                Accept: 'application/json;charset=ISO-8859-1'
            }
        })
        .then(response => {
            if (response.ok)
                return response.json();
            else
                return response.text().then(text => { throw new Error(text) });
        })
        .then(data => {
            task = data;
            if (window.parent) {
                window.parent.postMessage({ op: "refresh", data: "" }, "*");

            //    var $tlist = $(window.parent.document).find("#TaskList");
            //    if ($tlist.length > 0) {
            //        var $li = $tlist.find("[tid=" + task.id+"]");
            //        if ($li.length > 0)
            //            $li.remove();
            //    }
            }
            //if (window.parent["updateFilters"])
            //    window.parent.updateFilters(-2);

            LoadTask(task.id);
        })
        .catch(err => {
            alert(err);
        });
}
export function Release() {
    var s = prompt("Puoi indicare una motivazione ?", "Ho preso in carico per errore");
    if (!s) return;
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
    }

    fetch("/internalapi/tasklist/Release/" + task.id + "/" + s,
        {
            method: "PATCH",
            headers: {
                RequestVerificationToken: VerificationCode,
                'Content-Type': 'application/json;',
                Accept: 'application/json;charset=ISO-8859-1'
            }
        })
        .then(response => {
            return response.json();
        })
        .then(data => {
            task = data;
            //if (window.parent["updateFilters"])
            //    window.parent.updateFilters();
            //if (window.parent) {
                //var $tlist = $(window.parent.document).find("#TaskList");
                //if ($tlist.length > 0) {
                //    var $li = $tlist.find("[tid=" + task.id+"]");
                //    if ($li.length > 0)
                //        $li.remove();
                //}
                CloseWindow(true);
            //}
            //else LoadTask(task.id);
        })
        .catch(err => {
            alert(err);
        });

}
export function Reassign() {
    let pik = qs["pik"];
    if (pik == undefined || pik == "undefined") pik = "";
    window.location.href = "/tasks/Reassign?tid=" + task.id + "&pik=" + pik;
}


export function AddProgress(suspend) {
    var VerificationCode = "";
    var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
    if (RequestVerificationCode.length > 0) {
        VerificationCode = RequestVerificationCode[0].value;
        var Message = $("#notaProgresso").val();
        if (suspend) Message = "Salvataggio Progressi";
        var Percentage = $("#percentualeProgresso").val();
        var data = GetModel("name", task.taskItemInfo.form.data);
        var varstr = JSON.stringify( { percentage: Percentage, message: Message, variables: JSON.stringify(data) } );
        fetch("/internalapi/tasklist/Progress/" + task.id, // + "/" + escape(message) + "/" + escape(percentage),  //qs["tid"] + "/" + escape(message) + "/" + escape(percentage),
            {
                method: "POST",
                headers: {
                    RequestVerificationToken: VerificationCode,
                    'Content-Type': 'application/json;',
                    Accept: 'application/json;charset=ISO-8859-1'
                },
                body: varstr
            })
            .then(response => {
                return response.json();
            })
            .then(data => {
                if (!suspend) {
                    LoadTask(task.id, "#Task", "#Item"); //qs["tid"], "#Task", "#Item");
                }
                else {
                    CloseWindow();
                    }

                
            })
            .catch(err => {
                alert(err);
            });
    }

}
export function Execute() {
    return Save((formdata,abort) => {
        var pik = qs["pik"];
        var VerificationCode = "";
        var RequestVerificationCode = document.getElementsByName("__RequestVerificationToken");
        if (RequestVerificationCode.length > 0) {
            VerificationCode = RequestVerificationCode[0].value;
            var model = formdata.Data;
            model.attachments = formdata.Attachments;
            fetch("/internalapi/tasklist/Execute/" + task.id, // qs["tid"],
                {
                    method: "PATCH",
                    headers: {
                        RequestVerificationToken: VerificationCode,
                        'Content-Type': 'application/json;',
                        Accept: 'application/json;charset=ISO-8859-1'
                    },
                    body: JSON.stringify(JSON.stringify(model))
                })
                .then(response => {
                    if (response.ok)
                        return response.json();
                    else
                        return response.text().then(text => { throw new Error(text) });
                })
                .then(data => {
                    {
                        task = data;
                        if (window.parent) {
                            window.parent.postMessage({ op: "removeTask", data: data }, "*");
                        }
                        //if (window.parent["updateFilters"])
                        //    window.parent.updateFilters();
                        if (pik)
                            checkNextProcessTask(pik);
                        else {
                            if (window.parent) {
                                window.parent.postMessage({ op: "getNextTask", data: data }, "*");
                            }
                            else
                                CloseWindow(true);
                        }
                    }
                })
                .catch(err => {
                    if (abort) abort();
                    alert(err);
                });
        }
    });
}
export function checkNextProcessTask(pik) {
    //$("#Task").hide();
    window.location.href = "/PostSave?Id=" + pik;
//    $("#WaitForNewTask").show();
//    var timer = window.setTimeout(() => {
//        /* Verifica se ci sono task utente */
//        fetch("/internalapi/wf/getProcessTasks/" + pik)
//            .then(response => { return response.json(); })
//            .then(data => {
//                /*
//                 * dovrebbe ritornare l'id del processo da monitorare
//                 * In assenza di Id l'azione potrebbe avere inviato un messaggio
//                 * Se la proprietà Error non è vuota, viene mostrato l'errore.
//                 */
//                if (data.length) {
//                    /* Mostro il task */
//                    LoadTask(data[0].id);
//                } else
//                    checkNextProcessTask(pik);
//            }).
//            catch(err => {
//                checkNextProcessTask(pik);
//                console.debug("Si è verificato un errore: " + err);
//            })
//    }, 1000)
}
