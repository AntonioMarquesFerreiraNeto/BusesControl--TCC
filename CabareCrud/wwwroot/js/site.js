$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

$(document).ready(function () {
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
    $('.input-id-cliente').click(function () {
        var id = document.getElementById('ClienteFisicoList');
        let id_value = id.value;
        if (id_value == '') {
            console.log(111);
            //Não executa nada.
        } else {
            $('#clientes-selects').append("<tr><td>" + $("#ClienteFisicoList option:selected").text() + "<input type='checkbox' name='chkClient' id='chkClient' class='chkClient' checked='checked' value='" + $("#ClienteFisicoList option:selected").val() + "'><td></td></td><td><a class='link-trash' href='#'><i class='fa fa-trash-alt'></i></a></td><td></td></tr>");
            var display_btn = document.getElementById("display");
            display_btn.style.opacity = 1;
        }
    });
})