using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using sm;

Dimensions d = new Dimensions(Console.WindowWidth, Console.WindowHeight);

CObject screen = new(null, new Point(0, 0), d);
CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight));
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight));
CBox b2 = new(innerBox, new Point(0, 0), new Dimensions(50, 10));
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User");


bool keepRunning = true;
while (keepRunning)
{
    CRender.SetPos(new Point(150, screen.Dim.Height - 5));
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.N:
            break;
        default:
            break;
    }
}
