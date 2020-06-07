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
    /// Interaction logic for Horario.xaml
    /// </summary>
    public partial class Horario : Page
    {
        List<Turno> horarioTiles;
        User user;
        public Horario(User user)
        {
            InitializeComponent();
            this.user = user;
            Nome.Content = user.getNome();
            horarioTiles = new List<Turno>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            horarioTiles = db.getShifts(this.user.getCode());
            ListViewProducts.ItemsSource = horarioTiles;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
