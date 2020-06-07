using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
        User user;
        List<ConsulaInfo> listaConsultas;
        Random random = new Random();


        List<Doctor> doctor;
        List<String> horas;

        public Calendario(User user)
        {
            this.user = user;
            listaConsultas = new List<ConsulaInfo>();
            doctor = new List<Doctor>();
            horas = new List<String>();
            InitializeComponent();
            CalendarioConsultas.DisplayDate = DateTime.Now;
            CalendarioConsultas.SelectedDate = DateTime.Now;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            medicList.ItemsSource = doctor;
            hoursList.ItemsSource = horas;

            ListViewConsultas.ItemsSource = listaConsultas;
            listaConsultas = db.getPacienteConsultas(DateTime.Now, this.user.getCode());
            ListViewConsultas.Items.Refresh();
        }


        private void CalendarioConsultas_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalendarioConsultas.SelectedDate.HasValue)
            {
                listaConsultas.Clear();
                listaConsultas = db.getPacienteConsultas(CalendarioConsultas.SelectedDate.Value, this.user.getCode());
                ListViewConsultas.ItemsSource = listaConsultas;

                marcacao.Visibility = Visibility.Hidden;
                Book.Visibility = Visibility.Visible;
            }

        }


        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            hoursList.SelectedItem = null;
            medicList.SelectedItem = null;

            marcacao.Visibility = Visibility.Visible;
            Book.Visibility = Visibility.Hidden;
            HorasLabel.Visibility = Visibility.Hidden;
            Horas.Visibility = Visibility.Hidden;
            MarcarButton.Visibility = Visibility.Hidden;

            doctor.Clear();
            loadDoctors(CalendarioConsultas.SelectedDate.Value);
            
        }

        private void loadDoctors(DateTime date)
        {
            doctor = db.loadDoctor(date);
            medicList.ItemsSource = doctor;
        }

        private void MarcarConsulta(object sender, RoutedEventArgs e)
        {

            marcar(medicList.SelectedIndex, CalendarioConsultas.SelectedDate.Value, hoursList.SelectedIndex);

            marcacao.Visibility = Visibility.Hidden;
            Book.Visibility = Visibility.Visible;
          
        }

        private void marcar(int selectedIndex1, DateTime date, int selectedIndex2)
        {
            Doctor medico = doctor[selectedIndex1];
            String data = date.ToString("yyyy-MM-dd");
            String hora = horas[selectedIndex2];
            int consultorio = random.Next(99) + 1;

            db.inserConsulta(data, hora, consultorio, medico, this.user.getCode());

            listaConsultas.Add(
                    new ConsulaInfo(data, consultorio.ToString(), hora, medico.Nome)
                );

            ListViewConsultas.Items.Refresh();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HorasLabel.Visibility = Visibility.Visible;
            Horas.Visibility = Visibility.Visible;

            horas.Clear();
            getHoras(medicList.SelectedIndex, CalendarioConsultas.SelectedDate.Value);
        }

        private void getHoras(int selectedIndex, DateTime date)
        {
            if (selectedIndex < 0 || selectedIndex >= doctor.Count()) return;

            horas = db.loadHoras(date, doctor[selectedIndex].Nome);
            hoursList.ItemsSource = horas;
        }

        private void hoursList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarcarButton.Visibility = Visibility.Visible;
        }
    }
}
