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
using Scala.M2.Opdracht03.Core;

namespace Scala.M2.Opdracht03.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool isNew;
        bool isVerhuurNew;
        VakantiehuisService vakantiehuisService;
        VerhuurService verhuurService;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vakantiehuisService = new VakantiehuisService();
            verhuurService = new VerhuurService();

            grpVerhuur.Visibility = Visibility.Hidden;
            StartSituatie();
            VerhuurStandaardBeeld();
            ControlsLeegmaken();
            VulListbox();
        }
        private void VulListbox()
        {
            lstVakantiehuizen.ItemsSource = null;
            lstVakantiehuizen.ItemsSource = vakantiehuisService.Vakantiehuizen;
        }
        private void StartSituatie()
        {
            grpVakantiehuizen.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnBewaren.Visibility = Visibility.Hidden;
            btnAnnuleren.Visibility = Visibility.Hidden;
        }
        private void BewerkSituatie()
        {
            grpVakantiehuizen.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnBewaren.Visibility = Visibility.Visible;
            btnAnnuleren.Visibility = Visibility.Visible;
            grpVerhuur.Visibility = Visibility.Hidden;
        }
        private void ControlsLeegmaken()
        {
            txtAdres.Text = "";
            txtGemeente.Text = "";
            txtHuisnaam.Text = "";
            txtMaxPersonen.Text = "";
            txtPrijsPerNacht.Text = "";
        }
        private void lstVakantiehuizen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grpVerhuur.Visibility = Visibility.Hidden;
            ControlsLeegmaken();
            if(lstVakantiehuizen.SelectedItem != null)
            {
                Vakantiehuis vakantiehuis = (Vakantiehuis)lstVakantiehuizen.SelectedItem;
                txtHuisnaam.Text = vakantiehuis.Huisnaam;
                txtAdres.Text = vakantiehuis.Adres;
                txtGemeente.Text = vakantiehuis.Gemeente;
                txtPrijsPerNacht.Text = vakantiehuis.PrijsPerNacht.ToString("#,##0.00");
                txtMaxPersonen.Text = vakantiehuis.MaxPersonen.ToString();
                grpVerhuur.Visibility = Visibility.Visible;
                VulVerhuurListbox(vakantiehuis.ID);
            }
        }

        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            ControlsLeegmaken();
            BewerkSituatie();
            txtHuisnaam.Focus();
        }

        private void btnWijzig_Click(object sender, RoutedEventArgs e)
        {
            if (lstVakantiehuizen.SelectedItem != null)
            {
                isNew = false;
                BewerkSituatie();
                txtHuisnaam.Focus();
            }
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (lstVakantiehuizen.SelectedItem != null)
            {
                if(MessageBox.Show("Ben je zeker ?","Wissen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Vakantiehuis vakantiehuis = (Vakantiehuis)lstVakantiehuizen.SelectedItem;
                    verhuurService.VerwijderingVakantiehuis(vakantiehuis.ID);
                    vakantiehuisService.VerwijderVakantiehuis(vakantiehuis);
                    ControlsLeegmaken();
                    VulListbox();
                }
            }
        }

        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            StartSituatie();
            ControlsLeegmaken();
            if(lstVakantiehuizen.SelectedItem != null)
            {
                lstVakantiehuizen_SelectionChanged(null, null);
            }
        }

        private void btnBewaren_Click(object sender, RoutedEventArgs e)
        {
            string huisnaam = txtHuisnaam.Text.Trim();
            string adres = txtAdres.Text.Trim();
            string gemeente = txtGemeente.Text.Trim();
            decimal prijsPerNacht;
            byte maxPersonen;
            decimal.TryParse(txtPrijsPerNacht.Text, out prijsPerNacht);
            byte.TryParse(txtMaxPersonen.Text, out maxPersonen);
            Vakantiehuis vakantiehuis;
            if(isNew)
            {
                try
                {
                    vakantiehuis = new Vakantiehuis(huisnaam, adres, gemeente, prijsPerNacht, maxPersonen);
                }
                catch(Exception fout)
                {
                    MessageBox.Show(fout.Message);
                    return;
                }
                vakantiehuisService.VoegVakantiehuisToe(vakantiehuis);
            }
            else
            {
                vakantiehuis = (Vakantiehuis)lstVakantiehuizen.SelectedItem;
                try
                {
                    vakantiehuis.Huisnaam = huisnaam;
                    vakantiehuis.Adres = adres;
                    vakantiehuis.Gemeente = gemeente;
                    vakantiehuis.PrijsPerNacht = prijsPerNacht;
                    vakantiehuis.MaxPersonen = maxPersonen;
                }
                catch(Exception fout)
                {
                    MessageBox.Show(fout.Message);
                    return;
                }
            }
            VulListbox();
            StartSituatie();
            lstVakantiehuizen.SelectedItem = vakantiehuis;
        }

        // code voor verhuur

        private void VerhuurStandaardBeeld()
        {
            grpAlleVerhuren.IsEnabled = true;
            grpVerhuringDetail.IsEnabled = false;
            btnBewaarVerhuur.Visibility = Visibility.Hidden;
            btnAnnuleerVerhuur.Visibility = Visibility.Hidden;
            lblVerhuurError.Visibility = Visibility.Hidden;
        }
        private void VerhuurBewerkBeeld()
        {
            grpAlleVerhuren.IsEnabled = false;
            grpVerhuringDetail.IsEnabled = true;
            btnBewaarVerhuur.Visibility = Visibility.Visible;
            btnAnnuleerVerhuur.Visibility = Visibility.Visible;
            lblVerhuurError.Visibility = Visibility.Hidden;
        }
        private void VulVerhuurListbox(string vakantiehuisID)
        {
            lstVerhuur.ItemsSource = null;
            lstVerhuur.ItemsSource = verhuurService.VerhuringenVanHuis(vakantiehuisID);

        }
        private void VerhuurControlsLeegmaken()
        {
            txtKlant.Text = "";
            dtpVerhuurVanaf.SelectedDate = null;
            dtpVerhuurTot.SelectedDate = null;
            lblHuurprijs.Content = "";
            lblVerhuurError.Visibility = Visibility.Hidden;
        }
        private void lstVerhuur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VerhuurControlsLeegmaken();
            if(lstVerhuur.SelectedItem != null)
            {
                Verhuur verhuur = (Verhuur)lstVerhuur.SelectedItem;
                VulVerhuurContent(verhuur);
            }
        }
        private void VulVerhuurContent(Verhuur verhuur)
        {
            Vakantiehuis vakantiehuis = (Vakantiehuis)lstVakantiehuizen.SelectedItem;
            txtKlant.Text = verhuur.Klant;
            dtpVerhuurVanaf.SelectedDate = verhuur.Vanaf;
            dtpVerhuurTot.SelectedDate = verhuur.Tot;
            lblHuurprijs.Content = vakantiehuis.PrijsPerNacht * verhuur.AantalDagen;
            lblVerhuurError.Visibility = Visibility.Hidden;
        }

        private void btnNieuweVerhuur_Click(object sender, RoutedEventArgs e)
        {
            VerhuurControlsLeegmaken();
            isVerhuurNew = true;
            VerhuurBewerkBeeld();
            txtKlant.Focus();
        }

        private void btnWijzigVerhuur_Click(object sender, RoutedEventArgs e)
        {
            if(lstVerhuur.SelectedItem != null)
            {
                isVerhuurNew = false;
                VerhuurBewerkBeeld();
                txtKlant.Focus();
            }
        }

        private void btnVerwijderVerhuur_Click(object sender, RoutedEventArgs e)
        {
            if (lstVerhuur.SelectedItem != null)
            {
                if (MessageBox.Show("Ben je zeker?", "Verhuur wissen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Verhuur verhuur = (Verhuur)lstVerhuur.SelectedItem;
                    verhuurService.VerwijderVerhuring(verhuur);
                    Vakantiehuis vakantiehuis = (Vakantiehuis)lstVakantiehuizen.SelectedItem;
                    VulVerhuurListbox(vakantiehuis.ID);
                }
            }
        }


        private void btnBewaarVerhuur_Click(object sender, RoutedEventArgs e)
        {
            bool fouten = false;
            Vakantiehuis vakantiehuis = (Vakantiehuis)lstVakantiehuizen.SelectedItem;

            lblVerhuurError.Content = "";
            string klant = txtKlant.Text.Trim();
            DateTime vanaf = DateTime.MinValue;
            DateTime tot = DateTime.MinValue;
            if(klant.Length == 0)
            {
                fouten = true;
                lblVerhuurError.Content += "Klantnaam invoeren aub\n";
            }
            if(dtpVerhuurVanaf.SelectedDate == null)
            {
                fouten = true;
                lblVerhuurError.Content += "Startdatum selecteren aub\n";
            }
            if (dtpVerhuurTot.SelectedDate == null)
            {
                fouten = true;
                lblVerhuurError.Content += "Einddatum selecteren aub\n";
            }
            if(dtpVerhuurVanaf.SelectedDate != null && dtpVerhuurTot.SelectedDate != null)
            {
                vanaf = (DateTime)dtpVerhuurVanaf.SelectedDate;
                tot = (DateTime)dtpVerhuurTot.SelectedDate;
                if(tot <= vanaf)
                {
                    fouten = true;
                    lblVerhuurError.Content = "Startdatum moet voor einddatum vallen\n";
                }
            }
            if (isVerhuurNew)
            {
                if (verhuurService.IsErOverlap(vakantiehuis.ID, vanaf, tot))
                {
                    fouten = true;
                    lblVerhuurError.Content = "Overlap in boeking !\n";
                }
            }
            else
            {
                if (verhuurService.IsErOverlap((Verhuur)lstVerhuur.SelectedItem,vakantiehuis.ID, vanaf, tot))
                {
                    fouten = true;
                    lblVerhuurError.Content = "Overlap in boeking !\n";
                }
            }
            if (fouten)
            {
                lblVerhuurError.Visibility = Visibility.Visible;
                return;
            }
            Verhuur verhuur;
            if (isVerhuurNew)
            {                
                verhuur = new Verhuur(vakantiehuis.ID, klant, vanaf, tot);
                verhuurService.VoegVerhuurToe(verhuur);
            }
            else
            {
                verhuur = (Verhuur)lstVerhuur.SelectedItem;
                verhuur.Klant = klant;
                verhuur.Vanaf = vanaf;
                verhuur.Tot = tot;
            }
            VerhuurStandaardBeeld();
            VulVerhuurListbox(vakantiehuis.ID);
            lstVerhuur.SelectedItem = verhuur;
            lstVerhuur_SelectionChanged(null, null);
        }

        private void btnAnnuleerVerhuur_Click(object sender, RoutedEventArgs e)
        {
            VerhuurStandaardBeeld();
            VerhuurControlsLeegmaken();
            lstVerhuur_SelectionChanged(null, null);
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vakantiehuisService.SaveListToJson();
            verhuurService.SaveListToJson();

        }
    }
}
