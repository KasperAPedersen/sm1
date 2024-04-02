using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Xml.Linq;
using System.Threading;
using sm;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Dimensions d = new(Console.WindowWidth, Console.WindowHeight);


CForm form;

CObject screen = new(null, new Point(0, 0), d);
CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), Align.None, [BorderColor.red]);
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), Align.None, [BorderColor.rosybrown]);
CLabel title = new(innerBox, new Point(0, 0), Align.Left, "CRUDapp");
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User", [BorderColor.blue, FontColor.purple]);
CTable table = new(innerBox, new Point(0, 5), new Dimensions(Console.WindowWidth - 25, Console.WindowHeight), Align.Middle, [], ["Fornavn", "Efternavn", "EmailAdr", "Mobil", "Adresse", "Titel", "Edit", "Slet"], []);


int prideTimer = 700;
bool prideMode = false;
Thread tPride = new(pride);

// Main loop
bool keepRunning = true;
while (keepRunning)
{
    CRender.SetPos(new Point(Console.WindowWidth, Console.WindowHeight));
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.C:
            b4.ChangeStyling([BorderColor.blue, FontColor.red]);
            form = new(innerBox, "User Creation", ["fName", "lName", "email", "phone", "street"], [],  [["Mr.", "Mrs.", "Ms."]]);
            if(form.IsFinished) table.Add(form.GetValues());
            b4.ChangeStyling([BorderColor.blue, FontColor.purple]);
            break;
        case ConsoleKey.Enter:
            switch(table.selectIndex)
            {
                case 6:
                    form = new(innerBox, "User Editor", ["fName", "lName", "email", "phone", "street"], table.GetValues(), [["Mr.", "Mrs.", "Ms."]]);
                    if (form.IsFinished) table.Edit(form.GetValues());
                    break;
                case 7:
                    table.Delete();
                    break;
                default:
                    break;
            }
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
        case ConsoleKey.RightArrow:
            table.selectIndex++;
            table.Render();
            break;
        case ConsoleKey.LeftArrow:
            table.selectIndex--;
            table.Render();
            break;
        case ConsoleKey.UpArrow:
            table.contentIndex--;
            table.Render();
            break;
        case ConsoleKey.DownArrow:
            table.contentIndex++;
            table.Render();
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