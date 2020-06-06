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
    /// Interaction logic for Receitas.xaml
    /// </summary>
    public partial class ExamesAnalises : Page
    {
        User user;
        private SqlConnection cn;
        List<AnaliseTile> analises;
        int index = 0;


        public ExamesAnalises(User user)
        {
            this.user = user;
            InitializeComponent();
            Nome.Content = user.getNome();
            analises = new List<AnaliseTile>();

        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("select Analise.ID, Analise.Data, Codigo, Nome, Descricao " +
                "from Pulse.Analise join Pulse.TemAnalise on (Analise.ID = IDAnalise) join Pulse.Consulta on (TemAnalise.IDConsulta = Consulta.ID) join Pulse.Utilizador on (CodigoMedico = Codigo) " +
                "where CodigoPaciente = '" + user.getCode() + "' " +
                "order by Analise.Data desc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                AnaliseTile r = new AnaliseTile(
                    reader["ID"].ToString(),
                    reader["Data"].ToString(),
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString(),
                    reader["Descricao"].ToString()
                );
                analises.Add(r);
            }
            cn.Close();

            if (analises.Count() <= 1)
            {
                ArrowButtons.Visibility = Visibility.Hidden;
            }
            else
            {
                ArrowButtons.Visibility = Visibility.Visible;
            }

            if (analises.Count() != 0)
            {
                displayAnalise(0);
            }

        }

        private void displayAnalise(int index)
        {
            Analise.Visibility = Visibility.Visible;
            Informacao.Visibility = Visibility.Visible;

            CodigoPrescitor.Content = analises[index].codigoMedico;
            PrescritorReceita.Content = analises[index].medico;

            id.Content = analises[index].id;
            Data.Content = analises[index].data.Substring(0,10);
            Descricao.Text = analises[index].descricao;

        }


        private class AnaliseTile
        {
            public string id { get; set; }
            public string data { get; set; }
            public string codigoMedico { get; set; }
            public string medico { get; set; }
            public string descricao { get; set; }

            public AnaliseTile(String id, String data, String codigo, String nome, String descricao)
            {
                this.id = id;
                this.data = data;
                this.codigoMedico = codigo;
                this.medico = nome;
                this.descricao = descricao;
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

        private void Next(object sender, MouseButtonEventArgs e)
        {
            index = (index + 1) % analises.Count();
            displayAnalise(index);
        }

        private void Previous(object sender, MouseButtonEventArgs e)
        {
            index--;
            if (index == -1)
            {
                index = analises.Count() - 1;
            }
            displayAnalise(index);
        }
    }
}
