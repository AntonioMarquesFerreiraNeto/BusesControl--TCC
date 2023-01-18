using BusesControl.Filter;
using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class ContratoController : Controller {
        public string textoContratante;
        public string nomeCliente;
        private readonly IOnibusRepositorio _onibusRepositorio;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly ISection _section;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IContratoRepositorio _contratoRepositorio;
        private readonly IFinanceiroRepositorio _financeiroRepositorio;

        public ContratoController(IOnibusRepositorio onibusRepositorio, IFuncionarioRepositorio funcionarioRepositorio, ISection section,
                IClienteRepositorio clienteRepositorio, IContratoRepositorio contratoRepositorio, IFinanceiroRepositorio financeiroRepositorio) {
            _financeiroRepositorio = financeiroRepositorio;
            _onibusRepositorio = onibusRepositorio;
            _funcionarioRepositorio = funcionarioRepositorio;
            _section = section;
            _clienteRepositorio = clienteRepositorio;
            _contratoRepositorio = contratoRepositorio;
        }

        public static ModelsContrato modelsTest = new ModelsContrato {
            ListPessoaFisicaSelect = new List<PessoaFisica>(),
            ListPessoaJuridicaSelect = new List<PessoaJuridica>()
        };
        public IActionResult AddSelect(int id) {
            PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(id);
            if (pessoaFisica != null) {
                modelsTest.AddListFisico(pessoaFisica);
                return PartialView("_ListClientsSelect", modelsTest);
            }
            else {
                PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(id);
                modelsTest.AddListJuridico(pessoaJuridica);
                return PartialView("_ListClientsSelect", modelsTest);
            }
        }
        public IActionResult RemoveSelect(int id) {
            PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(id);
            if (pessoaFisica != null) {
                modelsTest.RemoveListFisico(pessoaFisica);
                return PartialView("_ListClientsSelect", modelsTest);
            }
            else {
                PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(id);
                modelsTest.RemoveListJuridico(pessoaJuridica);
                return PartialView("_ListClientsSelect", modelsTest);
            }
        }
        public IActionResult ReturnList() {
            return PartialView("_ListClientsSelect", modelsTest);
        }
        public IActionResult ClearList() {
            modelsTest.ListPessoaFisicaSelect.Clear();
            modelsTest.ListPessoaJuridicaSelect.Clear();
            return null;
        }
        public IActionResult Index() {
            ViewData["Title"] = "Contratos ativos";
            ModelsContratoAndUsuario contratosFuncionario = new ModelsContratoAndUsuario();
            contratosFuncionario.Contratos = _contratoRepositorio.ListContratoAtivo();
            contratosFuncionario.Usuario = _section.buscarSectionUser();
            return View(contratosFuncionario);
        }
        public IActionResult ListClientesContrato(int id) {
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            return PartialView("_ListClientesContrato", contrato);
        }
        public IActionResult Inativos() {
            ViewData["Title"] = "Contrato inativos";
            ModelsContratoAndUsuario contratosFuncionario = new ModelsContratoAndUsuario();
            contratosFuncionario.Contratos = _contratoRepositorio.ListContratoInativo();
            contratosFuncionario.Usuario = _section.buscarSectionUser();
            return View("Index", contratosFuncionario);
        }

        public IActionResult NovoContrato() {
            TempData["MensagemDeInfo"] = "Nº de parcelas não pode ultrapassar a quantidade de meses do contrato.";
            ViewData["Title"] = "Novo contrato";
            ModelsContrato modelsContrato = new ModelsContrato();
            modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();
            modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
            modelsContrato.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsContrato.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();
            Contrato contrato = new Contrato {
                Pagament = ModelPagament.Avista,
                DataEmissao = DateTime.Now
            };
            modelsContrato.Contrato = contrato;
            return View(modelsContrato);
        }
        [HttpPost]
        public IActionResult NovoContrato(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Novo contrato";
            try {
                modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();
                modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
                modelsContrato.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
                modelsContrato.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();

                //Recebendo a opção de pagamento do contrato e se o mesmo for à vista setando uma parcela para o mesmo. 
                int op = int.Parse(Request.Form["format_pagament"]);
                modelsContrato.Contrato.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                if (op == 0) {
                    modelsContrato.Contrato.QtParcelas = 1;
                }

                if (ValidarCampo(modelsContrato.Contrato) != true) {
                    TempData["MensagemDeErro"] = $"Informe os campos obrigatórios!";
                    return View(modelsContrato);
                }
                if (ModelState.IsValid) {
                    if (modelsTest.ListPessoaFisicaSelect.Count == 0 && modelsTest.ListPessoaJuridicaSelect.Count == 0) {
                        TempData["MensagemDeErro"] = "Não foi selecionado nenhum cliente!";
                        return View(modelsContrato);
                    }
                    if (!modelsContrato.Contrato.ValidarValorMonetario()) {
                        TempData["MensagemDeErro"] = "Valor monetário menor que R$ 150.00!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsContrato.Contrato)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsContrato);
                    }
                    if (ValidationQtParcelas(modelsContrato.Contrato)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateVencimento(modelsContrato.Contrato.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "O contrato não pode ser superior a dois anos!";
                        return View(modelsContrato);
                    }
                    modelsContrato.Contrato.StatusContrato = ContratoStatus.Ativo;
                    modelsContrato.Contrato.Aprovacao = StatusAprovacao.EmAnalise;
                    //Colocando a data atual novamente como medida de proteção em casos que o usuário desabilite a restrição do input pelo inspecionar. 
                    modelsContrato.Contrato.DataEmissao = DateTime.Now;
                    modelsContrato.ListPessoaFisicaSelect = modelsTest.ListPessoaFisicaSelect;
                    modelsContrato.ListPessoaJuridicaSelect = modelsTest.ListPessoaJuridicaSelect;
                    _contratoRepositorio.Adicionar(modelsContrato);
                    modelsTest.ListPessoaFisicaSelect.Clear();
                    modelsTest.ListPessoaJuridicaSelect.Clear();
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsContrato);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsContrato);
            }
        }

        public IActionResult EditarContrato(int id) {
            ViewData["Title"] = "Editar contrato";
            ModelsContrato modelsContrato = new ModelsContrato {
                ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato(),
                ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal(),
                MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab(),
                OnibusList = _onibusRepositorio.ListarTodosHab(),
                Contrato = _contratoRepositorio.ListarJoinPorId(id)
            };
            if (modelsContrato.Contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return RedirectToAction("Index");
            }
            if (modelsContrato.Contrato.Aprovacao != StatusAprovacao.Aprovado) GetClientesContrato(modelsContrato.Contrato);
            return View(modelsContrato);
        }
        [HttpPost]
        public IActionResult EditarContrato(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Editar contrato";
            try {
                modelsContrato.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();
                modelsContrato.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
                modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
                modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();

                //Recebendo a opção de pagamento do contrato e se o mesmo for à vista setando uma parcela para o mesmo. 
                if (modelsContrato.Contrato.Aprovacao != StatusAprovacao.Aprovado) {
                    int op = int.Parse(Request.Form["format_pagament"]);
                    modelsContrato.Contrato.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                    if (op == 0) {
                        modelsContrato.Contrato.QtParcelas = 1;
                    }
                }
                if (ModelState.IsValid) {
                    if (!modelsContrato.Contrato.ValidarValorMonetario()) {
                        TempData["MensagemDeErro"] = "Valor monetário menor que R$ 150.00!";
                        modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                        return View(modelsContrato);
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsContrato.Contrato)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                        return View(modelsContrato);
                    }
                    if (ValidationQtParcelas(modelsContrato.Contrato)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                        return View(modelsContrato);
                    }
                    if (ValidationDateVencimento(modelsContrato.Contrato.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "O contrato não pode ser superior a dois anos!";
                        modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                        return View(modelsContrato);
                    }
                    if (modelsContrato.Contrato.Aprovacao != StatusAprovacao.Aprovado) {
                        if (modelsTest.ListPessoaFisicaSelect.Count == 0 && modelsTest.ListPessoaJuridicaSelect.Count == 0) {
                            TempData["MensagemDeErro"] = "Não foi selecionado nenhum cliente!";
                            modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                            return View(modelsContrato);
                        }
                        modelsContrato.ListPessoaFisicaSelect = modelsTest.ListPessoaFisicaSelect;
                        modelsContrato.ListPessoaJuridicaSelect = modelsTest.ListPessoaJuridicaSelect;
                    }
                    _contratoRepositorio.EditarContrato(modelsContrato);
                    TempData["MensagemDeSucesso"] = $"Editado com sucesso!";
                    modelsTest.ListPessoaFisicaSelect.Clear();
                    modelsTest.ListPessoaJuridicaSelect.Clear();
                    return RedirectToAction("Index");
                }
                modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                return View(modelsContrato);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                return View(modelsContrato);
            }
        }

        public IActionResult Inativar(int id) {
            ViewData["Title"] = "Inativar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult Inativar(Contrato contrato) {
            ViewData["Title"] = "Inativar contrato";
            try {
                _contratoRepositorio.InativarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Inativado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        public IActionResult Ativar(int id) {
            ViewData["Title"] = "Ativar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult Ativar(Contrato contrato) {
            ViewData["Title"] = "Ativar contrato";
            try {
                _contratoRepositorio.AtivarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Ativado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        //Métodos de validação de campos e auxilio de IActions.
        public bool ValidarCampo(Contrato contrato) {

            if (contrato.MotoristaId == null || contrato.OnibusId == null
                || contrato.DataEmissao == null || contrato.DataVencimento == null || contrato.Detalhamento == null
                || contrato.ValorMonetario == null) {
                return false;
            }
            return true;
        }
        public bool ValidationDateEmissaoAndVencimento(Contrato contrato) {
            if (contrato.DataEmissao >= contrato.DataVencimento) {
                return true;
            }
            return false;
        }
        public bool ValidationDateVencimento(string value) {
            DateTime dataVencimento = DateTime.Parse(value);
            DateTime dataAtual = DateTime.Now;

            long dias = (int)dataVencimento.Subtract(dataAtual).TotalDays;
            long tempoValidation = dias / 365;

            if (tempoValidation >= 2) {
                return true;
            }
            return false;
        }
        public bool ValidationQtParcelas(Contrato contrato) {

            DateTime dateVencimento = DateTime.Parse(contrato.DataVencimento.ToString());
            DateTime dataEmissao = DateTime.Parse(contrato.DataEmissao.ToString());

            float dias = (float)dateVencimento.Subtract(dataEmissao).TotalDays;
            float ano = dias / 365;
            if (contrato.Pagament == ModelPagament.Parcelado) {
                bool resultado = (contrato.QtParcelas > ano * 12 || contrato.QtParcelas < 2 || string.IsNullOrEmpty(contrato.QtParcelas.ToString())) ? true : false;
                return resultado;
            }
            else {
                bool resultado = (contrato.QtParcelas < 1 || string.IsNullOrEmpty(contrato.QtParcelas.ToString())) ? true : false;
                return resultado;
            }
        }

        public Contrato ModelsError(Contrato value) {
            //Para não ter problema de referências de na view em momentos de erros.
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(value.Id);
            contrato.Pagament = (value.Pagament == ModelPagament.Avista) ? ModelPagament.Avista : ModelPagament.Parcelado;
            return contrato;
        }

        public void GetClientesContrato(Contrato value) {
            foreach (var item in value.ClientesContratos) {
                if (!string.IsNullOrEmpty(item.PessoaFisicaId.ToString())) {
                    modelsTest.AddListFisico(item.PessoaFisica);
                }
                if (!string.IsNullOrEmpty(item.PessoaJuridicaId.ToString())) {
                    modelsTest.AddListJuridico(item.PessoaJuridica);
                }
            }
        }

        //IActions de aprovar contrato (somente usuários administradores podem acessar as mesmas).
        [PagUserAdmin]
        public IActionResult ReturnAprovacaoContrato(int id) {
            Contrato contrato = new Contrato();
            contrato = _contratoRepositorio.ListarPorId(id);
            if (contrato != null) {
                return PartialView("_AprovacaoContrato", contrato);
            }
            return PartialView("_AprovacaoContrato", contrato);
        }

        [PagUserAdmin]
        public IActionResult AprovarContrato(int id) {
            ViewData["Title"] = "Aprovar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult AprovarContrato(Contrato contrato) {
            try {
                _contratoRepositorio.AprovarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Aprovado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        [PagUserAdmin]
        public IActionResult RevogarContrato(int id) {
            ViewData["Title"] = "Negar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult RevogarContrato(Contrato contrato) {
            try {
                _contratoRepositorio.RevogarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Negado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        [PagUserAdmin]
        public IActionResult ClientesContratoPdf(int? id) {
            Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
            List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View("Index", listContratos);
            }
            return PartialView("_ClientesContratoPdf", contrato);
        }

        [PagUserAdmin]
        public IActionResult PdfContrato(int? id, int? clienteFisicoId, int? clienteJuridicoId) {
            ViewData["Title"] = "Contratos ativos";
            Funcionario usuarioAutenticado = _section.buscarSectionUser();
            ModelsContratoAndUsuario contratosAndUsuario = new ModelsContratoAndUsuario();
            contratosAndUsuario.Contratos = _contratoRepositorio.ListContratoAtivo();
            contratosAndUsuario.Usuario = usuarioAutenticado;
            try {
                List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
                Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
                if (contrato == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return View("Index", contratosAndUsuario);
                }
                var pxPorMm = 72 / 25.2f;
                Document doc = new Document(PageSize.A4, 15 * pxPorMm, 15 * pxPorMm,
                    15 * pxPorMm, 15 * pxPorMm);
                MemoryStream stream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                writer.CloseStream = false;
                doc.Open();
                var fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 16,
                    iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY);
                Paragraph paragrofoJustificado = new Paragraph("",
                new Font(fonteBase, 12, Font.NORMAL));
                paragrofoJustificado.Alignment = Element.ALIGN_JUSTIFIED;
                Paragraph paragrafoCenter = new Paragraph("", new Font(fonteBase, 12, Font.NORMAL));
                paragrafoCenter.Alignment = Element.ALIGN_CENTER;
                var titulo = new Paragraph($"Contrato de serviço Nº {contrato.Id}\n\n", fonteParagrafo);
                titulo.Alignment = Element.ALIGN_CENTER;

                var caminhoImgLeft = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\anton\\Desktop\\Antonio\\faculdade\\Ws-vs2022\\CabareCrud\\CabareCrud\\wwwroot\\css\\Imagens\\LogoPdf.jpeg");
                if (caminhoImgLeft != null) {
                    Image logo = Image.GetInstance(caminhoImgLeft);
                    float razaoImg = logo.Width / logo.Height;
                    float alturaImg = 84;
                    float larguraLogo = razaoImg * alturaImg - 6f;
                    logo.ScaleToFit(larguraLogo, alturaImg);
                    var margemEsquerda = doc.PageSize.Width - doc.RightMargin - larguraLogo - 2;
                    var margemTopo = doc.PageSize.Height - doc.TopMargin - 60;
                    logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                    writer.DirectContent.AddImage(logo, false);
                }
                var caminhoImgRight = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\anton\\Desktop\\Antonio\\faculdade\\Ws-vs2022\\CabareCrud\\CabareCrud\\wwwroot\\css\\Imagens\\LogoPdfRight.jpg");
                if (caminhoImgRight != null) {
                    Image logo2 = Image.GetInstance(caminhoImgRight);
                    float razaoImg = logo2.Width / logo2.Height;
                    float alturaImg = 84;
                    float larguraLogo = razaoImg * alturaImg - 6f;
                    logo2.ScaleToFit(larguraLogo, alturaImg);
                    var margemRight = pxPorMm * 15;
                    var margemTopo = doc.PageSize.Height - doc.TopMargin - 60;
                    logo2.SetAbsolutePosition(margemRight, margemTopo);
                    writer.DirectContent.AddImage(logo2, false);
                }

                string titulo_contratante = "\nCONTRATANTE:";

                if (!string.IsNullOrEmpty(clienteFisicoId.ToString())) {
                    PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(clienteFisicoId.Value);
                    if (pessoaFisica == null) {
                        TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                        return View("Index", contratosAndUsuario);
                    }
                    if (!string.IsNullOrEmpty(pessoaFisica.IdVinculacaoContratual.ToString())) {
                        PessoaFisica pessoaFisicaResponsavel = _clienteRepositorio.ListarPorId(pessoaFisica.IdVinculacaoContratual.Value);
                        if (pessoaFisicaResponsavel != null) {
                            nomeCliente = pessoaFisica.Name;
                            textoContratante = $"{titulo_contratante}\n{pessoaFisicaResponsavel.Name} portador(a) do " +
                                $"CPF: {pessoaFisicaResponsavel.ReturnCpfCliente()}, RG: {pessoaFisicaResponsavel.Rg}, filho(a) da Sr. {pessoaFisicaResponsavel.NameMae}, residente domiciliado no imovel Nº {pessoaFisicaResponsavel.NumeroResidencial}({pessoaFisicaResponsavel.Logradouro}), próximo ao complemento residencial {pessoaFisicaResponsavel.ComplementoResidencial}, no bairro {pessoaFisicaResponsavel.Bairro}," +
                                $" da cidade de {pessoaFisicaResponsavel.Cidade} — {pessoaFisicaResponsavel.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaFisicaResponsavel.Ddd}){pessoaFisicaResponsavel.ReturnTelefoneCliente()}, {pessoaFisicaResponsavel.Email}. Neste ato representado(a) como responsável legal pelo(a) requerente do contrato que será descrito a seguir: " +
                                $"\n{pessoaFisica.Name} portador(a) do " +
                                $"CPF: {pessoaFisica.ReturnCpfCliente()}, RG: {pessoaFisica.Rg}, filho(a) da Sr. {pessoaFisica.NameMae}, residente domiciliado no imovel Nº {pessoaFisica.NumeroResidencial}({pessoaFisica.Logradouro}), próximo ao complemento residencial {pessoaFisica.ComplementoResidencial}, no bairro {pessoaFisica.Bairro}," +
                                $" da cidade de {pessoaFisica.Cidade} — {pessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaFisica.Ddd}){pessoaFisica.ReturnTelefoneCliente()}, {pessoaFisica.Email}.\n\n\n";
                        }
                        else {
                            nomeCliente = pessoaFisica.Name;
                            PessoaJuridica pessoaJuridicaResponsavel = _clienteRepositorio.ListarPorIdJuridico(pessoaFisica.IdVinculacaoContratual.Value);
                            textoContratante = $"{titulo_contratante}\n{pessoaJuridicaResponsavel.RazaoSocial}, inscrita no CNPJ: {pessoaJuridicaResponsavel.ReturnCnpjCliente()}, inscrição estadual: {pessoaJuridicaResponsavel.InscricaoEstadual}, inscrição municipal: {pessoaJuridicaResponsavel.InscricaoMunicipal}, portadora do nome fantasia {pessoaJuridicaResponsavel.NomeFantasia}, " +
                            $"residente domiciliado no imovel Nº {pessoaJuridicaResponsavel.NumeroResidencial} ({pessoaJuridicaResponsavel.Logradouro}), próximo ao complemento residencial {pessoaJuridicaResponsavel.ComplementoResidencial}, no bairro {pessoaJuridicaResponsavel.Bairro}," +
                            $" da cidade de {pessoaJuridicaResponsavel.Cidade} — {pessoaJuridicaResponsavel.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaJuridicaResponsavel.Ddd}){pessoaJuridicaResponsavel.ReturnTelefoneCliente()}, {pessoaJuridicaResponsavel.Email}. Neste ato representada como responsável legal pelo(a) requerente do contrato que será descrito a seguir:" +
                            $"\n{pessoaFisica.Name} portador(a) do " +
                                $"CPF: {pessoaFisica.ReturnCpfCliente()}, RG: {pessoaFisica.Rg}, filho(a) da Sr. {pessoaFisica.NameMae}, residente domiciliado no imovel Nº {pessoaFisica.NumeroResidencial}({pessoaFisica.Logradouro}), próximo ao complemento residencial {pessoaFisica.ComplementoResidencial}, no bairro {pessoaFisica.Bairro}," +
                                $" da cidade de {pessoaFisica.Cidade} — {pessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaFisica.Ddd}){pessoaFisica.ReturnTelefoneCliente()}, {pessoaFisica.Email}.\n\n\n";
                        }

                    }
                    else {
                        nomeCliente = pessoaFisica.Name;
                        textoContratante = $"{titulo_contratante}\n{pessoaFisica.Name} portador(a) do " +
                        $"CPF: {pessoaFisica.ReturnCpfCliente()}, RG: {pessoaFisica.Rg}, filho(a) da Sr. {pessoaFisica.NameMae}, residente domiciliado no imovel Nº {pessoaFisica.NumeroResidencial}({pessoaFisica.Logradouro}), próximo ao complemento residencial {pessoaFisica.ComplementoResidencial}, no bairro {pessoaFisica.Bairro}," +
                        $" da cidade de {pessoaFisica.Cidade} — {pessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaFisica.Ddd}){pessoaFisica.ReturnTelefoneCliente()}, {pessoaFisica.Email}. Neste ato representado(a) como o requerente do contrato.\n\n\n";
                    }
                }
                else {
                    PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(clienteJuridicoId.Value);
                    if (pessoaJuridica == null) {
                        TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                        return View("Index", contratosAndUsuario);
                    }
                    nomeCliente = pessoaJuridica.NomeFantasia;
                    textoContratante = $"{titulo_contratante}\n{pessoaJuridica.RazaoSocial}, inscrita no CNPJ: {pessoaJuridica.ReturnCnpjCliente()}, inscrição estadual: {pessoaJuridica.InscricaoEstadual}, inscrição municipal: {pessoaJuridica.InscricaoMunicipal}, portadora do nome fantasia {pessoaJuridica.NomeFantasia}, " +
                    $"residente domiciliado no imovel Nº {pessoaJuridica.NumeroResidencial} ({pessoaJuridica.Logradouro}), próximo ao complemento residencial {pessoaJuridica.ComplementoResidencial}, no bairro {pessoaJuridica.Bairro}," +
                    $" da cidade de {pessoaJuridica.Cidade} — {pessoaJuridica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaJuridica.Ddd}){pessoaJuridica.ReturnTelefoneCliente()}, {pessoaJuridica.Email}. Neste ato representada como a requerente do contrato.\n\n\n";
                }

                string titulo_contratada = $"CONTRATADA:";
                string textoContratada = $"{titulo_contratada}\nBuss viagens LTDA, pessoa jurídica de direito privado para prestação de serviço, na proteção da LEI Nº 13.429º. " +
                    $"Localizada na cidade de Goianésia (GO) — Brasil, inscrita no CNPJ nº 03.115.484/0001-02, sobre a liderança do sócio fundador Manoel Rodrigues." +
                    $" Neste ato representada como  a empresa responsável pela realização da prestações de serviços do contrato.\n\n\n";

                string titulo_primeira_clausula = $"1 — CLÁUSULA PRIMEIRA";
                string PrimeiraClausula = $"{titulo_primeira_clausula}\nO presente contrato tem por objeto a prestação de serviço especial de transporte rodoviário na rota definida no registro do contrato: {contrato.Detalhamento}\n\n\n";

                string titulo_segunda_clausula = $"2 — CLÁUSULA SEGUNDA";
                string SegundaClausula = $"{titulo_segunda_clausula} \nO(s) veículo(s) que realizará(ão) o transporte será(ão) discriminado(s) a seguir: \n" +
                    $"  • Veículo {contrato.Onibus.Marca}, modelo {contrato.Onibus.NameBus}, placa {contrato.Onibus.Placa}, número de chassi {contrato.Onibus.Chassi}, da cor {contrato.Onibus.ReturnCorBus()}, fabricado em {contrato.Onibus.DataFabricacao}, e com capacidade de lotação para {contrato.Onibus.Assentos} passageiros.\n No caso de problemas com o(s) veículo(s) acima designado(s), " +
                    $"poderá ser utilizado outro veículo, desde que conste habilitado no Sistema de Habilitação de Transportes de Passageiros – SisHAB, da ANTT. \n\n";

                string titulo_terceira_clausula = $"\n3 — CLÁUSULA TERCEIRA";
                string TerceiraClausula = $"{titulo_terceira_clausula} \nO contratante deve estar ciente que deverá cumprir com as datas de pagamento determinadas do contrato. Desta forma, estando ciente de valores de juros adicionais em caso de inadimplência. Nos quais são 2% ao mês por parcela atrasada.\n\n\n";

                string titulo_quarta_clausula = $"4 — CLÁUSULA QUARTA";
                string QuartaClausula;
                if (contrato.Pagament == Models.Enums.ModelPagament.Avista) {
                    QuartaClausula = $"{titulo_quarta_clausula} \nPelos serviços prestados a Contratante pagará a Contratada o valor de {contrato.ReturnValorTotCliente()}, na data atual com três dias úteis. Em parcela única, pois, o contrato foi deferido como à vista.\n\n";
                }
                else {
                    QuartaClausula = $"{titulo_quarta_clausula} \nPelos serviços prestados a Contratante pagará a Contratada o valor de {contrato.ReturnValorTotCliente()}, e os respectivos pagamentos serão realizados dia {contrato.ReturnDiaPagamento()} de cada mês. Dividos em {contrato.QtParcelas} parcelas no valor {contrato.ValorParcelaContratoPorCliente.Value.ToString("C2")}. No entanto, a primeira parcela do contrato terá três dias úteis para realização do pagamento após a aprovação do contrato.\n\n";
                }
                string titulo_quinta_clausula = $"\n5 — CLÁUSULA QUINTA";
                string QuintaClausula = $"{titulo_quinta_clausula}\nEm caso de rescisão de contrato anterior a data acordada sem o devido pagamento da(s) parcela(s), o cliente deve estar ciente que haverá multa de 3% do valor total por cliente ( {contrato.ReturnValorTotCliente()} ), pela rescisão do contrato.\n\n\n";

                string titulo_sexta_clausula = $"6 — CLÁUSULA SEXTA";
                string SextaClausula = $"{titulo_sexta_clausula} \nO período da prestação do serviço será de  {contrato.ReturnDateContrato()}, que é a data acordada no registro do contrato.\n\n\n";

                string titulo_setima_clausula = $"7 — CLÁUSULA SÉTIMA";
                string SetimaClausula = $"{titulo_setima_clausula}\nO CONTRATANTE fica ciente que somente será permitido o transporte de passageiros limitados à capacidade de passageiros sentados no(s) veículo(s) utilizado(s), ficando expressamente proibido o transporte de passageiros em pé ou acomodados no corredor, bem como passageiros que não estiverem constando na relação autorizada pela ANTT.\n\n\n";

                string traco = "\n___________________________________________\n";
                string assinaturaCliente = "Assinatura do representante legal contratante\n\n";
                string traco2 = "___________________________________________________________\n";
                string assinaturaEmpresa = "Assinatura da empresa representante da prestação do serviço";
                string traco3 = "________________________________________________\n";
                string assinaturaAdm = "Assinatura do administrador que aprovou o contrato\n\n";

                paragrofoJustificado.Add(textoContratante);
                paragrofoJustificado.Add(textoContratada);
                paragrofoJustificado.Add(PrimeiraClausula);
                paragrofoJustificado.Add(SegundaClausula);
                paragrofoJustificado.Add(TerceiraClausula);
                paragrofoJustificado.Add(QuartaClausula);
                paragrofoJustificado.Add(QuintaClausula);
                paragrofoJustificado.Add(SextaClausula);
                paragrofoJustificado.Add(SetimaClausula);

                paragrafoCenter.Add(traco);
                paragrafoCenter.Add(assinaturaCliente);
                paragrafoCenter.Add(traco3);
                paragrafoCenter.Add(assinaturaAdm);
                paragrafoCenter.Add(traco2);
                paragrafoCenter.Add(assinaturaEmpresa);

                doc.Add(titulo);
                doc.Add(paragrofoJustificado);
                doc.Add(paragrafoCenter);

                doc.Close();

                stream.Flush();
                stream.Position = 0;
                return File(stream, "application/pdf", $"Contrato - {nomeCliente}.pdf");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"Desculpe, houve um erro: {erro.Message}";
                List<Contrato> contratos = _contratoRepositorio.ListContratoAprovados();
                return View("Index", contratosAndUsuario);
            }
        }

        public IActionResult PdfRescisao(int? id) {
            ViewData["Title"] = "Contratos ativos";
            Funcionario usuarioAutenticado = _section.buscarSectionUser();
            ModelsContratoAndUsuario contratosAndUsuario = new ModelsContratoAndUsuario();
            contratosAndUsuario.Contratos = _contratoRepositorio.ListContratoAtivo();
            contratosAndUsuario.Usuario = usuarioAutenticado;
            try {
                List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
                ClientesContrato clientesContrato = _contratoRepositorio.ListarClientesContratoId(id.Value);
                if (clientesContrato == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return RedirectToAction("Index");
                }
                var pxPorMm = 72 / 25.2f;
                Document doc = new Document(PageSize.A4, 15 * pxPorMm, 15 * pxPorMm,
                    15 * pxPorMm, 15 * pxPorMm);
                MemoryStream stream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                writer.CloseStream = false;
                doc.Open();
                var fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 15,
                    iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY);
                Paragraph paragrofoJustificado = new Paragraph("",
                new Font(fonteBase, 12, Font.NORMAL));
                paragrofoJustificado.Alignment = Element.ALIGN_JUSTIFIED;
                Paragraph paragrafoCenter = new Paragraph("", new Font(fonteBase, 12, Font.NORMAL));
                paragrafoCenter.Alignment = Element.ALIGN_CENTER;

                var caminhoImgLeft = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\anton\\Desktop\\Antonio\\faculdade\\Ws-vs2022\\CabareCrud\\CabareCrud\\wwwroot\\css\\Imagens\\LogoPdf.jpeg");
                if (caminhoImgLeft != null) {
                    Image logo = Image.GetInstance(caminhoImgLeft);
                    float razaoImg = logo.Width / logo.Height;
                    float alturaImg = 84;
                    float larguraLogo = razaoImg * alturaImg - 6f;
                    logo.ScaleToFit(larguraLogo, alturaImg);
                    var margemEsquerda = doc.PageSize.Width - doc.RightMargin - larguraLogo - 2;
                    var margemTopo = doc.PageSize.Height - doc.TopMargin - 60;
                    logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                    writer.DirectContent.AddImage(logo, false);
                }

                string titulo_contratante = "1 - CONTRATANTE";

                if (!string.IsNullOrEmpty(clientesContrato.PessoaFisicaId.ToString())) {
                    if (!string.IsNullOrEmpty(clientesContrato.PessoaFisica.IdVinculacaoContratual.ToString())) {
                        PessoaFisica pessoaFisicaResponsavel = _clienteRepositorio.ListarPorId(clientesContrato.PessoaFisica.IdVinculacaoContratual.Value);
                        if (pessoaFisicaResponsavel != null) {
                            nomeCliente = clientesContrato.PessoaFisica.Name;
                            textoContratante = $"{titulo_contratante}\n{pessoaFisicaResponsavel.Name} portador(a) do " +
                                $"CPF: {pessoaFisicaResponsavel.ReturnCpfCliente()}, RG: {pessoaFisicaResponsavel.Rg}, filho(a) da Sr. {pessoaFisicaResponsavel.NameMae}, residente domiciliado no imovel Nº {pessoaFisicaResponsavel.NumeroResidencial}({pessoaFisicaResponsavel.Logradouro}), próximo ao complemento residencial {pessoaFisicaResponsavel.ComplementoResidencial}, no bairro {pessoaFisicaResponsavel.Bairro}," +
                                $" da cidade de {pessoaFisicaResponsavel.Cidade} — {pessoaFisicaResponsavel.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaFisicaResponsavel.Ddd}){pessoaFisicaResponsavel.ReturnTelefoneCliente()}, {pessoaFisicaResponsavel.Email}. Neste ato, sendo o representante legal do processo de rescisão de contrato do cliente vinculado que será descrito a seguir: " +
                                $"\n{clientesContrato.PessoaFisica.Name} portador(a) do " +
                                $"CPF: {clientesContrato.PessoaFisica.ReturnCpfCliente()}, RG: {clientesContrato.PessoaFisica.Rg}, filho(a) da Sr. {clientesContrato.PessoaFisica.NameMae}, residente domiciliado no imovel Nº {clientesContrato.PessoaFisica.NumeroResidencial}({clientesContrato.PessoaFisica.Logradouro}), próximo ao complemento residencial {clientesContrato.PessoaFisica.ComplementoResidencial}, no bairro {clientesContrato.PessoaFisica.Bairro}," +
                                $" da cidade de {clientesContrato.PessoaFisica.Cidade} — {clientesContrato.PessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({clientesContrato.PessoaFisica.Ddd}){clientesContrato.PessoaFisica.ReturnTelefoneCliente()}, {clientesContrato.PessoaFisica.Email}.\n\n\n";
                        }
                        else {
                            nomeCliente = clientesContrato.PessoaFisica.Name;
                            PessoaJuridica pessoaJuridicaResponsavel = _clienteRepositorio.ListarPorIdJuridico(clientesContrato.PessoaFisica.IdVinculacaoContratual.Value);
                            textoContratante = $"{titulo_contratante}\n{pessoaJuridicaResponsavel.RazaoSocial}, inscrita no CNPJ: {pessoaJuridicaResponsavel.ReturnCnpjCliente()}, inscrição estadual: {pessoaJuridicaResponsavel.InscricaoEstadual}, inscrição municipal: {pessoaJuridicaResponsavel.InscricaoMunicipal}, portadora do nome fantasia {pessoaJuridicaResponsavel.NomeFantasia}, " +
                            $"residente domiciliado no imovel Nº {pessoaJuridicaResponsavel.NumeroResidencial} ({pessoaJuridicaResponsavel.Logradouro}), próximo ao complemento residencial {pessoaJuridicaResponsavel.ComplementoResidencial}, no bairro {pessoaJuridicaResponsavel.Bairro}," +
                            $" da cidade de {pessoaJuridicaResponsavel.Cidade} — {pessoaJuridicaResponsavel.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaJuridicaResponsavel.Ddd}){pessoaJuridicaResponsavel.ReturnTelefoneCliente()}, {pessoaJuridicaResponsavel.Email}. Neste ato, sendo o representante legal do processo de rescisão de contrato do cliente vinculado que será descrito a seguir:" +
                            $"\n{clientesContrato.PessoaFisica.Name} portador(a) do " +
                                $"CPF: {clientesContrato.PessoaFisica.ReturnCpfCliente()}, RG: {clientesContrato.PessoaFisica.Rg}, filho(a) da Sr. {clientesContrato.PessoaFisica.NameMae}, residente domiciliado no imovel Nº {clientesContrato.PessoaFisica.NumeroResidencial}({clientesContrato.PessoaFisica.Logradouro}), próximo ao complemento residencial {clientesContrato.PessoaFisica.ComplementoResidencial}, no bairro {clientesContrato.PessoaFisica.Bairro}," +
                                $" da cidade de {clientesContrato.PessoaFisica.Cidade} — {clientesContrato.PessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({clientesContrato.PessoaFisica.Ddd}){clientesContrato.PessoaFisica.ReturnTelefoneCliente()}, {clientesContrato.PessoaFisica.Email}.\n\n\n";
                        }

                    }
                    else {
                        nomeCliente = clientesContrato.PessoaFisica.Name;
                        textoContratante = $"{titulo_contratante}\n{clientesContrato.PessoaFisica.Name} portador(a) do " +
                        $"CPF: {clientesContrato.PessoaFisica.ReturnCpfCliente()}, RG: {clientesContrato.PessoaFisica.Rg}, filho(a) da Sr. {clientesContrato.PessoaFisica.NameMae}, residente domiciliado no imovel Nº {clientesContrato.PessoaFisica.NumeroResidencial}({clientesContrato.PessoaFisica.Logradouro}), próximo ao complemento residencial {clientesContrato.PessoaFisica.ComplementoResidencial}, no bairro {clientesContrato.PessoaFisica.Bairro}," +
                        $" da cidade de {clientesContrato.PessoaFisica.Cidade} — {clientesContrato.PessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({clientesContrato.PessoaFisica.Ddd}){clientesContrato.PessoaFisica.ReturnTelefoneCliente()}, {clientesContrato.PessoaFisica.Email}. Neste ato, sendo o responsável e solicitador do processo de rescisão do contrato, garantindo seus direitos legais definidos pela lei, e garantidos pela cláusula cinco do contrato.\n\n\n";
                    }
                }
                else {
                    PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(clientesContrato.PessoaJuridicaId.Value);
                    if (pessoaJuridica == null) {
                        TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                        return View("Index", contratosAndUsuario);
                    }
                    nomeCliente = pessoaJuridica.NomeFantasia;
                    textoContratante = $"{titulo_contratante}\n{pessoaJuridica.RazaoSocial}, inscrita no CNPJ: {pessoaJuridica.ReturnCnpjCliente()}, inscrição estadual: {pessoaJuridica.InscricaoEstadual}, inscrição municipal: {pessoaJuridica.InscricaoMunicipal}, portadora do nome fantasia {pessoaJuridica.NomeFantasia}, " +
                    $"residente domiciliado no imovel Nº {pessoaJuridica.NumeroResidencial} ({pessoaJuridica.Logradouro}), próximo ao complemento residencial {pessoaJuridica.ComplementoResidencial}, no bairro {pessoaJuridica.Bairro}," +
                    $" da cidade de {pessoaJuridica.Cidade} — {pessoaJuridica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaJuridica.Ddd}){pessoaJuridica.ReturnTelefoneCliente()}, {pessoaJuridica.Email}. Neste ato, sendo o responsável e solicitador do processo de rescisão do contrato, garantindo seus direitos legais definidos pela lei, e garantidos pela cláusula cinco do contrato.\n\n\n";
                }

                string titulo_contratada = $"2 - CONTRATADA";
                string textoContratada = $"{titulo_contratada}\nBuss viagens LTDA, pessoa jurídica de direito privado para prestação de serviço, na proteção da LEI Nº 13.429º. " +
                    $"Localizada na cidade de Goianésia (GO) — Brasil, inscrita no CNPJ nº 03.115.484/0001-02, sobre a liderança do sócio fundador Manoel Rodrigues." +
                    $" Neste ato representada como  a empresa responsável pela realização da prestações de serviços do contrato.\n\n\n";


                decimal? valorTotCliente = clientesContrato.Contrato.ValorParcelaContratoPorCliente * clientesContrato.Contrato.QtParcelas;
                decimal valorMulta = (valorTotCliente.Value * 3) / 100;

                string titulo_quinta_clausula = $"\n3 - PROCESSO DE RESCISÃO";
                string QuintaClausula = $"{titulo_quinta_clausula}\n“Em caso de rescisão de contrato anterior a data acordada sem o devido pagamento da(s) parcela(s), o cliente deve estar ciente que haverá multa de 3% do valor total por cliente ( {clientesContrato.Contrato.ReturnValorTotCliente()} ), pela rescisão do contrato.”. " +
                    $"Com base e asseguração da quinta cláusula do contrato, é dever do cliente realizar o pagamento de {valorMulta.ToString("C2")} para rescendir o contrato.\n\n\n";

                string traco = "\n___________________________________________\n";
                string assinaturaCliente = "Assinatura do representante legal contratante\n\n";
                string traco2 = "___________________________________________________________\n";
                string assinaturaEmpresa = "Assinatura da empresa representante da prestação do serviço";

                paragrofoJustificado.Add(textoContratante);
                paragrofoJustificado.Add(textoContratada);
                paragrofoJustificado.Add(QuintaClausula);

                paragrafoCenter.Add(traco);
                paragrafoCenter.Add(assinaturaCliente);
                paragrafoCenter.Add(traco2);
                paragrafoCenter.Add(assinaturaEmpresa);

                var titulo = new Paragraph($"Rescisão contrato Nº {clientesContrato.ContratoId} - {nomeCliente} \n\n\n", fonteParagrafo);
                titulo.Alignment = Element.ALIGN_LEFT;
                doc.Add(titulo);
                doc.Add(paragrofoJustificado);
                doc.Add(paragrafoCenter);

                doc.Close();

                stream.Flush();
                stream.Position = 0;
                return File(stream, "application/pdf", $"Rescisão - {nomeCliente}.pdf");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"Desculpe, houve um erro: {erro.Message}";
                List<Contrato> contratos = _contratoRepositorio.ListContratoAprovados();
                return View("Index", contratosAndUsuario);
            }
        }
    }
}