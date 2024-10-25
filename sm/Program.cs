﻿using System.Drawing;
using sm;
using System.Text;
using Color = sm.Color;

Console.OutputEncoding = Encoding.UTF8;

Dimensions d = new(Console.WindowWidth, Console.WindowHeight);
CObject screen = new(null, new Point(0, 0), d);

CForm form;

CBox outerBox = new(screen, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.white).Build());
CBox innerBox = new(outerBox, new Point(0, 0), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.white).Build());
_ = new CLabel(innerBox, new Point(0, 0), Align.Left, "CRUDapp", new CStyleBuilder().Build());
CButton btnAddUser = new(innerBox, new Point(0, 2), new Dimensions(20, 0), Align.None, "Create User", new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
CButton btnAddPostal = new(innerBox, new Point(22, 2), new Dimensions(20, 0), Align.None, "Add Postal", new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build());
CButton btnAddJob = new(innerBox, new Point(42, 2), new Dimensions(20, 0), Align.None, "Add Job", new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build());
CButton btnAddEducation = new(innerBox, new Point(62, 2), new Dimensions(20, 0), Align.None, "Add Education", new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build());
CButton btnSettings = new(innerBox, new Point(62, 2), new Dimensions(20, 0), Align.Right, "Settings", new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build());
CTable table = new(innerBox, new Point(0, 5), new Dimensions(Console.WindowWidth, Console.WindowHeight), new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build(), Align.Middle, ["Fornavn", "Efternavn", "Adresse", "By", "Postnr", "Udd.", "Udd. Slut", "Job", "Job Start", "Job Slut", "Edit", "Slet"], []);

List<CButton> btns = [btnAddUser, btnAddPostal, btnAddJob, btnAddEducation, btnSettings];
int btnIndex = 0;

while (true)
{
    Console.SetCursorPosition(0, Console.WindowHeight - 1);
    switch (Console.ReadKey().Key)
    {
        case ConsoleKey.Enter:
            if(table.IsFocused)
            {
                if (table.Content.Count <= 0) continue;
                switch (table.SelectIndex)
                {
                    case 10: // Edit
                        List<string> values = table.Content[table.ContentIndex];
                        List<string> postalCodes = await CDatabase.GetPostalCodes();
                        List<string> schools = await CDatabase.GetSchools();
                        List<string> jobs = await CDatabase.GetJobs();
                        
                        int postalIndex = postalCodes.FindIndex(postal => $"{values[4]} {values[3]}" == postal);
                        int schoolIndex = schools.FindIndex(school => values[5] == school);
                        int jobIndex = jobs.FindIndex(job => values[7] == job);

                        form = new(innerBox, "User Editor", 
                            ["Fornavn", "Efternavn", "Adresse", "Postnr", "Udd.", "Udd. Slut (DD/MM/YYYY)", "Job", "Job Start (DD/MM/YYYY)", "Job Slut (DD/MM/YYYY)"], 
                            [typeof(CInput), typeof(CInput), typeof(CInput), typeof(CComboBox), typeof(CComboBox), typeof(CInput), typeof(CComboBox), typeof(CInput), typeof(CInput)], 
                            [values[0], values[1], values[2], values[6], values[8], values[9]], [postalCodes, schools, jobs],
                            [postalIndex, schoolIndex, jobIndex]);

                        if (form.IsFinished) table.Edit(form.GetValues());
                        break;
                    case 11: // Delete
                        table.Delete();
                        break;
                }
            } 
            else
            {
                switch (btnIndex)
                {
                    case 0: // Create user
                        List<string> postalCodes = await CDatabase.GetPostalCodes();
                        List<string> schools = await CDatabase.GetSchools();
                        List<string> jobs = await CDatabase.GetJobs();
                        
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorders([CRender.ActiveColor, Styling.Blink]).AddFonts([CRender.ActiveColor, Styling.Blink]).Build());
                        form = new(innerBox, "User Creation",
                            ["Fornavn", "Efternavn", "Adresse", "Postnr", "Udd.", "Udd. Slut (DD/MM/YYYY)", "Job", "Job Start (DD/MM/YYYY)", "Job Slut (DD/MM/YYYY)"], // Field names
                            [typeof(CInput), typeof(CInput), typeof(CInput), typeof(CComboBox), typeof(CComboBox), typeof(CInput), typeof(CComboBox), typeof(CInput), typeof(CInput)], // Field types
                            [], [postalCodes, schools, jobs], // CInput values, CCombobox values
                            []);
                        if (form.IsFinished) table.Add(form.GetValues());

                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
                        break;
                    case 1: // Add postal code
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorders([CRender.ActiveColor, Styling.Blink]).AddFonts([CRender.ActiveColor, Styling.Blink]).Build());
                        form = new(innerBox, "Add Postal",
                            ["Postal Code", "City"], // Field names
                            [typeof(CInput), typeof(CInput)], // Field types
                            [], []); // CInput values, CCombobox values
                        if (form.IsFinished) CDatabase.AddPostal(form.GetValues()[0], form.GetValues()[1]);

                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
                        break;
                    case 2: // Add job
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorders([CRender.ActiveColor, Styling.Blink]).AddFonts([CRender.ActiveColor, Styling.Blink]).Build());
                        form = new(innerBox, "Add Job",
                            ["Job"], // Field names
                            [typeof(CInput)], // Field types
                            [], []);  // CInput values, CCombobox values
                        if (form.IsFinished) CDatabase.AddJob(form.GetValues()[0]);

                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
                        break;
                    case 3: // Add education
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorders([CRender.ActiveColor, Styling.Blink]).AddFonts([CRender.ActiveColor, Styling.Blink]).Build());
                        form = new(innerBox, "Add Education",
                             ["Education"], // Field names
                             [typeof(CInput)], // Field types
                             [], []); // CInput values, CCombobox values
                        if (form.IsFinished) CDatabase.AddEducation(form.GetValues()[0]);
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
                        break;
                    case 4: // Settings
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorders([CRender.ActiveColor, Styling.Blink]).AddFonts([CRender.ActiveColor, Styling.Blink]).Build());

                        int activeColor = (int)CRender.ActiveColor - 1;

                        form = new(innerBox, "Settings",
                            ["Console Title", "ActiveColor"], // Field names
                            [typeof(CInput), typeof(CComboBox)], // Field types
                            [Console.Title],
                            [Enum.GetNames(typeof(Color)).ToList()], // CInput values, CCombobox values
                            [activeColor]);

                        if (form.IsFinished)
                        {
                            Console.Title = form.GetValues()[0];
                            CRender.ActiveColor = (Color)Enum.Parse(typeof(Color), form.GetValues()[1]);
                        }
                        btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
                        break;
                }
            }
            break;
        case ConsoleKey.RightArrow:
            if(table.IsFocused)
            {
                table.SelectIndex++;
                table.UpdateSelectIndex();
            } 
            else
            {
                if(btnIndex++ >= btns.Count) btnIndex = 0;

                for(int i = 0; i < btns.Count; i++)
                {
                    btns[i].ChangeStyling(new CStyleBuilder().AddBorder(i == btnIndex ? CRender.ActiveColor : Color.white).AddFont(i == btnIndex ? CRender.ActiveColor : Color.white).Build());
                }
            }
            break;
        case ConsoleKey.LeftArrow:
            if(table.IsFocused)
            {
                table.SelectIndex--;
                table.UpdateSelectIndex();
            } 
            else
            {
                if(btnIndex-- < 0) btnIndex = btns.Count - 1;
                for (int i = 0; i < btns.Count; i++)
                {
                    btns[i].ChangeStyling(new CStyleBuilder().AddBorder(i == btnIndex ? CRender.ActiveColor : Color.white).AddFont(i == btnIndex ? CRender.ActiveColor : Color.white).Build());
                }
            }
            break;
        case ConsoleKey.UpArrow:
            if(table.IsFocused) table.UpdateActiveContentRow(table.ContentIndex - 1);
            break;
        case ConsoleKey.DownArrow:
            if(table.IsFocused) table.UpdateActiveContentRow(table.ContentIndex + 1);
            break;
        case ConsoleKey.Tab:
            foreach(CButton btn in btns) btn.ChangeStyling(new CStyleBuilder().AddBorder(Color.white).AddFont(Color.white).Build());
            table.IsFocused = !table.IsFocused;

            if (!table.IsFocused) btns[btnIndex].ChangeStyling(new CStyleBuilder().AddBorder(CRender.ActiveColor).AddFont(CRender.ActiveColor).Build());
            table.Render();
            break;
    }
}