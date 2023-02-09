using BusesControl.Filter;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class HomeAcessoNegadoController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Principal";
            TempData["MensagemDeErro"] = "Desculpe, seu acesso foi negado!";
            return View();
        }
    }
}
