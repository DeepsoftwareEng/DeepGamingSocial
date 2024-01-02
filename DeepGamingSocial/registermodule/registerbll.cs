using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGamingSocial.registermodule
{
    public class registerbll
    {
        registerdal dalregister;
        public registerbll() {
            dalregister = new registerdal();
        }
        public bool registeraccount(useraccount t)
        {
            return dalregister.registeraccount(t);
        }
    }
}
