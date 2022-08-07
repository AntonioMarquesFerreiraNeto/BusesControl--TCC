//Campos do modal 
const form = document.querySelector("form#cadEdit");
const bordaInput = document.querySelector("input.form-control");
const _nome = document.querySelector("input#inputNome");
const _date = document.querySelector("input#inputDate");
const _cpf = document.querySelector("input#inputCpf");
const _rg = document.querySelector("input#inputRg");
const _email = document.querySelector("input#inputEmail");
const _tel = document.querySelector("input#inputTel");
const _nomeMae = document.querySelector("input#inputMae");
const _cep = document.querySelector("input#inputCep");
const _res = document.querySelector("input#inputRes");
const _logra = document.querySelector("input#inputLog");
const _comple = document.querySelector("input#inputComple");
const _bairro = document.querySelector("input#inputBar");
const _cid = document.querySelector("input#inputCid");
const _estado = document.querySelector("input#inputEstado");
const alert = document.getElementById('alerta');
const salvar = document.getElementById('salvar');

//Declarando e conectando icones de erros. 
const erroNome = document.querySelector("i#ErroNom");
const erroDate = document.querySelector("i#ErroDate");
const erroCpf = document.querySelector("i#ErroCpf");
const erroRg = document.querySelector("i#ErroRg");
const erroEmail = document.querySelector("i#ErroEmail");
const erroTel = document.querySelector("i#ErroTel");
const erroMae = document.querySelector("i#ErroMae");
const erroCep = document.querySelector("i#ErroCep");
const erroRes = document.querySelector("i#ErroRes");
const erroLog = document.querySelector("i#ErroLog");
const erroComp = document.querySelector("i#ErroComple");
const erroBairro = document.querySelector("i#ErroBar");
const erroCid = document.querySelector("i#ErroCid");
const erroEsta = document.querySelector("i#ErroEstado");

//Declarando e conectando icones de sucessos.
const sucesNome = document.querySelector("i#SucNom");
const sucesDate = document.querySelector("i#SucDate");
const sucesCpf = document.querySelector("i#SucCpf");
const sucesRg = document.querySelector("i#SucRg");
const sucesEmail = document.querySelector("i#SucEmail");
const sucesTel = document.querySelector("i#SucTel");
const sucesMae = document.querySelector("i#SucMae");
const sucesCep = document.querySelector("i#SucCep");
const sucesRes = document.querySelector("i#SucRes");
const sucesLog = document.querySelector("i#SucLog");
const sucesComp = document.querySelector("i#SucComple");
const sucesBairro = document.querySelector("i#SucBar");
const sucesCid = document.querySelector("i#SucCid");
const sucesEsta = document.querySelector("i#SucEstado");

// Validação para não deixar que o usuário entre com números nos campos. 
noNum(_nome);
noNum(_nomeMae);
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
    if (_nome.value == '' && _date.value == '' && _cpf.value == '' && _rg.value == '' && _nomeMae.value == ''
        && _cep.value == '' && _res.value == '' && _comple.value == '' && _logra.value == '' && _cid.value == '') {
        setErrorFor(erroNome, sucesNome, _nome, "Campo obrigatório!");
        setErrorFor(erroDate, sucesDate, _date, "Campo obrigatório!");
        setErrorFor(erroCpf, sucesCpf, _cpf, "Campo obrigatório!");
        setErrorFor(erroRg, sucesRg, _rg, "Campo obrigatório!");
        setErrorFor(erroMae, sucesMae, _nomeMae, "Campo obrigatório!");
        setErrorFor(erroCep, sucesCep, _cep, "Campo obrigatório!");
        setErrorFor(erroRes, sucesRes, _res, "Campo obrigatório!");
        setErrorFor(erroComp, sucesComp, _comple, "Campo obrigatório!");
        setErrorFor(erroLog, sucesLog, _logra, "Campo obrigatório!");
        setErrorFor(erroBairro, sucesBairro, _bairro, "Campo obrigatório!");
        setErrorFor(erroCid, sucesCid, _cid, "Campo obrigatório!");
        setErrorFor(erroEsta, sucesEsta, _estado, "Campo obrigatório!");
        alert.innerHTML = "Informe os campos obrigatórios!";
        alert.style.opacity = 1;
    }
    else {
        Nome(_nome.value);
        Data(_date.value);
        Cpf(_cpf.value);
        Rg(_rg.value);
        Tel_Email(_tel.value, _email.value);
        NomeMae(_nomeMae.value);
        Cep(_cep.value);
        Residencial(_res.value);
        Complemento(_comple.value);
        Logradouro(_logra.value);
        Bairro(_bairro.value);
        Cidade(_cid.value);
        Estado(_estado.value);
    }
}

