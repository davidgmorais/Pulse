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
    public partial class Receitas : Page
    {
        User user;
        List<MedicamentosTile> medicamentos;
        private SqlConnection cn;
        List<ReceitasPage> receitas;
        int index = 0;


        public Receitas(User user)
        {
            this.user = user;
            InitializeComponent();
            Nome.Content = user.getNome();
            medicamentos = new List<MedicamentosTile>();
            receitas = new List<ReceitasPage>();

        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("select Receita.ID, Receita.Data, Codigo, Nome " +
                "from Pulse.Receita join Pulse.TemReceita on (ID = IDReceita) join Pulse.Consulta on (TemReceita.IDConsulta = Consulta.ID) join Pulse.Utilizador on (CodigoMedico = Codigo) " +
                "where CodigoPaciente = '" + user.getCode() +"' " +
                "order by Receita.Data desc;", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                ReceitasPage r = new ReceitasPage(
                    reader["ID"].ToString(),
                    reader["Data"].ToString(),
                    reader["Codigo"].ToString(),
                    reader["Nome"].ToString()
                );
                receitas.Add(r);
            }
            cn.Close();


            if (receitas.Count() == 1 || receitas.Count() == 0)
            {
                ArrowButtons.Visibility = Visibility.Hidden;
            } else
            {
                ArrowButtons.Visibility = Visibility.Visible;
            }

            if (receitas.Count() != 0)
            {
                info.Visibility = Visibility.Visible;
                getMedicamento(receitas[index]);
            }

        }

        private void getMedicamento(ReceitasPage receitasPage)
        {
            if (!verifySGBDConnection())
                return;

            medicamentos.Clear();
            SqlCommand cmd = new SqlCommand("select * from Pulse.Medicamento where IDReceita = '" + receitasPage.id + "'; ", cn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                MedicamentosTile mt = new MedicamentosTile(
                    reader["ID"].ToString(),
                    reader["Designacao"].ToString(),
                    reader["Dosagem"].ToString()
                );
                medicamentos.Add(mt);
                mt.index = medicamentos.IndexOf(mt) + 1;

            }
            cn.Close();

            ListViewMedicamentos.ItemsSource = medicamentos;
            ListViewMedicamentos.Items.Refresh();

            CodigoReceita.Content = receitas[index].id;
            DataReceita.Content = receitas[index].data.Substring(0, 10);
            CodigoPrescitor.Content = receitas[index].codigoMedico;
            PrescritorReceita.Content = receitas[index].medico;
             


        }

        private class MedicamentosTile
        {
            public int index { get; set; }
            public string id { get; set; }
            public string nome { get; set; }
            public string dosagem { get; set; }

            public MedicamentosTile(String id, String nome, String dosagem)
            {
                this.id = id;
                this.nome = nome;
                this.dosagem = dosagem;
            }
        }
        
        
        
        private class ReceitasPage
        {
            public string id { get; set; }
            public string data { get; set; }
            public string codigoMedico { get; set; }
            public string medico { get; set; }

            public ReceitasPage(String id, String data, String codigo, String nome)
            {
                this.id = id;
                this.data = data;
                this.codigoMedico = codigo;
                this.medico = nome;
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
            index = (index + 1) % receitas.Count();
            getMedicamento(receitas[index]);
        }

        private void Previous(object sender, MouseButtonEventArgs e)
        {
            index--;
            if (index == -1)
            {
                index = receitas.Count() - 1;
            }
            getMedicamento(receitas[index]);
        }
    }
}
