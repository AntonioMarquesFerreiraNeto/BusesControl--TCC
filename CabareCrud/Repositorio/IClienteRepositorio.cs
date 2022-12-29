using BusesControl.Models;
using System;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IClienteRepositorio  {
        List<PessoaFisica> ListClienteFisicoLegal();
        public List<PessoaFisica> ListClienteFisicoLegalContrato();
        List<PessoaJuridica> ListClienteJuridicoLegal();   
        List<PessoaFisica> BuscarTodosHabilitados();
        List<PessoaJuridica> BuscarTodosHabJuridico();
        List<PessoaFisica> BuscarTodosDesabilitados();
        List<PessoaJuridica> BuscarTodosDesaJuridico();
        PessoaFisica Adicionar(PessoaFisica cliente);
        PessoaJuridica AdicionarJ(PessoaJuridica cliente);
        PessoaFisica ListarPorId(long id);
        PessoaJuridica ListarPorIdJuridico(long id);
        PessoaFisica Editar(PessoaFisica cliente);
        PessoaJuridica EditarJurico(PessoaJuridica cliente);
        PessoaFisica Desabilitar(PessoaFisica cliente);
        PessoaJuridica DesabilitarJuridico(PessoaJuridica cliente);
        PessoaFisica Habilitar(PessoaFisica cliente);
        PessoaJuridica HabilitarJuridico(PessoaJuridica cliente);
        Exception TratarErro(PessoaFisica cliente, Exception erro);
        Exception TratarErroJ(PessoaJuridica cliente, Exception erro);
        public PessoaJuridica TrimPessoaJuridica(PessoaJuridica value);
        public PessoaFisica TrimPessoaFisica(PessoaFisica value);
        public bool PessoaFisicaOrJuridica(int id);
    }
}
