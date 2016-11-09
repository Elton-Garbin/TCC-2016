$(function () {
    $(".datetimepicker").datetimepicker({
        locale: 'pt-br'
    });

    $(".timepicker").datetimepicker({
        locale: 'pt-br',
        format: 'LT'
    });

    $(".datepicker").datetimepicker({
        format: 'DD/MM/YYYY',
        locale: 'pt-br'
    });

    $("ul.droptrue").sortable({
        connectWith: "ul"
    });

    $(".cpf").inputmask("999.999.999-99", { "clearIncomplete": true });
})