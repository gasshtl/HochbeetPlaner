using Hochbeet_Planer.Data;
using Hochbeet_Planer.Models;
using System.Data.SQLite;
using System.Runtime.ConstrainedExecution;
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
        private List<Pflanze> pflanzenListe;
        private Dictionary<string, string> beetBelegung = new Dictionary<string, string>();


        public MainWindow() 
        {
            InitializeComponent();
            PflanzenLaden(); //PflanzenObjekte laden (später vl Datenbank dazu?)
            DatabaseHelper.InitializeDatabase(); //Datenbank laden für Beet speichern und laden
            BeeteAnzeigen();
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
            for (int j = 0; j < anzahlZeilen; j++)
            {
                for (int i = 0; i < anzahlSpalten; i++)
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
        //die ganze Pflanzenliste
        private void PflanzenLaden()
        {
            pflanzenListe = new List<Pflanze>
            {
                new Pflanze {Name = "Paradaiser", BreiteInZellen = 2, LaengeInZellen = 2,
                FarbeR =180, FarbeG=30, FarbeB=30,},
                new Pflanze {Name = "Gurke", BreiteInZellen = 3, LaengeInZellen = 2,
                FarbeR=30, FarbeG=130, FarbeB=30}
            };
        }

        private void btnBeetGenerieren_Click(object sender, RoutedEventArgs e)
        {
            int breite;
            int laenge;

            //wie gewohnt Eingabe mit Tryparse und Fehlermeldung auf wpf
            if (!int.TryParse(txtBreite.Text, out breite) || !int.TryParse(txtLaenge.Text, out laenge))
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

            if (ausgewaehltePflanze == null) return; //wenn nichts ausgewählt ist also null dann chill  

            //später unbedingt noch ausmerzen das die Farben sich überschreiben!
            if (zeile + ausgewaehltePflanze.LaengeInZellen > zellenGrid.GetLength(0))
            {
                MessageBox.Show(ausgewaehltePflanze.Name + " passt hier nicht rein!\n" +
                    "Größe(bxl): " + ausgewaehltePflanze.BreiteInZellen +
                    " x " + ausgewaehltePflanze.LaengeInZellen + " Zellen"); return;//return damit Programm nicht mehr abstürzt
            }
            if (spalte + ausgewaehltePflanze.BreiteInZellen > zellenGrid.GetLength(1))
            {
                MessageBox.Show(ausgewaehltePflanze.Name + " passt hier nicht rein!\n" +
                    "Größe(bxl): " + ausgewaehltePflanze.BreiteInZellen +
                    " x " + ausgewaehltePflanze.LaengeInZellen + " Zellen"); return;
            }

            for (int j = zeile; j < zeile + ausgewaehltePflanze.LaengeInZellen; j++)
            {
                for (int i = spalte; i < spalte + ausgewaehltePflanze.BreiteInZellen; i++)
                { 
                    zellenGrid[j, i].Background = new SolidColorBrush(Color.FromRgb
                                                            (ausgewaehltePflanze.FarbeR,
                                                            ausgewaehltePflanze.FarbeG,
                                                            ausgewaehltePflanze.FarbeB));
                    
                    beetBelegung[j + "_" + i] = ausgewaehltePflanze.Name;
                }
            }
        }

        private void PflanzeAuswaehlen(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton) sender; //"sender as RadioButton"geht nicht weil könnte null sein

            foreach (Pflanze p in pflanzenListe)
            {
                if (p.Name == rb.Tag.ToString()) //Tag von Xaml umwandeln weil kein Text
                {
                    ausgewaehltePflanze = p;
                }
            }
        }

        private void BeeteAnzeigen()
        {
            lstBeete.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Name FROM Beete";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lstBeete.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }

        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            string beetName = txtBeetName.Text;
            if (beetName == "") 
            {
                MessageBox.Show("Gib einen Beetnamen ein!");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open(); //connection zu db

                //Beet speichern
                string sql = @"
                    INSERT INTO Beete (Name, Breite, Laenge)
                    VALUES (@Name, @Breite, @Laenge)"; //@zum dynamischen befüllen später

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", beetName);
                    cmd.Parameters.AddWithValue("@Breite", grdHochbeet.ColumnDefinitions.Count);
                    cmd.Parameters.AddWithValue("@Laenge", grdHochbeet.RowDefinitions.Count);
                    cmd.ExecuteNonQuery();
                }

                //id vom gespeicherten Beet holen long oder int
                int beetId = (int) conn.LastInsertRowId; 

                foreach(KeyValuePair<string, string> eintrag in beetBelegung)
                {
                    string[] teile = eintrag.Key.Split('_');
                    int zeile = int.Parse(teile[0]);
                    int spalte = int.Parse(teile[1]);

                    string sqlBelegung = @"INSERT INTO BeetBelegung (BeetId, Zeile, Spalte, PflanzenName)
                                          VALUES (@BeetId, @Zeile, @Spalte, @PflanzenName)
                                         "; //@zum dynamischen befüllen später
                    using (SQLiteCommand cmdBelegung = new SQLiteCommand(sqlBelegung, conn) )
                    {
                        cmdBelegung.Parameters.AddWithValue("@BeetId", beetId);
                        cmdBelegung.Parameters.AddWithValue("@Zeile", zeile);
                        cmdBelegung.Parameters.AddWithValue("@Spalte", spalte);
                        cmdBelegung.Parameters.AddWithValue("@PflanzenName", eintrag.Value);
                        cmdBelegung.ExecuteNonQuery();

                    }

                }
            }
            MessageBox.Show("Beet gespeichert :)");
            BeeteAnzeigen();
        }

        private void btnLaden_Click(object sender, RoutedEventArgs e)
        {
            if (lstBeete.SelectedItem == null) 
            {
                MessageBox.Show("Bitte ein Beet aus der Liste auswählen!");
                return;
            }

            string beetName = lstBeete.SelectedItem.ToString();

            using (SQLiteConnection conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();

                // Beet laden
                string sql = "SELECT * FROM Beete WHERE Name = @Name";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", beetName);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int breite = (int)(long)reader["Breite"];
                            int laenge = (int)(long)reader["Laenge"];
                            BeetGenerieren(breite * zellenGroesse, laenge * zellenGroesse);
                        }
                    }
                }
                //Belegung laden
                int beetId2 = 0;
                string sqlId = "SELECT Id FROM Beete WHERE Name = @Name";

                using (SQLiteCommand cmdId = new SQLiteCommand(sqlId, conn))
                {
                    cmdId.Parameters.AddWithValue("@Name", beetName);
                    beetId2 = (int)(long)cmdId.ExecuteScalar();
                }

                string sqlBelegung = "SELECT * FROM BeetBelegung WHERE BeetId = @BeetId";

                using (SQLiteCommand cmdBelegung = new SQLiteCommand(sqlBelegung, conn))
                {
                    cmdBelegung.Parameters.AddWithValue("@BeetId", beetId2);
                    using (SQLiteDataReader readerBelegung = cmdBelegung.ExecuteReader())
                    {
                        while (readerBelegung.Read())
                        {
                            int zeile = (int)(long)readerBelegung["Zeile"];
                            int spalte = (int)(long)readerBelegung["Spalte"];
                            string pflanzenName = readerBelegung["PflanzenName"].ToString();

                            Pflanze p = pflanzenListe.Find(x => x.Name == pflanzenName);
                            if (p != null)
                            {
                                zellenGrid[zeile, spalte].Background =
                                    new SolidColorBrush(Color.FromRgb(p.FarbeR, p.FarbeG, p.FarbeB));
                            }
                        }
                    }
                }
            }


        }
       
        
    }
}