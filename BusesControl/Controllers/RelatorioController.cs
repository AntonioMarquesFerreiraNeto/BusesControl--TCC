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
using System.Reflection.Metadata;
using Document = iTextSharp.text.Document;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class RelatorioController : Controller {

        private readonly IRelatorioRepositorio _relatorioRepositorio;
        private readonly IContratoRepositorio _contratoRepositorio;

        public RelatorioController(IRelatorioRepositorio relatorioRepositorio, IContratoRepositorio contratoRepositorio) {
            _relatorioRepositorio = relatorioRepositorio;
            _contratoRepositorio = contratoRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Relatórios de contratos";
            ModelsRelatorio modelsRelatorio = new ModelsRelatorio();
            modelsRelatorio.Relatorio = PopularRelatorio();
            modelsRelatorio.Contratos = _contratoRepositorio.ListContratoAprovados();
            return View(modelsRelatorio);
        }
        public Relatorio PopularRelatorio() {
            Relatorio relatorio = new Relatorio();
            relatorio.ValTotAprovados = _relatorioRepositorio.ValorTotAprovados();
            relatorio.ValTotEmAnalise = _relatorioRepositorio.ValorTotEmAnalise();
            relatorio.ValTotContratos = _relatorioRepositorio.ValorTotContratos();
            relatorio.ValTotPago = _relatorioRepositorio.ValorTotPagoContrato();
            relatorio.ValTotPendente = _relatorioRepositorio.ValorTotPendenteContrato();
            relatorio.QtContratos = _relatorioRepositorio.QtContratos();
            relatorio.QtContratosAprovados = _relatorioRepositorio.QtContratosAprovados();
            relatorio.QtContratosNegados = _relatorioRepositorio.QtContratosNegados();
            relatorio.QtContratosEmAnalise = _relatorioRepositorio.QtContratosEmAnalise();
            relatorio.QtClientes = _relatorioRepositorio.QtClientes();
            relatorio.QtClientesAdimplente = _relatorioRepositorio.QtClientesAdimplentes();
            relatorio.QtClientesInadimplente = _relatorioRepositorio.QtClientesInadimplentes();
            relatorio.QtMotorista = _relatorioRepositorio.QtMotoristas();
            relatorio.QtMotoristaVinculado = _relatorioRepositorio.QtMotoristasVinculados();
            relatorio.QtOnibus = _relatorioRepositorio.QtOnibus();
            relatorio.QtOnibusVinculado = _relatorioRepositorio.QtOnibusVinculados();
            return relatorio;
        }

        public IActionResult ClientesContrato(int? id) {
            Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
            List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View("Index", listContratos);
            }
            return PartialView("_ClientesContrato", contrato);
        }

        public IActionResult PdfContrato(int? id) {
            try {
                ViewData["title"] = "Relatórios de contratos";
                Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
                if (contrato == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    ModelsRelatorio modelsRelatorio = new ModelsRelatorio();
                    modelsRelatorio.Relatorio = PopularRelatorio();
                    modelsRelatorio.Contratos = _contratoRepositorio.ListContratoAprovados();
                    return View("Index", modelsRelatorio);
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
                new Font(fonteBase, 10, Font.NORMAL));
                Paragraph paragrofoRodape = new Paragraph("",
                new Font(fonteBase, 09, Font.NORMAL));
                paragrofoJustificado.Alignment = Element.ALIGN_JUSTIFIED;
                var titulo = new Paragraph($"Relatório - contrato Nº {contrato.Id}\n\n", fonteParagrafo);
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

                var tabela = new PdfPTable(5);
                float[] larguraColunas = { 1.5f, 1f, 1f, 1f, 1f };
                tabela.SetWidths(larguraColunas);
                tabela.DefaultCell.BorderWidth = 0;
                tabela.WidthPercentage = 105;
                CriarCelulaTexto(tabela, "Cliente", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Situação", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Total pago", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Total de juros", PdfPCell.ALIGN_LEFT, true);
                CriarCelulaTexto(tabela, "Total de juros pago", PdfPCell.ALIGN_LEFT, true);
                foreach (var item in contrato.Financeiros) {
                    string situacao;
                    if (!string.IsNullOrEmpty(item.PessoaFisicaId.ToString())) {
                        situacao = (item.PessoaFisica.Adimplente == Adimplencia.Adimplente) ? "Adimplente" : "Inadimplente";
                        CriarCelulaTexto(tabela, item.PessoaFisica.Name, PdfPCell.ALIGN_LEFT);
                        CriarCelulaTexto(tabela, situacao, PdfPCell.ALIGN_LEFT);
                    }
                    else {
                        situacao = (item.PessoaJuridica.Adimplente == Adimplencia.Adimplente) ? "Adimplente" : "Inadimplente";
                        CriarCelulaTexto(tabela, item.PessoaJuridica.NomeFantasia, PdfPCell.ALIGN_LEFT);
                        CriarCelulaTexto(tabela, situacao, PdfPCell.ALIGN_LEFT);
                    }
                    if (!string.IsNullOrEmpty(item.ValorTotalPagoCliente.ToString())) {
                        CriarCelulaTexto(tabela, item.ValorTotalPagoCliente.Value.ToString("C2"), PdfPCell.ALIGN_LEFT);
                    }
                    else {
                        CriarCelulaTexto(tabela, "R$ 0,00", PdfPCell.ALIGN_LEFT);
                    }
                    decimal? valorTotJuros = _relatorioRepositorio.ValorTotJurosCliente(item.Id);
                    if (!string.IsNullOrEmpty(valorTotJuros.ToString())) {
                        CriarCelulaTexto(tabela, valorTotJuros.Value.ToString("C2"), PdfPCell.ALIGN_LEFT);
                        if (!string.IsNullOrEmpty(item.ValorTotTaxaJurosPaga.ToString())) {
                            CriarCelulaTexto(tabela, item.ValorTotTaxaJurosPaga.Value.ToString("C2"), PdfPCell.ALIGN_LEFT);
                        }
                        else {
                            CriarCelulaTexto(tabela, "R$ 0,00", PdfPCell.ALIGN_LEFT);
                        }
                    }
                }
                string pularLinha = "\n\n";
                if (!string.IsNullOrEmpty(contrato.ValorTotalPagoContrato.ToString())) {
                    string paragrafoValoresContrato = $"(Valor total pago: {contrato.ValorTotalPagoContrato.Value.ToString("C2")}; " +
                    $"Valor total pendente: {ReturnValorPendente(contrato.ValorMonetario, contrato.ValorTotalPagoContrato)}; Valor total do contrato: {contrato.ValorMonetario.Value.ToString("C2")})\n";
                    string paragrafoValoresPorCliente = $"( Quantidade de clientes: {contrato.ClientesContratos.Count}; Valor total por cliente: {contrato.ReturnValorTotCliente()} )\n\n";
                    paragrofoJustificado.Add(pularLinha);
                    paragrofoJustificado.Add(paragrafoValoresContrato);
                    paragrofoJustificado.Alignment = Element.ALIGN_CENTER;
                    paragrofoJustificado.Add(paragrafoValoresPorCliente);
                }
                else {
                    string paragrafoValoresContrato = $"( Valor total pago: R$ 0,00; " +
                    $"Valor total pendente: {contrato.ValorMonetario.Value.ToString("C2")}; Valor total do contrato: {contrato.ValorMonetario.Value.ToString("C2")} )\n";
                    string paragrafoValoresPorCliente = $"( Quantidade de clientes: {contrato.ClientesContratos.Count}; Valor total por cliente: {contrato.ReturnValorTotCliente()} )\n\n";
                    paragrofoJustificado.Add(pularLinha);
                    paragrofoJustificado.Add(paragrafoValoresContrato);
                    paragrofoJustificado.Alignment = Element.ALIGN_CENTER;
                    paragrofoJustificado.Add(paragrafoValoresPorCliente);
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

                doc.Add(titulo);
                doc.Add(paragrofoJustificado);
                doc.Add(tabela);
                if (contrato.Rescisoes != null || contrato.Rescisoes.Any()) {
                    Paragraph tituloJustificado = new Paragraph("",
                    new Font(fonteBase, 13, Font.NORMAL, BaseColor.DARK_GRAY));
                    tituloJustificado.Alignment = Element.ALIGN_CENTER;
                    string tituloRescisao = "\n\nTabela de rescisão do contrato\n\n";
                    //Tabela que contém os clientes que rescendiram o contrato. 
                    var tabelaRescisao = new PdfPTable(5);
                    float[] larguraColunas2 = { 1.5f, 1f, 1f, 1f, 1f };
                    tabelaRescisao.SetWidths(larguraColunas2);
                    tabelaRescisao.DefaultCell.BorderWidth = 0;
                    tabelaRescisao.WidthPercentage = 105;
                    CriarCelulaTexto(tabelaRescisao, "Cliente", PdfPCell.ALIGN_LEFT, true);
                    CriarCelulaTexto(tabelaRescisao, "Situação", PdfPCell.ALIGN_LEFT, true);
                    CriarCelulaTexto(tabelaRescisao, "Data de rescisão", PdfPCell.ALIGN_LEFT, true);
                    CriarCelulaTexto(tabelaRescisao, "Total pago", PdfPCell.ALIGN_LEFT, true);
                    CriarCelulaTexto(tabelaRescisao, "Valor efetuado pela multa", PdfPCell.ALIGN_LEFT, true);

                    foreach (var item in contrato.Rescisoes) {
                        string situacao;
                        if (!string.IsNullOrEmpty(item.PessoaFisicaId.ToString())) {
                            situacao = (item.PessoaFisica.Adimplente == Adimplencia.Adimplente) ? "Adimplente" : "Inadimplente";
                            CriarCelulaTexto(tabelaRescisao, item.PessoaFisica.Name, PdfPCell.ALIGN_LEFT);
                            CriarCelulaTexto(tabelaRescisao, situacao, PdfPCell.ALIGN_LEFT);
                        }
                        else {
                            situacao = (item.PessoaJuridica.Adimplente == Adimplencia.Adimplente) ? "Adimplente" : "Inadimplente";
                            CriarCelulaTexto(tabelaRescisao, item.PessoaJuridica.NomeFantasia, PdfPCell.ALIGN_LEFT);
                            CriarCelulaTexto(tabelaRescisao, situacao, PdfPCell.ALIGN_LEFT);
                        }
                        CriarCelulaTexto(tabelaRescisao, item.DataRescisao.Value.ToString("dd/MM/yyyy"), PdfPCell.ALIGN_LEFT);
                        if (!string.IsNullOrEmpty(item.ValorPagoContrato.ToString())) {
                            CriarCelulaTexto(tabelaRescisao, item.ValorPagoContrato.Value.ToString("C2"), PdfPCell.ALIGN_LEFT);
                        }
                        else {
                            CriarCelulaTexto(tabelaRescisao, "R$ 0,00", PdfPCell.ALIGN_LEFT);
                        }
                        CriarCelulaTexto(tabelaRescisao, item.Multa.Value.ToString("C2"), PdfPCell.ALIGN_LEFT);
                        
                        //Adicionando a tabela no documento e posicionando os mesmos.
                    }
                    tituloJustificado.Add(tituloRescisao);
                    doc.Add(tituloJustificado);
                    doc.Add(tabelaRescisao);
                }
                doc.Add(paragrofoRodape);
                doc.Close();

                string nomeContrato = $"contrato {contrato.Id}";
                stream.Flush();
                stream.Position = 0;

                return File(stream, "application/pdf", $"Relatório - {nomeContrato}.pdf");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"Desculpe, houve um erro: {erro.Message}";
                ModelsRelatorio modelsRelatorio = new ModelsRelatorio();
                modelsRelatorio.Relatorio = PopularRelatorio();
                modelsRelatorio.Contratos = _contratoRepositorio.ListContratoAprovados();
                return View("Index", modelsRelatorio);
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
