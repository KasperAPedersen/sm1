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
    /*internal class CComboBox : CObject, IController
    {
        CStyle Style { get; set; }

        readonly List<string> Content;
        public string Text { get; set; } = "";
        bool Expanded = false;
        int selectIndex = 0;

        public CComboBox(CObject _parent, Point _pos, Dimensions _dim, List<string> _content, CStyle _style, Align _align = Align.None) : base(_parent, _pos, _dim)
        {
            Dim = new Dimensions(Dim.Width, 3);
            ShouldRender = false;
            Content = _content;
            Style = _style;

            if (ShouldRender && NewObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            Remove(Pos.Absolute, Dim);

            if (Text.Length > Dim.Width - 8) Text = Text[..(Dim.Width - 8)];

            if (selectIndex < 0) selectIndex = Content.Count - 1;
            if (selectIndex > Content.Count - 1) selectIndex = 0;

            int currentHeight = 0;
            string tmp;

            tmp = $"{Border(Get.TopLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 6)}{Border(Get.HorizontalDown)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 3)}{Border(Get.TopRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            string pad = BuildString(" ", ((Dim.Width - 6) - Text.Length) / 2);
            int padRem = ((Dim.Width - 6) - Text.Length) % 2;
            tmp = $"{Border(Get.Vertical)}{pad}{Text}{pad}{(padRem > 0 ? BuildString(" ", padRem) : "")}{Border(Get.Vertical)}";
            tmp += $" {Border(Get.ArrowDown)} {Border(Get.Vertical)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 6)}{Border(Get.HorizontalUp)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 3)}{Border(Get.BottomRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);

            if (!Expanded) return;

            tmp = $"{Border(Get.VerticalLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 6)}{Border(Get.HorizontalUp)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 3)}{Border(Get.VerticalRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            
            for (int i = 0; i < Content.Count; i++)
            {
                pad = BuildString(" ", ((Dim.Width - 2) - Content[i].Length) / 2);
                padRem = ((Dim.Width - 2) - Content[i].Length) % 2;
                tmp = $"{Style.Set(Border(Get.Vertical), Style.Border)}";
                tmp += $"{pad}{(selectIndex == i ? Style.Set(Content[i], [CRender.ActiveColor]) : Content[i])}{pad}{(padRem > 0 ? BuildString(" ", padRem) : "")}";
                tmp += $"{Style.Set(Border(Get.Vertical), Style.Border)}";
                Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            }

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.BottomRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            Dim = new Dimensions(Dim.Width, currentHeight);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = false;
            Expanded = true;
            Render();

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (!Expanded) return ControllerState.Next;

                        selectIndex++;
                        Render();
                        break;
                    case ConsoleKey.UpArrow:
                        if (!Expanded) return ControllerState.Previous;

                        selectIndex--;
                        Render();
                        break;
                    case ConsoleKey.Escape:

                        if (!Expanded) return ControllerState.Cancel;

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

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }*/

    internal class CComboBox : CObject, IController
    {
        CStyle Style { get; set; }

        readonly List<string> Content;
        public string Text { get; set; } = "";
        int selectIndex = 0;
        bool isActive = false;

        public CComboBox(CObject _parent, Point _pos, Dimensions _dim, List<string> _content, CStyle _style, Align _align = Align.None) : base(_parent, _pos, _dim)
        {
            Dim = new Dimensions(Dim.Width, 3);
            ShouldRender = false;
            Content = _content;
            Style = _style;

            if (ShouldRender && NewObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            
            Remove(Pos.Absolute, Dim);

            

            if (selectIndex < 0) selectIndex = Content.Count - 1;
            if (selectIndex > Content.Count - 1) selectIndex = 0;

            Text = Content[selectIndex];

            if (Text.Length > Dim.Width - 8) Text = Text[..(Dim.Width - 8)];

            int currentHeight = 0;
            string tmp;

            tmp = $"{Border(Get.TopLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 7)}{Border(Get.HorizontalDown)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 4)}{Border(Get.TopRight)}";
            tmp = isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);


            tmp = $"{Border(Get.Vertical)}";
            tmp += $" {Text}{new string(' ', Dim.Width - 8 - Text.Length)}";
            tmp += $"{Border(Get.Vertical)}";
            tmp += $" {Border(Get.ArrowLeft)}{Border(Get.ArrowRight)} {Border(Get.Vertical)}";
            tmp = isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 7)}{Border(Get.HorizontalUp)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 4)}{Border(Get.BottomRight)}";
            tmp = isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);


            Dim = new Dimensions(Dim.Width, currentHeight);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = false;
            updateActiveField(true);

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        selectIndex++;
                        Render();
                        break;
                    case ConsoleKey.LeftArrow:
                        selectIndex--;
                        Render();
                        break;
                    case ConsoleKey.DownArrow:
                        Text = Content[selectIndex];
                        updateActiveField(false);
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                        Text = Content[selectIndex];
                        updateActiveField(false);
                        return ControllerState.Previous;
                    case ConsoleKey.Escape:
                        updateActiveField(false);
                        return ControllerState.Cancel;
                    case ConsoleKey.Enter:
                        updateActiveField(false);
                        Text = Content[selectIndex];
                        return ControllerState.Next;
                    default:
                        break;
                }

                Render();
            }
            return ControllerState.Idle;
        }

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal void updateActiveField(bool _active)
        {
            isActive = _active;
            Render();
        }
    }
}
