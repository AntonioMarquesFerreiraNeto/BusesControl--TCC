﻿$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: "/Contrato/ReturnList",
        success: function (result) {
            $("#clientes-selects").html(result);
        }
    });
    $('.btn-view-client').click(function () {
        var contratoId = $(this).attr('contrato-id');
        $.ajax({
            type: 'GET',
            url: "/Contrato/ListClientesContrato/" + contratoId,
            success: function (result) {
                $("#list-clients").html(result);
            }
        });
    });
    $('.btn-view-client-pdf').click(function () {
        var contratoIdPdf = $(this).attr('contrato-id-pdf');
        $.ajax({
            type: 'GET',
            url: "/AprovarContrato/ClientesContratoPdf/" + contratoIdPdf,
            success: function (result) {
                $("#list-clients-pdf").html(result);
            }
        });
    });
    $('.input-id-cliente').click(function () {
        var id = document.getElementById('ClienteFisicoList');
        let id_value = id.value;
        if (id_value == '') {
            //Não executa nada.
        } else {
            $.ajax({
                type: 'POST',
                url: "/Contrato/AddSelect/" + id_value,
                success: function (result) {
                    $("#clientes-selects").html(result);
                }
            });
        }
    });
    $('.clear-list').click(function () {
        $.ajax({
            type: 'GET',
            url: "/Contrato/ClearList"
        });
    })
})