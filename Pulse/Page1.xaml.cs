using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace Pulse
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private SqlConnection cn;
        
        public Page1()
        {
            InitializeComponent();
        }

        private void Page1_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("Data Source=DESKTOP-HB27C6M;Initial Catalog=Pulse;Integrated Security=True");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            String email = EmailTextBox.Text;
            String code = CodeTextBox.Password.ToString();
            if (verify(email, code))
            {
                Code_Error.Visibility = Visibility.Hidden;
                Email_Error.Visibility = Visibility.Hidden;

                if (!verifySGBDConnection())
                    return;
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pulse.Utilizador WHERE codigo = '" + code + "'", cn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    User u = new User(
                        reader["Codigo"].ToString(),
                        reader["Nome"].ToString(),
                        reader["DataNascimento"].ToString(),
                        reader["Morada"].ToString(),
                        reader["Email"].ToString(),
                        reader["Telefone"].ToString(),
                        reader["Telemovel"].ToString(),
                        reader["NIF"].ToString()
                    );

                    Pacient p = new Pacient(u);
                    this.NavigationService.Navigate(p);
                }

                cn.Close();
        
            } 

        }

        private bool verify(string email, string code)
        {
            if (!email.Contains('@') || !email.Contains('.')) return false;
            if (code.Length != 8)
            {
                Email_Error.Visibility = Visibility.Visible;
                return false;
            }


            //verificar base de dados
            if (!verifySGBDConnection())
                return false;
            SqlCommand cmd = new SqlCommand("SELECT email, codigo as codigo FROM Pulse.Utilizador WHERE email = '" + email + "'", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                if (code.Equals(reader["codigo"].ToString()))
                {
                    cn.Close();
                    return true;
                }
            } else
            {
                Email_Error.Visibility = Visibility.Visible;
                cn.Close();
                return false;
            }


            cn.Close();
            Code_Error.Visibility = Visibility.Visible;
            return false;
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        private void Register_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Page2 p = new Page2();
            this.NavigationService.Navigate(p);
        }

       
    }
}