function Nome(nome) {
    if (nome == '') {
        setErrorFor(erroNome, sucesNome, _nome, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesNome, erroNome, _nome);
    }
}
function Data(data) {
    let daata = new Date();
    let anoAtual = daata.getFullYear();
    //Lembrar de validar os dias, meses e ano da data de nascimento! Questões como ano bissexto deve ser levadas em consideração. 

    if (data == '') {
        setErrorFor(erroDate, sucesDate, _date, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesDate, erroDate, _date);
    }
}
function Cpf(cpf) {
    if (cpf == '') {
        setErrorFor(erroCpf, sucesCpf, _cpf, "Campo obrigatório!");
    }
    else if (cpf.length < 11) {
        setErrorFor(erroCpf, sucesCpf, _cpf, "Campo inválido!");
    }
    else {
        setSuccessFor(sucesCpf, erroCpf, _cpf);
    }
}
function Rg(rg) {
    if (rg == '') {
        setErrorFor(erroRg, sucesRg, _rg, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesRg, erroRg, _rg);
    }
}

function Tel_Email(tel, email) {
    if (tel == '' && email == '') {
        alert.innerHTML = "Deve ser informado pelo menos uma forma de contato!";
        alert.style.opacity = 1;
        setErrorFor(erroEmail, sucesEmail, _email, "Informe o e-mail ou telefone!");
        setErrorFor(erroTel, sucesTel, _tel, "Informe o e-mail ou telefone!");
    }
    else {
        alert.innerHTML = "";
        alert.style.opacity = 0;
        Tel(tel);
        if (email != '') {
            Email(email);
        }
    }
}
function Tel(tele) {
    if (tele.length > 0 && tele.length < 11) {
        setErrorFor(erroTel, sucesTel, _tel, "Campo inválido!");
    }
    else {
        setSuccessFor(sucesTel, erroTel, _tel);
    }
}
function Email(emai) {
    if (!CheckEmail(emai)) {
        setErrorFor(erroEmail, sucesEmail, _email, "Campo inválido!");
    }
    else {
        setSuccessFor(sucesEmail, erroEmail, _email);
    }
}
function CheckEmail(email) {
    return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(
        email
    );
}
function NomeMae(nomeMae) {
    if (nomeMae == '') {
        setErrorFor(erroMae, sucesMae, _nomeMae, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesMae, erroMae, _nomeMae);
    }
}
function Cep(cep) {
    let url = `https://viacep.com.br/ws/${cep}/json/`;
    if(cep == ''){
        setErrorFor(erroCep, sucesCep, _cep, "Campo obrigatório!");
        return
    }
    else if(cep.length < 8){
        setErrorFor(erroCep, sucesCep, _cep, "Campo inválido!");
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
        setErrorFor(erroCep, sucesCep, _cep, "Campo inválido!");
    }
    else{
        setSuccessFor(sucesCep, erroCep, _cep);
    }
}
function Residencial(res) {
    if (res == '') {
        setErrorFor(erroRes, sucesRes, _res, "Campo inválido!");
    }
    else {
        setSuccessFor(sucesRes, erroRes, _res);
    }
}
function Logradouro(logra) {
    if (logra == '') {
        setErrorFor(erroLog, sucesLog, _logra, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesLog, erroLog, _logra);
    }
}
function Complemento(complemento) {
    if (complemento == '') {
        setErrorFor(erroComp, sucesComp, _comple, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesComp, erroComp, _comple);
    }
}
function Bairro(bairro) {
    if (bairro == '') {
        setErrorFor(erroBairro, sucesBairro, _bairro, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesBairro, erroBairro, _bairro);
    }
}
function Cidade(cidade) {
    if (cidade == '') {
        setErrorFor(erroCid, sucesCid, _cid, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesCid, erroCid, _cid);
    }
}
function Estado(estado) {
    if (estado == '') {
        setErrorFor(erroEsta, sucesEsta, _estado, "Campo obrigatório!");
    }
    else {
        setSuccessFor(sucesEsta, erroEsta, _estado);
    }
}


