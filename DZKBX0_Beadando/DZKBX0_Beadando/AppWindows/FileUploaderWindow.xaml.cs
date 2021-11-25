using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DZKBX0_Beadando
{
    /// <summary>
    /// Interaction logic for FileUploaderWindow.xaml
    /// </summary>
    public partial class FileUploaderWindow : Window
    {
        string defState;
        public FileUploaderWindow()
        {
            InitializeComponent();
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Excel file (*.csv)|*.csv",
                Title = "Please, select the desired grammatic file (.csv)"
            };
            dlg.ShowDialog();

            //kiválasztott fájl elérési útvonalát a jelenlegi working directory-ig (pwd) levágjuk (minket csak a .csv neve érdekel)
            string relativePath = dlg.FileName.Remove(0, Environment.CurrentDirectory.Length + 1);

            //TODO: ItemSource-ot DataBinding-gal beállítani
            try
            {
                VeremAutomata.FileHandler(relativePath, out defState); //fájl beolvasása + szabálylista feltöltése

                dgRules.ItemsSource = VeremAutomata.Rules;
                tBlockResponse.Foreground = Brushes.ForestGreen;
                tBlockResponse.Text = "Sikeres fájlbeolvasás!";
                btnNext.IsEnabled = true;
            }
            catch(Exception ex)
            {
                tBlockResponse.Foreground = Brushes.DarkRed;
                tBlockResponse.Text = ex.Message;
            }
            
        }
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(defState).Show();
            this.Close();   
        }
    }
}
