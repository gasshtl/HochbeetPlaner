using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hochbeet_Planer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int zellenGroesse = 10; //1Zelle = 10cm

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BeetGenerieren(int breite, int laenge)
        {   //Hochbeet zeichnen Kasterl für Kasterl
            //zuerst Raster leeren falls schon etwas drinnen ist
            grdHochbeet.Children.Clear();
            grdHochbeet.RowDefinitions.Clear();
            grdHochbeet.ColumnDefinitions.Clear();

            int anzahlSpalten = breite / zellenGroesse;
            int anzahlZeilen = laenge / zellenGroesse;

            //Spalten anlegen
            for (int i = 0; i < anzahlSpalten; i++)
            {
                grdHochbeet.ColumnDefinitions.Add
                    (new ColumnDefinition { Width = new GridLength(30) });
            }

            //Zeilen anlegen
            for (int j = 0; j < anzahlZeilen; j++)
            {
                grdHochbeet.RowDefinitions.Add
                    (new RowDefinition { Height = new GridLength(30) });
            }

            //Zellen befüllen
            for (int j = 0;j < anzahlZeilen; j++)
            {
                for(int i = 0; i < anzahlSpalten; i++)
                {
                    Border zelle = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(139, 90, 43)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(101, 67, 33)),
                        Margin = new Thickness(2),
                    };
                    Grid.SetRow(zelle, j);
                    Grid.SetColumn(zelle, i);
                    grdHochbeet.Children.Add(zelle);
                }
            }

        }

        private void btnBeetGenerieren_Click(object sender, RoutedEventArgs e)
        {
            int breite;
            int laenge;

            //wie gewohnt Eingabe mit Tryparse und Fehlermeldung auf wpf
            if(!int.TryParse(txtBreite.Text, out breite) || !int.TryParse(txtLaenge.Text, out laenge))
            {
                MessageBox.Show("Bitte nur ganze Zahlen eingeben!");
                return;
            }
            MessageBox.Show($"Breite: {breite}cm, Länge: {laenge}cm");
            BeetGenerieren(breite, laenge); 
        }
    }
}