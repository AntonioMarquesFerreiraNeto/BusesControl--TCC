using BusesControl.Models;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Data {
    public class BancoContext : DbContext {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {
        }

        //Tabela cliente está sendo criada e depois acessada. 
        public DbSet<Cliente> Cliente { get; set; }
    }
}
