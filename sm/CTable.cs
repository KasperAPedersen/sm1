using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace sm
{
    internal class CTable : CObject
    {
        List<object> Styling = [];
        List<string> Headers = [];
        List<List<string>> Content = [];
        int currentHeight = 0;

        public int contentIndex { get; set; } = 0;
        public int selectIndex { get; set; } = 6;

        public CTable(CObject _parent, Point _pos, Dimensions _dim, Align _align = Align.None, List<object>? _styles = null, List<string>? _headers = null, List<List<string>>? _content = null) : base(_parent, _pos, _dim)
        {
            if (Dim.Height < 6) Dim = new Dimensions(Dim.Width, 6);
            if (Parent != null && Dim.Height > Parent.Dim.Height - Pos.Absolute.Y) Dim = new Dimensions(Dim.Width, Parent.Dim.Height - Pos.Absolute.Y + 1);

            if (_styles != null) Styling = _styles;
            if (_headers != null) Headers = _headers;
            if (_content != null) Content = _content;

            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            if (selectIndex > 7) selectIndex = 6;
            if (selectIndex < 6) selectIndex = 7;

            if (contentIndex > Content.Count - 1) contentIndex = 0;
            if (contentIndex < 0) contentIndex = Content.Count - 1;

            Remove(Pos.Absolute, Dim);
            
            string tmp;
            int tabWidth = Dim.Width / Headers.Count;

            // Header
            for (int i = 0; i < Headers.Count; i++)
            {
                currentHeight = 0;
                tmp = i == 0 ? Border(Get.TopLeft) : Border(Get.HorizontalDown);
                tmp += string.Concat(Enumerable.Repeat(Border(Get.Horizontal), tabWidth - 1));
                tmp += i == Headers.Count - 1 ? Border(Get.TopRight) : "";
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight++), tmp);

                tmp = Border(Get.Vertical);
                tmp += string.Concat(Enumerable.Repeat(" ", (tabWidth - Headers[i].Length) / 2));
                tmp += Headers[i];
                tmp += string.Concat(Enumerable.Repeat(" ", (tabWidth - 1 - Headers[i].Length) / 2));
                tmp += i == Headers.Count - 1 ? Border(Get.Vertical) : "";
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight++), tmp);

                tmp = i == 0 ? Border(Get.VerticalLeft) : Border(Get.Cross);
                tmp += string.Concat(Enumerable.Repeat(Border(Get.Horizontal), tabWidth - 1));
                tmp += i == Headers.Count - 1 ? Border(Get.VerticalRight) : "";
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight++), tmp);
            }

            // Content
            for (int i = 0; i < Content.Count; i++)
            {
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
                        tmp += string.Concat(Enumerable.Repeat(" ", (tabWidth - contentText.Length) / 2));
                        tmp += contentIndex == i && selectIndex == o ? CStyling.Set(contentText, [FontColor.red]) : contentText;
                        tmp += string.Concat(Enumerable.Repeat(" ", (tabWidth - 1 - contentText.Length) / 2));
                        tmp += o == Headers.Count - 1 ? Border(Get.Vertical) : "";
                        Write(new Point(Pos.Absolute.X + (o * tabWidth), Pos.Absolute.Y + currentHeight), tmp);
                    }
                    currentHeight++;
                }
            }

            // Footer
            for(int i = 0; i < Headers.Count; i++)
            {
                tmp = i == 0 ? Border(Get.VerticalLeft) : Border(Get.HorizontalUp);
                tmp += string.Concat(Enumerable.Repeat(Border(Get.Horizontal), tabWidth - 1));
                tmp += i == Headers.Count - 1 ? Border(Get.VerticalRight) : "";
                Write(new Point(Pos.Absolute.X + (i * tabWidth), Pos.Absolute.Y + currentHeight), tmp);
            }

            tmp = $"{Border(Get.Vertical)}{string.Concat(Enumerable.Repeat(" ", tabWidth * Headers.Count - 1))}{Border(Get.Vertical)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + ++currentHeight), tmp);
            
            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), tabWidth * Headers.Count - 1))}{Border(Get.BottomRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + ++currentHeight), tmp);
        }

        internal void Add(List<string> _content)
        {
            Content.Add(_content);
            Render();
        }

        internal void Edit(List<string> _content)
        {
            Content[contentIndex] = _content;
            Render();
        }

        internal void Delete()
        {
            Content.RemoveAt(contentIndex);
            Render();
        }

        internal List<string> GetValues()
        {

            return Content[contentIndex];
        }

        internal override void ChangeStyling(List<object> _styles)
        {
            Styling = _styles;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}