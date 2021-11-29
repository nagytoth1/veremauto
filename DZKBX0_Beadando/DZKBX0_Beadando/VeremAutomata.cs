using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DZKBX0_Beadando
{
    internal class VeremAutomata //táblázatelemzős módszer
    {
        private string input; //elemzendő kifejezés
        private Stack<char> stateStack; //elemzésre használt stack (nemterminális (helyettesítő) és terminális jelekkel)
        private List<byte> usedRules; //elemzésben felhasznált szabályok gyűjtése
        private static List<Rule> rules;

        public string Input 
        { 
            get { return input; }
            set
            {
                this.input = value[value.Length - 1] == '#' ? //van a végén '#' ?
                    value : //ha van, akkor nincs dolgunk
                    string.Concat(value, '#'); //ha nincs, akkor végére '#'
            }
        }
        public Stack<char> StateStack { get { return new Stack<char>(stateStack); } }
        public void InitializeStateStack(string defaultState) 
        {
            if (defaultState == string.Empty) { throw new DefaultStateEmptyException("Hibás fordító: üres Stack-kel nem indulhat a veremautomata"); }
            stateStack = new Stack<char>();
            foreach (char c in defaultState.Reverse())
                stateStack.Push(c);
        }
        public List<byte> UsedRules { get => usedRules; set => usedRules = value; }
        internal static List<Rule> Rules { get => rules; set => rules = value; }

        public VeremAutomata(string defState, string input)
        {
            stateStack = new Stack<char>();
            usedRules = new List<byte>();
            
            //beolvasástól függ a példányosítás --> először a fájlt kell kiválasztani - külön Window
            Input = input; //setter megnézi, hogy van-e a végén #-karakter? ha nincs, tegyen
            try { InitializeStateStack(defState); } //stack-hez hozzádobja a kezdőállapot elemeit (fájlban tároljuk) új stack --> push: "E#"
            catch (DefaultStateEmptyException) { throw; } //ha üres a StateStack, akkor nem tud elemezni --> ki kell lökni
        }
        public static void FileHandler(string path, out string defaultState)
        {
            string[] lineSplit;
            rules = new List<Rule>(); //üres listát inicializál
            StreamReader olvas = null;

            try
            {
                olvas = new StreamReader(path);
                defaultState = olvas.ReadLine().Split(';')[1]; //első sor második tagja
                while (!olvas.EndOfStream)
                {
                    lineSplit = olvas.ReadLine().Split(';');
                    //grammatika.csv egy sora: Ei;TV;1 --> elhelyezzük a szabályokat egy listában
                    rules.Add(new Rule(lineSplit[0], lineSplit[1], byte.Parse(lineSplit[2])));
                }
            }
            catch(IOException)
            {
                throw; //továbbdobja az őt hívó metódusnak
            }
            catch(IndexOutOfRangeException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if(olvas != null) olvas.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="components"></param>
        /// <exception cref="RuleNotFoundException">Ezt a hibát akkor dobja, amikor nem talál az aktuális bemenet-állapothoz visszatérési szabályt.</exception>
        /// <exception cref="InvalidOperationException">Ezt a hibát akkor dobja, amikor az állapotveremből nem tud több értéket kivenni (üres a verem)</exception>
        /// <exception cref="IndexOutOfRangeException">Ezt a hibát akkor dobja, amikor az inputról nem tud már több értéket venni.</exception>
        public async void Check((DataGrid dataGrid, TextBlock tb1, TextBlock tb2) components) //az elemzés során hozzáférést kap a képhez
        {
            components.tb1.Text = string.Empty; components.tb2.Text = string.Empty;
            
            DataTable stepData = new DataTable(); //adattáblában fogom eltárolni az egyes lépések leírásait
            stepData.Columns.Add(new DataColumn("input", typeof(string))); //éppen hol járunk az inputszalagon
            stepData.Columns.Add(new DataColumn("statestack", typeof(string))); //éppen hol tart a stack
            stepData.Columns.Add(new DataColumn("usedrules", typeof(string)));  //éppen melyik szabályra került a vezérlés
            
            DataRow row;
            //dataGrid.ItemsSource = stepData;
            components.dataGrid.DataContext = stepData;
            byte i = 0; //"író-olvasófej helye" inputszalagon, a szöveg indexelése
            char c_input; //inputról olvasott érték
            char c_state; //állapotstack érték
            Rule foundRule;
            
            //TODO: sleep-et betenni úgy, hogy ne fagyjon meg a GUI, hanem szépen egymás után írja ki a sorokat
            do
            {
                row = stepData.NewRow(); // új sort (tiszta rekordot) hoz létre az adattáblában definiált mezőkkel (típusok, mezők száma egyezzen)

                
                row[0] = input.Substring(i, input.Length - i);
                row[1] = ListToString(stateStack.ToList());
                row[2] = ListToString(usedRules);
                await Task.Delay(700);
                
                stepData.Rows.Add(row); // beemeljük az adattáblába a kész, beállított rekordot
                try
                {
                    c_input = input[i];
                    c_state = stateStack.Pop(); //tetejéről kiszedi az 'E'-t, pl.
                }
                catch (InvalidOperationException) { throw; }
                catch (IndexOutOfRangeException) { throw; }
                if (c_input == c_state)
                {
                    components.tb2.Text += $"POP {c_state}\n";
                    i++;
                    continue;
                }

                //ha nem találna szabályt, akkor vége, nem helyes
                
                if ((foundRule = rules.FirstOrDefault(r => r.InputState == string.Concat(c_state, c_input))) == null)
                {
                    throw new RuleNotFoundException($"Nem találok szabályt a következő kombinációra: {string.Concat(c_state, c_input)}");
                }
                //akár e-betűs, akár nem, hozzá kell adnia
                usedRules.Add(foundRule.RuleID);

                if (foundRule.ReturnState == "e") //üres szabály, nem kell csinálnunk semmit
                    continue; //mehetünk a kövi lefutásra, nem csinálunk semmit, mert már a pop megtörtént (ezért is üres szabály)

                //ha nem e-betűs szabály (vezérlés ide jut), akkor visszatérő szabály karaktereit fordítva pusholjuk a Stack-re
                foreach (char c in foundRule.ReturnState.Reverse()) //pl. "+TV"-t úgy kell, hogy a '+' legyen a tetején
                    stateStack.Push(c);
            } while (stateStack.Count != 0); //amíg verem nem üres
            components.tb1.Foreground = Brushes.LightGreen;
            components.tb1.Text = "A megadott kifejezés helyes!";
        }
        public string ListToString<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            list.ForEach(x => sb.Append(x));
            return sb.ToString();
        }
    }
}
