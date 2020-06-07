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
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Page
    {
        //DateTime Date;
        User user;
        List<ConsulaInfo> listaConsultas;

        public Consultas(User user)
        {
            this.user = user;
            listaConsultas = new List<ConsulaInfo>();
            InitializeComponent();
            CalendarioConsultas.DisplayDate = DateTime.Now;
            CalendarioConsultas.SelectedDate = DateTime.Now;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            ListViewConsultas.ItemsSource = listaConsultas;
            listaConsultas = db.getConsultas(DateTime.Now, this.user.getCode());

            ListViewConsultas.Items.Refresh();

        }

        private void CalendarioConsultas_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalendarioConsultas.SelectedDate.HasValue)
            {
                listaConsultas = db.getConsultas(CalendarioConsultas.SelectedDate.Value, this.user.getCode());

                ListViewConsultas.ItemsSource = listaConsultas;
            }

        }


        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }


        

    }
}

