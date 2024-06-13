using MySql.Data.MySqlClient;
using Org.BouncyCastle.Math.EC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace sm
{
    internal class CTable : CObject
    {
        CStyle Style;
        List<string> Headers = [];
        public List<List<string>> Content { get; } = [];
        int currentHeight = 0;

        int maxPerPage = 26;
        int currentPage = 0;

        public int contentIndex { get; set; } = 0;
        public int selectIndex { get; set; } = 11;

        public CTable(CObject _parent, Point _pos, Dimensions _dim, CStyle _style, Align _align = Align.None, List<string>? _headers = null, List<List<string>>? _content = null) : base(_parent, _pos, _dim)
        {
            if (Dim.Height < 6) Dim = new Dimensions(Dim.Width, 6);
            if (Parent != null && Dim.Height > Parent.Dim.Height - Pos.Absolute.Y) Dim = new Dimensions(Dim.Width, Parent.Dim.Height - Pos.Absolute.Y + 1);

            Style = _style;
            if (_headers != null) Headers = _headers;
            if (_content != null) Content = _content;

            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();

            fetch();
        }

        internal override void Render()
        {

            if (contentIndex > Content.Count - 1)
            {
                contentIndex = 0;
                currentPage = 0;
            }

            if (contentIndex < 0)
            {
                contentIndex = Content.Count - 1;
                currentPage = Content.Count / maxPerPage;
            }

            if (contentIndex > (currentPage + 1) * maxPerPage - 1)
            {
                currentPage++;
            }
            if (contentIndex < ((currentPage + 1) * maxPerPage) - maxPerPage && currentPage > 0)
            {
                currentPage--;
            }


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

                        if (o == Headers.Count - 2) contentText = "Edit";
                        if (o == Headers.Count - 1) contentText = "Slet";

                        if (contentIndex == i && selectIndex == o) contentText = $"> {contentText}";
                        tmp = Border(Get.Vertical);
                        tmp = Style.Set(tmp, Style.Border);
                        tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                        tmp += contentIndex == i && selectIndex == o ? Style.Set(contentText, [Color.red]) : Style.Set(contentText, Style.Font);
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

        internal void Add(List<string> _content)
        {
            string fName = _content[0];
            string lName = _content[1];
            string street = _content[2];
            string eduEnd = _content[3];
            string jobStart = _content[4];
            string jobEnd = _content[5];
            string postal = _content[6];

            string jobIndex = CDatabase.GetJobIndex(_content[8]).ToString();
            string eduIndex = CDatabase.GetEducationIndex(_content[7]).ToString();

            
            List<List<string>> result = CDatabase.Read1($"SELECT AUTO_INCREMENT FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'customer';", ["AUTO_INCREMENT"]);
            int customerID = Int32.Parse(result[0][0] ?? "0");

            CDatabase.Write1($"INSERT INTO customer (FirstName, LastName, Street, PostalID) VALUES ('{fName}','{lName}','{street}','{postal}');" +
                $"INSERT INTO education (customerid, educationName, educationEnd) VALUES ('{customerID}','{eduIndex}','{eduEnd}');" +
                $"INSERT INTO employment (customerid, EmploymentName, EmploymentStart, EmploymentEnd) VALUES ('{customerID}','{jobIndex}','{jobStart}', '{jobEnd}')");

            fetch();
        }

        internal void Edit(List<string> _content)
        {
            Content[contentIndex] = _content;
            Render();
        }

        internal void Delete()
        {
            string fName = Content[contentIndex][0];
            string lName = Content[contentIndex][1];
            int customerID = -1;

            List<List<string>> result = CDatabase.Read1($"SELECT id FROM customer WHERE FirstName = '{fName}' AND LastName = '{lName}';", ["id"]);
            customerID = Int32.Parse(result[0][0]);
            
            CDatabase.Write1($"DELETE FROM customer WHERE id = {customerID}; DELETE FROM education WHERE customerid = {customerID}; DELETE FROM employment WHERE customerid = {customerID}");

            Content.RemoveAt(contentIndex);
            Render();
        }

        internal void Reset()
        {
            Content.Clear();
            Render();
        }

        internal List<string> GetValues()
        {
            return Content[contentIndex];
        }

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal void fetch()
        {
            Reset();

            List<List<string>> result = CDatabase.Read1("SELECT customer.id, customer.FirstName, customer.LastName, customer.Street, " +
                "city.CityName, city.PostalCode, " +
                "schools.schoolsName, education.educationEnd, " +
                "jobs.JobName, employment.EmploymentStart, employment.EmploymentEnd " +
                "FROM customer, city, schools, education, jobs, employment " +
                "WHERE city.PostalCode = customer.PostalID AND education.customerid = customer.id " +
                "AND schools.educationID = education.educationName AND employment.customerid = customer.id " +
                "AND jobs.JobID = employment.EmploymentName", ["FirstName", "LastName", "Street", "CityName", "PostalCode", "schoolsName", "educationEnd", "JobName", "EmploymentStart", "EmploymentEnd"]);


            for(int i = 0; i < result.Count; i++) {

                List<string> tmp = result[i];
                for(int o = 0; o < result[i].Count; o++)
                {
                    if (tmp[o].Length > 10)
                    {
                        tmp[o] = tmp[o].Substring(0, 11);
                    }
                }
                Content.Add(tmp);
            }

            Render();
        }

        internal void updateSelectIndex(int index)
        {
            if (selectIndex > 11) selectIndex = 10;
            if (selectIndex < 10) selectIndex = 11;

            int tabWidth = Dim.Width / Headers.Count;

            string tmp = "";
            for (int i = (currentPage + 1) * maxPerPage - maxPerPage; i < Content.Count; i++)
            {
                if (i > (currentPage + 1) * maxPerPage - 1) break;
                if (i > Content.Count || i < 0) break;

                int cIndex = contentIndex - (maxPerPage * currentPage);

                for(int o = 10; o <= 11; o++)
                {
                    string contentText = o == 10 ? "Edit" : "Slet";

                    if (o == selectIndex) contentText = $"> {contentText}";

                    tmp = Border(Get.Vertical);
                    tmp += BuildString(" ", (tabWidth - contentText.Length) / 2);
                    tmp += o == selectIndex ? Style.Set(contentText, [Color.red]) : contentText;
                    tmp += BuildString(" ", (tabWidth - 1 - contentText.Length) / 2);
                    tmp += Border(Get.Vertical);

                    Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + cIndex + 3), tmp);
                }
            }
        }
    }
}