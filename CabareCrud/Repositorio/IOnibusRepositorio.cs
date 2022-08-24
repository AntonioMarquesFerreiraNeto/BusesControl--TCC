using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IOnibusRepositorio {
        public Onibus AdicionarBus(Onibus onibus);
        public List<Onibus> ListarTodosHab();
        public List<Onibus> ListarTodosDesa();
        public Onibus ListarPorId(long id);
        public Onibus EditarOnibus(Onibus onibus);
        public Onibus Desabilitar(Onibus onibus);
        public Onibus Habilitar(Onibus onibus);
    }
}
