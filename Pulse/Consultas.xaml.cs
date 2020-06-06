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
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Page
    {
        //DateTime Date;
        User user;
        List<ConsultasTile> listaConsultas;
        private SqlConnection cn;



        public Consultas(User user)
        {
            this.user = user;
            listaConsultas = new List<ConsultasTile>();
            InitializeComponent();
            CalendarioConsultas.DisplayDate = DateTime.Now;
            CalendarioConsultas.SelectedDate = DateTime.Now;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            ListViewConsultas.ItemsSource = listaConsultas;
            getConsultas(DateTime.Now);

           
        }

        private void getConsultas(DateTime date)
        {


            if (!verifySGBDConnection())
                return;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select Hora, Data, NrConsultorio, Nome " +
                "from Pulse.Consulta join Pulse.Utilizador on (CodigoPaciente = Codigo) " +
                "where CodigoMedico = '" + this.user.getCode() + "' and Data = '" + data + "' " +
                "order by Hora asc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ConsultasTile c = new ConsultasTile(
                    reader["Data"].ToString().Substring(0, 10),
                    reader["NrConsultorio"].ToString(),
                    reader["Hora"].ToString().Substring(0, 5),
                    reader["Nome"].ToString()
                );
                listaConsultas.Add(c);
            }

            ListViewConsultas.Items.Refresh();
            cn.Close();
        }


        private void CalendarioConsultas_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalendarioConsultas.SelectedDate.HasValue)
            {
                listaConsultas.Clear();
                getConsultas(CalendarioConsultas.SelectedDate.Value);

            }


        }


        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        private class ConsultasTile
        {
            public String Data { get; set; }
            public String Consultorio { get; set; }
            public String Hora { get; set; }
            public String paciente { get; set; }

            public ConsultasTile(String Data, String Consultorio, String hora, String paciente)
            {
                this.Data = Data;
                this.Consultorio = Consultorio;
                this.Hora = hora;
                this.paciente = paciente;
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


    }
}

