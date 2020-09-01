using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BwInfAufgabe2
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
        int BestNumberOfZeros;
        private void ButtonRechnen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BestNumberOfZeros = int.MaxValue;
                TextboxAusgabe.Text = string.Empty;
                string Input = TextboxEingabe.Text;
                int[] DigitsLeft = new int[Input.Length * 2];
                int[] DigitsChosen = new int[Input.Length * 2];
                DeclareArrays(DigitsLeft, DigitsChosen, Input);

                //Starte die Rekursive Methode
                Recursion(DigitsLeft, DigitsChosen, 0);

                if (BestNumberOfZeros < int.MaxValue)
                {
                    MessageBox.Show(BestNumberOfZeros.ToString() + " Zahlenbloecke beginnen mit einer Null");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Die Eingabeparamter konnten nicht entgegengenommen werden");
                //throw;
            }

        }
        private void DeclareArrays(int[] DigitsLeft, int[] DigitsChoose, string Input)
        {
            for (int i = 0; i < Input.Length; i++)
            {
                DigitsLeft[i] = int.Parse(Input[i].ToString());
            }

            for (int i = Input.Length; i < DigitsLeft.Length; i++)
            {
                DigitsLeft[i] = -2;
            }

            for (int i = 0; i < DigitsChoose.Length; i++)
            {
                DigitsChoose[i] = -2;
            }
        }
        private void Recursion(int[] DigitsLeft, int[] DigitsChosen, int NumberOfSteps)
        {
            //Wenn keine Zahlen mehr übrig sind
            if (DigitsLeft[0] == -2)
            {
                CheckResult(DigitsChosen, true);
                return;
            }
            //Wenn Schritt zu weit geht
            else if (GetFirstFreeIndex(DigitsLeft) < NumberOfSteps)
            {
                return;
            }
            else
            {
                //Choose - Gehe um Schritt nach vorne
                GeheSchritte(ref DigitsLeft, ref DigitsChosen, NumberOfSteps);

                //Check - Schaue ob Zwischenergebnis ein möglicher Pfad ist
                if (CheckResult(DigitsChosen, false))
                {
                    Recursion(DigitsLeft, DigitsChosen, 4);
                    Recursion(DigitsLeft, DigitsChosen, 3);
                    Recursion(DigitsLeft, DigitsChosen, 2);
                }

                //Un-choose - Gehe diesen Schritt wieder zurück
                GeheSchritteZurueck(ref DigitsLeft, ref DigitsChosen, NumberOfSteps);
            }
        }

        private bool CheckResult(int[] DigitsChosen, bool Finished)
        {
            if (GetNumberOfZeros(DigitsChosen) > BestNumberOfZeros)
            {
                return false;
            }
            if (Finished)
            {
                int NumberOfZeros = GetNumberOfZeros(DigitsChosen);
                if (NumberOfZeros < BestNumberOfZeros)
                {
                    BestNumberOfZeros = NumberOfZeros;
                    SetAusgabe(DigitsChosen);
                }
            }
            return true;
        }
        private int GetFirstFreeIndex(int[] Array)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                if (Array[i] == -2)
                {
                    return i;
                }
            }
            return -1;
        }
        private int GetNumberOfZeros(int[] DigitsChosen)
        {
            int NumberOfZeros = 0;
            if (DigitsChosen[0] == 0)
            {
                NumberOfZeros++;

            }
            for (int i = 0; i < DigitsChosen.Length - 1; i++)
            {
                if (DigitsChosen[i] == -1 && DigitsChosen[i + 1] == 0)
                {
                    NumberOfZeros++;
                }
            }
            return NumberOfZeros;
        }
        private void SetAusgabe(int[] DigitsChosen)
        {
            string Ausgabe = string.Empty;
            for (int i = 0; i < DigitsChosen.Length; i++)
            {
                if (DigitsChosen[i] == -1 && DigitsChosen[i + 1] == -2)
                {
                    break;
                }
                else if (DigitsChosen[i] == -1)
                {
                    Ausgabe += "-";
                    continue;
                }
                Ausgabe += DigitsChosen[i];
            }

            Ausgabe += "\n";
            TextboxAusgabe.Text = Ausgabe;
        }
        private void GeheSchritte(ref int[] DigitsLeft, ref int[] DigitsChosen, int NumberOfSteps)
        {
            //Ermittle erste freien Index von DigitsChosen
            int Index = GetFirstFreeIndex(DigitsChosen);

            //Übertrage die Zahlen von "Zahlen" nach "Chosen"
            if (NumberOfSteps == 0)
            {
                return;
            }
            if (NumberOfSteps >= 2)
            {
                DigitsChosen[Index] = DigitsLeft[0];
                DigitsChosen[Index + 1] = DigitsLeft[1];
            }
            if (NumberOfSteps >= 3)
            {
                DigitsChosen[Index + 2] = DigitsLeft[2];
            }
            if (NumberOfSteps == 4)
            {
                DigitsChosen[Index + 3] = DigitsLeft[3];
            }

            //Setze Leerzeichen
            DigitsChosen[Index + NumberOfSteps] = -1;

            //Ziehe Zahl um Schritte nach vorne und loesche das Ende
            for (int i = 0; i < DigitsLeft.Length - NumberOfSteps; i++)
            {
                DigitsLeft[i] = DigitsLeft[i + NumberOfSteps];
                DigitsLeft[i + NumberOfSteps] = -2;
            }
        }
        private void GeheSchritteZurueck(ref int[] DigitsLeft, ref int[] DigitsChosen, int NumberOfSteps)
        {
            //Zahlenarray um Schritte nach hinten verschieben
            for (int i = DigitsLeft.Length - 1; i >= NumberOfSteps; i--)
            {
                DigitsLeft[i] = DigitsLeft[i - NumberOfSteps];
            }

            //Startbedinung
            if (NumberOfSteps == 0) { return; }

            //Ermittle Index, an dem Chosen endet 
            int Index = GetFirstFreeIndex(DigitsChosen) - 1;

            //Loesche -1 Trenner
            DigitsChosen[Index] = -2;

            //Ende von Chosen in Zahlenarray einfügen
            if (NumberOfSteps == 4)
            {
                DigitsLeft[3] = DigitsChosen[Index - 1];
                DigitsLeft[2] = DigitsChosen[Index - 2];
                DigitsLeft[1] = DigitsChosen[Index - 3];
                DigitsLeft[0] = DigitsChosen[Index - 4];

                DigitsChosen[Index - 4] = -2;
                DigitsChosen[Index - 3] = -2;
            }
            else if (NumberOfSteps == 3)
            {
                DigitsLeft[2] = DigitsChosen[Index - 1];
                DigitsLeft[1] = DigitsChosen[Index - 2];
                DigitsLeft[0] = DigitsChosen[Index - 3];

                DigitsChosen[Index - 3] = -2;
            }
            else if (NumberOfSteps == 2)
            {
                DigitsLeft[1] = DigitsChosen[Index - 1];
                DigitsLeft[0] = DigitsChosen[Index - 2];

            }
            DigitsChosen[Index - 1] = -2;
            DigitsChosen[Index - 2] = -2;
        }
        private bool CheckIfResultAlreadyExist(string Ausgabe)
        {
            string TextbosText = TextboxAusgabe.Text;
            string[] Text = Regex.Split(TextbosText, "\n");

            try
            {
                int LastIndex = Text.Length - 2;

                //MessageBox.Show(Ausgabe + "\n" + Text[LastIndex]);


                if (Text[LastIndex] == Ausgabe)
                {
                    return true;
                }
            }
            catch (Exception) { }

            return false;
        }
        private void GebeArrayAus(int[] Array1, int[] Array2, int NumberSteps)
        {
            string Ausgabe = "Schritte: " + NumberSteps + "\n";

            for (int i = 0; i < Array1.Length; i++)
            {
                Ausgabe += Array1[i].ToString() + "     " + Array2[i].ToString() + "\n";
            }

            MessageBox.Show(Ausgabe);
        }

        private void ButtonBeispiel1_Click(object sender, RoutedEventArgs e)
        {
            TextboxEingabe.Text = "005480000005179734";
            TextboxAusgabe.Text = string.Empty;
        }

        private void ButtonBeispiel2_Click(object sender, RoutedEventArgs e)
        {
            TextboxEingabe.Text = "03495929533790154412660";
            TextboxAusgabe.Text = string.Empty;
        }

        private void ButtonBeispiel3_Click(object sender, RoutedEventArgs e)
        {
            TextboxEingabe.Text = "5319974879022725607620179";
            TextboxAusgabe.Text = string.Empty;
        }

        private void ButtonBeispiel4_Click(object sender, RoutedEventArgs e)
        {
            TextboxEingabe.Text = "9088761051699482789038331267";
            TextboxAusgabe.Text = string.Empty;
        }

        private void ButtonBeispiel5_Click(object sender, RoutedEventArgs e)
        {
            TextboxEingabe.Text = "011000000011000100111111101011";
            TextboxAusgabe.Text = string.Empty;
        }
    }
}
