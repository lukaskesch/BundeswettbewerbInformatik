using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace BwInf38Runde2Aufgabe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte Task;
        double CalculationTime;

        int Digit, nDigit;
        long GoalNumber;

        int NumberOfNotNaturalTerms, NumberOfDoubleTerms;
        int NeededNumberOfDigits1;

        bool BoolModulo = false;
        bool LogFile = false;
        bool BoolAB = false;
        bool GoalNumber1Reached, GoalNumber2Reached;

        long[] Statistics;
        string StatisticsString;

        List<List<Term>> ListTerms = new List<List<Term>>(20);
        SortedDictionary<long, Term> DictionaryResult = new SortedDictionary<long, Term>();
        Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            TextBoxDigit.Text = 1.ToString();
        }


        private void ButtonCalculateB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BoolModulo = false;
                NumberOfDoubleTerms = 0;
                NumberOfNotNaturalTerms = 0;
                GoalNumber1Reached = false;
                GoalNumber2Reached = false;

                Statistics = new long[30];
                StatisticsString = string.Empty;

                ListTerms = new List<List<Term>>();
                DictionaryResult = new SortedDictionary<long, Term>();

                LogFile = (bool)CheckLogFile.IsChecked;
                BoolModulo = (bool)CheckBoxModulo.IsChecked;
                GoalNumber = long.Parse(TextBoxNumberToCalculate.Text);
                Digit = int.Parse(TextBoxDigit.Text);

                if (Digit < 1 || Digit > 9 || GoalNumber < 0)
                {
                    MessageBox.Show("Die eingegebenen Parameter konnten nicht entgegen genommen werden");
                    return;
                }
                else if (GoalNumber == 0)
                {
                    LabelResult1Time.Content = 0.ToString();
                    LabelResult1NeededTerms.Content = 1.ToString();
                    LabelResult1nDigits.Content = 2.ToString();
                    LabelResult1Term.Content = "(" + Digit.ToString() + "-" + Digit.ToString() + ")";

                    LabelResult2Time.Content = 0.ToString();
                    LabelResult2NeededTerms.Content = 1.ToString();
                    LabelResult2nDigits.Content = 2.ToString();
                    LabelResult2Term.Content = "(" + Digit.ToString() + "-" + Digit.ToString() + ")";
                    return;
                }

                if (sender == null)
                {
                    Statistics[0] = Digit;
                    Statistics[1] = GoalNumber;

                    stopwatch.Restart();
                    Task = 1;
                    CalculateTerm();
                    CalculationTime = stopwatch.ElapsedMilliseconds;
                    Statistics[2] = stopwatch.ElapsedMilliseconds;
                    CalculationTime /= 1000;
                    stopwatch.Stop();

                    if (LogFile)
                    {
                        WriteStatisticsString();
                        SaveStatistics();
                    }

                    LabelResult1Time.Content = CalculationTime.ToString();
                    LabelResult1NeededTerms.Content = DictionaryResult.Count.ToString();
                    LabelResult1NumberOfCheckedTerms.Content = (NumberOfDoubleTerms + NumberOfNotNaturalTerms + DictionaryResult.Count).ToString();
                    LabelResult1NumberOfNotNaruralTerms.Content = NumberOfNotNaturalTerms.ToString();
                    LabelResult1NumberOfDoubleTerms.Content = NumberOfDoubleTerms.ToString();
                    LabelResult1PossibleTerms.Content = GetNumberOfPossibleTerms(nDigit + 1).ToString();
                    LabelResult1nDigits.Content = (nDigit + 1).ToString();
                }

                if (sender != null || BoolAB)
                {
                    stopwatch.Restart();
                    ListTerms = new List<List<Term>>();
                    DictionaryResult = new SortedDictionary<long, Term>();

                    NumberOfNotNaturalTerms = 0;
                    NumberOfDoubleTerms = 0;

                    Statistics = new long[30];
                    StatisticsString = string.Empty;
                    Statistics[0] = Digit;
                    Statistics[1] = GoalNumber;

                    Task = 2;
                    CalculateTerm();
                    CalculationTime = stopwatch.ElapsedMilliseconds;
                    Statistics[2] = stopwatch.ElapsedMilliseconds;
                    CalculationTime /= 1000;
                    stopwatch.Stop();

                    LabelResult2Time.Content = CalculationTime.ToString();
                    LabelResult2NeededTerms.Content = DictionaryResult.Count.ToString();
                    LabelResult2nDigits.Content = (nDigit + 1).ToString();

                    LabelResult2NumberOfCheckedTerms.Content = (NumberOfDoubleTerms + NumberOfNotNaturalTerms + DictionaryResult.Count).ToString();
                    LabelResult2NumberOfNotNaruralTerms.Content = NumberOfNotNaturalTerms.ToString();
                    LabelResult2NumberOfDoubleTerms.Content = NumberOfDoubleTerms.ToString();

                    if (LogFile)
                    {
                        WriteStatisticsString();
                        SaveStatistics();
                    }

                    DictionaryResult = null;
                    ListTerms = null;
                    BoolAB = false;
                }

            }
            catch
            {
                MessageBox.Show("Die eingegebenen Parameter konnten nicht entgegen genommen werden");
                throw;
            }
        }


        private void CalculateTerm()
        {

            //Erstelle den ersten Digit
            Literal FirstLiteral = new Literal(Digit);

            //Lege eine Liste an Index 0 an und speicher in ihr First Literal
            ListTerms.Add(new List<Term>(100000));
            ListTerms[0].Add(FirstLiteral);
            DictionaryResult.Add(FirstLiteral.GetResult(), FirstLiteral);

            //Schaue, ob Literal GoalNumber ist
            if (FirstLiteral.GetResult() == GoalNumber)
            {
                LabelResult1Term.Content = FirstLiteral.GetResult().ToString();
                NeededNumberOfDigits1 = 1;
                return;
            }

            //Erstelle für jede Ziffernlänge (alle) Terme
            for (nDigit = 1; true; nDigit++)
            {
                //Lege Liste für nDigit an
                ListTerms.Add(new List<Term>());

                //Erstelle Literal für nDigit
                long LiteralValue = ListTerms[nDigit - 1][0].GetResult();
                LiteralValue = LiteralValue * 10 + Digit;
                Literal NewLiteral = new Literal(LiteralValue);
                ListTerms[nDigit].Add(NewLiteral);

                //Muss theoretisch noch überprüft werden, ob wert nicht schon erreicht
                if (LiteralValue == GoalNumber)
                {
                    GoalNumber1Reached = true;
                    LabelResult1Term.Content = LiteralValue.ToString();
                }

                if (Task == 2)
                {
                    //Wende Fakultät für nDigit-1 an
                    Term OldTerm;
                    int Lenght = ListTerms[nDigit - 1].Count;

                    for (int i = 0; i < Lenght; i++)
                    {
                        OldTerm = ListTerms[nDigit - 1][i];
                        while (FactorialOperator.IsCalculatable(OldTerm))
                        {
                            Term NewTerm = new FactorialOperator(OldTerm);

                            if (CheckTerm(NewTerm))
                            {
                                ListTerms[nDigit - 1].Add(NewTerm);
                                DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                            }
                            else if (NewTerm.GetResult() == OldTerm.GetResult())
                            {
                                break;
                            }
                            OldTerm = NewTerm;
                        }
                    }
                }
                if (Task == 2 && NeededNumberOfDigits1 - 1 <= nDigit && BoolAB)
                {
                    LabelResult2Term.Content = "Keine kürzere Lösung gefunden";
                    return;
                }

                //Gehe Ziffernlänge bis zur Hälfte der aktuellen hoch
                for (int DigitLenght = 0; DigitLenght < (nDigit + 1) / 2; DigitLenght++)
                {
                    //Für jede dieser Ziffernlänge gehe alle ihre Terme durch
                    int UpperBound = ListTerms[DigitLenght].Count;
                    for (int ElementsOfDigitLength = 0; ElementsOfDigitLength < UpperBound; ElementsOfDigitLength++)
                    {
                        //Für jede dieser Terme verknüpfe sie mit mit allen nDigit-DigitLenght Termen
                        int RemainingDigitDifference = nDigit - DigitLenght - 1;
                        for (int ElementsOfRemainingDigitDifference = 0; ElementsOfRemainingDigitDifference < ListTerms[RemainingDigitDifference].Count; ElementsOfRemainingDigitDifference++)
                        {
                            //Erstelle alle sinnvollen Terme aus den zwei aktuellen Termen
                            CreateTerms(DigitLenght, ElementsOfDigitLength, RemainingDigitDifference, ElementsOfRemainingDigitDifference);

                            //Breche ab, wenn GoalNumber erreicht

                            if (Task == 2 && GoalNumber2Reached)
                            {
                                return;
                            }
                            else if (Task == 1 && GoalNumber1Reached)
                            {
                                NeededNumberOfDigits1 = nDigit + 1;
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void CreateTerms(int IndexA1, int IndexA2, int IndexB1, int IndexB2)
        {
            Term NewTerm;
            Term Term1 = ListTerms[IndexA1][IndexA2];
            Term Term2 = ListTerms[IndexB1][IndexB2];

            //Schaue ob Term2 größer ist als Term1, wenn ja tausche diese
            if (Term1.GetResult() < Term2.GetResult())
            {
                Term1 = ListTerms[IndexB1][IndexB2];
                Term2 = ListTerms[IndexA1][IndexA2];
            }

            //Modulo
            if (BoolModulo)
            {
                NewTerm = new ModuloOperator(Term1, Term2);
                if (CheckTerm(NewTerm))
                {
                    Statistics[nDigit + 2]++;

                    ListTerms[nDigit].Add(NewTerm);
                    DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                }
            }

            //Addition
            if (AddOperator.IsCalculatable(Term1, Term2))
            {
                NewTerm = new AddOperator(Term1, Term2);
                if (CheckTerm(NewTerm))
                {
                    Statistics[nDigit + 2]++;

                    ListTerms[nDigit].Add(NewTerm);
                    DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                }
            }
            else
            {
                NumberOfNotNaturalTerms++;
            }


            //Subtraction
            NewTerm = new SubtractOperator(Term1, Term2);
            if (CheckTerm(NewTerm))
            {
                Statistics[nDigit + 2]++;

                ListTerms[nDigit].Add(NewTerm);
                DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
            }

            //Multiplication
            if (MultiplyOperator.IsCalculatable(Term1, Term2))
            {
                NewTerm = new MultiplyOperator(Term1, Term2);
                if (CheckTerm(NewTerm))
                {
                    Statistics[nDigit + 2]++;

                    ListTerms[nDigit].Add(NewTerm);
                    DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                }
            }
            else
            {
                NumberOfNotNaturalTerms++;
            }


            //Division
            if (DivisionOperator.IsCalculatable(Term1, Term2))
            {
                NewTerm = new DivisionOperator(Term1, Term2);
                if (CheckTerm(NewTerm))
                {
                    Statistics[nDigit + 2]++;

                    ListTerms[nDigit].Add(NewTerm);
                    DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                }
            }
            else
            {
                NumberOfNotNaturalTerms++;
            }


            //Power - Wird nur ausgeführt, wenn GoalNumber1 schon erreicht wurde
            if (Task == 2)
            {
                if (PowerOperator.IsCalculatable(Term1, Term2))
                {
                    NewTerm = new PowerOperator(Term1, Term2);
                    if (CheckTerm(NewTerm))
                    {
                        Statistics[nDigit + 2]++;

                        ListTerms[nDigit].Add(NewTerm);
                        DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                    }
                }
                else
                {
                    NumberOfNotNaturalTerms++;
                }

                if (PowerOperator.IsCalculatable(Term2, Term1))
                {
                    NewTerm = new PowerOperator(Term2, Term1);
                    if (CheckTerm(NewTerm))
                    {
                        Statistics[nDigit + 2]++;

                        ListTerms[nDigit].Add(NewTerm);
                        DictionaryResult.Add(NewTerm.GetResult(), NewTerm);
                    }
                }
                else
                {
                    NumberOfNotNaturalTerms++;
                }

            }
        }
        private bool CheckTerm(Term NewTerm)
        {
            long TermResult = NewTerm.GetResult();

            //Überprüfe, ob Ergebnis schon existiert
            if (DictionaryResult.ContainsKey(TermResult))
            {
                NumberOfDoubleTerms++;
                return false;
            }
            else if (TermResult <= 0)
            {
                NumberOfNotNaturalTerms++;
                return false;
            }
            else if (Task == 2 && TermResult == GoalNumber)
            {
                //GoalNumber2 wurde getroffen
                GoalNumber2Reached = true;
                LabelResult2Term.Content = NewTerm.PrintTerm();
                //TB2.Text = NewTerm.PrintTerm();

                return false;
            }
            else if (Task == 1 && TermResult == GoalNumber)
            {
                //GoalNumber1 wurde getroffen
                GoalNumber1Reached = true;
                LabelResult1Term.Content = NewTerm.PrintTerm();
                //TB1.Text = NewTerm.PrintTerm();

                return false;
            }
            return true;
        }



        private long GetNumberOfPossibleTerms(int nDigit)
        {
            switch (nDigit)
            {
                case 1:
                    return 1;
                case 2:
                    return 9;
                case 3:
                    return 73;
                case 4:
                    return 1233;
                case 5:
                    return 15121;
                case 6:
                    return 252377;
                case 7:
                    return 3827801;
                case 8:
                    return 69786529;
                case 9:
                    return 1130435617;
                case 10:
                    return 20622154665;
                case 11:
                    return 355410904681;
                case 12:
                    return 6203366677218;
                case 13:
                    return 114580802109113;
                case 14:
                    return 2169128857899390;
                default:
                    return -1;
            }
        }

        private void ButtonCalculateAB_Click(object sender, RoutedEventArgs e)
        {
            BoolAB = true;
            ButtonCalculateB_Click(null, e);
        }

        private void ButtonCalculateA_Click(object sender, RoutedEventArgs e)
        {
            ButtonCalculateB_Click(null, e);
        }
        private void DigitPlus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Digit = int.Parse(TextBoxDigit.Text);
                if (Digit == 9)
                {
                    return;
                }
                TextBoxDigit.Text = (++Digit).ToString();

            }
            catch
            {
                TextBoxDigit.Text = 1.ToString();
                return;
            }
        }
        private void DigitMinus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Digit = int.Parse(TextBoxDigit.Text);
                if (Digit == 1)
                {
                    return;
                }
                TextBoxDigit.Text = (--Digit).ToString();

            }
            catch
            {
                TextBoxDigit.Text = 1.ToString();
                return;
            }
        }
        private void SaveStatistics()
        {
            string FileString = StatisticsString;
            //string FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Statistics.txt";
            //StreamWriter Writer = new StreamWriter(FileString, true, Encoding.Unicode);
            StreamWriter WriterStatistics = File.AppendText("Statistics.csv");
            try
            {
                WriterStatistics.WriteLine(FileString);
            }
            finally
            {
                WriterStatistics.Close();
            }

        }
        private void WriteStatisticsString()
        {
            StatisticsString += Statistics[0].ToString() + ",";
            StatisticsString += Statistics[1].ToString() + ",";
            StatisticsString += (nDigit + 1).ToString() + ",";
            StatisticsString += Statistics[2].ToString() + ",";
            StatisticsString += DictionaryResult.Count + ",";
            StatisticsString += 1.ToString();
            if (Statistics[0] == Statistics[1])
            {

                return;
            }
            else
            {
                StatisticsString += ",";
                if (Statistics[3] == 0)
                {
                    Statistics[3]++;
                }
            }

            for (int i = 3; i < Statistics.Length - 1; i++)
            {

                if (Statistics[i + 1] != 0)
                {
                    StatisticsString += Statistics[i].ToString() + ",";
                }
                else if (Statistics[i] != 0)
                {
                    StatisticsString += Statistics[i].ToString();

                }

            }
        }
    }
}
