using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pulse
{
    /// <summary>
    /// Interaction logic for ProfSaude.xaml
    /// </summary>
    public partial class Profissional : Page
    {
        User user;
        private SqlConnection cn;

        public Profissional(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void HorarioPress(object sender, MouseButtonEventArgs e)
        {
            HorarioButton.Width = 129;
            HorarioButton.Height = 149;
            HorarioLabel.FontSize = 15;
        }

        private void OpenHorario(object sender, MouseButtonEventArgs e)
        {
            HorarioButton.Width = 126;
            HorarioButton.Height = 146;
            HorarioLabel.FontSize = 16;

            Horario h = new Horario(this.user);
            this.NavigationService.Navigate(h);

        }

        private void ConsultasPress(object sender, MouseButtonEventArgs e)
        {
            ConsultasButton.Width = 129;
            ConsultasButton.Height = 149;
            ConsultasLabel.FontSize = 15;
        }

        private void OpenConsultas(object sender, MouseButtonEventArgs e)
        {
            ConsultasButton.Width = 126;
            ConsultasButton.Height = 146;
            ConsultasLabel.FontSize = 16;
        }

        private void AtualizarPress(object sender, MouseButtonEventArgs e)
        {
            AtualizarButton.Width = 129;
            AtualizarButton.Height = 149;
            AtualizarLabel.FontSize = 15;
        }

        private void OpenAtualizar(object sender, MouseButtonEventArgs e)
        {
            AtualizarButton.Width = 126;
            AtualizarButton.Height = 146;
            AtualizarLabel.FontSize = 16;
        }
        
        private void GoToPaciente(object sender, MouseButtonEventArgs e)
        {
            Pacient p = new Pacient(this.user);
            this.NavigationService.Navigate(p);
        }

        private void GoToAcompanhante(object sender, MouseButtonEventArgs e)
        {
            Acompanhante acomp = new Acompanhante(this.user);
            this.NavigationService.Navigate(acomp);
        }
        
        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            Page1 p = new Page1();
            this.NavigationService.Navigate(p);
        }

        private void Profissional_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
            String consulta = getNearestAppointment();
            String turno = getNearestShift();

            if (consulta != null)
            {
                DataPoximaConsulta.Visibility = Visibility.Visible;
                ProximaConsulta.Visibility = Visibility.Visible;

                DataPoximaConsulta.Content = consulta.Substring(0, 10);
            }
            else
            {
                DataPoximaConsulta.Visibility = Visibility.Hidden;
                ProximaConsulta.Visibility = Visibility.Hidden;

            }
            

            if (turno != null)
            {
                DataPoximoTurno.Visibility = Visibility.Visible;
                ProximoTurno.Visibility = Visibility.Visible;
                DataPoximoTurno.Content = turno.Substring(0, 9);
            }
            else
            {
                DataPoximoTurno.Visibility = Visibility.Hidden;
                ProximoTurno.Visibility = Visibility.Hidden;

            }



        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("Data Source=DESKTOP-HB27C6M;Initial Catalog=Pulse;Integrated Security=True");

        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        private string getNearestAppointment()
        {
            String newest = null;
            if (!verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select max(Data) as dataConsulta "  + 
                "from Pulse.Utilizador join Pulse.Consulta on (Utilizador.Codigo = Consulta.CodigoMedico) " +
                "where data > ' " + DateTime.Now.ToString("yyyy/MM/dd") + "' and CodigoMedico = '" + user.getCode() + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (!reader.IsDBNull(0))
            {
                newest = reader["dataConsulta"].ToString();
            }


            cn.Close();
            return newest;
        }
        
        private string getNearestShift()
        {
            String newest = null;
            if (!verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select max(Data) as dataTurno, min(HoraInicio) as hora " + 
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Utilizador.Codigo) " + 
                "where data > '" + DateTime.Now.ToString("yyyy/MM/dd") + "' and HoraInicio > '" + DateTime.Now.ToString("hh:mm") + "' and IDMedico = '" + user.getCode() + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine(DateTime.Now.ToString("hh:mm"));

            reader.Read();
            if (!reader.IsDBNull(0))
            {
                newest = reader["dataTurno"].ToString() + " " + reader["hora"].ToString();
            }


            cn.Close();
            return newest;
        }


    }
}
