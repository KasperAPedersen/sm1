using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using sm;

Dimensions d = new Dimensions(Console.WindowWidth, Console.WindowHeight);

CObject screen = new(null, new Point(0, 0), d);
CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight));
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight));
CBox b2 = new(innerBox, new Point(0, 0), new Dimensions(30, 30));
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User");

CLabel l = new(b2, new Point(0, 0), Align.Middle, "Inputs");
CInput i = new CInput(b2, new Point(0, 5), new Dimensions(20, 3), "Fornavn");
_ = new CInput(b2, new Point(0, 8), new Dimensions(20, 3), "Efternavn");
_ = new CInput(b2, new Point(0, 11), new Dimensions(20, 3));

Console.WriteLine(i.Text);


CController.Idle();

bool keepRunning = true;
while (keepRunning)
{
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.N:
            CController.Run(CController.controllerIndex);
            break;
        case ConsoleKey.F:
            foreach (string s in CController.GetValues()) CRender.Write(new Point(50, 25), s);
            break;
        default:
            break;
    }
}