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
            return new SqlConnection("Data Source=tcp:mednat.ieeta.pt\\SQLSERVER,8101;Initial Catalog=p2g6;Persist Security Info=True;User ID=p2g6;Password=BDcemi@34");

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
        /* Method to load user from database to data structure User
        Parameter String code - user's code
        Returns the object User with the user whose code is code*/
        public static User loadUser(string email, string pwd)
        {

            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pulse.login('" + email + "','" + pwd + "');", cn);
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
        /* Method used to register a User in the db
        Parameter User novo - user to register
        Returns 0 if error on code, -1 if error on email, or 1 if correct*/
        public static void insertUser(User novo,  String password)
        {
           
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Utilizador(Codigo, Nome, DataNascimento, Email, NIF, PalavraPasse) VALUES ( '" + novo.getCode() + "', '" + novo.getNome() + "', '" + novo.getBDay() + "', '" + novo.getEmail() + "', '" + novo.getNIF() + "', CONVERT( VARBINARY(128),'" + password + "'));", cn);

            cmd.ExecuteNonQuery();
            cn.Close();
        }



        //PERFIL FUNCTIONS //
        /* Method used to alter a user's phone number in the db
        Parameter String phone - new phone nummber
        Parameter String code - user's code to update*/
        public static void alterPhone(string phone, string codigo)
        {
            Console.WriteLine(phone);
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Telemovel = '" + phone + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        /* Method used to alter a user's telephone number in the db
        Parameter String telephone - new telephone nummber
        Parameter String code - user's code to update*/
        public static void alterTelephone(string telephone, string codigo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Telefone = '" + telephone + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        /* Method used to alter a user's emmail in the db
        Parameter String mail - new email
        Parameter String code - user's code to update*/
        public static void alterMail(string mail, string codigo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Email = '" + mail + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        /* Method used to alter user's address in the db
        Parameter String address - new address
        Parameter String code - user's code to update*/
        public static void alterAddress(string address, string codigo)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Utilizador set Morada = '" + address + "' where Codigo = '" + codigo + "';", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        //UPDATE PAGE FUNCTIONS
        /* Method that retrieves a Pacient from the db who is associated with a doctor
        Parameter String codigoPaciente - Pacient's code
        Parameter String codigoMedico - Doctor's code
        returns the pacient info in the Object Paciente*/
        public static Paciente getPaciente(string codigoPaciente, string codigoMedico)
        {

            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select * from Pulse.getPacientes('"+ codigoMedico + "', '" + codigoPaciente + "');", cn);
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

        /* Method that discharges a paciente, changes his state to 'Alta' and removes it from the relactionship Atende
        Parameter String codigoPaciente - Pacient's code
        Parameter String codigoMedico - associated Doctor's code*/
        public static void darAlta(string codigoPaciente, string codigoMedico)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("delete from Pulse.Atende WHERE CodigoPacienteAtende = '" + codigoPaciente + "' and CodigoMedicoAtende = '" + codigoMedico + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();

        }

        /* Method that alters a pacient location inside the hospital
        Parameter String code - Pacient's code
        Parameter String location - new Pacient's location*/
        public static void alterLocation(string code, string location)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Paciente set Localizacao = '" + location + "' where Codigo = '" + code + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        /* Method that alters a pacient state
        Parameter String code - Pacient's code
        Parameter String text - new Pacient's state*/
        public static void alterInfo(string code, string text)
        {
            if (!VerifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Paciente set Estado = '" + text + "' where Codigo = '" + code + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        //RECEITAS PAGE FUNCTIONS
        /* Method used to load all the user's prescriptions
        Parameter String code - Pacient's code
        Returns a list of objects ReceitaInfo each containing a prescription*/
        public static List<ReceitaInfo> loadReceitasInfo(string code)
        {
            List<ReceitaInfo> receitas = new List<ReceitaInfo>();

            if (!VerifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("select * from Pulse.LoadReceitas('" + code +"') order by Data desc;", cn);
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

        /* Method used to load all the medicine from a single prescriptions
        Parameter String id - Prescription id
        Returns a list of objects Medicamento each containing Medicine information*/
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
        /* Method used to get a doctor's next appointment's date
        Parameter String code - Doctor's code
        Returns the nearest appointment date*/
        public static string getNearestAppointment(String code)
        {
            String newest = null;
            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select Pulse.getNearestAppointment('" + code + "') as dataConsulta;", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (!reader.IsDBNull(0))
            {
                newest = reader["dataConsulta"].ToString();
            }

            cn.Close();
            return newest;
        }

        /* Method used to get a doctor's next shifts's date
        Parameter String code - Doctor's code
        Returns the nearest shift date*/
        public static string getNearestShift(String code)
        {
            String newest = null;
            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select * from Pulse.getNearestShift('" + code + "');", cn);
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
        /* Method used to get a pacients's next appointment's date
        Parameter String code - pacients's code
        Returns the nearest appointment date*/
        public static string getNearestPacientAppointment(String code)
        {
            String newest = null;

            if (!VerifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select Pulse.getNearestPacientAppointment('" + code + "') as Data", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (!reader.IsDBNull(0))
            {
                newest = reader["Data"].ToString();
            }


            cn.Close();
            return newest;
        }

        //HORARIO PAGE FUNCTIONS
        /* Method used to get a doctor's shif schedule
        Parameter String code - Doctor's code
        Returns a list of Turnos which contain each shift info*/
        public static List<Turno> getShifts(string code)
        {
            List<Turno> shift = new List<Turno>();

            if (!VerifySGBDConnection())
                return shift;

            SqlCommand cmd = new SqlCommand("select * from Pulse.getShifts('" + code + "') ORDER BY Data asc, HoraInicio asc; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Turno ht = new Turno(
                    reader["Data"].ToString().Substring(0, 5),
                    reader["HoraInicio"].ToString(),
                    reader["HoraFim"].ToString(),
                    reader["Descricao"].ToString()
                );
                shift.Add(ht);
            }

            cn.Close();
            return shift;
        }

        //FATURAS PAGE FUNCTIONS
        /* Method used to get all the user's receipts
        Parameter String code - User's code
        Returns a list of FaturaIndo which contain each receipt info*/
        public static List<FaturaInfo> getFaturas(string code)
        {
            List<FaturaInfo> faturas = new List<FaturaInfo>();

            if (!VerifySGBDConnection())
                return faturas;

            SqlCommand cmd = new SqlCommand("select * from Pulse.Fatura where CodigoPaciente = '" + code + "'; ", cn);
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
        /* Method used to get all the user's exams
        Parameter String code - User's code
        Returns a list of Analises which contain each exam info*/
        public static List<Analises> loadAnalises(string code)
        {
            List<Analises> analises = new List<Analises>();

            if (!VerifySGBDConnection())
                return analises;

            SqlCommand cmd = new SqlCommand("select *  from Pulse.LoadAnalises('" + code + "') order by Data desc; ", cn);
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
        /* Method used to get a doctor's full list of appointments in a specific day
        Parameter String code - Doctor's code
        Parameter DateTime date - day of appointment
        Returns a list of ConsultaInfo which contains each appointment*/
        public static List<ConsulaInfo> getConsultas(DateTime date, String code)
        {
            List<ConsulaInfo> listaConsultas = new List<ConsulaInfo>();

            if (!VerifySGBDConnection())
                return listaConsultas;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select * from Pulse.getConsultas('" + code + "', '" + data + "')  order by Hora asc;", cn);
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
        /* Method used to get a user's full list of appointments in a specific day
        Parameter String code - Pacient's code
        Parameter DateTime date - day of appointment
        Returns a list of ConsultaInfo which contains each appointment*/
        public static List<ConsulaInfo> getPacienteConsultas(DateTime date, String code)
        {
            List<ConsulaInfo> listaConsultas = new List<ConsulaInfo>();

            if (!VerifySGBDConnection())
                return listaConsultas;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select * from Pulse.getPacienteConsultas('" + code + "', '" + data + "') order by Hora asc;", cn);
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

        /* Method used to get a list of available doctors to make an appointment in a specific day 
        Parameter DateTime date - day of appointment
        Returns a list of available doctors*/
        public static List<Doctor> loadDoctor(DateTime date)
        {
            List<Doctor> doctor = new List<Doctor>();
            if (!VerifySGBDConnection())
                return doctor;

            String data = date.ToString("yyyy-MM-dd");
            Console.WriteLine(data);
            SqlCommand cmd = new SqlCommand("select * from Pulse.loadDoctorsList('" + data + "');", cn);
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

        /* Method to add an appointment to the db
        Parameter String data - day of appointment
        Parameter String hora - hour of appointment
        Parameter int consultório - room of appointment
        Parameter Doctor medico - object that has the doctor's info
        Parameter String codigo - code of pacient that is scheduling an appointment
        Returns a list of available doctors*/
        public static void inserConsulta(string data, string hora, int consultorio, Doctor medico, String codigo)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Consulta(Hora, Data, NrConsultorio, CodigoMedico, CodigoPaciente) VALUES('" + hora + "', '" + data + "'," + consultorio.ToString() + ", '" + medico.Codigo + "', '" + codigo + "');", cn);
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        /* Method used to get a list of available hours to make an appointment in a specific day with a doctor
        The appointment's schedules are splitted in half an hour intervals.
        Parameter DateTime date - day of appointment
        Parameter String nome - name of the doctor
        Returns a list of available doctors*/
        public static List<String> loadHoras(DateTime date, String nome)
        {
            List<String> horas = new List<string>();

             if (!VerifySGBDConnection())
                return horas;

            String data = date.ToString("yyyy-MM-dd");


            SqlCommand cmd = new SqlCommand("select * from Pulse.loadListHoras('" + data + "', '" + nome + "'); ", cn);
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
        /* Method used to get a list of all the Patients a user accompanies
        Parameter String code - code of the companion
        Returns a list of AcompanhanteInfo, each containing a pacients info*/
        public static List<AcompanhanteInfo> getAcompanhantes(String code)
        {
            List<AcompanhanteInfo> acompanhantes = new List<AcompanhanteInfo>();
            if (!VerifySGBDConnection())
                return acompanhantes;

            SqlCommand cmd = new SqlCommand("select * from Pulse.getAcompanhantes('" + code + "'); ", cn);
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

        /* Method used to verify if the tuple (name, code) represents an existing Paciente in the db and if it does, returns it
        Parameter String name - name of the pacient
        Parameter String code - code of the pacient
        Returns An AcompanhanteInfo, which contains a pacients info*/
        public static AcompanhanteInfo verifyAcompanhante(string name, string code)
        {
            if (!VerifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("select * from Pulse.verifyAcompanhantes('" + code + "'); ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (!reader.Read())
            {
                cn.Close();
                return null;
            }
            else if (!reader["Nome"].ToString().Equals(name))
            {
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

        /* Method used to add pacient - companion relationship to the db
        Parameter AcompanhanteInfo at - pacient to accompanie;
        Parameter String code - code of the user*/
        public static void addAcompanhante(AcompanhanteInfo at, String Code)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Acompanha(CodigoPacienteAcompanha, CodigoAcompanhanteAcompanha) VALUES ( '" + at.Code + "', '" + Code + "');", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        /* Method used to remove pacient - companion relationship of the db
        Parameter AcompanhanteInfo at - pacient to accompanie;
        Parameter String code - code of the user*/
        public static void removeAcompanhante(AcompanhanteInfo at, String Code)
        {
            if (!VerifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("DELETE FROM Pulse.Acompanha WHERE CodigoPacienteAcompanha = '" + at.Code + "' and CodigoAcompanhanteAcompanha = '" + Code + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

    }
}
