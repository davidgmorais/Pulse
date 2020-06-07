using System;

namespace Pulse
{
    internal class Paciente
    {
        private string Nome;
        private string Estado;
        private string Localizacao;

        public Paciente(string nome, string estado, string localizacao)
        {
            Nome = nome;
            Estado = estado;
            Localizacao = localizacao;
        }

        public string getName()
        {
            return this.Nome;
        }

        public string getEstado()
        {
            return this.Estado;
        }

        public string getLocal()
        {
            return this.Localizacao;
        }
    }
}