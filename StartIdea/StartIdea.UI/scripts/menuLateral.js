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
    })
})