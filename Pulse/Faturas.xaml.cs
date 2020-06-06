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
    /// Interaction logic for Faturas.xaml
    /// </summary>
    public partial class Faturas : Page
    {
        User user;
        private SqlConnection cn;
        List<FaturasTile> faturas;

        public Faturas(User user)
        {
            this.user = user;
            InitializeComponent();
            Nome.Content = user.getNome();
            faturas = new List<FaturasTile>();

        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cn = getSGBDConnection();
            getFaturas();
            ListViewFaturas.ItemsSource = faturas;
        }

        private void getFaturas()
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("select * from Pulse.Fatura " +
                "where CodigoPaciente = '" + user.getCode() + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                FaturasTile ft = new FaturasTile(
                    reader["NrFatura"].ToString(),
                    reader["Preco"].ToString(),
                    reader["Data"].ToString()
                );
                faturas.Add(ft);
            }


            cn.Close();
        }

        private class FaturasTile
        {
            public string nr { get; set; }
            public string price { get; set; }
            public string date { get; set; }

            public FaturasTile(String nr, String price, String date)
            {
                this.nr = nr;
                this.price = price;
                this.date = date.Substring(0,10);
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
