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
    public partial class MainWindow : Window
    {
        string defState;
        private VeremAutomata v;
        public MainWindow(string defState)
        {
            this.defState = defState;
            InitializeComponent();
            this.tBlock1.Text = defState;
        }
        private void butStart_Click(object sender, RoutedEventArgs e)
        {
            //TODO: mi van, ha üres?
            if(tBoxExpression.Text != "")
            {
                v = new VeremAutomata(defState, tBoxExpression.Text);
                v.Check(dgSteps);
            }
        }

        private void tBoxExpression_GotFocus(object sender, RoutedEventArgs e)
        {
            tBoxExpression.Foreground = Brushes.Black;
            tBoxExpression.Text = "";
        }
    }
}
