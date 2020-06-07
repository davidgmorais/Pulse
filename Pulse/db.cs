using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse
{
    class db
    {
        private static SqlConnection cn;

        public static void innit()
        {
            cn = GetSGBDConnection();
        }

        private static SqlConnection GetSGBDConnection()
        {
            return new SqlConnection("Data Source=DESKTOP-HB27C6M;Initial Catalog=Pulse;Integrated Security=True");

        }
        private static bool VerifySGBDConnection()
        {
            if (cn == null)
                cn = GetSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        //PAGE 1 FUNCTIONS
        public static int Verify(string email, string code)
        {
            if (!email.Contains('@') || !email.Contains('.')) return -1;
            if (code.Length != 8)
            {
                return 0;
            }

            if (!VerifySGBDConnection())
                return -2;
            SqlCommand cmd = new SqlCommand("SELECT email, codigo as codigo FROM Pulse.Utilizador WHERE email = '" + email + "'", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                if (code.Equals(reader["codigo"].ToString()))
                {
                    cn.Close();
                    return 1;
                }
            }
            else
            {
                cn.Close();
                return -1;
            }


            cn.Close();
            return 0;
        }

        public static User loadUser(string code)
        {
            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pulse.Utilizador WHERE codigo = '" + code + "'", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            User u = null;

            if (reader.Read())
            {
                u = new User(
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["DataNascimento"].ToString(),
                    reader["Morada"].ToString(),
                    reader["Email"].ToString(),
                    reader["Telefone"].ToString(),
                    reader["Telemovel"].ToString(),
                    reader["NIF"].ToString()
                );
            }
            cn.Close();

            return u;

        }


        //PAGE 2 FUNCTIONS
        public static bool verificar(string code, string email, string nif)
        {

            if (code.Length != 8)
            {
                return false;
            }

            if (!email.Contains('@') || !email.Contains('.')) return false;
            if (nif.Length != 9) return false;

            if (!VerifySGBDConnection())
                return false;
            SqlCommand cmd = new SqlCommand("SELECT email as email, codigo as codigo FROM Pulse.Utilizador WHERE email = '" + email + "' or codigo = " + code, cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                cn.Close();
                return false;
            }

            cn.Close();
            return true;
        }

        public static void insertUser(User novo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Utilizador(Codigo, Nome, DataNascimento, Email, NIF) VALUES ( '" + novo.getCode() + "', '" + novo.getNome() + "', '" + novo.getBDay() + "', '" + novo.getEmail() + "', '" + novo.getNIF() + "');", cn);

            cmd.ExecuteNonQuery();
            cn.Close();
        }



        //PERFIL FUNCTIONS
        public static void alterPhone(string phone, string codigo)
        {
            Console.WriteLine(phone);
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Telemovel = '" + phone + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public static void alterTelephone(string telephone, string codigo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Telefone = '" + telephone + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public static void alterMail(string mail, string codigo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Email = '" + mail + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public static void alterAddress(string address, string codigo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Morada = '" + address + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        //UPDATE PAGE FUNCTIONS
        public static Paciente getPaciente(string codigoPaciente, string codigoMedico)
        {

            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select Nome, Estado, Localizacao " +
                "from Pulse.Atende join Pulse.Paciente on (CodigoPacienteAtende = Codigo) join Pulse.Utilizador on (Paciente.Codigo = Utilizador.Codigo) " +
                "where CodigoMedicoAtende = '" + codigoMedico + "' and CodigoPacienteAtende = '" + codigoPaciente + "';", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            Paciente p = null;

            if (reader.Read())
            {
                p = new Paciente(
                    reader["Nome"].ToString(),
                    reader["Estado"].ToString(),
                    reader["Localizacao"].ToString()
                );
            }

            cn.Close();
            return p;

        }

        public static void darAlta(string codigoPaciente, string codigoMedico)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("delete from Pulse.Atende WHERE CodigoPacienteAtende = '" + codigoPaciente + "' and CodigoMedicoAtende = '" + codigoMedico + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();

        }

        public static void alterLocation(string code, string location)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Paciente set Localizacao = '" + location + "' where Codigo = '" + code + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        public static void alterInfo(string code, string text)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Paciente set Estado = '" + text + "' where Codigo = '" + code + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        //RECEITAS PAGE FUNCTIONS
        public static List<ReceitaInfo> loadReceitasInfo(string code)
        {
            List<ReceitaInfo> receitas = new List<ReceitaInfo>();

            if (!VerifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("select Receita.ID, Receita.Data, Codigo, Nome " +
                "from Pulse.Receita join Pulse.TemReceita on (ID = IDReceita) join Pulse.Consulta on (TemReceita.IDConsulta = Consulta.ID) join Pulse.Utilizador on (CodigoMedico = Codigo) " +
                "where CodigoPaciente = '" + code + "' " +
                "order by Receita.Data desc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ReceitaInfo r = new ReceitaInfo(
                    reader["ID"].ToString(),
                    reader["Data"].ToString(),
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString()
                );
                receitas.Add(r);
            }
            cn.Close();
            return receitas;
        }

       public static List<Medicamento> loadMedicamentos(String id)
        {
            if (!VerifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("select * from Pulse.Medicamento where IDReceita = '" + id + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Medicamento> medicamentos = new List<Medicamento>();

            while (reader.Read())
            {
                Medicamento mt = new Medicamento(
                    reader["ID"].ToString(),
                    reader["Designacao"].ToString(),
                    reader["Dosagem"].ToString()
                );
                medicamentos.Add(mt);
                mt.index = medicamentos.IndexOf(mt) + 1;

            }
            cn.Close();
            return medicamentos;
        }

        //PROFISSIONAL PAGE FUNCTIONS 
        public static string getNearestAppointment(String code)
        {
            String newest = null;
            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select max(Data) as dataConsulta " +
                "from Pulse.Utilizador join Pulse.Consulta on (Utilizador.Codigo = Consulta.CodigoMedico) " +
                "where data > ' " + DateTime.Now.ToString("yyyy/MM/dd") + "' and CodigoMedico = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (!reader.IsDBNull(0))
            {
                newest = reader["dataConsulta"].ToString();
            }

            cn.Close();
            return newest;
        }

        public static string getNearestShift(String code)
        {
            String newest = null;
            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select max(DataTurno) as dataTurno, min(Hora) as hora " +
                "from Pulse.DatasTurnos " +
                "where DataTurno > '" + DateTime.Now.ToString("yyyy/MM/dd") + "' and Hora > '" + DateTime.Now.ToString("hh:mm") + "' and IDMedico = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (!reader.IsDBNull(0))
            {
                newest = reader["dataTurno"].ToString() + " " + reader["hora"].ToString();
            }

            cn.Close();
            return newest;
        }

        //PACIENT PAGE FUNCTIONS
        public static string getNearestPacientAppointment(String code)
        {
            String newest = null;

            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select Data from Pulse.Consulta join Pulse.Utilizador on (CodigoPaciente = Codigo) where Codigo = " + code + " order by Data desc", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                newest = reader["Data"].ToString();
            }


            cn.Close();
            return newest;
        }

        //HORARIO PAGE FUNCTIONS
        public static List<Turno> getShifts(string code)
        {
            List<Turno> horarioTiles = new List<Turno>();

            if (!VerifySGBDConnection())
                return horarioTiles;

            SqlCommand cmd = new SqlCommand("select Data, HoraInicio, HoraFim, Descricao " +
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Utilizador.Codigo) " +
                "where IDMedico = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Turno ht = new Turno(
                    reader["Data"].ToString().Substring(0, 5),
                    reader["HoraInicio"].ToString(),
                    reader["HoraFim"].ToString(),
                    reader["Descricao"].ToString()
                );
                horarioTiles.Add(ht);
            }

            cn.Close();
            return horarioTiles;
        }

        //FATURAS PAGE FUNCTIONS
        public static List<FaturaInfo> getFaturas(string code)
        {
            List<FaturaInfo> faturas = new List<FaturaInfo>();

            if (!VerifySGBDConnection())
                return faturas;

            SqlCommand cmd = new SqlCommand("select * from Pulse.Fatura " +
                "where CodigoPaciente = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                FaturaInfo ft = new FaturaInfo(
                    reader["NrFatura"].ToString(),
                    reader["Preco"].ToString(),
                    reader["Data"].ToString()
                );
                faturas.Add(ft);
            }


            cn.Close();
            return faturas;
            
        }

        //EXAMESANALISES PAGE FUNCTIONS
        public static List<Analises> loadAnalises(string code)
        {
            List<Analises> analises = new List<Analises>();

            if (!VerifySGBDConnection())
                return analises;

            SqlCommand cmd = new SqlCommand("select Analise.ID, Analise.Data, Codigo, Nome, Descricao " +
                "from Pulse.Analise join Pulse.TemAnalise on (Analise.ID = IDAnalise) join Pulse.Consulta on (TemAnalise.IDConsulta = Consulta.ID) join Pulse.Utilizador on (CodigoMedico = Codigo) " +
                "where CodigoPaciente = '" + code + "' " +
                "order by Analise.Data desc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Analises r = new Analises(
                    reader["ID"].ToString(),
                    reader["Data"].ToString(),
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["Descricao"].ToString()
                );
                analises.Add(r);
            }
            cn.Close();
            return analises;

        }

        //CONSULTAS PAGE FUNCTIONS
        public static List<ConsulaInfo> getConsultas(DateTime date, String code)
        {
            List<ConsulaInfo> listaConsultas = new List<ConsulaInfo>();

            if (!VerifySGBDConnection())
                return listaConsultas;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select Hora, Data, NrConsultorio, Nome " +
                "from Pulse.Consulta join Pulse.Utilizador on (CodigoPaciente = Codigo) " +
                "where CodigoMedico = '" + code + "' and Data = '" + data + "' " +
                "order by Hora asc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ConsulaInfo c = new ConsulaInfo(
                    reader["Data"].ToString().Substring(0, 10),
                    reader["NrConsultorio"].ToString(),
                    reader["Hora"].ToString().Substring(0, 5),
                    reader["Nome"].ToString()
                );
                listaConsultas.Add(c);
            }

            cn.Close();
            return listaConsultas;
        }

        //CALENDARIO PAGE FUNCTION
        public static List<ConsulaInfo> getPacienteConsultas(DateTime date, String code)
        {
            List<ConsulaInfo> listaConsultas = new List<ConsulaInfo>();

            if (!VerifySGBDConnection())
                return listaConsultas;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select Hora, Data, NrConsultorio, Nome " +
                "from Pulse.Consulta join Pulse.Utilizador on (CodigoMedico = Codigo) " +
                "where CodigoPaciente = '" + code + "' and Data = '" + data + "' order by Hora asc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ConsulaInfo c = new ConsulaInfo(
                    reader["Data"].ToString().Substring(0, 10),
                    reader["NrConsultorio"].ToString(),
                    reader["Hora"].ToString().Substring(0, 5),
                    reader["Nome"].ToString()
                );
                listaConsultas.Add(c);
            }
            cn.Close();
            return listaConsultas;
        }

        public static List<Doctor> loadDoctor(DateTime date)
        {
            List<Doctor> doctor = new List<Doctor>();
            if (!VerifySGBDConnection())
                return doctor;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select Codigo, Nome " +
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Codigo) " +
                "where Data = '" + data + "' and Descricao = 'Consulta'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                doctor.Add(
                    new Doctor(
                        reader["Codigo"].ToString(),
                        reader["Nome"].ToString()
                    )
                );
            }

            cn.Close();
            return doctor;

        }
        public static void inserConsulta(string data, string hora, int consultorio, Doctor medico, String codigo)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Consulta(Hora, Data, NrConsultorio, CodigoMedico, CodigoPaciente) VALUES('" + hora + "', '" + data + "'," + consultorio.ToString() + ", '" + medico.Codigo + "', '" + codigo + "');", cn);
            Console.WriteLine("COnsulta marcada");
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public static List<String> loadHoras(DateTime date, String nome)
        {
            List<String> horas = new List<string>();

             if (!VerifySGBDConnection())
                return horas;

            String data = date.ToString("yyyy-MM-dd");


            SqlCommand cmd = new SqlCommand("select HoraInicio, HoraFim " +
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Codigo) " +
                "where Data = '" + data + "' and Nome = '" + nome + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                DateTime parsedDate = DateTime.Parse(reader["HoraInicio"].ToString());
                DateTime finalTime = DateTime.Parse(reader["HoraFim"].ToString());
                TimeSpan tempo = new TimeSpan(0, 30, 0);

                while (!parsedDate.Equals(finalTime))
                {
                    parsedDate = parsedDate.Add(tempo);
                    horas.Add(parsedDate.TimeOfDay.ToString());
                }

            }
            cn.Close();
            return horas;
        }

        //ACOMPANHANTES PAGE FUNCTIONS
        public static List<AcompanhanteInfo> getAcompanhantes(String code)
        {
            List<AcompanhanteInfo> acompanhantes = new List<AcompanhanteInfo>();
            if (!VerifySGBDConnection())
                return acompanhantes;

            SqlCommand cmd = new SqlCommand("select Nome, Estado,Localizacao, Pulse.Paciente.Codigo " +
                "from Pulse.Utilizador join Pulse.Paciente on (Utilizador.Codigo = Paciente.Codigo) join Pulse.Acompanha on (Paciente.Codigo = CodigoPacienteAcompanha) " +
                "where Pulse.Acompanha.CodigoAcompanhanteAcompanha = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                AcompanhanteInfo at = new AcompanhanteInfo(
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["Estado"].ToString(),
                    reader["Localizacao"].ToString()
                );
                acompanhantes.Add(at);

            }
            cn.Close();
            return acompanhantes;
        }

        public static AcompanhanteInfo verifyAcompanhante(string name, string code)
        {
            if (!VerifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("select Paciente.Codigo, Nome, Estado, Localizacao " +
                "from Pulse.Utilizador join Pulse.Paciente on (Utilizador.Codigo = Paciente.Codigo) " +
                "where Paciente.Codigo = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (!reader.Read())
            {
                //paciente nao existe
                //CodeError.Visibility = Visibility.Visible;
                cn.Close();
                return null;
            }
            else if (!reader["Nome"].ToString().Equals(name))
            {
                //paciente não corresponde ao codigo
                //NomeError.Visibility = Visibility.Visible;
                cn.Close();
                return null;
            }
            else
            {
                AcompanhanteInfo at = new AcompanhanteInfo(
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["Estado"].ToString(),
                    reader["Localizacao"].ToString()
                );
                cn.Close();
                return at;
            }
        }
        public static void addAcompanhante(AcompanhanteInfo at, String Code)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Acompanha(CodigoPacienteAcompanha, CodigoAcompanhanteAcompanha) VALUES ( '" + at.Code + "', '" + Code + "');", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        public static void removeAcompanahnte(AcompanhanteInfo at, String Code)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("DELETE FROM Pulse.Acompanha WHERE CodigoPacienteAcompanha = '" + at.Code + "' and CodigoAcompanhanteAcompanha = '" + Code + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

    }
}
