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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Page
    {
        private User user;

        String state;
        String location;
        String code;

        public Update(User user)
        {
            this.user = user;
            InitializeComponent();
        }


        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Paciente pacienteProcurado = db.getPaciente(codigo.Text, this.user.getCode());

            if (pacienteProcurado != null)
            {
                CodeWarning.Visibility = Visibility.Hidden;
                nome.Text = pacienteProcurado.getName();
                estadoInfo.Text = pacienteProcurado.getEstado();
                localizacaoInfo.Text = pacienteProcurado.getLocal();
                EditState.Visibility = Visibility.Visible;
                EditLocation.Visibility = Visibility.Visible;

                this.code = codigo.Text;
                Alta.IsEnabled = true;

            }
            else
            {
                CodeWarning.Visibility = Visibility.Visible;
                nome.Text = "";
                estadoInfo.Text = "";
                localizacaoInfo.Text = "";
                EditState.Visibility = Visibility.Hidden;
                EditLocation.Visibility = Visibility.Hidden;

                this.code = null;
                Alta.IsEnabled = false;

            }
        }

        

        private void ButtonAlta_MouseUp(object sender, RoutedEventArgs e)
        {
            db.darAlta(this.code, this.user.getCode());

            nome.Text = "";
            estadoInfo.Text = "";
            localizacaoInfo.Text = "";
            EditState.Visibility = Visibility.Hidden;
            EditLocation.Visibility = Visibility.Hidden;
            this.code = null;
            Alta.IsEnabled = false;
        }

        

        private void editInfoState(object sender, MouseButtonEventArgs e)
        {

            estadoInfo.IsEnabled = true;
            guardar.Visibility = Visibility.Visible;
            cancelar.Visibility = Visibility.Visible;
            EditState.Visibility = Visibility.Hidden;
            EditLocation.Visibility = Visibility.Hidden;

             state = estadoInfo.Text;
        }


        private void editInfoLocation(object sender, MouseButtonEventArgs e)
        {
            localizacaoInfo.IsEnabled = true;
            guardar.Visibility = Visibility.Visible;
            cancelar.Visibility = Visibility.Visible;
            EditState.Visibility = Visibility.Hidden;
            EditLocation.Visibility = Visibility.Hidden;

            location = localizacaoInfo.Text;
        }

        private void ButtonGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (state != estadoInfo.Text)
            {
                db.alterInfo(this.code, estadoInfo.Text);
                guardar.Visibility = Visibility.Collapsed;
                cancelar.Visibility = Visibility.Collapsed;
                estadoInfo.IsEnabled = false;
                localizacaoInfo.IsEnabled = false;
                EditState.Visibility = Visibility.Visible;
                EditLocation.Visibility = Visibility.Visible;


            } 
            
            if (location != localizacaoInfo.Text)
            {
                db.alterLocation(this.code, localizacaoInfo.Text);
                guardar.Visibility = Visibility.Collapsed;
                cancelar.Visibility = Visibility.Collapsed;
                estadoInfo.IsEnabled = false;
                localizacaoInfo.IsEnabled = false;
                EditState.Visibility = Visibility.Visible;
                EditLocation.Visibility = Visibility.Visible;
            }

        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            estadoInfo.Text = state;
            guardar.Visibility = Visibility.Collapsed;
            cancelar.Visibility = Visibility.Collapsed;
            estadoInfo.IsEnabled = false;
            localizacaoInfo.IsEnabled = false;
            EditState.Visibility = Visibility.Visible;
            EditLocation.Visibility = Visibility.Visible;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();

        }
    }

}
