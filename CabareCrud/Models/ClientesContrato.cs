﻿namespace BusesControl.Models {
    public class ClientesContrato {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int? PessoaJuridicaId { get; set; }
        public int? PessoaFisicaId { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual Contrato Contrato { get; set; }
        //Pensar na possibilidade de adicionar o motorista e onibus aqui. 
    }
}
