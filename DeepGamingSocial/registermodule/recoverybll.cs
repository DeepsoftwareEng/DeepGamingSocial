using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGamingSocial.registermodule
{
    public class recoverybll
    {
        recovery recoverydal;
        public recoverybll()
        {
            recoverydal = new recovery();
        }
        public bool recoveryacc(useraccount t)
        {
            return recoverydal.Updateaccount(t);
        }
    }
}
