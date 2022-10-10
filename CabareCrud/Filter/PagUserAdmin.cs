using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace BusesControl.Filter {
    public class PagUserAdmin : ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            string sectionUser = filterContext.HttpContext.Session.GetString("sectionUserAutenticado");

            if (string.IsNullOrEmpty(sectionUser)) {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Logar" }, { "action", "Index" } });
            }
            else {
                Funcionario usuario = JsonConvert.DeserializeObject<Funcionario>(sectionUser);
                if (usuario == null) {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Logar" }, { "action", "Index" } });
                }
                if (usuario.Cargos != CargoFuncionario.Administrador) {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "HomeAcessoNegado" }, { "action", "Index" } });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
