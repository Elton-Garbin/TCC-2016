$(function () {
    $("#btn-menu-lateral").click(function () {
        if ($("#menu-lateral").css("display") == "none") {
            $("#menu-lateral").animate({ width: 'toggle' }, 350);
        }
    });

    $("#btn-fechar-menu").click(function () {
        if ($("#menu-lateral").css("display") == "block") {
            $("#menu-lateral").animate({ width: 'toggle' }, 350);
        }
    });

    var div = $("#menu-lateral"); // seleciona a div específica
    $("body").on("click", function (e) {
        if (div.has(e.target).length || e.target == div[0] || e.target == $("#btn-menu-lateral")[0])
            return;

        if ($("#menu-lateral").css("display") == "block") {
            $("#menu-lateral").animate({ width: 'toggle' }, 350);
        }
    });

    $(".datetimepicker").datetimepicker({
        locale: 'pt-br'
    });

    $(".datepicker").datetimepicker({
        format: 'DD/MM/YYYY',
        locale: 'pt-br'
    });

    $("ul.droptrue").sortable({
        connectWith: "ul"
    });

    $("#sortable1, #sortable2, #sortable3").disableSelection();
})