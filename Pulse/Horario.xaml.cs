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
    /// Interaction logic for Horario.xaml
    /// </summary>
    public partial class Horario : Page
    {

        List<HorarioTile> horarioTiles;
        private SqlConnection cn;
        User user;


        public Horario(User user)
        {
            InitializeComponent();
            this.user = user;
            horarioTiles = new List<HorarioTile>();

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            cn = getSGBDConnection();
            getShifts();
            ListViewProducts.ItemsSource = horarioTiles;
        }


        private void getShifts()
        {

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("select Data, HoraInicio, HoraFim, Descricao " + 
                "from Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Utilizador.Codigo) " +
                "where IDMedico = '" + user.getCode() + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            
           while (reader.Read())
           {
                HorarioTile ht = new HorarioTile(
                    reader["Data"].ToString(),
                    reader["HoraInicio"].ToString(),
                    reader["HoraFim"].ToString(),
                    reader["Descricao"].ToString()
                );
                horarioTiles.Add(ht);
            }


            cn.Close();
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


        private class HorarioTile{

            public string day { get; set; }
            public string horaInicio { get; set; }
            public string horaFim { get; set; }
            public string description { get; set; }


            public HorarioTile(String dia, String horaI, String horaF, String d)
            {
                this.day = dia;
                this.horaInicio = horaI;
                this.horaFim = horaF;
                this.description = d;
            }
       }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
