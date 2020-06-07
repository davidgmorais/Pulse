using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class ReceitaInfo
    {
        public string id { get; set; }
        public string data { get; set; }
        public string codigoMedico { get; set; }
        public string medico { get; set; }

        public ReceitaInfo(String id, String data, String codigo, String nome)
        {
            this.id = id;
            this.data = data;
            this.codigoMedico = codigo;
            this.medico = nome;
        }
    }
}
