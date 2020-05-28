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

namespace Pulse
{
    /// <summary>
    /// Interaction logic for Pacient.xaml
    /// </summary>
    public partial class Pacient : Page
    {
        String code;

        public Pacient(String code)
        {
            InitializeComponent();
            this.code = code;

            if (code.Substring(0,2).Equals("99")) {
                Professional.Visibility = Visibility.Visible;
            } else
            {
                Doente.Visibility = Visibility.Visible;
            }

        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            code = null;
            Page1 p = new Page1();
            this.NavigationService.Navigate(p);

        }
    }
}
