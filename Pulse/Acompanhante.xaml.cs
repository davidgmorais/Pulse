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
    /// Interaction logic for Acompanhante.xaml
    /// </summary>
    public partial class Acompanhante : Page
    {
        User user;
        List<AcompanhanteInfo> acompanhantes;

        public Acompanhante(User user)
        {
            InitializeComponent();
            this.user = user;
            acompanhantes = new List<AcompanhanteInfo>();

            if (user.getCode().Substring(0, 2).Equals("99"))
            {
                Professional.Visibility = Visibility.Visible;
                Doente.Visibility = Visibility.Hidden;

            }
            else
            {
                Doente.Visibility = Visibility.Visible;
                Professional.Visibility = Visibility.Hidden;
            }
        }

        public void GoToPaciente(object sender, MouseButtonEventArgs e)
        {
            Pacient paciente = new Pacient(this.user);
            this.NavigationService.Navigate(paciente);
        }

        public void GoToProf(object sender, MouseButtonEventArgs e)
        {
            Profissional prof = new Profissional(this.user);
            this.NavigationService.Navigate(prof);
        }
        
        public void LogOut(object sender, MouseButtonEventArgs e)
        {
            Perfil p = new Perfil(this.user);
            this.NavigationService.Navigate(p);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            acompanhantes = db.getAcompanhantes(this.user.getCode());
            ListViewAcompanhante.ItemsSource = acompanhantes;
        }

        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CodeInsert.Visibility = Visibility.Hidden;
            NameTextBox.Text = "";
            CodigoTextBox.Text = "";
            
            CodeRemove.Visibility = Visibility.Hidden;
            CodigoTextBox2.Text = "";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CodeError.Visibility = Visibility.Hidden;
            NomeError.Visibility = Visibility.Hidden;
            
            //verificar base de dados
            String name = NameTextBox.Text;
            String code = CodigoTextBox.Text;
            AcompanhanteInfo at = db.verifyAcompanhante(name, code);

            //adicionar a lista
            if (at != null)
            {
                acompanhantes.Add(at);
                ListViewAcompanhante.Items.Refresh();

                //adicionar a base de dados
                db.addAcompanhante(at, this.user.getCode());
                CodeInsert.Visibility = Visibility.Hidden;

                NameTextBox.Text = "";
                CodigoTextBox.Text = "";
            } else
            {
                CodeError.Visibility = Visibility.Visible;
                NomeError.Visibility = Visibility.Visible;
            }
        }


        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //verificar base de dados
            String code = CodigoTextBox2.Text;
            AcompanhanteInfo at = null;

            foreach (AcompanhanteInfo acomp in acompanhantes)
            {

                if (acomp.Code.Equals(code))
                {
                    at = acomp;
                    break;
                }
            }

            //adicionar a lista
            if (at != null)
            {
                acompanhantes.Remove(at);
                ListViewAcompanhante.Items.Refresh();
                //adicionar a base de dados
                db.removeAcompanhante(at, this.user.getCode());
                CodeRemove.Visibility = Visibility.Hidden;
            }
        }     

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CodeInsert.Visibility = Visibility.Visible;
            NomeError.Visibility = Visibility.Hidden;
            CodeError.Visibility = Visibility.Hidden;
        }
        
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            CodeRemove.Visibility = Visibility.Visible;
            CodeError2.Visibility = Visibility.Hidden;
        }
     
    }
}

