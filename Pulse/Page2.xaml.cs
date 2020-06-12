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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            DatePicker DataNascimento = new DatePicker();
            InitializeComponent();
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Email_Error.Visibility = Visibility.Hidden;
            Code_Error.Visibility = Visibility.Hidden;
        
            DateTime? date = DataNascimento.SelectedDate;
            if (date == null) return;       
            
            String Name = NameTextBox.Text;
            String Code = CodeTextBox.Text;
            String Email = EmailTextBox.Text;
            String NIF = NIFTextBox.Text;
            String password = PasswordTextBox.Password;

            try
            {
                String dateStr = date.Value.ToString("MM/dd/yyyy");
                
                User novo = new User(Name, Code, Email, dateStr, NIF);
                db.insertUser(novo, password);

                Pacient p = new Pacient(novo);
                this.NavigationService.Navigate(p);

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Email_Error.Visibility = Visibility.Visible;
                Code_Error.Visibility = Visibility.Visible;
            }


        }

       
        private void Register_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Page1 page1 = new Page1();
            this.NavigationService.Navigate(page1);

        }
    }
}
