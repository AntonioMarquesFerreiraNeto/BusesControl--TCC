using BusesControl.Models;

namespace BusesControl.Helper {
    public interface ISection {
        void CriarSection(Funcionario usuario);
        void EncerrarSection();
        Funcionario buscarSectionUser();
    }
}
