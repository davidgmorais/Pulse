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
    /// Interaction logic for Calendario.xaml
    /// </summary>
    public partial class Calendario : Page
    {
        DateTime Date;
        User user;

        public Calendario(User user)
        {
            InitializeComponent();
            this.user = user;
            CalendarioConsultas.DisplayDate = DateTime.Now;

           

        }
    }
}
