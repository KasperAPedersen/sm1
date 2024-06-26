using System.Drawing;
using System.Globalization;

namespace sm
{
    internal class CTable : CObject
    {
        public bool isFocused = false;
        CStyle Style;
        readonly List<string> Headers = [];
        public List<List<string>> Content { get; } = [];
        int currentHeight = 0;

        readonly int maxPerPage = 26;
        int currentPage = 0;

        public int ContentIndex { get; set; } = 0;
        public int SelectIndex { get; set; } = 11;

        public CTable(CObject _parent, Point _pos, Dimensions _dim, CStyle _style, Align _align = Align.None, List<string>? _headers = null, List<List<string>>? _content = null) : base(_parent, _pos, _dim)
        {
            if (Dim.Height < 6) Dim = new Dimensions(Dim.Width, 6);
            if (Parent != null && Dim.Height > Parent.Dim.Height - Pos.Absolute.Y) Dim = new Dimensions(Dim.Width, Parent.Dim.Height - Pos.Absolute.Y + 1);

            Style = _style;
            if (_headers != null) Headers = _headers;
            if (_content != null) Content = _content;

            if (ShouldRender && NewObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();

            Fetch();
        }

        internal override void Render()
        {
            Remove(Pos.Absolute, new Dimensions(Dim.Width + 1, Dim.Height));

            string tmp;
            int tabWidth = Dim.Width / Headers.Count;

            // Header
            for (int i = 0; i < Headers.Count; i++)
            {
                currentHeight = 0;
                tmp = i == 0 ? Border(Get.TopLeft) : Border(Get.HorizontalDown);
                tmp += BuildString(Border(Get.Horizontal), tabWidth - 1);
                tmp += i == Headers.Count - 1 ? Border(Get.TopRight) : "";
                tmp = Style.Set(tmp, Style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight++), tmp);

                tmp = Border(Get.Vertical);
                tmp += BuildString(" ", (tabWidth - Headers[i].Length) / 2);
                tmp += Style.Set(Headers[i], Style.Font);
                tmp += BuildString(" ", (tabWidth - 1 - Headers[i].Length) / 2);
                tmp += i == Headers.Count - 1 ? Style.Set(Border(Get.Vertical), Style.Border) : "";
                tmp = Style.Set(tmp, Style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight++), tmp);

                tmp = i == 0 ? Border(Get.VerticalLeft) : Border(Get.Cross);
                tmp += BuildString(Border(Get.Horizontal), tabWidth - 1);
                tmp += i == Headers.Count - 1 ? Style.Set(Border(Get.VerticalRight), Style.Border) : "";
                tmp = Style.Set(tmp, Style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight++), tmp);
            }

