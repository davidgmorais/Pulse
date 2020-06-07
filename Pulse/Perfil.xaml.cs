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
    /// Interaction logic for Perfil.xaml
    /// </summary>
    public partial class Perfil : Page
    {
        String address;
        String mail;
        String telephone;
        String phone;
        User user;

        public Perfil(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
            Nome.Content = this.user.getNome();
            Codiga.Content = this.user.getCode();
            email.Text = this.user.getEmail();
            morada.Text = this.user.getMorada();
            telemovel.Text = this.user.getTelemovel();
            telefone.Text = this.user.getTelefone();
            Nif.Text = this.user.getNIF();    
           
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page1 page = new Page1();
            this.NavigationService.Navigate(page);

        }
        private void ButtonGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (address != morada.Text)
            {
                db.alterAddress(morada.Text, this.user.getCode());
                this.user.setMorada(morada.Text);

            }

            if (mail != email.Text)
            {
                db.alterMail(email.Text, this.user.getCode());
                this.user.setEmail(email.Text);

            }

            if (telephone != telefone.Text)
            {
                db.alterTelephone(telefone.Text, this.user.getCode());
                this.user.setTelefone(telefone.Text);

            }

            if (phone != telemovel.Text)
            {
                db.alterPhone(telemovel.Text, this.user.getCode());
                this.user.setTelemove(telemovel.Text);

            }

            guardar.Visibility = Visibility.Collapsed;
            cancelar.Visibility = Visibility.Collapsed;
            email.IsEnabled = false;
            morada.IsEnabled = false;
            telemovel.IsEnabled = false;
            telefone.IsEnabled = false;
            edit.Visibility = Visibility.Visible;
            buttonLogout.Visibility = Visibility.Visible;

        }

        private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
        {
            guardar.Visibility = Visibility.Hidden;
            cancelar.Visibility = Visibility.Hidden;
            email.IsEnabled = false;
            morada.IsEnabled = false;
            telemovel.IsEnabled = false;
            telefone.IsEnabled = false;
            buttonLogout.Visibility = Visibility.Visible;
        }

        private void edit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            morada.IsEnabled = true;
            email.IsEnabled = true;
            cancelar.IsEnabled = true;
            telemovel.IsEnabled = true;
            telefone.IsEnabled = true;
            guardar.Visibility = Visibility.Visible;
            cancelar.Visibility = Visibility.Visible;
            buttonLogout.Visibility = Visibility.Hidden;

            address = morada.Text;
            mail = email.Text;
            telephone = telefone.Text;
            phone = telemovel.Text;

        }
    }
}
