using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class Medicamento
    {
        public int index { get; set; }
        public string id { get; set; }
        public string nome { get; set; }
        public string dosagem { get; set; }

        public Medicamento(String id, String nome, String dosagem)
        {
            this.id = id;
            this.nome = nome;
            this.dosagem = dosagem;
        }
    }
}
