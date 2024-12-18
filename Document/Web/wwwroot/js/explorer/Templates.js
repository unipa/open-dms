var explorerTemplate = `
    <div class="MainPanel Background">
        <div class="filters"></div>
        <div class='ErrorMessage'></div>

        <div class="nav-toolbar-container MainToolbar">
            <ul class="nav nav-toolbar FilesToolbar hidden">

                <li id="tbDownload">
                    <a href="#" role="button" class="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false"><i class="fa fa-download"></i> Scarica <span class="fa fa-caret-down"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="#" class="btnDownload" title="Scarica i documenti selezionati nel formato originale"><i class="fa fa-download"></i> Documento in formato originale</a></li>
                        <li><a href="#" class="btnPDF"  title="Scarica i documenti selezionati in formato PDF"><i class="fa fa-file-pdf-o"></i> Anteprima PDF</a></li>
                    </ul>
                </li>

                <li id="tbShare">
                    <a href="#" role="button" class="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false" title="Condivide i documenti selezionati con altri utenti/ruoli/strutture creando una attività"><i class="fa fa-share-alt"></i> Condividi<span class="fa fa-caret-down"></span></a>
                    <ul class="dropdown-menu">
                        <li><a href="#" class="btnShare" title="Condivide i documenti selezionati con altri utenti/ruoli/strutture creando una attività"><i class="fa fa-bell"></i> Invia messaggio interno...</a></li>

                        <li id="tbMAIL"><a href="#" class="btnMail" title="Invia i documenti selezionati via Posta Elettronica"><i class="fa fa-envelope"></i> Invia per Email / PEC...</a></li>
                        <li class="separator"></li>
                        <li><a href="#" class="btnAddToFolder" title="Inserisce i documenti selezionati in uno o più fascicoli"><i class="fa fa-folder"></i> Inserisci in fascicoli...</a></li>
                    </ul>
                </li>
                <li id="tbCopy"><a href="#" class="btnCopy" title="Copia i documenti selezionati negli appunti della piattaforma"><i class="fa fa-files-o"></i> Copia</a></li>
                <li id="tbSIGN" class="hidden"><a href="#" class="btnSign" title="Avvia un processo di firma digitale sui documenti selezionati"><i class="fa fa-pencil"></i> Firma</a></li>
                <li id="tbADMIN" class="hidden"><a href="#" class="btnPermissions" title="Modifica i permessi sui documenti selezionati"><i class="fa fa-unlock"></i> Permessi</a></li>
                <li>
                    <a href="#" role="button" class="dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false"><i class="fa fa-trash-o"></i> Rimuovi<span class="fa fa-caret-down"></span></a>
                    <ul class="dropdown-menu">
                        <li id="tbUNFOLD"><a href="#" class="btnUnfold" title="Rimuove i documenti selezionati dal fascicolo senza eliminarli"><i class="fa fa-folder-open-o"></i> Rimuovi dal fascicolo...</a></li>
                        <li id="tbDELETE"><a href="#" class="btnDelete" title="Cancella i documenti selezionati"><i class="fa fa-trash-o"></i> Elimina...</a></li>
                    </ul>
                </li>
            </ul>

            <ul class="nav nav-toolbar">
                <li id="tbExport">
                    <a href="#" class="btnExport" title="Esporta i dati in formato CSV"><i class="fa fa-file-excel-o"></i> Esporta Dati</a>
                </li>
            </ul>

            <ul class="nav nav-toolbar">
                <li class='NoEmpty tbCustomize'><a href="#" class="dropdown-item" onclick="return ShowCustomize()" title="Personalizza le colonne"><i class="fa fa-wrench"></i></a></li>
                <li class='NoEmpty separator'></li>
                <li class='NoEmpty'><a href="#" class="tbListView" title="mostra i documenti come lista"><i class="fa fa-list"></i></a></li>
                <li class='NoEmpty'><a href="#" class="tbGridView" title="mostra i documenti come anteprima"><i class="fa fa-th"></i></a></li>
                <li class='NoEmpty separator'></li>

                <li><a href="#" class="tbRefresh" title="Aggiorna i risultati" title="Aggiorna l'elenco dei documenti"><i class="fa fa-refresh"></i></a></li>
            </ul>

        </div>
        <div class='explorer MainPanel'></div>
    </div>

`;


var filterTemplate = `
    <li style="position:relative">
        <a href="#" data-bs-toggle="dropdown" class="dropdown-toggle" >{{label}} <i class="fa fa-caret-down"></i></a>
        <ul class="dropdown-menu" style="max-height:250px;overflow:auto;min-width:400px;transform: traslate(0, 24px)">
            {{#Values}}
                <li  style="position:relative">
                    <a href="#" onclick="ChangeFilter('{{columnId}}','{{value}}')" style="min-height:36px">
                        <span>{{Description}} </span>
                        <span style='position:absolute;right:16px;top:4px;'>{{total}}</span>
                    </a>
                </li>
            {{/Values}}
        </ul>
    </li>
`
var rowTemplate = `
{{#rows}}
               <tr rowid="{{key}}" onclick="return ShowDocument({{key}}, {{IsFolder}})" class="Status{{rowState}}">
                    {{^HideSelection}}
                        {{#selectable}}
                            <td style="text-align:center">
                                <input type="checkbox" class="checkbox selectRow" rowid="{{key}}" />
                            </td>
                        {{/selectable}}
                        {{^selectable}}
                            <td></td>
                        {{/selectable}}
                    {{/HideSelection}}
                    {{#HideSelection}}
                            <td></td>
                    {{/HideSelection}}

                    {{#columns}}
                        <td col-id="{{id}}" style='{{AlignmentClass}}{{IsFirstClass}}'>
                            <span class='content {{AggregateClass}}' title='{{tooltip}}'>
                            {{#IsDateOrNumber}}
                                {{Prefix}} {{description}} <small>{{Suffix}}</small>
                            {{/IsDateOrNumber}}
                            {{#IsAvatar}}
                                <img class='smallavatar' src='/internalapi/identity/avatar/0{{value}}'> {{description}}
                            {{/IsAvatar}}
                            {{#IsAction}}
                                <a href='#' onclick='{{&value}}' title='{{tooltip}}'><i class='fa {{&description}}'></i></a>
                            {{/IsAction}}
                            {{#IsText}}
                                {{&description}}
                            {{/IsText}}
                            </span>
                        </td>
                    {{/columns}}
                </tr>
{{/rows}}
`

