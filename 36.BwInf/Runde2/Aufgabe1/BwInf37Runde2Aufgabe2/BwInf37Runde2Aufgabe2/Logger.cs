using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BwInf36Runde2Aufgabe1
{
    public class Logger
    {
        ConsoleWindow consoleWindow;

        public Logger()
        {
            Debug.Write("Test");
            consoleWindow = new ConsoleWindow();
        }

        public void Start()
        {
            consoleWindow.Show();
        }
        public void Print(string s)
        {
            consoleWindow.TextBlockConsole.Text += Environment.NewLine;
            consoleWindow.TextBlockConsole.Text += s;
        }
        public void Stop()
        {
            consoleWindow.Close();
        }
    }
}
