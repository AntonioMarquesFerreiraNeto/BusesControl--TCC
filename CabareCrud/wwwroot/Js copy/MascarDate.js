function mascara_data(campo, valor) {
    var mydata = '';
    mydata = mydata + valor;
    if (mydata.length == 2) {
        mydata = mydata + '/';
        campo.value = mydata;
    }
    if (mydata.length == 5) {
        mydata = mydata + '/';
        campo.value = mydata;
    }
}