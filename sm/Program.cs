using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Xml.Linq;
using sm;

Dimensions d = new Dimensions(Console.WindowWidth, Console.WindowHeight);


CObject screen = new(null, new Point(0, 0), d);
CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), Align.None, [BorderColor.red]);
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), Align.None, [BorderColor.rosybrown]);
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User", [BorderColor.blue, FontColor.purple]);
CBox creationBox = new(innerBox, new Point(0, 0), new Dimensions(35, 30), Align.Middle, [BorderColor.lime]);

// User creation form inputs
CLabel title = new(creationBox, new Point(0, 0), Align.Middle, "User Creation");
CInput fName = new(creationBox, new Point(0, 5), new Dimensions(20, 3), "Fornavn", Align.Middle, [BorderColor.yellow3_1]);
CInput lName = new(creationBox, new Point(0, 8), new Dimensions(20, 3), "Efternavn", Align.Middle, [BorderColor.yellow3_1]);
CInput email = new(creationBox, new Point(5, 11), new Dimensions(20, 3), "Email", Align.Middle, [BorderColor.yellow3_1]);
CInput phone = new(creationBox, new Point(5, 14), new Dimensions(20, 3), "Phone", Align.Middle, [BorderColor.yellow3_1]);
CInput street = new(creationBox, new Point(5, 17), new Dimensions(20, 3), "Street", Align.Middle, [BorderColor.yellow3_1]);

bool keepRunning = true;
while (keepRunning)
{
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.C:
            CForm form = new([creationBox, title, fName, lName, email, phone, street]);
            form.Show();
            break;
        default:
            break;
    }
}