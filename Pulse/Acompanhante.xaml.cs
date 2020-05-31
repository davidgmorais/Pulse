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
    /// Interaction logic for Acompanhante.xaml
    /// </summary>
    public partial class Acompanhante : Page
    {
        User user;
        private SqlConnection cn;
        List<AcompanhanteTile> acompanhantes;

        public Acompanhante(User user)
        {
            InitializeComponent();
            this.user = user;
            acompanhantes = new List<AcompanhanteTile>();

            if (user.getCode().Substring(0, 2).Equals("99"))
            {
                Professional.Visibility = Visibility.Visible;
                Doente.Visibility = Visibility.Hidden;

            }
            else
            {
                Doente.Visibility = Visibility.Visible;
                Professional.Visibility = Visibility.Hidden;
            }
        }

        public void GoToPaciente(object sender, MouseButtonEventArgs e)
        {
            Pacient paciente = new Pacient(this.user);
            this.NavigationService.Navigate(paciente);
        }

        public void GoToProf(object sender, MouseButtonEventArgs e)
        {
            Profissional prof = new Profissional(this.user);
            this.NavigationService.Navigate(prof);
        }
        
        public void LogOut(object sender, MouseButtonEventArgs e)
        {
            Page1 p = new Page1();
            this.NavigationService.Navigate(p);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cn = getSGBDConnection();
            getAcompanhantes();
            ListViewAcompanhante.ItemsSource = acompanhantes;
        }

        private void getAcompanhantes()
        {

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("select Nome, Estado, Pulse.Paciente.Codigo " +
                "from Pulse.Utilizador join Pulse.Paciente on (Utilizador.Codigo = Paciente.Codigo) join Pulse.Acompanha on (Paciente.Codigo = CodigoPacienteAcompanha) " +
                "where Pulse.Acompanha.CodigoAcompanhanteAcompanha = '" + user.getCode() + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                AcompanhanteTile at = new AcompanhanteTile(
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["Estado"].ToString(),
                    "No Hospital", //reader["Localização"].ToString(),
                    "N/A"//reader["WaitingTime"].ToString()
                );
                acompanhantes.Add(at);
                
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


              
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CodeInsert.Visibility = Visibility.Hidden;
            NameTextBox.Text = "";
            CodigoTextBox.Text = "";
            
            CodeRemove.Visibility = Visibility.Hidden;
            CodigoTextBox2.Text = "";
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //verificar base de dados
            String name = NameTextBox.Text;
            String code = CodigoTextBox.Text;
            AcompanhanteTile at = verifyDB(name, code);
            //adicionar a lista
            if (at != null)
            {
                acompanhantes.Add(at);
                ListViewAcompanhante.Items.Refresh();
                //adicionar a base de dados
                addToDB(at);
                CodeInsert.Visibility = Visibility.Hidden;
                CodeError.Visibility = Visibility.Hidden;
                NomeError.Visibility = Visibility.Hidden;

                NameTextBox.Text = "";
                CodigoTextBox.Text = "";
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //verificar base de dados
            String code = CodigoTextBox2.Text;
            AcompanhanteTile at = null;

            foreach (AcompanhanteTile acomp in acompanhantes)
            {

                if (acomp.Code.Equals(code))
                {
                    at = acomp;
                    break;
                }
            }

            //adicionar a lista
            if (at != null)
            {
                acompanhantes.Remove(at);
                ListViewAcompanhante.Items.Refresh();
                //adicionar a base de dados
                removeFromDB(at);
                CodeRemove.Visibility = Visibility.Hidden;
            }
        }

        private void removeFromDB(AcompanhanteTile at)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("DELETE FROM Pulse.Acompanha WHERE CodigoPacienteAcompanha = '" + at.Code + "' and CodigoAcompanhanteAcompanha = '" + user.getCode() + "';", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        private void addToDB(AcompanhanteTile at)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Acompanha(CodigoPacienteAcompanha, CodigoAcompanhanteAcompanha) VALUES ( '" + at.Code + "', '" + user.getCode() + "');", cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

 

        private AcompanhanteTile verifyDB(string name, string code)
        {
            if (!verifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("select Paciente.Codigo, Nome, Estado " +
                "from Pulse.Utilizador join Pulse.Paciente on (Utilizador.Codigo = Paciente.Codigo) " +
                "where Paciente.Codigo = '" + code + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (!reader.Read())
            {
                //paciente nao existe
                CodeError.Visibility = Visibility.Visible;
                cn.Close();
                return null;
            } else if (!reader["Nome"].ToString().Equals(name))
            {
                //paciente não corresponde ao codigo
                NomeError.Visibility = Visibility.Visible;
                cn.Close();
                return null;
            } else 
            {
                AcompanhanteTile at = new AcompanhanteTile(
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["Estado"].ToString(),
                    "No Hospital", //reader["Localização"].ToString(),
                    "N/A"//reader["WaitingTime"].ToString()
                );
                cn.Close();
                return at;
            }
        }

       
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CodeInsert.Visibility = Visibility.Visible;
            NomeError.Visibility = Visibility.Hidden;
            CodeError.Visibility = Visibility.Hidden;
        }
        
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            CodeRemove.Visibility = Visibility.Visible;
            CodeError2.Visibility = Visibility.Hidden;
        }


        private class AcompanhanteTile
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string State { get; set; }
            public string Location { get; set; }
            public string WaitingTime { get; set; }

            public AcompanhanteTile(String Code, String nome, String estado, String localizacao, String tempo)
            {
                this.Code = Code;
                this.Name = nome;
                this.State = estado;
                this.Location = localizacao;
                this.WaitingTime = tempo;

            }

        }
    }
}

