using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hochbeet_Planer.Views
{
    /// <summary>
    /// Interaktionslogik für StartFenster.xaml
    /// </summary>
    public partial class StartFenster : Window
    {
        public StartFenster()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow hauptfenster = new MainWindow();
            hauptfenster.Show(); //startfenster zeigt sich
            this.Close(); //startfenster schließt sich wieder
        }
    }
}
