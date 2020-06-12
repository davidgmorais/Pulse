using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{

	public class User
    {
		private String Codigo;
		private String Nome;
		private String DataNascimento;
		private String morada;
		private String email;
		private String telefone;
		private String telemovel;
		private String NIF;

		public User(string codigo, string nome, string dataNascimento, string morada, string email, string telefone, string telemovel, string nIF) 
		{
			this.Codigo = codigo;
			this.Nome = nome;
			this.DataNascimento = dataNascimento;
			this.morada = morada;
			this.email = email;
			this.telefone = telefone;
			this.telemovel = telemovel;
			this.NIF = nIF;
		}

		public User(String Nome, String Codigo, String Email, String Data, String NIF)
		{
			this.Nome = Nome;
			this.Codigo = Codigo;           
			this.email = Email;
			this.DataNascimento = Data;
			this.NIF = NIF;
		}



		public String getNome()
		{
			return this.Nome;
		}

		public String getCode()
		{
			return this.Codigo;
		}

		public String getBDay()
		{
			return this.DataNascimento;
		}

		public String getEmail()
		{
			return this.email;
		}

		public String getNIF()
		{
			return this.NIF;
		}

		public String getMorada()
		{
			return this.morada;
		}

		public String getTelefone()
		{
			return this.telefone;
		}
		
		public String getTelemovel()
		{
			return this.telemovel;
		}

		public void setTelefone(String telefone)
		{
			this.telefone = telefone;
		}

		public void setMorada(String morada)
		{
			this.morada = morada;
		}

		public void setTelemove(String telefone)
		{
			this.telemovel = telefone;
		}
		public void setEmail(String e)
		{
			this.email = e;
		}
		

	}
}
