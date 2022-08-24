using BusesControl.Data;
using BusesControl.Models;
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
                _bancoContext.Onibus.Add(onibus);
                _bancoContext.SaveChanges();
                return onibus;
            }
            catch (Exception erro) {
                TratarErro(onibus, erro);
                return null;
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
            Onibus onibus = _bancoContext.Onibus.FirstOrDefault(x => x.Id == id);
            return onibus;
        }

        public Onibus Desabilitar(Onibus onibus) {
            Onibus onibusDesabilitar = ListarPorId(onibus.Id);
            if (onibusDesabilitar == null) throw new System.Exception("Desculpe, houve um erro ao desabilitar.");
            onibusDesabilitar.StatusOnibus = OnibusStatus.Desabilitado;
            _bancoContext.Update(onibusDesabilitar);
            _bancoContext.SaveChanges();
            return onibusDesabilitar;
        }
        public Onibus Habilitar(Onibus onibus) {
            Onibus onibusHabilitar = ListarPorId(onibus.Id);
            if (onibusHabilitar == null) throw new System.Exception("Desculpe, houve um erro ao habilitar.");
            onibusHabilitar.StatusOnibus = OnibusStatus.Habilitado;
            _bancoContext.Update(onibusHabilitar);
            _bancoContext.SaveChanges();
            return onibus;
        }
        public Exception TratarErro(Onibus onibus, Exception erro) {
            if (erro.InnerException.Message.Contains(onibus.Placa)) {
                throw new System.Exception("Ônibus já se encontra cadastrado.");
            }
            if (erro.InnerException.Message.Contains(onibus.Renavam)) {
                throw new System.Exception("Ônibus já se encontra cadastrado.");
            }
            if (erro.InnerException.Message.Contains(onibus.Chassi)) {
                throw new System.Exception("Ônibus já se encontra cadastrado.");
            }
            throw new System.Exception("Desculpe, houve alguma falha na aplicação.");
        }
    }
}
