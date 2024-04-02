using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace sm
{
    internal class CComboBox : CObject, IController
    {
        List<object> Styling = [];
        List<string> Content;
        public string Text { get; set; } = "Mr.";
        bool Expanded = false;
        int selectIndex = 0;

        public CComboBox(CObject _parent, Point _pos, Dimensions _dim, List<string> _content, Align _align = Align.None, List<object>? _styles = null) : base(_parent, _pos, _dim)
        {
            Dim = new Dimensions(Dim.Width, 3);
            shouldRender = false;
            Content = _content;

            if (_styles != null) Styling = _styles;
            
            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            Remove(Pos.Absolute, Dim);
            
            if (Text.Length > Dim.Width - 8) Text = Text.Substring(0, Dim.Width - 8);

            if(selectIndex < 0) selectIndex = Content.Count - 1;
            if (selectIndex > Content.Count - 1) selectIndex = 0;

            int currentHeight = 0;
            string tmp;

            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 6))}{Border(Get.HorizontalDown)}";
            tmp += $"{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), 3))}{Border(Get.TopRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            string pad = string.Concat(Enumerable.Repeat(" ", ((Dim.Width - 6) - Text.Length) / 2));
            int padRem = ((Dim.Width - 6) - Text.Length) % 2;
            tmp = $"{Border(Get.Vertical)}{pad}{Text}{pad}{(padRem > 0 ? string.Concat(Enumerable.Repeat(" ", padRem)) : "")}{Border(Get.Vertical)}";
            tmp += $" {Border(Get.ArrowDown)} {Border(Get.Vertical)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 6))}{Border(Get.HorizontalUp)}";
            tmp += $"{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), 3))}{Border(Get.BottomRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);

            if (!Expanded) return;

            tmp = $"{Border(Get.VerticalLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 6))}{Border(Get.HorizontalUp)}";
            tmp += $"{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), 3))}{Border(Get.VerticalRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            for(int i = 0; i < Content.Count; i++)
            {
                pad = string.Concat(Enumerable.Repeat(" ", ((Dim.Width - 2) - Content[i].Length) / 2));
                padRem = ((Dim.Width - 2) - Content[i].Length) % 2;
                tmp = $"{Border(Get.Vertical)}{pad}{(selectIndex == i ? CStyling.Set(Content[i], [FontColor.red]) : Content[i])}{pad}{(padRem > 0 ? string.Concat(Enumerable.Repeat(" ", padRem)) : "")}{Border(Get.Vertical)}";
                Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            }

            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.BottomRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            
            Dim = new Dimensions(Dim.Width, currentHeight);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = false;
            Expanded = true;
            Render();

            bool keepRunning = true;
            while(keepRunning)
            {
                SetPos(new Point(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1));
                ConsoleKeyInfo key = Console.ReadKey();
                switch(key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (!Expanded) return ControllerState.Next;

                        selectIndex++;
                        Render();
                        break;
                    case ConsoleKey.UpArrow:
                        if(!Expanded) return ControllerState.Previous;

                        selectIndex--;
                        Render();
                        break;
                    case ConsoleKey.Escape:
                        
                        if(!Expanded) return ControllerState.Cancel;

                        Expanded = false;
                        Render();
                        break;
                    case ConsoleKey.Enter:
                        Expanded = false;
                        Text = Content[selectIndex];
                        Render();
                        return ControllerState.Next;
                    default:
                        break;
                }

                Render();
            }
            return ControllerState.Idle;
        }

        internal override void ChangeStyling(List<object> _styles)
        {
            Styling = _styles;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}
