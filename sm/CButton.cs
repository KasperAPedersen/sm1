using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CButton : CObject, IController
    {
        List<object> Styling;
        public string Text { get; set; }

        public CButton(CObject _parent, Point _pos, Dimensions _dim, Align _align, string _text, List<object>?_styles = null) : base(_parent, _pos, new Dimensions(_dim.Width < _text.Length + 2 ? _text.Length + 2 : _dim.Width, _dim.Height < 3 ? 3 : _dim.Height))
        {
            Text = _text;
            Styling = _styles ?? [];

            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp;

            // Border of box 
            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.TopRight)}";
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            int maxWidth = Dim.Width - 2;
            int diff = maxWidth - Text.Length;
            int diffRem = diff % 2;

            // Border of text
            tmp = CStyling.Set($"{Border(Get.Vertical)}", CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            tmp += $"{string.Concat(Enumerable.Repeat(" ", diff / 2))}";

            // Text styling
            tmp += CStyling.Set(Text, CStyling.Get([typeof(FontBgColor), typeof(FontColor), typeof(FontStyling)], Styling));
            tmp += $"{string.Concat(Enumerable.Repeat(" ", diff / 2 + diffRem))}";

            // Border of text
            tmp += CStyling.Set($"{Border(Get.Vertical)}", CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            // Border of box
            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.BottomRight)}";
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override void ChangeStyling(List<object> _styles)
        {
            Styling = _styles;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal override ControllerState Init()
        {
            List<object> OldStyling = Styling;
            ChangeStyling([FontColor.red]);
            Console.CursorVisible = false;
            bool keepRunning = true;
            while (keepRunning)
            {
                SetPos(new Point(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1));
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                        ChangeStyling(OldStyling);
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.Escape:
                        ChangeStyling(OldStyling);
                        Render();
                        return ControllerState.Previous;
                    case ConsoleKey.Enter:
                        return Text == "Confirm" ? ControllerState.Finish : ControllerState.Cancel;
                    default:
                        break;
                }

                Render();
            }
            return ControllerState.Idle;
        }
    }
}
