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
        List<Analises> analises;
        int index = 0;


        public ExamesAnalises(User user)
        {
            this.user = user;
            InitializeComponent();
            Nome.Content = user.getNome();
            analises = new List<Analises>();

        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            analises = db.loadAnalises(this.user.getCode());
            

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
