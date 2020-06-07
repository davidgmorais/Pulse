using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for ProfSaude.xaml
    /// </summary>
    public partial class Profissional : Page
    {
        User user;
        public Profissional(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void HorarioPress(object sender, MouseButtonEventArgs e)
        {
            HorarioButton.Width = 129;
            HorarioButton.Height = 149;
            HorarioLabel.FontSize = 15;
        }

        private void OpenHorario(object sender, MouseButtonEventArgs e)
        {
            HorarioButton.Width = 126;
            HorarioButton.Height = 146;
            HorarioLabel.FontSize = 16;

            Horario h = new Horario(this.user);
            this.NavigationService.Navigate(h);

        }

        private void ConsultasPress(object sender, MouseButtonEventArgs e)
        {
            ConsultasButton.Width = 129;
            ConsultasButton.Height = 149;
            ConsultasLabel.FontSize = 15;
        }

        private void OpenConsultas(object sender, MouseButtonEventArgs e)
        {
            ConsultasButton.Width = 126;
            ConsultasButton.Height = 146;
            ConsultasLabel.FontSize = 16;

            Consultas consulta = new Consultas(this.user);
            this.NavigationService.Navigate(consulta);
        }

        private void AtualizarPress(object sender, MouseButtonEventArgs e)
        {
            AtualizarButton.Width = 129;
            AtualizarButton.Height = 149;
            AtualizarLabel.FontSize = 15;
        }

        private void OpenAtualizar(object sender, MouseButtonEventArgs e)
        {
            AtualizarButton.Width = 126;
            AtualizarButton.Height = 146;
            AtualizarLabel.FontSize = 16;
            Update update = new Update(this.user);
            this.NavigationService.Navigate(update);

        }
        
        private void GoToPaciente(object sender, MouseButtonEventArgs e)
        {
            Pacient p = new Pacient(this.user);
            this.NavigationService.Navigate(p);
        }

        private void GoToAcompanhante(object sender, MouseButtonEventArgs e)
        {
            Acompanhante acomp = new Acompanhante(this.user);
            this.NavigationService.Navigate(acomp);
        }
        
        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            Perfil p = new Perfil(this.user);
            this.NavigationService.Navigate(p);
        }

        private void Profissional_Load(object sender, EventArgs e)
        {
            String consulta = db.getNearestAppointment(user.getCode());
            String turno = db.getNearestShift(user.getCode());

            if (consulta != null)
            {
                DataPoximaConsulta.Visibility = Visibility.Visible;
                ProximaConsulta.Visibility = Visibility.Visible;

                DataPoximaConsulta.Content = consulta.Substring(0, 10);
            }
            else
            {
                DataPoximaConsulta.Visibility = Visibility.Hidden;
                ProximaConsulta.Visibility = Visibility.Hidden;

            }
            

            if (turno != null)
            {
                DataPoximoTurno.Visibility = Visibility.Visible;
                ProximoTurno.Visibility = Visibility.Visible;
                DataPoximoTurno.Content = turno.Substring(0, 10);
            }
            else
            {
                DataPoximoTurno.Visibility = Visibility.Hidden;
                ProximoTurno.Visibility = Visibility.Hidden;

            }

        }

    }
}
