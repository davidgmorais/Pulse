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
using System.Data.SqlClient;
using System.Data;

namespace Pulse
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        
        public Page1()
        {
            InitializeComponent();
            db.innit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Code_Error.Visibility = Visibility.Hidden;
            Email_Error.Visibility = Visibility.Hidden;

            String email = EmailTextBox.Text;
            String code = CodeTextBox.Password.ToString();

            User u = db.loadUser(email, code);
            if (u != null)
            {
                if (u!= null) { 
                    if (u.getCode().Substring(0,2).Equals("99"))
                    {
                        Profissional next = new Profissional(u);
                        this.NavigationService.Navigate(next);
                    }
                    else
                    {
                        Pacient next = new Pacient(u);
                        this.NavigationService.Navigate(next);
                    }
                }

            } else
            {
                Code_Error.Visibility = Visibility.Visible;
                Email_Error.Visibility = Visibility.Visible;

            }


        }


        private void Register_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Page2 p = new Page2();
            this.NavigationService.Navigate(p);
        }

       
    }
}
