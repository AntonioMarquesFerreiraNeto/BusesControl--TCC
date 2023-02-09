$('.close-alert').click(function () {
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
    $.ajax({
        type: 'GET',
        url: "/Financeiro/ReturnDashFinanceiro",
        success: function (result) {
            $("#show-dash").html(result);
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
            url: "/Contrato/ClientesContratoPdf/" + contratoIdPdf,
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
    $('.btn-view-client2').click(function () {
        var contratoId = $(this).attr('contrato-id');
        $.ajax({
            type: 'GET',
            url: "/Relatorio/ClientesContrato/" + contratoId,
            success: function (result) {
                $("#list-clients").html(result);
            }
        });
    });
    $('.return-responsavel-2').click(function () {
        console.log(100);
        var clienteResponsavelId = $(this).attr('clienteresponsavel-id');
        $.ajax({
            type: 'GET',
            url: "/Financeiro/ReturnClienteResponsavel/" + clienteResponsavelId,
            success: function (result) {
                $("#cliente-responsavel2").html(result);
            }
        });
    });
    $('.return-inativarlancamento').click(function () {
        var financeiroId = $(this).attr('financeiro-id');
        console.log(financeiroId);
        $.ajax({
            type: 'GET',
            url: "/Financeiro/InativarLancamento/" + financeiroId,
            success: function (result) {
                $("#contrato-inativar").html(result);
            }
        });
    });
    $('.return-modal-aprovacao').click(function () {
        var idContrato = $(this).attr('contrato-id');
        console.log(idContrato);
        $.ajax({
            type: 'GET',
            url: "/Contrato/ReturnAprovacaoContrato/" + idContrato,
            success: function (result) {
                $("#show-aprovacao").html(result);
            }
        });
    });
    $('.clear-list').click(function () {
        $.ajax({
            type: 'GET',
            url: "/Contrato/ClearList"
        });
    })
    $('.clear-filtros').click(function () {
        $.ajax({
            type: 'POST',
            url: "/Financeiro/ClearFiltros" 
        });
    });
})