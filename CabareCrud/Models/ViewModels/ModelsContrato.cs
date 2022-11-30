using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BusesControl.Models.ViewModels {
    public class ModelsContrato {

        public List<Onibus> OnibusList { get; set; }
        public List<Funcionario> MotoristaList { get; set; }
        public List<PessoaFisica> ClienteFisicoList { get; set; }
        public List<PessoaJuridica> ClienteJuridicoList { get; set; }
        public Contrato Contrato { get; set; }
        public PessoaFisica PessoaFisica { get; set; }
        public List<PessoaFisica> ListPessoaFisicaSelect { get; set; }
        public List<PessoaJuridica> ListPessoaJuridicaSelect { get; set; }
        public int? TotClientes { get; set; }

        public void AddListFisico(PessoaFisica pessoaFisica) {
            if (!ListPessoaFisicaSelect.Any(x => x.Id == pessoaFisica.Id)){
                ListPessoaFisicaSelect.Add(pessoaFisica);
            }
        }
        public void RemoveListFisico(PessoaFisica pessoaFisica) {
            ListPessoaFisicaSelect.RemoveAll(x => x.Id == pessoaFisica.Id);
        }

        public void AddListJuridico(PessoaJuridica pessoaJuridica) {
            if (!ListPessoaJuridicaSelect.Any(x => x.Id == pessoaJuridica.Id)) {
                ListPessoaJuridicaSelect.Add(pessoaJuridica);
            }
        }
        public void RemoveListJuridico(PessoaJuridica pessoaJuridica) {
            ListPessoaJuridicaSelect.RemoveAll(x => x.Id == pessoaJuridica.Id);
        }

        //Construtor vazio para poder istânciar um objeto na controller. 
        public ModelsContrato() {
           
        }
    }
}
