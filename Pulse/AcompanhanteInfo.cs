using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class AcompanhanteInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Location { get; set; }

        public AcompanhanteInfo(String Code, String nome, String estado, String localizacao)
        {
            this.Code = Code;
            this.Name = nome;
            this.State = estado;
            this.Location = localizacao;

        }
    }
}
