using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;

namespace DZKBX0_Beadando
{
    public partial class MainWindow : Window
    {
        string defState;
        private VeremAutomata v;
        private (DataGrid dataGrid, TextBlock tb1, TextBlock tb2) windowComponents; //mikhez férhet hozzá a veremautomata? - nem egy class, hanem egy tuple
        string placeholderExpression = "i+i*i alakú kifejezés";
        public MainWindow(string defState)
        {
            this.defState = defState;
            InitializeComponent();
            windowComponents = (dgSteps, tBlock1, tBlock2);
            tBoxExpression.Text = placeholderExpression; //"i+i*i alakú kifejezés"
        }
        private void butStart_Click(object sender, RoutedEventArgs e)
        {
            if (tBoxExpression.Text.Equals(placeholderExpression) || tBoxExpression.Text == "") { tBlock1.Text = "Kérem, adjon meg egy kifejezést!"; tBoxExpression.Focus(); return; }

            tBoxExpression.IsEnabled = false;
            butStart.IsEnabled = false;
            v = new VeremAutomata(defState, tBoxExpression.Text);
            try
            {
                v.Check(windowComponents);
            }
            catch(RuleNotFoundException r)
            {
                tBlock1.Text = r.Message;
            }
            catch(InvalidOperationException inop)
            {
                tBlock1.Text = inop.Message;
            }
            catch(IndexOutOfRangeException our)
            {
                tBlock1.Text = our.Message;
            }
            finally
            {
                tBoxExpression.IsEnabled = true;
                butStart.IsEnabled = true;
            }
            
        }

        private void tBoxExpression_GotFocus(object sender, RoutedEventArgs e)
        {
            tBoxExpression.Foreground = Brushes.Black;
            tBoxExpression.Text = "";
        }

        private void tBoxExpression_LostFocus(object sender, RoutedEventArgs e)
        {
            if(tBoxExpression.Text == "")
            {
                tBoxExpression.Text = "i+i*i alakú kifejezés";
                tBoxExpression.Foreground = Brushes.Gray;
            }
        }

        private void tBoxExpression_KeyDown(object sender, KeyEventArgs e) //Enterrel is működik az input!
        {
            if(e.Key.Equals(Key.Enter))
            {
                butStart_Click(sender, e);
            }
        }

        private void butExit_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void butRestart_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            Process.GetCurrentProcess().Kill();
            
        }
    }
}
