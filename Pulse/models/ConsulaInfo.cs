using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class ConsulaInfo
    {
        public String Data { get; set; }
        public String Consultorio { get; set; }
        public String Hora { get; set; }
        public String Nome { get; set; }

        public ConsulaInfo(String Data, String Consultorio, String hora, String paciente)
        {
            this.Data = Data;
            this.Consultorio = Consultorio;
            this.Hora = hora;
            this.Nome = paciente;
        }
    }
}
