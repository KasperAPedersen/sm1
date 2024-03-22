using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using sm;


CObject screen = new(null, new Position(new Point(0, 0), new Point(0,0)), new Dimensions(Console.WindowWidth, Console.WindowHeight));

CBox outerBox = new(screen);
CBox innerBox = new(outerBox);
CBox b = new(innerBox, Align.Middle, new Dimensions(20, 3));
CLabel l = new(innerBox, "Blah", Align.Middle);

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