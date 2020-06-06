using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Interaction logic for Calendario.xaml
    /// </summary>
    public partial class Calendario : Page
    {
        //DateTime Date;
        User user;
        List<ConsultasTile> listaConsultas;
        private SqlConnection cn;

        List<Doctor> doctor;
        List<String> horas;



        public Calendario(User user)
        {
            this.user = user;
            listaConsultas = new List<ConsultasTile>();
            doctor = new List<Doctor>();
            horas = new List<String>();
            InitializeComponent();
            CalendarioConsultas.DisplayDate = DateTime.Now;
            CalendarioConsultas.SelectedDate = DateTime.Now;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            
            medicList.ItemsSource = doctor;
            hoursList.ItemsSource = horas;
            //medicList.SelectedIndex = 0;


            ListViewConsultas.ItemsSource = listaConsultas;
            getConsultas(DateTime.Now);

            Style dayButtonStyle = new Style() { BasedOn = (Style) this.Resources["calendarDayButtonStyle"] };


            var dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = new DateTime(2020, 06, 15) };
            dataTrigger.Setters.Add(new Setter(BackgroundProperty, "red"));
            dayButtonStyle.Triggers.Add(dataTrigger);

        }

        private void getConsultas(DateTime date)
        {


            if (!verifySGBDConnection())
                return;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select Hora, Data, NrConsultorio, Nome " +
                "from Pulse.Consulta join Pulse.Utilizador on (CodigoMedico = Codigo) " +
                "where CodigoPaciente = '" + this.user.getCode() + "' and Data = '" + data + "';", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ConsultasTile c = new ConsultasTile(
                    reader["Data"].ToString().Substring(0,10),
                    reader["NrConsultorio"].ToString(),
                    reader["Hora"].ToString().Substring(0,5),
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

                marcacao.Visibility = Visibility.Hidden;
                Book.Visibility = Visibility.Visible;

            }


        }


        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private class Doctor
        {
            public String Nome { get; set; }
            public String Codigo { get; set; }
            public Doctor(String c, String n)
            {
                this.Nome = n;
                this.Codigo = c;
            }
        }

        private class ConsultasTile
        {
            public String Data { get; set; }
            public String Consultorio { get; set; }
            public String Hora { get; set; }
            public String medico { get; set; }
            
            public ConsultasTile(String Data, String Consultorio, String hora, String medico) 
            {
                this.Data = Data;
                this.Consultorio = Consultorio;
                this.Hora = hora;
                this.medico = medico;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            marcacao.Visibility = Visibility.Visible;
            Book.Visibility = Visibility.Hidden;
            HorasLabel.Visibility = Visibility.Hidden;
            Horas.Visibility = Visibility.Hidden;
            MarcarButton.Visibility = Visibility.Hidden;

            doctor.Clear();
            loadDoctors(CalendarioConsultas.SelectedDate.Value);
            
        }

        private void loadDoctors(DateTime date)
        {
            if (!verifySGBDConnection())
                return;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select Codigo, Nome " + 
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Codigo) "+
                "where Data = '" + data + "'; ", cn);  //"' and Descricao = 'Consulta'"
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

            Console.WriteLine(doctor.Count());
            medicList.Items.Refresh();
            cn.Close();
        }

        private void MarcarConsulta(object sender, RoutedEventArgs e)
        {

            marcar(medicList.SelectedIndex, CalendarioConsultas.SelectedDate.Value, hoursList.SelectedIndex);

            marcacao.Visibility = Visibility.Hidden;
            Book.Visibility = Visibility.Visible;
          
        }

        private void marcar(int selectedIndex1, DateTime date, int selectedIndex2)
        {
            Doctor medico = doctor[selectedIndex1];
            String data = date.ToString("yyyy-MM-dd");
            String hora = horas[selectedIndex2];


            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Consulta(Hora, Data, NrConsultorio, CodigoMedico, CodigoPaciente) VALUES('" + hora + "', '" + data + "', 24, '" + medico.Codigo + "', '" + this.user.getCode() + "');", cn);

            cmd.ExecuteNonQuery();
            cn.Close();




        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HorasLabel.Visibility = Visibility.Visible;
            Horas.Visibility = Visibility.Visible;

            horas.Clear();
            getHoras(medicList.SelectedIndex, CalendarioConsultas.SelectedDate.Value);
        }

        private void getHoras(int selectedIndex, DateTime date)
        {
            if (!verifySGBDConnection())
                return;

            String data = date.ToString("yyyy-MM-dd");

            SqlCommand cmd = new SqlCommand("select HoraInicio, HoraFim " +
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Codigo) " +
                "where Data = '" + data + "' and Nome = '" + doctor[selectedIndex].Nome + "'; ", cn);
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

            Console.WriteLine(horas.Count());

            hoursList.Items.Refresh();
            cn.Close();
        }

        private void hoursList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarcarButton.Visibility = Visibility.Visible;
        }
    }
}
