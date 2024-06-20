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
        CStyle Style;
        public string Text { get; set; }

        public CButton(CObject _parent, Point _pos, Dimensions _dim, Align _align, string _text, CStyle _style) : base(_parent, _pos, new Dimensions(_dim.Width < _text.Length + 2 ? _text.Length + 2 : _dim.Width, _dim.Height < 3 ? 3 : _dim.Height))
        {
            Text = _text;
            Style = _style;

            if (ShouldRender && NewObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp;

            tmp = $"{Border(Get.TopLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.TopRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            int maxWidth = Dim.Width - 2;
            int diff = maxWidth - Text.Length;
            int diffRem = diff % 2;

            tmp = Style.Set(Border(Get.Vertical), Style.Border);
            tmp += BuildString(" ", diff / 2);

            tmp += Style.Set(Text, Style.Font);
            tmp += BuildString(" ", diff / 2 + diffRem);

            tmp += Style.Set(Border(Get.Vertical), Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.BottomRight)}";
            tmp = Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal override ControllerState Init()
        {
            CStyle OldStyling = Style;
            ChangeStyling(new CStyleBuilder().AddFont(Color.aquamarine1).Build());
            Console.CursorVisible = false;
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
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
