﻿using System;
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
        }

        public void Start()
        {
            consoleWindow = new ConsoleWindow();
            consoleWindow.Show();
            consoleWindow.Topmost = true;
        }
        public void Print(string s)
        {
            consoleWindow.TextBlockConsole.Text += DateTime.Now.ToString("HH:mm:ss") + ": ";
            consoleWindow.TextBlockConsole.Text += s;
            consoleWindow.TextBlockConsole.Text += Environment.NewLine;
            consoleWindow.ScollViewerConsole.ScrollToEnd();
        }
        public void Stop()
        {
            consoleWindow.Close();
        }
    }
}
