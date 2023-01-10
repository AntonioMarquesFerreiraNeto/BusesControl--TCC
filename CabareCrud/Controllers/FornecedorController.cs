using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class FornecedorController : Controller {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        public FornecedorController(IFornecedorRepositorio fornecedorRepositorio) {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Fornecedores físicos habilitados";
            List<FornecedorFisico> fornecedoresHabilitados = _fornecedorRepositorio.ListFornecedoreFisicos();
            return View(fornecedoresHabilitados);
        }
        public IActionResult Desabilitados() {
            ViewData["Title"] = "Fornecedores físicos desabilitados";
            List<FornecedorFisico> fornecedoresDesabilitados = _fornecedorRepositorio.ListFornecedoreFisicoDesa();
            return View("Index", fornecedoresDesabilitados);
        }
        public IActionResult IndexJuridico() {
            ViewData["Title"] = "Fornecedores jurídicos habilitados";
            List<FornecedorJuridico> fornecedoresJuridicos = _fornecedorRepositorio.ListFornecedoresJuridicos();
            return View(fornecedoresJuridicos);
        }
        public IActionResult JuridicosDesabilitados() {
            ViewData["Title"] = "Fornecedores jurídicos desabilitados";
            List<FornecedorJuridico> fornecedoresJuridicos = _fornecedorRepositorio.ListFornecedoreJuriDesa();
            return View("IndexJuridico", fornecedoresJuridicos);
        }

        public IActionResult NovoFornecedorFisico() {
            TempData["MensagemDeInfo"] = "O e-mail não é obrigatório para clientes/fornecedores.";
            ViewData["Title"] = "Incluir fonecedor físico";
            return View();
        }
        [HttpPost]
        public IActionResult NovoFornecedorFisico(FornecedorFisico fornecedorFisico) {
            ViewData["Title"] = "Incluir fornecedor físico";
            try {
                if (ModelState.IsValid) {
                    _fornecedorRepositorio.AdicionarFornecedorFisico(fornecedorFisico);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(fornecedorFisico);

            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorFisico);
            }
        }

        public IActionResult NovoFornecedorJuridico() {
            TempData["MensagemDeInfo"] = "O e-mail não é obrigatório para clientes/fornecedores.";
            ViewData["Title"] = "Incluir fonecedor jurídico";
            return View();
        }
        [HttpPost]
        public IActionResult NovoFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            ViewData["Title"] = "Incluir fonecedor jurídico";
            try {
                if (ModelState.IsValid) {
                    _fornecedorRepositorio.AdicionarFornecedorJuridico(fornecedorJuridico);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("IndexJuridico");
                }
                return View(fornecedorJuridico);

            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorJuridico);
            }
        }

        public IActionResult EditarFornecedorFisico(int id) {
            ViewData["Title"] = "Editar fornecedor";
            FornecedorFisico fornecedorFisico = _fornecedorRepositorio.ListPorIdFisico(id);
            if (fornecedorFisico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            return View(fornecedorFisico);
        }
        [HttpPost]
        public IActionResult EditarFornecedorFisico(FornecedorFisico fornecedorFisico) {
            ViewData["Title"] = "Editar fornecedor";
            try {
                if (ModelState.IsValid) {
                    _fornecedorRepositorio.EditarFornecedorFisico(fornecedorFisico);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(fornecedorFisico);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorFisico);
            }
        }

        public IActionResult EditarFornecedorJuridico(int id) {
            ViewData["Title"] = "Editar fornecedor";
            FornecedorJuridico fornecedorJuridico = _fornecedorRepositorio.ListPorIdJuridico(id);
            if (fornecedorJuridico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("IndexJuridico");
            }
            return View(fornecedorJuridico);
        }
        [HttpPost]
        public IActionResult EditarFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            ViewData["Title"] = "Editar fornecedor";
            try {
                if (ModelState.IsValid) {
                    _fornecedorRepositorio.EditarFornecedorJuridico(fornecedorJuridico);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("IndexJuridico");
                }
                return View(fornecedorJuridico);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorJuridico);
            }
        }


        public IActionResult DesabFornecedorFisico(int id) {
            ViewData["Title"] = "Desabilitar";
            FornecedorFisico fornecedorFisico = _fornecedorRepositorio.ListPorIdFisico(id);
            if (fornecedorFisico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return View(fornecedorFisico);
            }
            return View(fornecedorFisico);
        }
        [HttpPost]
        public IActionResult DesabFornecedorFisico(FornecedorFisico fornecedorFisico) {
            ViewData["Title"] = "Desabilitar";
            FornecedorFisico fornecedorFisicoError = _fornecedorRepositorio.ListPorIdFisico(fornecedorFisico.Id);
            try {
                _fornecedorRepositorio.InativarFornecedorFisico(fornecedorFisico);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorFisicoError);
            }
        }

        public IActionResult DesabFornecedorJuridico(int id) {
            ViewData["Title"] = "Desabilitar";
            FornecedorJuridico fornecedorJuridico = _fornecedorRepositorio.ListPorIdJuridico(id);
            if (fornecedorJuridico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return View(fornecedorJuridico);
            }
            return View(fornecedorJuridico);
        }
        [HttpPost]
        public IActionResult DesabFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            ViewData["Title"] = "Desabilitar";
            FornecedorJuridico fornecedorJuridicoError = _fornecedorRepositorio.ListPorIdJuridico(fornecedorJuridico.Id);
            try {
                _fornecedorRepositorio.InativarFornecedorJuridico(fornecedorJuridico);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("IndexJuridico");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorJuridicoError);
            }
        }

        public IActionResult HabFornecedorFisico(int id) {
            ViewData["Title"] = "Habilitar";
            FornecedorFisico fornecedorFisico = _fornecedorRepositorio.ListPorIdFisico(id);
            if (fornecedorFisico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return View(fornecedorFisico);
            }
            return View(fornecedorFisico);
        }

        [HttpPost]
        public IActionResult HabFornecedorFisico(FornecedorFisico fornecedorFisico) {
            ViewData["Title"] = "Habilitar";
            FornecedorFisico fornecedorFisicoError = _fornecedorRepositorio.ListPorIdFisico(fornecedorFisico.Id);
            try {
                _fornecedorRepositorio.AtivarFornecedorFisico(fornecedorFisico);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorFisicoError);
            }
        }

        public IActionResult HabFornecedorJuridico(int id) {
            ViewData["Title"] = "Habilitar";
            FornecedorJuridico fornecedorJuridico = _fornecedorRepositorio.ListPorIdJuridico(id);
            if (fornecedorJuridico == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return View(fornecedorJuridico);
            }
            return View(fornecedorJuridico);
        }

        [HttpPost]
        public IActionResult HabFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            ViewData["Title"] = "Habilitar";
            FornecedorJuridico fornecedorJuridicoError = _fornecedorRepositorio.ListPorIdJuridico(fornecedorJuridico.Id);
            try {
                _fornecedorRepositorio.AtivarFornecedorJuridico(fornecedorJuridico);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("IndexJuridico");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(fornecedorJuridicoError);
            }
        }
    }
}
