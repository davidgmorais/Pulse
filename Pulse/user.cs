﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{



	//2 construtores
	//1 para medicos + acompanhantes
	//1 para pacientes
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


		public User(String Nome, String Codigo, String Email, String Data, String NIF)
		{
			this.Nome = Nome;
			this.Codigo = Codigo;           //parse int??
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


	}
}