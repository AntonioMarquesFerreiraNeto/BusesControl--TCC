function filtros() {
    //declarando e estabelecendo conexão com as tags do HTML.
    const form = document.getElementById("teste");
    const nome = document.getElementById("FinputNome");
    const cpf = document.getElementById("FinputCPF");
    const tel = document.getElementById("FinputTel");

    // Para não recarregar a página no momento que clicar no botão filtro.
    form.addEventListener("submit", (e) => {
        e.preventDefault();

        CheckInputs();
    });

    // Validação para não deixar que o usuário entre com números nos campos. 
    noNum(nome);
    function noNum(no) {
        no.addEventListener("keypress", function (e) {
            const keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode > 47 && keycode < 58) {
                e.preventDefault();
            }
        });
    }

    function CheckInputs() {
        const ClientNameValue = nome.value;
        const ClientCpfNameValue = cpf.value;
        const ClientTelValue = tel.value;
        if (ClientNameValue == '' && ClientCpfNameValue == '' && ClientTelValue == '') {
            window.alert("\nDeve ser informado pelo menos um campo para realizar a filtragem.");
            IconAlert(1);
        }
        else {
            IconAlert(0);
            Nome(ClientNameValue);
            Cpf(ClientCpfNameValue);
            Tel(ClientTelValue);
        }
    }

    //Validações dos campos do filtro.
    function Nome(entre) {
        const error = document.querySelector('i#nomeIError');
        const sucess = document.querySelector('i#nomeISuc');
        if (entre.length == '') {
            DefaultBorder(nome);
        }
        else if (entre.length > 0 && entre.length < 5) {
            setErrorFor(nome, "Campo inválido!", error, sucess);
        }
        else {
            setSuccess(nome, '', sucess, error);
        }
    }
    function Cpf(entre) {
        const error = document.querySelector('i#cpfIError');
        const sucess = document.querySelector('i#cpfISuc');
        if (entre.length == '') {
            DefaultBorder(cpf);
        }
        else if (entre.length > 0 && entre.length < 11) {
            setErrorFor(cpf, "Campo inválido!", error, sucess);
        }
        else {
            setSuccess(cpf, '', sucess, error);
        }
    }

    function Tel(entre) {
        const error = document.querySelector('i#emailIError');
        const sucess = document.querySelector('i#emailISuc');
        if (entre == '') {
            DefaultBorder(tel);
        }
        else if (entre.length < 8 || entre.length > 9) {
            setErrorFor(tel, "Campo inválido!", error, sucess);
        }
        else {
            setSuccess(tel, '', sucess, error);
        }
    }
    function setErrorFor(input, mensagem, visible, hidden) {
        const formControl = input.parentElement;
        const small = formControl.querySelector("small");

        small.innerHTML = mensagem;
        input.style.border = '1px solid red';
        visible.style.opacity = 1;
        hidden.style.opacity = 0;
    }
    function setSuccess(input, mensagem, visible, hidden) {
        const formControl = input.parentElement;
        const small = formControl.querySelector("small");
        small.innerHTML = mensagem;
        input.style.border = "1px solid #3ffc3fca";
        visible.style.opacity = 1;
        hidden.style.opacity = 0;
    }

    function IconAlert(opacity) {
        const eNome = document.querySelector('i#nomeIError');
        const eCpf = document.querySelector('i#cpfIError');
        const eEmail = document.querySelector('i#emailIError');

        eNome.style.opacity = opacity;
        eCpf.style.opacity = opacity;
        eEmail.style.opacity = opacity;
    }
    function DefaultBorder(borda) {
        borda.style.border = "1px solid #81818192";
    }
}