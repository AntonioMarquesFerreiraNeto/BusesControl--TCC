function OcultDisplay() {
    var senha = document.getElementById("senha");
    if (senha.type == "password") {
        senha.type = "text";
    }
    else {
        senha.type = "password"
    }

    var senhaAtual = document.getElementById("senha_atual");
    if (senhaAtual.type == "password") {
        senhaAtual.type = "text";
    }
    else {
        senhaAtual.type = "password"
    }

    var senhaConfirm = document.getElementById("confirm_senha");
    if (senhaConfirm.type == "password") {
        senhaConfirm.type = "text";
    }
    else {
        senhaConfirm.type = "password"
    }
}