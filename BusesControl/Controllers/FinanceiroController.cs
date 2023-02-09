using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class FinanceiroController : Controller {
        private readonly IContratoRepositorio _contratoRepositorio;
        private readonly IFinanceiroRepositorio _financeiroRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IRelatorioRepositorio _relatorioRepositorio;

        public FinanceiroController(IContratoRepositorio contratoRepositorio, IFinanceiroRepositorio financeiroRepositorio,
                IClienteRepositorio clienteRepositorio, IFornecedorRepositorio fornecedorRepositorio, IRelatorioRepositorio relatorioRepositorio) {
            _contratoRepositorio = contratoRepositorio;
            _financeiroRepositorio = financeiroRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _fornecedorRepositorio = fornecedorRepositorio;
            _relatorioRepositorio = relatorioRepositorio;
        }

        public static Filtros FiltrosStatic = new Filtros { ReceitasDespesas = "todos", DataFiltro = "não" };
        public void PopularFiltros(Filtros filtros) {
            if (filtros != null) {
                FiltrosStatic.DataFiltro = filtros.DataFiltro;
                FiltrosStatic.DataInicial = filtros.DataInicial;
                FiltrosStatic.DataTermino = filtros.DataTermino;
                FiltrosStatic.ReceitasDespesas = filtros.ReceitasDespesas;
            }
        }
        public IActionResult ClearFiltros() {
            FiltrosStatic = new Filtros { ReceitasDespesas = "todos", DataFiltro = "não" };
            return null;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Financeiro";
            _financeiroRepositorio.TaskMonitorParcelas();
            _financeiroRepositorio.TaskMonitorParcelasLancamento();
            ModelsFinanceiroIndex modelsFinanceiroIndex = new ModelsFinanceiroIndex();
            if (FiltrosStatic != null) {
                modelsFinanceiroIndex.Financeiros = _financeiroRepositorio.ListFinanceirosFiltros(FiltrosStatic);
                modelsFinanceiroIndex.Filtros = FiltrosStatic;
            }
            else {
                modelsFinanceiroIndex.Financeiros = _financeiroRepositorio.ListFinanceiros();
            }
            return View(modelsFinanceiroIndex);
        }
        public IActionResult ReturnFiltros() {
            return PartialView("_ReturnFiltros");
        }
        [HttpPost]
        public IActionResult ReturnResultFiltros(ModelsFinanceiroIndex modelsFinanceiroIndex) {
            ViewData["Title"] = "Financeiro";
            ModelsFinanceiroIndex modelsFinanceiroIndexDB = new ModelsFinanceiroIndex {
                Filtros = modelsFinanceiroIndex.Filtros,
                Financeiros = _financeiroRepositorio.ListFinanceirosFiltros(modelsFinanceiroIndex.Filtros)
            };
            PopularFiltros(modelsFinanceiroIndex.Filtros);
            return View("Index", modelsFinanceiroIndexDB);
        }
        public IActionResult ReturnDashFinanceiro() {
            Relatorio relatorio = new Relatorio();
            relatorio.ValTotReceitas = _relatorioRepositorio.ValorTotReceitas();
            relatorio.ValTotDespesas = _relatorioRepositorio.ValorTotDespesas();
            relatorio.ValTotEfetuadoDespesa = _relatorioRepositorio.ValorTotPagoDespesas();
            relatorio.ValTotEfetuadoReceita = _relatorioRepositorio.ValorTotPagoReceitas();
            return PartialView("_ReturnDashView", relatorio);
        }

        public IActionResult NovaDespesa() {
            ViewData["Title"] = "Nova despesa";
            ModelsFinanceiroRD modelsFinanceiroRD = new ModelsFinanceiroRD();
            modelsFinanceiroRD.CredorFisicoList = _fornecedorRepositorio.ListFornecedoreFisicos();
            modelsFinanceiroRD.CredorJuridicoList = _fornecedorRepositorio.ListFornecedoresJuridicos();
            Financeiro financeiro = new Financeiro {
                Pagament = ModelPagament.Avista,
                DataEmissao = DateTime.Now
            };
            modelsFinanceiroRD.Financeiro = financeiro;
            return View(modelsFinanceiroRD);
        }

        [HttpPost]
        public IActionResult NovaDespesa(ModelsFinanceiroRD modelsFinanceiroRD) {
            ViewData["Title"] = "Nova despesa";
            modelsFinanceiroRD.CredorFisicoList = _fornecedorRepositorio.ListFornecedoreFisicos();
            modelsFinanceiroRD.CredorJuridicoList = _fornecedorRepositorio.ListFornecedoresJuridicos();
            try {
                int op = int.Parse(Request.Form["format_pagament"]);
                modelsFinanceiroRD.Financeiro.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                if (op == 0) {
                    modelsFinanceiroRD.Financeiro.QtParcelas = 1;
                }
                if (ModelState.IsValid) {
                    FornecedorFisico fornecedorFisico = new FornecedorFisico();
                    fornecedorFisico = _fornecedorRepositorio.ListPorIdFisico(modelsFinanceiroRD.CredorDevedorId.Value);
                    if (fornecedorFisico != null) {
                        modelsFinanceiroRD.Financeiro.FornecedorFisicoId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    else {
                        modelsFinanceiroRD.Financeiro.FornecedorJuridicoId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationQtParcelas(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationDateVencimento(modelsFinanceiroRD.Financeiro.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "A receita/despesa não pode ser superior a dois anos!";
                        return View(modelsFinanceiroRD);
                    }
                    _financeiroRepositorio.AdicionarDespesa(modelsFinanceiroRD.Financeiro);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsFinanceiroRD);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"{erro.Message}";
                return View(modelsFinanceiroRD);
            }
        }

        public IActionResult NovaReceita() {
            ViewData["Title"] = "Nova receita";
            ModelsFinanceiroRD modelsFinanceiroRD = new ModelsFinanceiroRD();
            modelsFinanceiroRD.PessoaFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsFinanceiroRD.PessoaJuridicaList = _clienteRepositorio.ListClienteJuridicoLegal();
            Financeiro financeiro = new Financeiro {
                Pagament = ModelPagament.Avista,
                DataEmissao = DateTime.Now
            };
            modelsFinanceiroRD.Financeiro = financeiro;
            return View(modelsFinanceiroRD);
        }
        [HttpPost]
        public IActionResult NovaReceita(ModelsFinanceiroRD modelsFinanceiroRD) {
            ViewData["Title"] = "Nova receita";
            modelsFinanceiroRD.PessoaFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsFinanceiroRD.PessoaJuridicaList = _clienteRepositorio.ListClienteJuridicoLegal();
            try {
                int op = int.Parse(Request.Form["format_pagament"]);
                modelsFinanceiroRD.Financeiro.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                if (op == 0) {
                    modelsFinanceiroRD.Financeiro.QtParcelas = 1;
                }
                if (ModelState.IsValid) {
                    PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(modelsFinanceiroRD.CredorDevedorId.Value);
                    if (pessoaFisica != null) {
                        modelsFinanceiroRD.Financeiro.PessoaFisicaId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    else {
                        modelsFinanceiroRD.Financeiro.PessoaJuridicaId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationQtParcelas(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationDateVencimento(modelsFinanceiroRD.Financeiro.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "A receita/despesa não pode ser superior a dois anos!";
                        return View(modelsFinanceiroRD);
                    }
                    _financeiroRepositorio.AdicionarReceita(modelsFinanceiroRD.Financeiro);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsFinanceiroRD);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"{erro.Message}";
                return View(modelsFinanceiroRD);
            }
        }

        public IActionResult EditarLancamento(int? id) {
            ViewData["Title"] = $"Editar despesa/receita";
            ModelsFinanceiroRD modelsFinanceiroRD = new ModelsFinanceiroRD();
            modelsFinanceiroRD.PessoaFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsFinanceiroRD.PessoaJuridicaList = _clienteRepositorio.ListClienteJuridicoLegal();
            modelsFinanceiroRD.CredorFisicoList = _fornecedorRepositorio.ListFornecedoreFisicos();
            modelsFinanceiroRD.CredorJuridicoList = _fornecedorRepositorio.ListFornecedoresJuridicos();
            modelsFinanceiroRD.Financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
            if (modelsFinanceiroRD.Financeiro == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            if (modelsFinanceiroRD.Financeiro.DespesaReceita == DespesaReceita.Receita) {
                if (!string.IsNullOrEmpty(modelsFinanceiroRD.Financeiro.PessoaFisicaId.ToString())) {
                    modelsFinanceiroRD.CredorDevedorId = modelsFinanceiroRD.Financeiro.PessoaFisicaId;
                }
                else {
                    modelsFinanceiroRD.CredorDevedorId = modelsFinanceiroRD.Financeiro.PessoaJuridicaId;
                }
            }
            else {
                if (!string.IsNullOrEmpty(modelsFinanceiroRD.Financeiro.FornecedorFisicoId.ToString())) {
                    modelsFinanceiroRD.CredorDevedorId = modelsFinanceiroRD.Financeiro.FornecedorFisicoId;
                }
                else {
                    modelsFinanceiroRD.CredorDevedorId = modelsFinanceiroRD.Financeiro.FornecedorJuridicoId;
                }
            }
            return View(modelsFinanceiroRD);
        }
        [HttpPost]
        public IActionResult EditarLancamento(ModelsFinanceiroRD modelsFinanceiroRD) {
            ViewData["Title"] = $"Editar despesa/receita";
            modelsFinanceiroRD.PessoaFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsFinanceiroRD.PessoaJuridicaList = _clienteRepositorio.ListClienteJuridicoLegal();
            modelsFinanceiroRD.CredorFisicoList = _fornecedorRepositorio.ListFornecedoreFisicos();
            modelsFinanceiroRD.CredorJuridicoList = _fornecedorRepositorio.ListFornecedoresJuridicos();
            try {
                int op = int.Parse(Request.Form["format_pagament"]);
                modelsFinanceiroRD.Financeiro.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                if (op == 0) {
                    modelsFinanceiroRD.Financeiro.QtParcelas = 1;
                }
                if (ModelState.IsValid) {
                    if (modelsFinanceiroRD.Financeiro.DespesaReceita == DespesaReceita.Receita) {
                        PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(modelsFinanceiroRD.CredorDevedorId.Value);
                        if (pessoaFisica != null) {
                            modelsFinanceiroRD.Financeiro.PessoaFisicaId = modelsFinanceiroRD.CredorDevedorId.Value;
                        }
                        else {
                            modelsFinanceiroRD.Financeiro.PessoaJuridicaId = modelsFinanceiroRD.CredorDevedorId.Value;
                        }
                    }
                    else {
                        FornecedorFisico fornecedorFisico = _fornecedorRepositorio.ListPorIdFisico(modelsFinanceiroRD.CredorDevedorId.Value);
                        if (fornecedorFisico != null) {
                            modelsFinanceiroRD.Financeiro.FornecedorFisicoId = modelsFinanceiroRD.CredorDevedorId.Value;
                        }
                        else {
                            modelsFinanceiroRD.Financeiro.FornecedorJuridicoId = modelsFinanceiroRD.CredorDevedorId.Value;
                        }
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationQtParcelas(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationDateVencimento(modelsFinanceiroRD.Financeiro.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "A receita/despesa não pode ser superior a dois anos!";
                        return View(modelsFinanceiroRD);
                    }
                    _financeiroRepositorio.EditarLancamento(modelsFinanceiroRD.Financeiro);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsFinanceiroRD);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"{erro.Message}";
                return View(modelsFinanceiroRD);
            }
        }


        public IActionResult InativarLancamento(int? id) {
            Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
            return PartialView("_InativarLancamento", financeiro);
        }

        [HttpPost]
        public IActionResult InativarLancamento(Financeiro financeiro) {
            try {
                if (financeiro != null) {
                    _financeiroRepositorio.InativarReceitaOrDespesa(financeiro);
                    TempData["MensagemDeSucesso"] = "Inativado com sucesso!";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Contabilizar(int? id) {
            Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
            if (financeiro == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            string name;
            if (!string.IsNullOrEmpty(financeiro.ContratoId.ToString())) {
                name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                ViewData["Title"] = $"Parcelas contrato Nº {financeiro.ContratoId} – {name}";
            }
            else {
                if (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString()) || !string.IsNullOrEmpty(financeiro.PessoaJuridicaId.ToString())) {
                    name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                    ViewData["Title"] = $"Parcelas – {name}";
                }
                else {
                    name = (!string.IsNullOrEmpty(financeiro.FornecedorFisicoId.ToString())) ? financeiro.FornecedorFisico.Name : financeiro.FornecedorJuridico.RazaoSocial;
                    ViewData["Title"] = $"Parcelas – {name}";
                }
            }
            financeiro.Parcelas = financeiro.Parcelas.OrderBy(x => x.DataVencimentoParcela.Value).ToList();
            return View(financeiro);
        }

        [HttpPost]
        public IActionResult Contabilizar(int parcelaId, int financeiroId) {
            try {
                _financeiroRepositorio.ContabilizarParcela(parcelaId);
                Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(financeiroId);
                if (financeiro != null) {
                    string name;
                    if (!string.IsNullOrEmpty(financeiro.ContratoId.ToString())) {
                        name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                        ViewData["Title"] = $"Parcelas contrato Nº {financeiro.ContratoId} – {name}";
                    }
                    else {
                        if (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString()) || !string.IsNullOrEmpty(financeiro.PessoaJuridicaId.ToString())) {
                            name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                            ViewData["Title"] = $"Parcelas – {name}";
                        }
                        else {
                            name = (!string.IsNullOrEmpty(financeiro.FornecedorFisicoId.ToString())) ? financeiro.FornecedorFisico.Name : financeiro.FornecedorJuridico.RazaoSocial;
                            ViewData["Title"] = $"Parcelas – {name}";
                        }
                    }
                    TempData["MensagemDeSucesso"] = "Contabilizado com sucesso!";
                    financeiro.Parcelas = financeiro.Parcelas.OrderBy(x => x.DataVencimentoParcela.Value).ToList();
                    return View(financeiro);
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult ReturnClienteResponsavel(int? id) {
            PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(id.Value);
            if (pessoaFisica != null) {
                if (string.IsNullOrEmpty(pessoaFisica.Email)) {
                    pessoaFisica.Email = "Não foi informado.";
                }
                return PartialView("_ClienteResponsavelFisico", pessoaFisica);
            }
            PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(id.Value);
            if (pessoaJuridica != null) {
                if (string.IsNullOrEmpty(pessoaJuridica.Email)) {
                    pessoaJuridica.Email = "Não foi informado.";
                }
                return PartialView("_ClienteResponsavelJuridico", pessoaJuridica);
            }
            //returna um cliente fisico nulo para que a mensagem de id não encontrado seja captada na página.
            return PartialView("_ClienteResponsavelFisico", pessoaFisica);
        }

        public bool ValidationDateEmissaoAndVencimento(Financeiro financeiro) {
            if (financeiro.DataEmissao >= financeiro.DataVencimento) {
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
        public bool ValidationQtParcelas(Financeiro financeiro) {

            DateTime dateVencimento = DateTime.Parse(financeiro.DataVencimento.ToString());
            DateTime dataEmissao = DateTime.Parse(financeiro.DataEmissao.ToString());

            float dias = (float)dateVencimento.Subtract(dataEmissao).TotalDays;
            float ano = dias / 365;
            if (financeiro.Pagament == ModelPagament.Parcelado) {
                bool resultado = (financeiro.QtParcelas > ano * 12 || financeiro.QtParcelas < 2 || string.IsNullOrEmpty(financeiro.QtParcelas.ToString())) ? true : false;
                return resultado;
            }
            else {
                bool resultado = (financeiro.QtParcelas < 1 || string.IsNullOrEmpty(financeiro.QtParcelas.ToString())) ? true : false;
                return resultado;
            }
        }

        public IActionResult PdfRelatorioParcelas(int? id) {
            try {
                Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
                if (financeiro == null) {
                    ViewData["Title"] = "Financeiro";
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return RedirectToAction("Index");
                }
                var pxPorMm = 72 / 35.2f;
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
                new Font(fonteBase, 10, Font.NORMAL));
                Paragraph paragrofoRodape = new Paragraph("",
                new Font(fonteBase, 09, Font.NORMAL));
                paragrofoJustificado.Alignment = Element.ALIGN_JUSTIFIED;
                var titulo = new Paragraph($"Parcelas - {financeiro.ReturnNameClienteOrCredor()}\n\n\n", fonteParagrafo);
                titulo.Alignment = Element.ALIGN_LEFT;

                var caminhoImgLeft = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\anton\\Desktop\\Antonio\\faculdade\\Ws-vs2022\\BusesControl\\BusesControl\\wwwroot\\css\\Imagens\\LogoPdf.jpeg");
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

                var tabela = new PdfPTable(7);
                float[] larguraColunas = { 0.4f, 0.7f, 1.1f, 1f, 1f, 1f, 1.3f };
                tabela.SetWidths(larguraColunas);
                tabela.DefaultCell.BorderWidth = 0;
                tabela.WidthPercentage = 105;
                CriarCelulaTexto(tabela, "ID", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Nome", PdfPCell.ALIGN_CENTER, true);
                CriarCelulaTexto(tabela, "Valor das parcelas", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Taxa de juros", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Efetuação", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Vencimento", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Status da parcela", PdfPCell.ALIGN_CENTER, true);

                foreach (var item in financeiro.Parcelas.OrderBy(x => x.DataVencimentoParcela)) {
                    CriarCelulaTexto(tabela, item.Id.ToString(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnNomeParcela(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.Financeiro.ReturnValorParcela(), PdfPCell.ALIGN_CENTER);
                    CriarCelulaTexto(tabela, item.ReturnValorJuros(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnDateEfetuacao(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnDateVencimento(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnStatusPagamento(), PdfPCell.ALIGN_LEFT);
                }

                Paragraph footer = new Paragraph($"Data de emissão do documento: {DateTime.Now:dd/MM/yyyy}", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK));
                //footer.Alignment = Element.ALIGN_LEFT;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.WidthPercentage = 100f;
                footerTbl.TotalWidth = 1000f;
                footerTbl.HorizontalAlignment = 0;
                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 0;
                cell.HorizontalAlignment = 0;
                footerTbl.DefaultCell.HorizontalAlignment = 0;
                footerTbl.WidthPercentage = 100;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -30, 350, 30, writer.DirectContent);

                string rodape = $"Quantidade de parcelas: {financeiro.Parcelas.Count} " +
                                $"\nValor efetuado: {financeiro.ReturnValorTotEfetuado()}" +
                                $"\nValor total: {financeiro.ReturnValorTot()}";
                paragrofoRodape.Add(rodape);
                doc.Add(titulo);
                doc.Add(paragrofoJustificado);
                doc.Add(tabela);
                doc.Add(paragrofoRodape);
                doc.Close();

                string nomeContrato = $"relatório financeiro";
                stream.Flush();
                stream.Position = 0;
                return File(stream, "application/pdf", $"{nomeContrato}.pdf");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"Desculpe, houve um erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult PdfRelatorioFinanceiro(Filtros filtros) {
            try {
                ViewData["title"] = "Financeiro";
                List<Financeiro> financeiros = _financeiroRepositorio.ListFinanceirosFiltros(filtros);
                if (financeiros == null || filtros == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return RedirectToAction("Index");
                }
                var pxPorMm = 72 / 35.2f;
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
                new Font(fonteBase, 10, Font.NORMAL));
                Paragraph paragrofoRodape = new Paragraph("",
                new Font(fonteBase, 09, Font.NORMAL));
                paragrofoJustificado.Alignment = Element.ALIGN_JUSTIFIED;
                var titulo = new Paragraph($"Relatório financeiro\n\n\n", fonteParagrafo);
                titulo.Alignment = Element.ALIGN_CENTER;

                var caminhoImgLeft = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\anton\\Desktop\\Antonio\\faculdade\\Ws-vs2022\\BusesControl\\BusesControl\\wwwroot\\css\\Imagens\\LogoPdf.jpeg");
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
                var caminhoImgRight = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\anton\\Desktop\\Antonio\\faculdade\\Ws-vs2022\\BusesControl\\BusesControl\\wwwroot\\css\\Imagens\\LogoPdfRight.jpg");
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
                var tabela = new PdfPTable(7);
                float[] larguraColunas = { 0.5f, 1f, 1f, 1f, 1f, 1f, 1f };
                tabela.SetWidths(larguraColunas);
                tabela.DefaultCell.BorderWidth = 0;
                tabela.WidthPercentage = 105;
                CriarCelulaTexto(tabela, "ID", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Credor/Devedor", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Status financeiro", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Receita/Despesas", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Val total", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Val efetuado", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Vencimento", PdfPCell.ALIGN_LEFT, true);
                decimal valorTotAtivos = 0, valorTotInativos = 0, valEfetuado = 0;
                foreach (var item in financeiros.OrderBy(x => x.DespesaReceita)) {
                    if (item.FinanceiroStatus == FinanceiroStatus.Ativo) {
                        valorTotAtivos += item.ValorTotDR.Value;
                    }
                    else {
                        valorTotInativos += item.ValorTotDR.Value;
                    }
                    if (!string.IsNullOrEmpty(item.ValorTotalPagoCliente.ToString())) {
                        valEfetuado += item.ValorTotalPagoCliente.Value;
                    }
                    CriarCelulaTexto(tabela, item.Id.ToString(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnNameClienteOrCredor(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnStatusFinanceiro(), PdfPCell.ALIGN_CENTER);
                    CriarCelulaTexto(tabela, item.ReturnTypeFinanceiro(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnValorTot(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.ReturnValorTotEfetuado(), PdfPCell.ALIGN_LEFT);
                    CriarCelulaTexto(tabela, item.DataVencimento.Value.ToString("dd/MM/yyyy"), PdfPCell.ALIGN_LEFT);
                }

                Paragraph footer = new Paragraph($"Data de emissão do documento: {DateTime.Now:dd/MM/yyyy}", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK));
                //footer.Alignment = Element.ALIGN_LEFT;
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.WidthPercentage = 100f;
                footerTbl.TotalWidth = 1000f;
                footerTbl.HorizontalAlignment = 0;
                PdfPCell cell = new PdfPCell(footer);
                cell.Border = 0;
                cell.Colspan = 1;
                cell.PaddingLeft = 0;
                cell.HorizontalAlignment = 0;
                footerTbl.DefaultCell.HorizontalAlignment = 0;
                footerTbl.WidthPercentage = 100;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -30, 350, 30, writer.DirectContent);

                string rodape = $"Quantidade de lançamentos solicitados: {financeiros.Count}" +
                    $"\nValor total listado (ativos): {valorTotAtivos.ToString("C2")}" +
                    $"\nValor total listado (inativos): {valorTotInativos.ToString("C2")}" +
                    $"\nValor total efetuado: {valEfetuado.ToString("C2")}";
                if (filtros.DataFiltro == "não") filtros.DataFiltro = "nenhuma";
                string rodape3 = $"\nFiltros: lançamento = {filtros.ReceitasDespesas}, tipo de data = {filtros.DataFiltro}";
                if (!string.IsNullOrEmpty(filtros.DataInicial.ToString()) && !string.IsNullOrEmpty(filtros.DataTermino.ToString())
                    && filtros.DataFiltro != "nenhuma") {
                    rodape3 += $" ({filtros.DataInicial.Value.ToString("dd/MM/yyyy")} a {filtros.DataTermino.Value.ToString("dd/MM/yyyy")})";
                }
                string rodape2 = $"\nDocumento gerado em: {DateTime.Now.ToString("dd/MM/yyyy")}";
                paragrofoRodape.Add(rodape);
                paragrofoRodape.Add(rodape3);
                paragrofoRodape.Add(rodape2);
                doc.Add(titulo);
                doc.Add(paragrofoJustificado);
                doc.Add(tabela);
                doc.Add(paragrofoRodape);
                doc.Close();

                string nomeContrato = $"relatório financeiro";
                stream.Flush();
                stream.Position = 0;
                return File(stream, "application/pdf", $"{nomeContrato}.pdf");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"Desculpe, houve um erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
        public string ReturnValorPendente(decimal? valueTotal, decimal? valuePago) {
            decimal result = valueTotal.Value - valuePago.Value;
            return $"{result.ToString("C2")}";
        }
        static void CriarCelulaTexto(PdfPTable tabela, string texto, int alinhamentoHorz = PdfPCell.ALIGN_LEFT,
                bool negrito = false, bool italico = false, int tamanhoFont = 10, int alturaCelula = 30) {

            int estilo = Font.NORMAL;
            if (negrito && italico) {
                estilo = Font.BOLDITALIC;
            }
            else if (negrito) {
                estilo = Font.BOLD;
            }
            else if (italico) {
                estilo = Font.ITALIC;
            }
            var fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            var fonteCelula = new Font(fonteBase, tamanhoFont, estilo, BaseColor.DARK_GRAY);
            var bgColor = BaseColor.WHITE;
            if (tabela.Rows.Count % 2 == 1) {
                bgColor = new BaseColor(0.95f, 0.95f, 0.95f);
            }
            var celula = new PdfPCell(new Phrase(texto, fonteCelula));
            celula.HorizontalAlignment = alinhamentoHorz;
            celula.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            celula.Border = 0;
            celula.BorderWidthBottom = 1;
            celula.BackgroundColor = bgColor;
            celula.FixedHeight = alturaCelula;
            tabela.AddCell(celula);
        }
    }
}