function setSuccessFor(visible, hidden, input) {
    const formControl = input.parentElement;
    const smallError = formControl.querySelector("small");
    visible.style.opacity = 1;
    hidden.style.opacity = 0;
    input.style.border = "1px solid #3ffc3fca";
    smallError.innerHTML = '';
}
function setErrorFor(visible, hidden, input, small) {
    const formControl = input.parentElement;
    const smallError = formControl.querySelector("small");

    visible.style.opacity = 1;
    hidden.style.opacity = 0;
    input.style.border = "1px solid red";
    smallError.innerHTML = small;
}
//Devido ao fator da aplicação deixar o estado padrão de vários inputs de entrada, foi necessário criar esta função para reutilizar código, já que as ações serão as mesmas para todos os campos. 
function setNone(hidden1, hidden2, borderHidden) {
    hidden1.style.opacity = 0;
    hidden2.style.opacity = 0;
    borderHidden.style.border = "1px solid #81818192";
    const formControl = borderHidden.parentElement;
    const smallError = formControl.querySelector("small");
    smallError.innerText = '';
}
//Deixa o modal no seu estado inicial. Dessa forma, bordas, mensanges e icones voltam para o estado padrão da aplicação(Erros e sucessos).
function closse() {
    alert.innerHTML = '';
    alert.style.opacity = 0;
    setNone(erroNome, sucesNome, _nome);
    setNone(erroDate, sucesDate, _date);
    setNone(erroCpf, sucesCpf, _cpf);
    setNone(erroRg, sucesRg, _rg);
    setNone(erroEmail, sucesEmail, _email);
    setNone(erroTel, sucesTel, _tel);
    setNone(erroMae, sucesMae, _nomeMae);
    setNone(erroCep, sucesCep, _cep);
    setNone(erroRes, sucesRes, _res);
    setNone(erroComp, sucesComp, _comple);
    setNone(erroLog, sucesLog, _logra);
    setNone(erroBairro, sucesBairro, _bairro);
    setNone(erroCid, sucesCid, _cid);
    setNone(erroEsta, sucesEsta, _estado);
    setApagarValueInputs();
}

function ApiCorreio() {
    let cep = document.querySelector("input#inputCep").value;
    let url = `https://viacep.com.br/ws/${cep}/json/`;
    if(cep == ''){
        setErrorFor(erroCep, sucesCep, _cep, "Campo obrigatório!");
        return
    }
    else if(cep.length < 8){
        setErrorFor(erroCep, sucesCep, _cep, "Campo inválido!");
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
        setErrorFor(erroCep, sucesCep, _cep, "Campo inválido!");
    }
    else {
        setSuccessFor(sucesCep, erroCep, _cep);
        const logradouro = document.querySelector("input#inputLog").value = dados.logradouro;
        const bairro = document.querySelector("input#inputBar").value = dados.bairro;
        const localidade = document.querySelector("input#inputCid").value = dados.localidade;
        const estado = document.querySelector("input#inputEstado").value = dados.uf;
        const complemento = document.querySelector("input#inputComple").value = dados.complemento;
    }
}
function setApagarValueInputs() {
    let logradouro = document.querySelector("input#inputLog").value = '';
    let complemento = document.querySelector("input#inputComple").value = '';
    let bairro = document.querySelector("input#inputBar").value = '';
    let localidade = document.querySelector("input#inputCid").value = ''; 
    let _nome = document.querySelector("input#inputNome");
    let _date = document.querySelector("input#inputDate").value = '';
    let _cpf = document.querySelector("input#inputCpf").value = '';
    let _rg = document.querySelector("input#inputRg").value = '';
    let _email = document.querySelector("input#inputEmail").value = '';
    let _tel = document.querySelector("input#inputTel").value = '';
    let _nomeMae = document.querySelector("input#inputMae").value = '';
    let _cep = document.querySelector("input#inputCep").value = '';
    let _res = document.querySelector("input#inputRes").value = '';
    let _estado = document.querySelector("input#inputEstado").value = '';
    let nome = document.querySelector("input#inputNome").value = '';
}
