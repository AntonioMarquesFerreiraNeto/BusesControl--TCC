using BusesControl.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusesControl.ViewComponents {
    public class MenuSec : ViewComponent {
        public async Task<IViewComponentResult> InvokeAsync() {

            string sectionUser = HttpContext.Session.GetString("sectionUserAutenticado");

            if (string.IsNullOrEmpty(sectionUser)) {
                return null;
            }
            Funcionario funcionario = JsonConvert.DeserializeObject<Funcionario>(sectionUser);
            return View(funcionario);
        }
    }
}
