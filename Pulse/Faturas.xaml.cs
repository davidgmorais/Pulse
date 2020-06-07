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
        List<FaturaInfo> faturas;

        public Faturas(User user)
        {
            this.user = user;
            InitializeComponent();
            Nome.Content = user.getNome();
            faturas = new List<FaturaInfo>();

        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            faturas = db.getFaturas(this.user.getCode());
            ListViewFaturas.ItemsSource = faturas;
        }


    }
}
