using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGamingSocial.registermodule
{
    public class Registerdbcontext : DbContext
    {
        static string z = "useraccount";
        public Registerdbcontext() : base(z)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<Registerdbcontext>());
        }
        public DbSet<useraccount>  useraccounts { get; set;}
    }
}
