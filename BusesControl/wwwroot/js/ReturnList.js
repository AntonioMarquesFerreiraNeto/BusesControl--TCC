$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: "/Frota/ListFrotaAtiva",
        success: function (result) {
            $("#list-frota").html(result);
        }
    })
    $('.list-ativos-frota').click(function () {
        $.ajax({
            type: 'GET',
            url: "/Frota/ListFrotaAtiva",
            success: function (result) {
                $("#list-frota").html(result);
            }
        })
    });
    $('.list-inativo-frota').click(function () {
        $.ajax({
            type: 'GET',
            url: "/Frota/ListFrotaInativa",
            success: function (result) {
                $("#list-frota").html(result);
            }
        })
    });
})