$(document).ready(function () {
    $('.remove-ajax').click(function () {
        var clienteId = $(this).attr('cliente-id');
        $.ajax({
            type: 'GET',
            url: "/Contrato/RemoveSelect/" + clienteId,
            success: function (result) {
                $("#clientes-selects").html(result);
            }
        });
    });
})
