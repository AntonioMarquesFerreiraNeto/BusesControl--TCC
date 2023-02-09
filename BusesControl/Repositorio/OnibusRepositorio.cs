using BusesControl.Data;
using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Repositorio {
    public class OnibusRepositorio : IOnibusRepositorio {
        private readonly BancoContext _bancoContext;
        public OnibusRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }
        public Onibus AdicionarBus(Onibus onibus) {
            try {
                if (Duplicata(onibus)) {
                    throw new Exception("Ônibus já se encontra cadastrado!");
                }
                //Chamando o método "TrimOnibus" para retirar os espaços vázios do objeto (antes e depois do mesmo).
                onibus = TrimOnibus(onibus);
                _bancoContext.Onibus.Add(onibus);
                _bancoContext.SaveChanges();
                return onibus;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public List<Onibus> ListarTodosHab() {
            var list = _bancoContext.Onibus.ToList();
            return list.Where(x => x.StatusOnibus == OnibusStatus.Habilitado).ToList();
        }
        public List<Onibus> ListarTodosDesa() {
            var list = _bancoContext.Onibus.ToList();
            return list.Where(x => x.StatusOnibus == OnibusStatus.Desabilitado).ToList();
        }

        public Onibus ListarPorId(long id) {
            return _bancoContext.Onibus.AsNoTracking().Include("Contratos").FirstOrDefault(x => x.Id == id);
        }
        public Onibus EditarOnibus(Onibus onibus) {
            try {
                Onibus onibusDB = ListarPorId(onibus.Id);
                if (DuplicataEditar(onibus, onibusDB)) {
                    throw new Exception("Ônibus já se encontra cadastrado!");
                }
                if (onibusDB == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
                onibusDB.NameBus = onibus.NameBus.Trim();
                onibusDB.Marca = onibus.Marca.Trim();
                onibusDB.DataFabricacao = onibus.DataFabricacao.Trim();
                onibusDB.Placa = onibus.Placa.Trim();
                onibusDB.Renavam = onibus.Renavam.Trim();
                onibusDB.Assentos = onibus.Assentos.Trim();
                onibusDB.Chassi = onibus.Chassi.Trim();
                onibusDB.corBus = onibus.corBus;
                _bancoContext.Update(onibusDB);
                _bancoContext.SaveChanges();
                return onibus;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Onibus Desabilitar(Onibus onibus) {
            Onibus onibusDesabilitar = ListarPorId(onibus.Id);
            if (onibusDesabilitar == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            if (onibusDesabilitar.Contratos.Any(x => x.StatusContrato == ContratoStatus.Ativo && x.Aprovacao != StatusAprovacao.Negado)) {
                throw new Exception("Ônibus vinculado em contrato em andamento!");
            }
            onibusDesabilitar.StatusOnibus = OnibusStatus.Desabilitado;
            _bancoContext.Update(onibusDesabilitar);
            _bancoContext.SaveChanges();
            return onibusDesabilitar;
        }
        public Onibus Habilitar(Onibus onibus) {
            Onibus onibusHabilitar = ListarPorId(onibus.Id);
            if (onibusHabilitar == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            onibusHabilitar.StatusOnibus = OnibusStatus.Habilitado;
            _bancoContext.Update(onibusHabilitar);
            _bancoContext.SaveChanges();
            return onibus;
        }

        public Onibus TrimOnibus(Onibus value) {
            value.NameBus = value.NameBus.Trim();
            value.Marca = value.Marca.Trim();
            value.DataFabricacao = value.DataFabricacao.Trim();
            value.Placa = value.Placa.Trim();
            value.Renavam = value.Renavam.Trim();
            value.Assentos = value.Assentos.Trim();
            value.Chassi = value.Chassi.Trim();
            return value;
        }

        public bool Duplicata(Onibus onibus) {
            if (_bancoContext.Onibus.Any(x => x.Placa == onibus.Placa || x.Renavam == onibus.Renavam || x.Chassi == onibus.Chassi)) {
                return true;
            }
            return false;
        }
        public bool DuplicataEditar(Onibus onibus, Onibus onibusBD) {
            if (_bancoContext.Onibus.Any(x => (x.Placa == onibus.Placa && onibus.Placa != onibusBD.Placa)
               || (x.Renavam == onibus.Renavam && onibus.Renavam != onibusBD.Renavam)
               || (x.Chassi == onibus.Chassi && onibus.Chassi != onibusBD.Chassi))) {
                return true;
            }
            return false;
        }
    }
}
