
//Campos do modal que serão validados no Js.
const _nome = document.querySelector("input#inputNome");
const _nomeMae = document.querySelector("input#inputMae");
const _cidade = document.getElementById('inputCid');
const _estado = document.getElementById('inputEstado');
const _cep = document.querySelector("input#inputCep");
const alert = document.getElementById('alerta');
const salvar = document.getElementById('salvar');


// Validação para não deixar que o usuário entre com números nos campos. 
noNum(_nome);
noNum(_nomeMae);
noNum(_cidade);
noNum(_estado);
function noNum(no) {
    no.addEventListener("keypress", function (e) {
        const keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode > 47 && keycode < 58) {
            e.preventDefault();
        }
    });
}

//validações dos campos do modal
function salva() {
        Tel_Email(_tel.value, _email.value)
        Cep(_cep.value);
}
function Cep(cep) {
    let url = `https://viacep.com.br/ws/${cep}/json/`;
    if(cep == ''){
        setErrorFor(_cep, "Campo obrigatório!");
        return
    }
    else if(cep.length < 8){
        setErrorFor(_cep, "Campo inválido!");
        return;
    }
    fetch(url).then(function (response) {
        response.json().then(function (data) {
            validarCepAPI(data);
        });
    });
}
function validarCepAPI(data){
    if(data.erro){
        setErrorFor(_cep, "Campo inválido!");
    }
    else{
        setSuccessFor(_cep);
    }
}

function setSuccessFor(input) {
    const formControl = input.parentElement;
    const smallError = formControl.querySelector("small");
    smallError.innerHTML = '';
}
function setErrorFor(input, small) {
    const formControl = input.parentElement;
    const smallError = formControl.querySelector("small");
    smallError.innerHTML = small;
}


function ApiCorreio() {
    let cep = document.querySelector("input#inputCep").value;
    let url = `https://viacep.com.br/ws/${cep}/json/`;
    if(cep == ''){
        setErrorFor(_cep, "Campo obrigatório!");
        return
    }
    else if(cep.length < 8){
        setErrorFor(_cep, "Campo inválido!");
        return;
    }
    fetch(url).then(function (response) {
        response.json().then(function (data) {
            mostrarEndereço(data);
        });
    });
}
function mostrarEndereço(dados) {
    let cep = _cep.value
    if (dados.erro) {
        setErrorFor(_cep, "Campo inválido!");
    }
    else {
        setSuccessFor(_cep);
        const logradouro = document.querySelector("input#inputLog").value = dados.logradouro;
        const bairro = document.querySelector("input#inputBar").value = dados.bairro;
        const localidade = document.querySelector("input#inputCid").value = dados.localidade;
        const estado = document.querySelector("input#inputEstado").value = dados.uf;
        const complemento = document.querySelector("input#inputComple").value = dados.complemento;
        const ddd = document.querySelector("input#inputDdd").value = dados.ddd;
    }
}
function setApagarValueInputs() {
    let logradouro = document.querySelector("input#inputLog").value = '';
    let complemento = document.querySelector("input#inputComple").value = '';
    let bairro = document.querySelector("input#inputBar").value = '';
    let localidade = document.querySelector("input#inputCid").value = ''; 
    let _email = document.querySelector("input#inputEmail").value = '';
    let _tel = document.querySelector("input#inputTel").value = '';
    //let _cep = document.querySelector("input#inputCep").value = '';
}