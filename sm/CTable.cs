using System.Drawing;
using System.Globalization;

namespace sm
{
    internal class CTable : CObject
    {
        public bool IsFocused = false;
        private CStyle _style;
        private readonly List<string> _headers = [];
        public List<List<string>> Content { get; } = [];
        private int _currentHeight;

        private readonly int _maxPerPage = 26;
        private int _currentPage;

        public int ContentIndex { get; private set; }
        public int SelectIndex { get; set; } = 11;

        public CTable(CObject parent, Point pos, Dimensions dim, CStyle style, Align align = Align.None, List<string>? headers = null, List<List<string>>? content = null) : base(parent, pos, dim)
        {
            if (Dim.Height < 6) Dim = new Dimensions(Dim.Width, 6);
            if (Parent != null && Dim.Height > Parent.Dim.Height - Pos.Absolute.Y) Dim = new Dimensions(Dim.Width, Parent.Dim.Height - Pos.Absolute.Y + 1);

            _style = style;
            if (headers != null) _headers = headers;
            if (content != null) Content = content;

            Initialize(parent, pos, align);
            Fetch();
        }
        
        private void Initialize(CObject parent, Point pos, Align align)
        {
            if (ShouldRender && NewObjPos(parent, Aligner(align, parent, pos), Dim)) Render();
        }

        internal override void Render()
        {
            Remove(Pos.Absolute, new Dimensions(Dim.Width + 1, Dim.Height));

            string tmp;
            int tabWidth = Dim.Width / _headers.Count;

            // Header
            for (int i = 0; i < _headers.Count; i++)
            {
                _currentHeight = 0;
                tmp = i == 0 ? Border(Get.TopLeft) : Border(Get.HorizontalDown);
                tmp += BuildString(Border(Get.Horizontal), tabWidth - 1);
                tmp += i == _headers.Count - 1 ? Border(Get.TopRight) : "";
                tmp = _style.Set(tmp, _style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + _currentHeight++), tmp);

                tmp = Border(Get.Vertical);
                tmp += BuildString(" ", (tabWidth - _headers[i].Length) / 2);
                tmp += _style.Set(_headers[i], _style.Font);
                tmp += BuildString(" ", (tabWidth - 1 - _headers[i].Length) / 2);
                tmp += i == _headers.Count - 1 ? _style.Set(Border(Get.Vertical), _style.Border) : "";
                tmp = _style.Set(tmp, _style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + _currentHeight++), tmp);

