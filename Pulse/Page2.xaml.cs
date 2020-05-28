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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private SqlConnection cn;

        public Page2()
        {
            DatePicker DataNascimento = new DatePicker();
            InitializeComponent();
        }

        private void Page2_Load(object sender, EventArgs e)
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

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? date = DataNascimento.SelectedDate;
            if (date == null) return;       
            
            String Name = NameTextBox.Text;
            String Code = CodeTextBox.Text;
            String Email = EmailTextBox.Text;
            String NIF = NIFTextBox.Text;


            verificar(Code, Email, NIF);
            String dateStr = date.Value.ToString("MM/dd/yyyy");
            Console.WriteLine(dateStr);

            User novo = new User(Name, Code, Email, dateStr, NIF);
            insert(novo);

            Pacient p = new Pacient(novo.getCode());
            this.NavigationService.Navigate(p);
            //crir user
            //insert na bd

        }

        private void insert(User novo)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO Pulse.Utilizador(Codigo, Nome, DataNascimento, Email, NIF) VALUES ( '" + novo.getCode() + "', '" + novo.getNome() + "', '" + novo.getBDay() + "', '" + novo.getEmail() + "', '" + novo.getNIF() + "');", cn);
            Console.WriteLine(cmd.ToString());
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        private bool verificar(string code, string email, string nif)
        {

            if (code.Length != 8)
            {
                Code_Error.Visibility = Visibility.Visible;
                return false;
            }

            if (!email.Contains('@') || !email.Contains('.')) return false;
            if (nif.Length != 9) return false;

            if (!verifySGBDConnection())
                return false;
                SqlCommand cmd = new SqlCommand("SELECT email as email, codigo as codigo FROM Pulse.Utilizador WHERE email = '" + email + "' or codigo = " + code, cn);
                SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                if (email.Equals(reader["email"])) Email_Error.Visibility = Visibility.Visible;
                if (code.Equals(reader["codigo"])) Code_Error.Visibility = Visibility.Visible;

                cn.Close();
                return false;
            }

            Email_Error.Visibility = Visibility.Hidden;
            Code_Error.Visibility = Visibility.Hidden;
            cn.Close();
            return true;
        }

        private void Register_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Page1 page1 = new Page1();
            this.NavigationService.Navigate(page1);

        }
    }
}
