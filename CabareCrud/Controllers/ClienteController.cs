using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class ClienteController : Controller {
        private readonly IClienteRepositorio _clienteRepositorio;
        public ClienteController(IClienteRepositorio iclienteRepositorio) {
            _clienteRepositorio = iclienteRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Clientes físicos habilitados";
            List<PessoaFisica> clientesHabilitados = _clienteRepositorio.BuscarTodosHabilitados();
            return View(clientesHabilitados);
        }
        public IActionResult IndexJuridico() {
            ViewData["Title"] = "Clientes jurídicos habilitados";
            List<PessoaJuridica> clientesHabilitados = _clienteRepositorio.BuscarTodosHabJuridico();
            return View(clientesHabilitados);
        }

        public IActionResult Desabilitados() {
            ViewData["Title"] = "Clientes físicos desabilitados";
            List<PessoaFisica> clientesDesabilitados = _clienteRepositorio.BuscarTodosDesabilitados();
            return View("Index", clientesDesabilitados);
        }
        public IActionResult DesabilitadosJuridico() {
            ViewData["Title"] = "Clientes jurídicos desabilitados";
            List<PessoaJuridica> clienteDesabilitados = _clienteRepositorio.BuscarTodosDesaJuridico();
            return View("IndexJuridico", clienteDesabilitados);
        }

        public IActionResult NovoCliente() {
            ViewData["Title"] = "Incluir";
            ModelsCliente modelsCliente = new ModelsCliente {
                ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal(),
                ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal()
            };

            TempData["MensagemDeInfo"] = "O e-mail não é obrigatório para clientes.";
            return View(modelsCliente);
        }
        public IActionResult NovoClienteJuridico() {
            ViewData["Title"] = "Incluir";
            TempData["MensagemDeInfo"] = "O e-mail não é obrigatório para clientes/fornecedores.";
            return View();
        }
        [HttpPost]
        public IActionResult NovoCliente(ModelsCliente modelsCliente) {
            ViewData["Title"] = "Incluir";
            try {
                modelsCliente.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();
                modelsCliente.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal();
                PessoaFisica cliente = modelsCliente.ClienteFisico;

                if (ValidarCampo(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(modelsCliente);
                }
                if (ValidationVinculoMenorIdade(cliente.DataNascimento.ToString(), cliente.IdVinculacaoContratual.ToString())) {
                    TempData["MensagemDeErro"] = "Cliente menor de idade sem vínculo ao mesmo!";
                    return View(modelsCliente);
                }
                if (ModelState.IsValid) {
                    if (ValidationVinculoMaiorIdade(cliente.DataNascimento.ToString(), cliente.IdVinculacaoContratual.ToString())) {
                        TempData["MensagemDeErro"] = "Não é possível vincular cliente maior de idade!";
                        return View(modelsCliente);
                    }
                    cliente.Status = StatuCliente.Habilitado;
                    _clienteRepositorio.Adicionar(cliente);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsCliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsCliente);
            }
        }
        [HttpPost]
        public IActionResult NovoClienteJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Incluir";
            try {
                if (ValidarCampoJurico(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(cliente);
                }
                if (ModelState.IsValid) {
                    cliente.Status = StatuCliente.Habilitado;
                    cliente = _clienteRepositorio.AdicionarJ(cliente);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("IndexJuridico");
                }
                return View(cliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        public IActionResult EditarCliente(long id) {
            ViewData["Title"] = "Editar";
            ModelsCliente modelsCliente = new ModelsCliente {
                ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal(),
                ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal(),
                ClienteFisico = _clienteRepositorio.ListarPorId(id)
            };
            if (modelsCliente.ClienteFisico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return RedirectToAction("Index");
            }
            if (!string.IsNullOrEmpty(modelsCliente.ClienteFisico.IdVinculacaoContratual.ToString())) {
                PessoaFisica pessoaFisicaResponsavel = _clienteRepositorio.ListarPorId(modelsCliente.ClienteFisico.IdVinculacaoContratual.Value);
                if (pessoaFisicaResponsavel != null) {
                    modelsCliente.ClienteFisicoList.Add(pessoaFisicaResponsavel);
                }
                else {
                    PessoaJuridica pessoaJuridicaResponsavel = _clienteRepositorio.ListarPorIdJuridico(modelsCliente.ClienteFisico.IdVinculacaoContratual.Value);
                    if (pessoaJuridicaResponsavel != null) {
                        modelsCliente.ClienteJuridicoList.Add(pessoaJuridicaResponsavel);
                    }
                }
            }
            return View(modelsCliente);
        }
        public IActionResult EditarClienteJuridico(long id) {
            ViewData["Title"] = "Editar";
            PessoaJuridica cliente = _clienteRepositorio.ListarPorIdJuridico(id);
            if (cliente == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(cliente);
            }
            return View(cliente);
        }
        [HttpPost]
        public IActionResult EditarCliente(ModelsCliente modelsCliente) {
            ViewData["Title"] = "Editar";
            try {
                modelsCliente.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();
                modelsCliente.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal();
                PessoaFisica cliente = modelsCliente.ClienteFisico;
                if (!string.IsNullOrEmpty(modelsCliente.ClienteFisico.IdVinculacaoContratual.ToString())) {
                    PessoaFisica pessoaFisicaResponsavel = _clienteRepositorio.ListarPorId(modelsCliente.ClienteFisico.IdVinculacaoContratual.Value);
                    if (pessoaFisicaResponsavel != null) {
                        if (pessoaFisicaResponsavel.Adimplente == Adimplencia.Inadimplente) {
                            modelsCliente.ClienteFisicoList.Add(pessoaFisicaResponsavel);
                        }
                    }
                    else {
                        PessoaJuridica pessoaJuridicaResponsavel = _clienteRepositorio.ListarPorIdJuridico(modelsCliente.ClienteFisico.IdVinculacaoContratual.Value);
                        if (pessoaJuridicaResponsavel != null) {
                            if (pessoaJuridicaResponsavel.Adimplente == Adimplencia.Inadimplente) {
                                modelsCliente.ClienteJuridicoList.Add(pessoaJuridicaResponsavel);
                            }
                        }
                    }
                }
                if (ValidarCampo(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(modelsCliente);
                }
                if (ValidationVinculoMenorIdade(cliente.DataNascimento.ToString(), cliente.IdVinculacaoContratual.ToString())) {
                    TempData["MensagemDeErro"] = "Cliente menor de idade sem vínculo ao mesmo!";
                    return View(modelsCliente);
                }
                if (ValidationClienteContratoIdade(cliente)) {
                    TempData["MensagemDeErro"] = "Clientes que possuem contratos em andamento não podem ser menores de idade!";
                    return View(modelsCliente);
                }
                if (ModelState.IsValid) {
                    if (ValidationVinculoMaiorIdade(cliente.DataNascimento.ToString(), cliente.IdVinculacaoContratual.ToString())) {
                        TempData["MensagemDeErro"] = "Não é possível vincular cliente maior de idade!";
                        return View(modelsCliente);
                    }
                    if (!string.IsNullOrEmpty(cliente.ToString())) {
                        //Chama o método que válida a inadimplência e alteração da vinculação contratual. 
                        if (ValidarClienteResponAlterInvalid(cliente)) {
                            TempData["MensagemDeErro"] = "Cliente inadimplente não pode ter vinculação alterada!";
                            return View(modelsCliente);
                        }
                    }
                    _clienteRepositorio.Editar(cliente);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsCliente);

            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsCliente);
            }
        }
        [HttpPost]
        public IActionResult EditarClienteJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Editar";
            try {
                if (ValidarCampoJurico(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(cliente);
                }
                if (ModelState.IsValid) {
                    _clienteRepositorio.EditarJurico(cliente);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("IndexJuridico");
                }
                return View(cliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }

        public IActionResult Desabilitar(long id) {
            ViewData["Title"] = "Desabilitar";
            PessoaFisica cliente = _clienteRepositorio.ListarPorId(id);
            if (cliente == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(cliente);
            }
            return View(cliente);
        }
        public IActionResult DesabilitarJuridico(long id) {
            ViewData["Title"] = "Desabilitar";
            PessoaJuridica cliente = _clienteRepositorio.ListarPorIdJuridico(id);
            if (cliente == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(cliente);
            }
            return View(cliente);
        }
        [HttpPost]
        public IActionResult Desabilitar(PessoaFisica cliente) {
            ViewData["Title"] = "Desabilitar";
            PessoaFisica clienteError = _clienteRepositorio.ListarPorId(cliente.Id);
            try {
                _clienteRepositorio.Desabilitar(cliente);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(clienteError);
            }
        }
        [HttpPost]
        public IActionResult DesabilitarJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Desabilitar";
            PessoaJuridica clienteError = _clienteRepositorio.ListarPorIdJuridico(cliente.Id);
            try {
                _clienteRepositorio.DesabilitarJuridico(cliente);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("IndexJuridico");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(clienteError);
            }
        }

        public IActionResult Habilitar(long id) {
            ViewData["Title"] = "Habilitar";
            PessoaFisica cliente = _clienteRepositorio.ListarPorId(id);
            if (cliente == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(cliente);
            }
            return View(cliente);
        }
        public IActionResult HabilitarJuridico(long id) {
            ViewData["Title"] = "Habilitar";
            PessoaJuridica cliente = _clienteRepositorio.ListarPorIdJuridico(id);
            if (cliente == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(cliente);
            }
            return View(cliente);
        }
        [HttpPost]
        public IActionResult HabilitarJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Habilitar";
            try {
                _clienteRepositorio.HabilitarJuridico(cliente);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("IndexJuridico");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        [HttpPost]
        public IActionResult Habilitar(PessoaFisica cliente) {
            ViewData["Title"] = "Habilitar";
            try {
                _clienteRepositorio.Habilitar(cliente);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }

        //Métodos abaixo apenas para retornar mensagem de erro em geral, já que a ModelState não os deixa serem registrados no banco de dados. 
        public bool ValidarCampo(PessoaFisica cliente) {
            if (cliente.Name == null || cliente.Cpf == null || cliente.Rg == null || cliente.NameMae == null || cliente.Cep == null ||
                   cliente.NumeroResidencial == null || cliente.ComplementoResidencial == null ||
                   cliente.Logradouro == null || cliente.Bairro == null || cliente.Cidade == null || cliente.Estado == null || cliente.Telefone == null || cliente.DataNascimento == null) {
                return true;
            }
            else return false;
        }
        public bool ValidarCampoJurico(PessoaJuridica cliente) {
            if (cliente.NomeFantasia == null || cliente.Cnpj == null || cliente.InscricaoEstadual == null || cliente.InscricaoMunicipal == null || cliente.RazaoSocial == null || cliente.Cep == null ||
                   cliente.NumeroResidencial == null || cliente.ComplementoResidencial == null ||
                   cliente.Logradouro == null || cliente.Bairro == null || cliente.Cidade == null || cliente.Estado == null || cliente.Ddd == null || cliente.Telefone == null) {
                return true;
            }
            else {
                return false;
            }
        }
        public bool ValidationVinculoMenorIdade(string date, string vinculo) {
            DateTime dataNascimento = DateTime.Parse(date);
            DateTime dataAtual = DateTime.Now;

            long dias = (int)dataAtual.Subtract(dataNascimento).TotalDays;
            long idade = dias / 365;

            if ((idade > 0 && idade < 18) && (string.IsNullOrEmpty(vinculo))) {
                return true;
            }
            else {
                return false;
            }
        }
        public bool ValidationVinculoMaiorIdade(string date, string vinculo) {
            DateTime dataAtual = DateTime.Now.Date;
            DateTime dataNascimento = DateTime.Parse(date).Date;

            long dias = (int)dataAtual.Subtract(dataNascimento).TotalDays;
            long idade = dias / 365;

            if (idade >= 18 && string.IsNullOrEmpty(vinculo) != true) {
                return true;
            }
            return false;
        }
        public bool ValidationClienteContratoIdade(PessoaFisica value) {
            DateTime dataNascimento = DateTime.Parse(value.DataNascimento.ToString());
            DateTime dataAtual = DateTime.Now;

            long dias = (int)dataAtual.Subtract(dataNascimento).TotalDays;
            long idade = dias / 365;
            PessoaFisica clienteValidation = _clienteRepositorio.ListarPorId(value.Id);
            if (!string.IsNullOrEmpty(clienteValidation.IdVinculacaoContratual.ToString())) {
                return false;
            }
            if ((idade > 0 && idade < 18) && (!string.IsNullOrEmpty(value.IdVinculacaoContratual.ToString()))
                &&  clienteValidation.ClientesContratos.Any(x => x.Contrato.StatusContrato == ContratoStatus.Ativo && x.Contrato.Aprovacao == StatusAprovacao.Aprovado)) {
                return true;
            }
            else {
                return false;
            }
        }

        //Método que não deixa cliente inadimplente ter seu responsável alterado. 
        public bool ValidarClienteResponAlterInvalid(PessoaFisica pessoaFisica) {
            PessoaFisica pessoaFisicaDB = _clienteRepositorio.ListarPorId(pessoaFisica.Id);
            if (pessoaFisicaDB.Adimplente == Adimplencia.Inadimplente && pessoaFisica.IdVinculacaoContratual != pessoaFisicaDB.IdVinculacaoContratual) {
                return true;
            }
            return false;
        }
    }
}