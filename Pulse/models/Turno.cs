using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class Turno
    {
        public string day { get; set; }
        public string horaInicio { get; set; }
        public string horaFim { get; set; }
        public string description { get; set; }


        public Turno(String dia, String horaI, String horaF, String d)
        {
            this.day = dia;
            this.horaInicio = horaI;
            this.horaFim = horaF;
            this.description = d;
        }
    }
}
