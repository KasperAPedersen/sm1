using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Xml.Linq;
using System.Threading;
using sm;

Dimensions d = new Dimensions(Console.WindowWidth, Console.WindowHeight);


CForm form;

CObject screen = new(null, new Point(0, 0), d);
CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), Align.None, [BorderColor.red]);
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), Align.None, [BorderColor.rosybrown]);
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User", [BorderColor.blue, FontColor.purple]);
int prideTimer = 700;

// Pride mode
bool prideMode = false;
Thread tPride = new Thread(pride);

// Main loop
bool keepRunning = true;
while (keepRunning)
{
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.C:
            b4.ChangeStyling([BorderColor.blue, FontColor.red]);
            form = new(innerBox, "User Creation", ["fName", "lName", "email", "phone", "street"]);
            b4.ChangeStyling([BorderColor.blue, FontColor.purple]);
            break;
        case ConsoleKey.E:
            form = new(innerBox, "User Editor", ["email", "phone", "street"]);
            break;
        case ConsoleKey.P:
            if(tPride.ThreadState == ThreadState.Unstarted) tPride.Start();
            prideMode = !prideMode;
            break;
        case ConsoleKey.OemPlus:
            prideTimer += 100;
            break;
        case ConsoleKey.OemMinus:
            prideTimer -= 100;
            break;
        default:
            break;
    }
}

void pride()
{
    BorderColor[] bc = [BorderColor.red, BorderColor.orange1, BorderColor.yellow, BorderColor.green, BorderColor.indianred, BorderColor.violet];
    int colorIndex = 0;
    
    while (Thread.CurrentThread.ThreadState == ThreadState.Running)
    {
        
        while (prideMode)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Black;
            Thread.Sleep(prideTimer);

            if (colorIndex + 1 > bc.Length) colorIndex = 0;
            outerBox.ChangeStyling([bc[colorIndex++]]);
            innerBox.ChangeStyling([bc[colorIndex++]]);
            b4.ChangeStyling([bc[colorIndex++]]);
        }
    }
    
}