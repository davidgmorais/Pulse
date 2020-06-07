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
    /// Interaction logic for Pacient.xaml
    /// </summary>
    public partial class Pacient : Page
    {
        User user;
        public Pacient(User user)
        {
            InitializeComponent();
            this.user = user;

            if (user.getCode().Substring(0,2).Equals("99")) {
                Professional.Visibility = Visibility.Visible;
                Doente.Visibility = Visibility.Hidden;

            }
            else
            {
                Doente.Visibility = Visibility.Visible;
                Professional.Visibility = Visibility.Hidden;
            }

        }

        private void Pacient_Load(object sender, EventArgs e)
        {
            String date = db.getNearestPacientAppointment(this.user.getCode());

            if (date != null)
            {
                DataPoximaConsulta.Visibility = Visibility.Visible;
                PoximaConsulta.Visibility = Visibility.Visible;
                DataPoximaConsulta.Content = date.Substring(0, 10);
            } else
            {
                DataPoximaConsulta.Visibility = Visibility.Hidden;
                PoximaConsulta.Visibility = Visibility.Hidden;
            }
        }
       

        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            Perfil p = new Perfil(this.user);
            this.NavigationService.Navigate(p);

        }

        private void OpenCalendario(object sender, MouseButtonEventArgs e)
        {
            CalendarButton.Width = 126;
            CalendarButton.Height = 146;
            CalendarLabel.FontSize = 16;

           

            Calendario calendar = new Calendario(this.user);
            this.NavigationService.Navigate(calendar);

        }

        private void OpenEA(object sender, MouseButtonEventArgs e)
        {
            EAButton.Width = 126;
            EAButton.Height = 146;
            EALabel.FontSize = 16;

            ExamesAnalises ea = new ExamesAnalises(this.user);
            this.NavigationService.Navigate(ea);


        }
        private void OpenReceitas(object sender, MouseButtonEventArgs e)
        {
            ReceitasButton.Width = 126;
            ReceitasButton.Height = 146;
            ReceitasLabel.FontSize = 16;

            Receitas receitas = new Receitas(this.user);
            this.NavigationService.Navigate(receitas);


        }
        private void OpenFaturas(object sender, MouseButtonEventArgs e)
        {
            FaturasButton.Width = 126;
            FaturasButton.Height = 146;
            FaturasLabel.FontSize = 16;

            Faturas faturas = new Faturas(this.user);
            this.NavigationService.Navigate(faturas);

        }



        private void CalendarioPress(object sender, MouseButtonEventArgs e)
        {
            CalendarButton.Width = 129;
            CalendarButton.Height = 149;
            CalendarLabel.FontSize = 15;

      

        }

        private void EAPress(object sender, MouseButtonEventArgs e)
        {
            EAButton.Width = 129;
            EAButton.Height = 149;
            EALabel.FontSize = 15;


        }
        private void ReceitasPress(object sender, MouseButtonEventArgs e)
        {
            ReceitasButton.Width = 129;
            ReceitasButton.Height = 149;
            ReceitasLabel.FontSize = 15;

        }
        private void FaturasPress(object sender, MouseButtonEventArgs e)
        {
            FaturasButton.Width = 129;
            FaturasButton.Height = 149;
            FaturasLabel.FontSize = 15;

        }

        private void GoToAcompanhante(object sender, MouseButtonEventArgs e)
        {
            Acompanhante acomp = new Acompanhante(this.user);
            this.NavigationService.Navigate(acomp);

        }
        
        private void GoToProfissional(object sender, MouseButtonEventArgs e)
        {
            Profissional prof = new Profissional(this.user);
            this.NavigationService.Navigate(prof);

        }
    }
}
