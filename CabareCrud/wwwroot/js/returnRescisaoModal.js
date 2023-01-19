$(document).ready(function () {
    $('.return-clienterescisao').click(function () {
        var clienteRescisao = $(this).attr('clienterescisao-id');
        console.log(clienteRescisao);
        $.ajax({
            type: 'GET',
            url: "/Contrato/RescendirContrato/" + clienteRescisao,
            success: function (result) {
                $("#cliente-rescisao").html(result);
            }
        });
    });
})