                tmp = i == 0 ? Border(Get.VerticalLeft) : Border(Get.Cross);
                tmp += BuildString(Border(Get.Horizontal), tabWidth - 1);
                tmp += i == _headers.Count - 1 ? _style.Set(Border(Get.VerticalRight), _style.Border) : "";
                tmp = _style.Set(tmp, _style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + _currentHeight++), tmp);
            }

            // Content
            for (int i = (_currentPage + 1) * _maxPerPage - _maxPerPage; i < Content.Count; i++)
            {
                if (i > (_currentPage + 1) * _maxPerPage - 1) break;
                if (i > Content.Count || i < 0) break;

                if (_currentHeight + 3 < Dim.Height)
                {
                    for (int o = 0; o < _headers.Count; o++)
                    {
                        string contentText;
                        contentText = (o > Content[i].Count - 1) ? (_headers.Count - 2).ToString() : Content[i][o];

                        if (contentText.Length > 10)
                        {
                            contentText = contentText[..11];
                        }

                        if (o == 6 || o == 8 || o == 9)
                        {
                            DateTime dt = DateTime.ParseExact(contentText.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            contentText = dt.ToString("d MMM yyyy", CultureInfo.InvariantCulture);
                        }

                        if (o == _headers.Count - 2) contentText = "Edit";
                        if (o == _headers.Count - 1) contentText = "Slet";

                        if (ContentIndex == i && SelectIndex == o && IsFocused) contentText = $"> {contentText}";

                        tmp = Border(Get.Vertical);
                        tmp = _style.Set(tmp, _style.Border);
                        tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                        tmp += ContentIndex == i && SelectIndex == o && IsFocused ? _style.Set(contentText, [CRender.ActiveColor]) : _style.Set(contentText, _style.Font);
                        tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                        tmp += o == _headers.Count - 1 ? _style.Set(Border(Get.Vertical), _style.Border) : "";
                        Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + _currentHeight), tmp);
                    }
                    _currentHeight++;
                }
            }

            // Footer
            for (int i = 0; i < _headers.Count; i++)
            {
                tmp = i == 0 ? Border(Get.VerticalLeft) : Border(Get.HorizontalUp);
                tmp += BuildString(Border(Get.Horizontal), tabWidth - 1);
                tmp += i == _headers.Count - 1 ? Border(Get.VerticalRight) : "";
                tmp = _style.Set(tmp, _style.Border);
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + _currentHeight), tmp);
            }

            string footerText = $"Page {_currentPage + 1}/{((Content.Count / _maxPerPage + (Content.Count % _maxPerPage == 0 ? 0 : 1)) == 0 ? 1 : Content.Count / _maxPerPage + (Content.Count % _maxPerPage == 0 ? 0 : 1))}";
            int footerTextPadRem = (tabWidth * _headers.Count - 1 - footerText.Length) % 2;
            string footerTextPad = BuildString(" ", (tabWidth * _headers.Count - 1 - footerText.Length) / 2);
            
            tmp = $"{Border(Get.Vertical)}{footerTextPad}{footerText}{footerTextPad}{(footerTextPadRem != 0 ? BuildString(" ", footerTextPadRem) : "")}{Border(Get.Vertical)}";
            tmp = _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + ++_currentHeight), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), tabWidth * _headers.Count - 1)}{Border(Get.BottomRight)}";
            tmp = _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + ++_currentHeight), tmp);
        }

        internal async void Add(List<string> content)
        {
            string fName = content[0];
            string lName = content[1];
            string street = content[2];
            string postal = content[3];
            string eduIndex = await CDatabase.GetEducationIndex(content[4]);
            string eduEnd = ConvertDate(content[5]);
            string jobIndex = await CDatabase.GetJobIndex(content[6]);
            string jobStart = ConvertDate(content[7]);
            string jobEnd = ConvertDate(content[8]);
            
            List<string[]> result = await CDatabase.Exec("SELECT AUTO_INCREMENT FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'customer';");
            int customerId = Int32.Parse(result[0][0]);

            await CDatabase.Exec($"INSERT INTO customer (FirstName, LastName, Street, PostalID) VALUES ('{fName}','{lName}','{street}','{postal}');");
            await CDatabase.Exec($"INSERT INTO education (customerid, educationName, educationEnd) VALUES ('{customerId}','{eduIndex}','{eduEnd}');");
            await CDatabase.Exec($"INSERT INTO employment (customerid, EmploymentName, EmploymentStart, EmploymentEnd) VALUES ('{customerId}','{jobIndex}','{jobStart}', '{jobEnd}')");
            
            Fetch();
        }

        internal async void Edit(List<string> content)
        {
            string fName = content[0];
            string lName = content[1];
            string street = content[2];
            string postal = content[3];
            string eduIndex = await CDatabase.GetEducationIndex(content[4]);
            string eduEnd = ConvertDate(content[5]);
            string jobIndex = await CDatabase.GetJobIndex(content[6]);
            string jobStart = ConvertDate(content[7]);
            string jobEnd = ConvertDate(content[8]);

            string oldFName = Content[ContentIndex][0];
            string oldLName = Content[ContentIndex][1];

            List<string[]> result = await CDatabase.Exec($"SELECT id FROM customer WHERE FirstName = '{oldFName}' AND LastName = '{oldLName}'");
            int customerId = Int32.Parse(result[0][0]);

            await CDatabase.Exec($"UPDATE customer SET FirstName = '{fName}', LastName = '{lName}', Street = '{street}', PostalID = '{postal}' WHERE id = {customerId};");
            await CDatabase.Exec($"UPDATE education SET educationName = {eduIndex}, educationEnd = '{eduEnd}' WHERE customerid = {customerId};");
            await CDatabase.Exec($"UPDATE employment SET EmploymentName = {jobIndex}, EmploymentStart = '{jobStart}', EmploymentEnd = '{jobEnd}' WHERE customerid = {customerId};");

            Fetch();
        }

        internal async void Delete()
        {
            string fName = Content[ContentIndex][0];
            string lName = Content[ContentIndex][1];

            List<string[]> result = await CDatabase.Exec($"SELECT id FROM customer WHERE FirstName = '{fName}' AND LastName = '{lName}';");
            int customerId = Int32.Parse(result[0][0]);

            await CDatabase.Exec($"DELETE FROM customer WHERE id = {customerId};");
            await CDatabase.Exec($"DELETE FROM education WHERE customerid = {customerId};");
            await CDatabase.Exec($"DELETE FROM employment WHERE customerid = {customerId};");

            Content.RemoveAt(ContentIndex);
            UpdateActiveContentRow(ContentIndex);
            Render();
        }

        private void Reset()
        {
            Content.Clear();
            Render();
        }

        internal List<string> GetValues()
        {
            return Content[ContentIndex];
        }

        internal override void ChangeStyling(CStyle style)
        {
            _style = style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        private async void Fetch()
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

            int tabWidth = Dim.Width / _headers.Count;
            for (int i = (_currentPage + 1) * _maxPerPage - _maxPerPage; i < Content.Count; i++)
            {
                int cIndex = ContentIndex - (_maxPerPage * _currentPage);

                for(int o = 10; o <= 11; o++)
                {
                    string contentText = o == 10 ? "Edit" : "Slet";
                    if (o == SelectIndex) contentText = $"> {contentText}";

                    string tmp = Border(Get.Vertical);
                    tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                    tmp += o == SelectIndex ? _style.Set(contentText, [CRender.ActiveColor]) : _style.Set(contentText, [Color.white]);
                    tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                    tmp += Border(Get.Vertical);
                    tmp = _style.Set(tmp, [Color.white]);

                    Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + cIndex + 3), tmp);
                }
            }
        }

        internal void UpdateActiveContentRow(int newIndex)
        {
            int tabWidth = Dim.Width / _headers.Count;
            for (int o = 10; o <= 11; o++)
            {
                string contentText = o == 10 ? "Edit" : "Slet";

                string tmp = Border(Get.Vertical);
                tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                tmp += contentText;
                tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                tmp += Border(Get.Vertical);

                tmp = _style.Set(tmp, [Color.white]);

                Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + 3 + (ContentIndex >= _maxPerPage ? ContentIndex - (_maxPerPage * _currentPage) : ContentIndex)), tmp);
            }

            ContentIndex = newIndex;

            if (ContentIndex > Content.Count - 1)
            {
                ContentIndex = 0;
                _currentPage = 0;
                Render();
            }

            if (ContentIndex < 0)
            {
                ContentIndex = Content.Count - 1;
                _currentPage = Content.Count / _maxPerPage;
                Render();
            }

            if (ContentIndex > (_currentPage + 1) * _maxPerPage - 1)
            {
                _currentPage++;
                Render();
            }
            if (ContentIndex < ((_currentPage + 1) * _maxPerPage) - _maxPerPage && _currentPage > 0)
            {
                _currentPage--;
                Render();
            }

            UpdateSelectIndex();
        }

        private static string ConvertDate(string date)
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