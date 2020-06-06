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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        private User user;
        private SqlConnection cn;

        String state;
        String location;
        String code;

        public Update(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cn = getSGBDConnection();
 

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


        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            getPaciente(codigo.Text);
        }

        private void getPaciente(string code)
        {
            
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("select Nome, Estado " +
                "from Pulse.Atende join Pulse.Paciente on (CodigoPacienteAtende = Codigo) join Pulse.Utilizador on (Paciente.Codigo = Utilizador.Codigo) " +
                "where CodigoMedicoAtende = '" + user.getCode() + "' and CodigoPacienteAtende = '" + code + "';", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                CodeWarning.Visibility = Visibility.Hidden;
                nome.Text = reader["Nome"].ToString();
                estadoInfo.Text = reader["Estado"].ToString();
                localizacaoInfo.Text = "No Hospital";
                EditState.Visibility = Visibility.Visible;
                EditLocation.Visibility = Visibility.Visible;

                this.code = codigo.Text;
                Alta.IsEnabled = true;


            }
            else
            {
                CodeWarning.Visibility = Visibility.Visible;
                nome.Text = "";
                estadoInfo.Text = "";
                localizacaoInfo.Text = "";
                EditState.Visibility = Visibility.Hidden;
                EditLocation.Visibility = Visibility.Hidden;

                this.code = null;
                Alta.IsEnabled = false;

            }

            cn.Close();

        }

        private void ButtonAlta_MouseUp(object sender, RoutedEventArgs e)
        {
            alterInfo(this.code, "Alta");
            darAlta(this.code);

            nome.Text = "";
            estadoInfo.Text = "";
            localizacaoInfo.Text = "";
            EditState.Visibility = Visibility.Hidden;
            EditLocation.Visibility = Visibility.Hidden;
            this.code = null;
            Alta.IsEnabled = false;
        }

        private void darAlta(string code)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("delete from Pulse.Atende WHERE CodigoPacienteAtende = '" + code + "' and CodigoMedicoAtende = '" + user.getCode() + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();

        }

        private void editInfoState(object sender, MouseButtonEventArgs e)
        {

            estadoInfo.IsEnabled = true;
            guardar.Visibility = Visibility.Visible;
            cancelar.Visibility = Visibility.Visible;
            EditState.Visibility = Visibility.Hidden;
            EditLocation.Visibility = Visibility.Hidden;

             state = estadoInfo.Text;
        }


        private void editInfoLocation(object sender, MouseButtonEventArgs e)
        {
            localizacaoInfo.IsEnabled = true;
            guardar.Visibility = Visibility.Visible;
            cancelar.Visibility = Visibility.Visible;
            EditState.Visibility = Visibility.Hidden;
            EditLocation.Visibility = Visibility.Hidden;

            location = localizacaoInfo.Text;
        }

        private void ButtonGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (state != estadoInfo.Text)
            {
                alterInfo(this.code, estadoInfo.Text);
                guardar.Visibility = Visibility.Collapsed;
                cancelar.Visibility = Visibility.Collapsed;
                estadoInfo.IsEnabled = false;
                localizacaoInfo.IsEnabled = false;
                EditState.Visibility = Visibility.Visible;
                EditLocation.Visibility = Visibility.Visible;


            } else if (location != localizacaoInfo.Text)
            {
                //alterLocation(this.code, location);
                guardar.Visibility = Visibility.Collapsed;
                cancelar.Visibility = Visibility.Collapsed;
                estadoInfo.IsEnabled = false;
                localizacaoInfo.IsEnabled = false;
                EditState.Visibility = Visibility.Visible;
                EditLocation.Visibility = Visibility.Visible;
            }


        }

        private void alterInfo(string code, string text)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("update Pulse.Paciente set Estado = '" + text + "' where Codigo = '" +code + "';", cn);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Updated");

            cn.Close();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            estadoInfo.Text = state;
            guardar.Visibility = Visibility.Collapsed;
            cancelar.Visibility = Visibility.Collapsed;
            estadoInfo.IsEnabled = false;
            localizacaoInfo.IsEnabled = false;
            EditState.Visibility = Visibility.Visible;
            EditLocation.Visibility = Visibility.Visible;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();

        }
    }


    

}