var aggregateTemplate = `
    <tr>
        <td></td>
        {{#Aggregates}}
            <td col-id="{{id}}" style='text-align:right'>
                <sub>{{tooltip}}</sub>
                <span class="aggregate">{{description}}</span>
            </td>
        {{/Aggregates}}
    </tr>
`



var previewTemplate = `
{{#rows}}
    <div class="PreviewRow" rowid="{{key}}" class="Status{{rowState}}">
        <div class="PreviewImage"  onclick="return ShowDocument({{key}}, {{IsFolder}})" >
            <image-viewer id="image-viewer" type="compatto" documentId="{{key}}" imageId=0 pages="1">
            </image-viewer>
            <div style="position:absolute;bottom:0px;right:12px;font-size:2rem;line-height:normal">
                {{&icon}}
            </div>
        </div>
        <div class="PreviewName">
            {{title}}
            <small>{{docType}}</small>
            <strong>{{protocol}}</strong>
        </div>
        {{^HideSelection}}
            {{#selectable}}
                <div rowid="{{key}}" style="position:absolute;top:0px;left:12px;font-size:2rem;line-height:normal" class="selectRow">
                    <input type="checkbox" class="checkbox" />
                </div>
            {{/selectable}}
        {{/HideSelection}}
    </div>
{{/rows}}
`


var listTemplate = `
        <div class="MainPanel Files InfinityScroll">
            <table>
                <thead>
                    <tr>
                        {{^HideSelection}}
                                <th style="min-width:36px;text-align:center">
                                    <input type="checkbox" class="checkbox selectAll" />
                                </th>
                        {{/HideSelection}}
                        {{#HideSelection}}
                                <th></th>
                        {{/HideSelection}}

                        {{#Columns}}
                            <th col-id="{{id}}" style='{{Width}}{{AlignmentClass}}'>

                            {{#isSortable}}
                                <a href="#" onclick="return SortColumn('{{View}}', '{{id}}')">
                                    <span>{{&settings.title}}</span>
                                    <span class="{{#SortingTypeAscending}}fa fa-sort-up{{/SortingTypeAscending}}{{#SortingTypeDescending}}fa fa-sort-down{{/SortingTypeDescending}}"></span>
                                </a>
                            {{/isSortable}}
                            {{^isSortable}}
                                <span>{{&settings.title}}</span>
                            {{/isSortable}}
                            </th>
                        {{/Columns}}
                    </tr>
                </thead>
                <tbody class='rows'>
                </tbody>
                <thead class='ProgressIndicator hidden'>
                    <caption style="text-align:center">
                        <i class='fa fa-spin fa-refresh fa-2x'></i> Caricamento in corso...
                    </caption>
                </thead>
                <tfoot>

                </tfoot>
            </table>
        </div>
        <div class="Bottom">
            <div class="nav-toolbar-container w-100">
                <ul class="nav nav-toolbar">
                    <li class="SelectedDocs hidden">
                        <b><span class="Selected"></span></b> / <span> {{Records }}</span>
                        <span>Selezionati </span>
                    </li>
                    <li class="UnselectedDocs">
                        <b><span>{{Records}}</span></b>
                        <span> documenti trovati </span>
                    </li>
                    <li>
                        <i class="fa fa-image" style="margin-left:16px"></i><b><span class='renderTime'></span></b>
                        <span>ms</span>
                    </li>
                    <li>
                        <i class="fa fa-database" style="margin-left:16px"></i> <b><span class='queryTime'></span></b>
                        <span>ms</span>
                    </li>
                </ul>
            </div>
        </div>
`

var gridTemplate = `
{{^HideSelection}}
<div class="Content">
    <input type="checkbox" class="checkbox selectAll" /><a href="#" onclick="$(this).prev().click()">Seleziona Tutti/Nessuno</a>
</div>
{{/HideSelection}}

        <div class="PreviewContainer MainPanel InfinityScroll rows">
        </div>
        <div class="Bottom">
            <div class="nav-toolbar-container w-100">
                <ul class="nav nav-toolbar">
                    <li class="SelectedDocs hidden">
                        <b><span class="Selected"></span></b> / <span> {{Records }}</span>
                        <span>Selezionati </span>
                    </li>
                    <li class="UnselectedDocs">
                        <b><span>{{Records}}</span></b>
                        <span> documenti trovati </span>
                    </li>
                    <li>
                        <i class="fa fa-image" style="margin-left:16px"></i><b><span class='renderTime'></span></b>
                        <span>ms</span>
                    </li>
                    <li>
                        <i class="fa fa-database" style="margin-left:16px"></i> <b><span class='queryTime'></span></b>
                        <span>ms</span>
                    </li>
                </ul>
            </div>
        </div>
`

var emptyTemplate = `
        <div class="InfoPanel">
            <i class="fa fa-info-circle"></i>
            <h3> Nessun documento trovato</h3>
        </div>
`
