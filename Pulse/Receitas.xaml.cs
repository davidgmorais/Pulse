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
        List<Medicamento> medicamentos;
        List<ReceitaInfo> receitas;
        int index = 0;


        public Receitas(User user)
        {
            this.user = user;
            InitializeComponent();
            Nome.Content = user.getNome();
            medicamentos = new List<Medicamento>();
            receitas = new List<ReceitaInfo>();

        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            receitas = db.loadReceitasInfo(user.getCode());

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

        private void getMedicamento(ReceitaInfo receitasPage)
        {
            medicamentos.Clear();
            medicamentos = db.loadMedicamentos(receitasPage.id);

            ListViewMedicamentos.ItemsSource = medicamentos;
            ListViewMedicamentos.Items.Refresh();

            CodigoReceita.Content = receitas[index].id;
            DataReceita.Content = receitas[index].data.Substring(0, 10);
            CodigoPrescitor.Content = receitas[index].codigoMedico;
            PrescritorReceita.Content = receitas[index].medico;

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
