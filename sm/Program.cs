﻿using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Xml.Linq;
using System.Threading;
using sm;
using System.Text;
using Color = sm.Color;

Console.OutputEncoding = Encoding.UTF8;

Dimensions d = new(Console.WindowWidth, Console.WindowHeight);
CObject screen = new(null, new Point(0, 0), d);

CForm form;

CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.red).Build(), Align.None);
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.rosybrown).Build(), Align.None);
CLabel title = new(innerBox, new Point(0, 0), Align.Left, "CRUDapp", new CStyleBuilder().Build());
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User", new CStyleBuilder().AddBorder(Color.blue).AddFont(Color.purple).Build());
CTable table = new(innerBox, new Point(0, 5), new Dimensions(Console.WindowWidth - 25, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build(), Align.Middle, ["Fornavn", "Efternavn", "EmailAdr", "Mobil", "Adresse", "Titel", "Edit", "Slet"], []);


int prideTimer = 700;
bool prideMode = false;
Thread tPride = new(pride);

bool keepRunning = true;
while (keepRunning)
{
    CRender.SetPos(new Point(Console.WindowWidth, Console.WindowHeight));
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.C:
            b4.ChangeStyling(new CStyleBuilder().AddBorder(Color.blue).AddFont(Color.red).Build());
            form = new(innerBox, "User Creation", ["fName", "lName", "email", "phone", "street"], [], [["Mr.", "Mrs.", "Ms."]]);
            if (form.IsFinished) table.Add(form.GetValues());
            b4.ChangeStyling(new CStyleBuilder().AddBorder(Color.blue).AddFont(Color.purple).Build());
            break;
        case ConsoleKey.Enter:
            if (table.Content.Count <= 0) continue;

            switch (table.selectIndex)
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
            if (tPride.ThreadState == ThreadState.Unstarted) tPride.Start();
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
    Color[] bc = [Color.red, Color.orange1, Color.yellow, Color.green, Color.indianred, Color.violet];
    int colorIndex = 0;

    while (Thread.CurrentThread.ThreadState == ThreadState.Running)
    {
        while (prideMode)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Black;
            Thread.Sleep(prideTimer);

            if (colorIndex + 1 > bc.Length) colorIndex = 0;
            outerBox.ChangeStyling(new CStyleBuilder().AddBorder(bc[colorIndex++]).Build());
            innerBox.ChangeStyling(new CStyleBuilder().AddBorder(bc[colorIndex++]).Build());
            b4.ChangeStyling(new CStyleBuilder().AddBorder(bc[colorIndex++]).Build());
        }
    }
}