using BusesControl.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BusesControl.Helper {
    public class Section : ISection {

        private readonly IHttpContextAccessor _httpContext;

        public Section(IHttpContextAccessor httpContext) { 
            _httpContext = httpContext;
        }

        public Funcionario buscarSectionUser() {
            string sectionUser = _httpContext.HttpContext.Session.GetString("sectionUserAutenticado");
            if (string.IsNullOrEmpty(sectionUser)) {
                return null;
            }
            else {
                return JsonConvert.DeserializeObject<Funcionario>(sectionUser);
            }
        }

        public void CriarSection(Funcionario usuario) {
            string valor = JsonConvert.SerializeObject(usuario);
            _httpContext.HttpContext.Session.SetString("sectionUserAutenticado", valor);
        }

        public void EncerrarSection() {
            _httpContext.HttpContext.Session.Remove("sectionUserAutenticado");
        }
    }
}
