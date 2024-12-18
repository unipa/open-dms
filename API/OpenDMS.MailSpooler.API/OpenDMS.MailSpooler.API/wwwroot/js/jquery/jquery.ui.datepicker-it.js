/* Italian initialisation for the jQuery UI date picker plugin. */
/* Written by Antonello Pasella (antonello.pasella@gmail.com). */
(function ($) {
    $.datepicker.regional['it'] = {
        renderer: $.datepicker.defaultRenderer,
        monthNames: ['Gennaio', 'Febbraio', 'Marzo', 'Aprile', 'Maggio', 'Giugno',
                'Luglio', 'Agosto', 'Settembre', 'Ottobre', 'Novembre', 'Dicembre'],
        monthNamesShort: ['Gen', 'Feb', 'Mar', 'Apr', 'Mag', 'Giu',
                'Lug', 'Ago', 'Set', 'Ott', 'Nov', 'Dic'],
        dayNames: ['Domenica', 'Luned&#236', 'Marted&#236', 'Mercoled&#236', 'Gioved&#236', 'Venerd&#236', 'Sabato'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mer', 'Gio', 'Ven', 'Sab'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Me', 'Gi', 'Ve', 'Sa'],
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        prevText: '&#x3c;Prec', prevStatus: '',
        prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: '',
        nextText: 'Succ&#x3e;', nextStatus: '',
        nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: '',
        currentText: 'Oggi', currentStatus: '',
        todayText: 'Oggi', todayStatus: '',
        clearText: '-', clearStatus: '',
        closeText: 'Chiudi', closeStatus: '',
        yearStatus: '', monthStatus: '',
        weekText: 'Sm', weekStatus: '',
        dayStatus: 'DD d MM',
        defaultStatus: '',
        isRTL: false
    };
    $.extend($.datepicker.setDefaults($.datepicker.regional['it']));
})(jQuery);