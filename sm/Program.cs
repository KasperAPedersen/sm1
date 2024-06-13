using System.Drawing;
using sm;
using System.Text;
using Color = sm.Color;

Console.OutputEncoding = Encoding.UTF8;

Dimensions d = new(Console.WindowWidth, Console.WindowHeight);
CObject screen = new(null, new Point(0, 0), d);

CForm form;

CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.red).Build(), Align.None);
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.rosybrown).Build(), Align.None);
_ = new CLabel(innerBox, new Point(0, 0), Align.Left, "CRUDapp", new CStyleBuilder().Build());
CButton b4 = new(innerBox, new Point(0, 0), new Dimensions(0, 0), Align.Right, "Create User", new CStyleBuilder().AddBorder(Color.blue).AddFont(Color.purple).Build());
CTable table = new(innerBox, new Point(0, 5), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build(), Align.Middle, ["Fornavn", "Efternavn", "Adresse", "By", "Postnr", "Udd.", "Udd. Slut", "Job", "Job Start", "Job Slut", "Edit", "Slet"], []);

bool keepRunning = true;
while (keepRunning)
{
    CRender.SetPos(new Point(Console.WindowWidth, Console.WindowHeight));
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.C:
            b4.ChangeStyling(new CStyleBuilder().AddBorder(Color.blue).AddFont(Color.red).Build());
            form = new(innerBox, "User Creation", ["First", "Last", "Adresse", "Udd. Slut (YYYY-MM-DD)", "Job Start (YYYY-MM-DD)", "Job Slut (YYYY-MM-DD)"], [], [CDatabase.GetPostalCodes(), CDatabase.GetSchools(), CDatabase.GetJobs()]);
            if (form.IsFinished) table.Add(form.GetValues());
            
            b4.ChangeStyling(new CStyleBuilder().AddBorder(Color.blue).AddFont(Color.purple).Build());
            break;
        case ConsoleKey.Enter:
            if (table.Content.Count <= 0) continue;

            switch (table.SelectIndex)
            {
                case 10:
                    form = new(innerBox, "User Editor", ["First", "Last", "Adresse", "Udd. Slut (YYYY-MM-DD)", "Job Start (YYYY-MM-DD)", "Job Slut (YYYY-MM-DD)"], [], [CDatabase.GetPostalCodes(), CDatabase.GetSchools(), CDatabase.GetJobs()]);
                    if (form.IsFinished) table.Edit(form.GetValues());
                    break;
                case 11:
                    table.Delete();
                    break;
                default:
                    break;
            }
            break;
        case ConsoleKey.RightArrow:
            table.SelectIndex++;
            table.UpdateSelectIndex();
            break;
        case ConsoleKey.LeftArrow:
            table.SelectIndex--;
            table.UpdateSelectIndex();
            break;
        case ConsoleKey.UpArrow:
            table.UpdateActiveContentRow(table.ContentIndex - 1);
            break;
        case ConsoleKey.DownArrow:
            table.UpdateActiveContentRow(table.ContentIndex + 1);
            break;
        default:
            break;
    }
}