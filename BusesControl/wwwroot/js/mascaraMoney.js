String.prototype.reverse = function () {
    return this.split('').reverse().join('');
};

let checkPagamentAvist = document.getElementById('a-vista');
if (checkPagamentAvist.checked) {
    document.getElementById('inputQuantiParcela').readOnly = true;
}
function mascaraMoeda(campo, evento) {
    var tecla = (!evento) ? window.event.keyCode : evento.which;
    var valor = campo.value.replace(/[^\d]+/gi, '').reverse();
    var resultado = "";
    var mascara = "##.###.###,##".reverse();
    for (var x = 0, y = 0; x < mascara.length && y < valor.length;) {
        if (mascara.charAt(x) != '#') {
            resultado += mascara.charAt(x);
            x++;
        } else {
            resultado += valor.charAt(y);
            y++;
            x++;
        }
    }
    campo.value = resultado.reverse();
}
function onReandoly() {
    document.getElementById('inputQuantiParcela').readOnly = true;
    document.querySelector('input#inputQuantiParcela').value = '';
}
function onReandolyEdit() {
    document.getElementById('inputQuantiParcela').readOnly = true;
}
function offReandoly() {
    document.getElementById('inputQuantiParcela').readOnly = false;
}