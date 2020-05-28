using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    /// Interaction logic for Pacient.xaml
    /// </summary>
    public partial class Pacient : Page
    {
        User user;
        private SqlConnection cn;


        public Pacient(User user)
        {
            InitializeComponent();
            this.user = user;

            if (user.getCode().Substring(0,2).Equals("99")) {
                Professional.Visibility = Visibility.Visible;
            } else
            {
                Doente.Visibility = Visibility.Visible;
            }

        }

        private void Pacient_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
            String date = getNearestAppointment();

            if (date != null)
            {
                DataPoximaConsulta.Visibility = Visibility.Visible;
                PoximaConsulta.Visibility = Visibility.Visible;
                DataPoximaConsulta.Content = date.Substring(0, 10);
            } else
            {
                DataPoximaConsulta.Visibility = Visibility.Hidden;
                PoximaConsulta.Visibility = Visibility.Hidden;
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
            Console.WriteLine(user.getCode());
            String newest = null;
            if (!verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("select Data from Pulse.Consulta join Pulse.Utilizador on (CodigoPaciente = Codigo) where Codigo = " + user.getCode() + " order by Data desc", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                newest = reader["Data"].ToString();
            }
                        
            
            cn.Close();
            return newest;
        }

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            user = null;
            Page1 p = new Page1();
            this.NavigationService.Navigate(p);

        }

        private void OpenCalendario(object sender, MouseButtonEventArgs e)
        {
            CalendarButton.Width = 126;
            CalendarButton.Height = 146;
            CalendarLabel.FontSize = 16;

        }

        private void OpenEA(object sender, MouseButtonEventArgs e)
        {
            EAButton.Width = 126;
            EAButton.Height = 146;
            EALabel.FontSize = 16;

        }
        private void OpenReceitas(object sender, MouseButtonEventArgs e)
        {
            ReceitasButton.Width = 126;
            ReceitasButton.Height = 146;
            ReceitasLabel.FontSize = 16;

        }
        private void OpenFaturas(object sender, MouseButtonEventArgs e)
        {
            FaturasButton.Width = 126;
            FaturasButton.Height = 146;
            FaturasLabel.FontSize = 16;

        }



        private void CalendarioPress(object sender, MouseButtonEventArgs e)
        {
            CalendarButton.Width = 129;
            CalendarButton.Height = 149;
            CalendarLabel.FontSize = 15;

        }

        private void EAPress(object sender, MouseButtonEventArgs e)
        {
            EAButton.Width = 129;
            EAButton.Height = 149;
            EALabel.FontSize = 15;


        }
        private void ReceitasPress(object sender, MouseButtonEventArgs e)
        {
            ReceitasButton.Width = 129;
            ReceitasButton.Height = 149;
            ReceitasLabel.FontSize = 15;

        }
        private void FaturasPress(object sender, MouseButtonEventArgs e)
        {
            FaturasButton.Width = 129;
            FaturasButton.Height = 149;
            FaturasLabel.FontSize = 15;

        }
    }
}