            // Content
            for (int i = (currentPage + 1) * maxPerPage - maxPerPage; i < Content.Count; i++)
            {
                if (i > (currentPage + 1) * maxPerPage - 1) break;
                if (i > Content.Count || i < 0) break;

                if (currentHeight + 3 < Dim.Height)
                {
                    for (int o = 0; o < Headers.Count; o++)
                    {
                        string contentText;
                        contentText = (o > Content[i].Count - 1) ? (Headers.Count - 2).ToString() : Content[i][o];

                        if (contentText.Length > 10)
                        {
                            contentText = contentText[..11];
                        }

                        if (o == 6 || o == 8 || o == 9)
                        {
                            DateTime dt = DateTime.ParseExact(contentText.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            contentText = dt.ToString("d MMM yyyy", CultureInfo.InvariantCulture);
                        }

                        if (o == Headers.Count - 2) contentText = "Edit";
                        if (o == Headers.Count - 1) contentText = "Slet";

                        if (ContentIndex == i && SelectIndex == o && isFocused) contentText = $"> {contentText}";

                        tmp = Border(Get.Vertical);
                        tmp = Style.Set(tmp, Style.Border);
                        tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                        tmp += ContentIndex == i && SelectIndex == o && isFocused ? Style.Set(contentText, [CRender.ActiveColor]) : Style.Set(contentText, Style.Font);
                        tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                        tmp += o == Headers.Count - 1 ? Style.Set(Border(Get.Vertical), Style.Border) : "";
                        Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + currentHeight), tmp);
                    }
                    currentHeight++;
                }
            }

            // Footer
            for (int i = 0; i < Headers.Count; i++)
            {
                tmp = i == 0 ? Border(Get.VerticalLeft) : Border(Get.HorizontalUp);
                tmp += BuildString(Border(Get.Horizontal), tabWidth - 1);
                tmp += i == Headers.Count - 1 ? Border(Get.VerticalRight) : "";
                tmp = Style.Set(tmp, Style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight), tmp);
            }

            string footerText = $"Page {currentPage + 1}/{((Content.Count / maxPerPage + (Content.Count % maxPerPage == 0 ? 0 : 1)) == 0 ? 1 : Content.Count / maxPerPage + (Content.Count % maxPerPage == 0 ? 0 : 1))}";
            int footerTextPadRem = (tabWidth * Headers.Count - 1 - footerText.Length) % 2;
            string footerTextPad = BuildString(" ", (tabWidth * Headers.Count - 1 - footerText.Length) / 2);
            
            tmp = $"{Border(Get.Vertical)}{footerTextPad}{footerText}{footerTextPad}{(footerTextPadRem != 0 ? BuildString(" ", footerTextPadRem) : "")}{Border(Get.Vertical)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + ++currentHeight), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), tabWidth * Headers.Count - 1)}{Border(Get.BottomRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + ++currentHeight), tmp);
        }

        internal async void Add(List<string> _content)
        {
            string fName = _content[0];
            string lName = _content[1];
            string street = _content[2];
            string postal = _content[3];
            string eduIndex = await CDatabase.GetEducationIndex(_content[4]);
            string eduEnd = ConvertDate(_content[5]);
            string jobIndex = await CDatabase.GetJobIndex(_content[6]);
            string jobStart = ConvertDate(_content[7]);
            string jobEnd = ConvertDate(_content[8]);
            
            List<string[]> result = await CDatabase.Exec("SELECT AUTO_INCREMENT FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'customer';");
            int customerID = Int32.Parse(result[0][0] ?? "0");

            await CDatabase.Exec($"INSERT INTO customer (FirstName, LastName, Street, PostalID) VALUES ('{fName}','{lName}','{street}','{postal}');");
            await CDatabase.Exec($"INSERT INTO education (customerid, educationName, educationEnd) VALUES ('{customerID}','{eduIndex}','{eduEnd}');");
            await CDatabase.Exec($"INSERT INTO employment (customerid, EmploymentName, EmploymentStart, EmploymentEnd) VALUES ('{customerID}','{jobIndex}','{jobStart}', '{jobEnd}')");
            
            Fetch();
        }

        internal async void Edit(List<string> _content)
        {
            string fName = _content[0];
            string lName = _content[1];
            string street = _content[2];
            string postal = _content[3];
            string eduIndex = await CDatabase.GetEducationIndex(_content[4]);
            string eduEnd = ConvertDate(_content[5]);
            string jobIndex = await CDatabase.GetJobIndex(_content[6]);
            string jobStart = ConvertDate(_content[7]);
            string jobEnd = ConvertDate(_content[8]);

            string oldFName = Content[ContentIndex][0];
            string oldLName = Content[ContentIndex][1];

            List<string[]> result = await CDatabase.Exec($"SELECT id FROM customer WHERE FirstName = '{oldFName}' AND LastName = '{oldLName}'");
            int customerID = Int32.Parse(result[0][0]);

            await CDatabase.Exec($"UPDATE customer SET FirstName = '{fName}', LastName = '{lName}', Street = '{street}', PostalID = '{postal}' WHERE id = {customerID};");
            await CDatabase.Exec($"UPDATE education SET educationName = {eduIndex}, educationEnd = '{eduEnd}' WHERE customerid = {customerID};");
            await CDatabase.Exec($"UPDATE employment SET EmploymentName = {jobIndex}, EmploymentStart = '{jobStart}', EmploymentEnd = '{jobEnd}' WHERE customerid = {customerID};");

            Fetch();
        }

        internal async void Delete()
        {
            string fName = Content[ContentIndex][0];
            string lName = Content[ContentIndex][1];

            List<string[]> result = await CDatabase.Exec($"SELECT id FROM customer WHERE FirstName = '{fName}' AND LastName = '{lName}';");
            int customerID = Int32.Parse(result[0][0] ?? "-1");

            await CDatabase.Exec($"DELETE FROM customer WHERE id = {customerID};");
            await CDatabase.Exec($"DELETE FROM education WHERE customerid = {customerID};");
            await CDatabase.Exec($"DELETE FROM employment WHERE customerid = {customerID};");

            Content.RemoveAt(ContentIndex);
            UpdateActiveContentRow(ContentIndex);
            Render();
        }

        internal void Reset()
        {
            Content.Clear();
            Render();
        }

        internal List<string> GetValues()
        {
            return Content[ContentIndex];
        }

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal async void Fetch()
        {
            Reset();

            List<string[]> res = await CDatabase.Exec("SELECT customer.FirstName, customer.LastName, customer.Street, " +
                "city.CityName, city.PostalCode, " +
                "schools.schoolsName, education.educationEnd, " +
                "jobs.JobName, employment.EmploymentStart, employment.EmploymentEnd " +
                "FROM customer, city, schools, education, jobs, employment " +
                "WHERE city.PostalCode = customer.PostalID AND education.customerid = customer.id " +
                "AND schools.educationID = education.educationName AND employment.customerid = customer.id " +
                "AND jobs.JobID = employment.EmploymentName");

            foreach (string[] s in res) Content.Add([.. s]);

            Render();
        }

        internal void UpdateSelectIndex()
        {
            if (SelectIndex > 11) SelectIndex = 10;
            if (SelectIndex < 10) SelectIndex = 11;

            int tabWidth = Dim.Width / Headers.Count;
            for (int i = (currentPage + 1) * maxPerPage - maxPerPage; i < Content.Count; i++)
            {
                int cIndex = ContentIndex - (maxPerPage * currentPage);

                for(int o = 10; o <= 11; o++)
                {
                    string contentText = o == 10 ? "Edit" : "Slet";
                    if (o == SelectIndex) contentText = $"> {contentText}";

                    string tmp = Border(Get.Vertical);
                    tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                    tmp += o == SelectIndex ? Style.Set(contentText, [CRender.ActiveColor]) : Style.Set(contentText, [Color.white]);
                    tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                    tmp += Border(Get.Vertical);
                    tmp = Style.Set(tmp, [Color.white]);

                    Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + cIndex + 3), tmp);
                }
            }
        }

        internal void UpdateActiveContentRow(int newIndex)
        {
            int tabWidth = Dim.Width / Headers.Count;
            for (int o = 10; o <= 11; o++)
            {
                string contentText = o == 10 ? "Edit" : "Slet";

                string tmp = Border(Get.Vertical);
                tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                tmp += contentText;
                tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                tmp += Border(Get.Vertical);

                tmp = Style.Set(tmp, [Color.white]);

                Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + 3 + (ContentIndex >= maxPerPage ? ContentIndex - (maxPerPage * currentPage) : ContentIndex)), tmp);
            }

            ContentIndex = newIndex;

            if (ContentIndex > Content.Count - 1)
            {
                ContentIndex = 0;
                currentPage = 0;
                Render();
            }

            if (ContentIndex < 0)
            {
                ContentIndex = Content.Count - 1;
                currentPage = Content.Count / maxPerPage;
                Render();
            }

            if (ContentIndex > (currentPage + 1) * maxPerPage - 1)
            {
                currentPage++;
                Render();
            }
            if (ContentIndex < ((currentPage + 1) * maxPerPage) - maxPerPage && currentPage > 0)
            {
                currentPage--;
                Render();
            }

            UpdateSelectIndex();
        }

        internal static string ConvertDate(string date)
        {
            string[] tmp = date.Split('/');
            tmp[2] = tmp[2].Split(' ')[0];

            if (tmp.Length < 3) return date;
            
            for (int i = 0; i < tmp.Length; i++) tmp[i] = tmp[i].Trim();
            date = $"{tmp[2]}-{tmp[1]}-{tmp[0]}";
            
            return date;
        }
    }
}