using Hochbeet_Planer.Models;
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
        private Border[,] zellenGrid;
        private Pflanze ausgewaehltePflanze;
        

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

            //Border Array anlegen - brauche border kein int oder so!!
            zellenGrid = new Border[anzahlZeilen, anzahlSpalten];

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
                        Cursor = Cursors.Hand //hand zeiger wie startseite
                    };
                    Grid.SetRow(zelle, j);
                    Grid.SetColumn(zelle, i);
                    grdHochbeet.Children.Add(zelle);
                    zellenGrid[j, i] = zelle;
                    zelle.MouseLeftButtonDown += Zelle_Click;
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
            BeetGenerieren(breite, laenge); 
        }

        private void Zelle_Click(object sender, MouseButtonEventArgs e)
        {
            
            Border zelle = (Border)sender; //der sender(die angeklickte Zelle) ist ein Border

            int zeile = Grid.GetRow(zelle); //hineinschreiben und ablesen ins array
            int spalte = Grid.GetColumn(zelle);

            if (zeile + ausgewaehltePflanze.LaengeInZellen > zellenGrid.GetLength(0)) return;//damit Programm nicht mehr abstürzt
            if (spalte + ausgewaehltePflanze.BreiteInZellen > zellenGrid.GetLength(1)) return;

            if (ausgewaehltePflanze == null ) return; //wenn nichts ausgewählt ist also null dann chill  

            if (rbParadaiser.IsChecked == true) //ob Paradaeiser angehakerlt ist - ausgewählte Pflanze
            {
                for (int j = zeile; j < zeile + ausgewaehltePflanze.LaengeInZellen; j++)
                {
                    for (int i = spalte; i < spalte + ausgewaehltePflanze.BreiteInZellen; i++)
                    {
                        zellenGrid[j, i].Background =
                            new SolidColorBrush(Color.FromRgb(180, 30, 30));
                    }
                }
            }
        }

        private void rbParadaiser_Checked(object sender, RoutedEventArgs e)
        {
            ausgewaehltePflanze = new Pflanze
            {
                Name = "Paradaiser",
                BreiteInZellen = 2,
                LaengeInZellen = 2,
                FarbeR = 180,
                FarbeG = 30,
                FarbeB = 30
            };
        }
    }
}