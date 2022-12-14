using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class AprovarContratoController : Controller {
        private readonly IContratoRepositorio _contratoRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;

        public AprovarContratoController(IContratoRepositorio contratoRepositorio, IClienteRepositorio clienteRepositorio) {
            _contratoRepositorio = contratoRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index() {
            List<Contrato> ContratosList = _contratoRepositorio.ListContratoEmAnalise();
            ViewData["Title"] = "Contratos em análise";
            return View(ContratosList);
        }

        public IActionResult Negados() {
            List<Contrato> ContratoList = _contratoRepositorio.ListContratoNegados();
            ViewData["Title"] = "Contratos negados";
            return View("Index", ContratoList);
        }

        public IActionResult Aprovados() {
            List<Contrato> ContratoList = _contratoRepositorio.ListContratoAprovados();
            ViewData["Title"] = "Contratos aprovados";
            return View("Index", ContratoList);
        }

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

        public IActionResult ClientesContratoPdf(int? id) {
            Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
            List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View("Index", listContratos);
            }
            return PartialView("_ClientesContratoPdf", contrato);
        }
        public IActionResult PdfContrato(int? id, int? clienteFisicoId, int? clienteJuridicoId) {

            ViewData["title"] = "Contratos aprovados";
            List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
            Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return View("Index", listContratos);
            }

            var pxPorMm = 72 / 25.2f;
            Document doc = new Document(PageSize.A4, 15 * pxPorMm, 15 * pxPorMm,
                15 * pxPorMm, 15 * pxPorMm);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            writer.CloseStream = false;

            doc.Open();

            var fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 17,
                iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph paragrofoJustificado = new Paragraph("",
            new Font(fonteBase, 12, Font.NORMAL));
            paragrofoJustificado.Alignment = Element.ALIGN_JUSTIFIED;

            Paragraph paragrafoCenter = new Paragraph("", new Font(fonteBase, 12, Font.NORMAL));
            paragrafoCenter.Alignment = Element.ALIGN_CENTER;

            var titulo = new Paragraph($"Contrato de prestação de serviço Nº {contrato.Id}\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_CENTER;

            string titulo_contratante = "\nCONTRATANTE:";

            string textoContratante;
            string nomeCliente;
            if (!string.IsNullOrEmpty(clienteFisicoId.ToString())) {
                PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(clienteFisicoId.Value);
                if (pessoaFisica == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return View("Index", listContratos);
                }
                nomeCliente = pessoaFisica.Name;
                textoContratante = $"{titulo_contratante} \n\n{pessoaFisica.Name} portadora do " +
                $"CPF: {pessoaFisica.ReturnCpfCliente()}, RG: {pessoaFisica.Rg}, filho(a) da Sr. {pessoaFisica.NameMae}, residente domiciliado no imovel Nº {pessoaFisica.NumeroResidencial}, do bairro {pessoaFisica.Bairro}," +
                $" na cidade de {pessoaFisica.Cidade} — {pessoaFisica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaFisica.Ddd}){pessoaFisica.ReturnTelefoneCliente()}, {pessoaFisica.Email}. Neste ato representada como o requerente do contrato.\n\n\n";
            }
            else {
                PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(clienteJuridicoId.Value);
                if (pessoaJuridica == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return View("Index", listContratos);
                }
                nomeCliente = pessoaJuridica.NomeFantasia;
                textoContratante = $"{titulo_contratante} \n\n{pessoaJuridica.RazaoSocial}, inscrita no CNPJ: {pessoaJuridica.ReturnCnpjCliente()}, inscrição estadual: {pessoaJuridica.InscricaoEstadual}, inscrição municipal: {pessoaJuridica.InscricaoMunicipal}, portadora do nome fantasia {pessoaJuridica.NomeFantasia}, " +
                $"residente domiciliado no imovel Nº {pessoaJuridica.NumeroResidencial}, do bairro {pessoaJuridica.Bairro}," +
                $" na cidade de {pessoaJuridica.Cidade} — {pessoaJuridica.Estado}. Tendo como forma de contato os seguintes canais: ({pessoaJuridica.Ddd}){pessoaJuridica.ReturnTelefoneCliente()}, {pessoaJuridica.Email}. Neste ato representada como a requerente do contrato.\n\n\n";
            }

            string titulo_contratada = $"CONTRATADA:";
            string textoContratada = $"{titulo_contratada} \n\nBuss viagens LTDA, pessoa jurídica de direito privado para prestação de serviço, na proteção da LEI Nº 13.429º. " +
                $"Localizada na cidade de Goianésia (GO) — Brasil, inscrita no CNPJ nº 03.115.484/0001-02, sobre a liderança do sócio fundador Manoel Rodrigues." +
                $" Neste ato representada como  a empresa responsável pela realização da prestações de serviços do contrato.\n\n\n";

            string titulo_primeira_clausula = $"1 — CLÁUSULA PRIMEIRA";
            string PrimeiraClausula = $"{titulo_primeira_clausula}\n\nO presente contrato tem por objeto a prestação de serviço especial de transporte rodoviário na rota definida no registro do contrato: {contrato.Detalhamento}\n\n\n";

            string titulo_segunda_clausula = $"2 — CLÁUSULA SEGUNDA";
            string SegundaClausula = $"{titulo_segunda_clausula} \n\nO(s) veículo(s) que realizará(ão) o transporte será(ão) discriminado(s) a seguir: \n" +
                $"  • Veículo {contrato.Onibus.Marca}, modelo {contrato.Onibus.NameBus}, placa {contrato.Onibus.Placa}, número de chassi {contrato.Onibus.Chassi}, da cor {contrato.Onibus.ReturnCorBus()}, fabricado em {contrato.Onibus.DataFabricacao}, e com capacidade de lotação para {contrato.Onibus.Assentos} passageiros.\n No caso de problemas com o(s) veículo(s) acima designado(s), " +
                $"poderá ser utilizado outro veículo, desde que conste habilitado no Sistema de Habilitação de Transportes de Passageiros – SisHAB, da ANTT. \n\n";

            string titulo_terceira_clausula = $"\n3 — CLÁUSULA TERCEIRA";
            string TerceiraClausula = $"{titulo_terceira_clausula} \n\nO contratante deve estar ciente que deverá cumprir com as datas de pagamento determinadas do contrato. Desta forma, estando ciente de valores de juros adicionais em caso de inadimplência. Nos quais são dois porcentos ao mês por parcela atrasada.\n\n\n";

            string titulo_quarta_clausula = $"4 — CLÁUSULA QUARTA";
            string QuartaClausula;
            if (contrato.Pagament == Models.Enums.ModelPagament.Avista) {
                QuartaClausula = $"{titulo_quarta_clausula} \n\nPelos serviços prestados a Contratante pagará a Contratada o valor de {contrato.ReturnValorTotCliente()}, na data atual. Em parcela única, pois, o contrato foi deferido como à vista.\n\n";
            }
            else {
                QuartaClausula = $"{titulo_quarta_clausula} \n\nPelos serviços prestados a Contratante pagará a Contratada o valor de {contrato.ReturnValorTotCliente()}, e os respectivos pagamentos serão realizados dia {contrato.ReturnDiaPagamento()} de cada mês. Dividos em {contrato.QtParcelas} parcelas no valor {contrato.ValorParcelaContratoPorCliente.Value.ToString("C2")}.\n\n";
            }
            string titulo_quinta_clausula = $"\n5 — CLÁUSULA QUINTA";
            string QuintaClausula = $"{titulo_quinta_clausula}\n\n Em caso de rescisão de contrato anterior a data acordada, o cliente deve estar ciente que haverá multa de 10% do valor do contrato pela rescisão do contrato.\n\n\n";

            string titulo_sexta_clausula = $"6 — CLÁUSULA SEXTA";
            string SextaClausula = $"{titulo_sexta_clausula} \n\nO período da prestação do serviço será de  {contrato.ReturnDateContrato()}, que é a data acordada no registro do contrato.\n\n\n";

            string titulo_setima_clausula = $"7 — CLÁUSULA SÉTIMA";
            string SetimaClausula = $"{titulo_setima_clausula} \n\nO CONTRATANTE fica ciente que somente será permitido o transporte de passageiros limitados à capacidade de passageiros sentados no(s) veículo(s) utilizado(s), ficando expressamente proibido o transporte de passageiros em pé ou acomodados no corredor, bem como passageiros que não estiverem constando na relação autorizada pela ANTT.\n\n\n";

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
    }
}
