using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class Doctor
    {
        public String Nome { get; set; }
        public String Codigo { get; set; }
        public Doctor(String c, String n)
        {
            this.Nome = n;
            this.Codigo = c;
        }
    }
}
