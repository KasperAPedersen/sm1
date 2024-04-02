using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CInput : CObject, IController
    {
        List<object> Styling = [];
        public string Text { get; set; }
        public string Label { get; set; }
        readonly int maxLength = 15;

        public CInput(CObject _parent, Point _pos, Dimensions _dim) : this(_parent, _pos, _dim, "") { }
        public CInput(CObject _parent, Point _pos, Dimensions _dim, string _label = "", Align _align = Align.None, List<object>? _styles = null) : base(_parent, _pos, _dim)
        {
            shouldRender = false;
            Text = "";
            Label = _label;
            maxLength = _dim.Width - 4;

            if(_styles != null) Styling = _styles;
            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp;

            // Top border
            tmp = Border(Get.TopLeft) + string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2)) + Border(Get.TopRight);
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            // Content line
            tmp = Border(Get.Vertical) + string.Concat(Enumerable.Repeat(" ", Dim.Width - 2)) + Border(Get.Vertical);
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);

            // TODO STYLING AND ADD GREY COLOR TO LABEL
            tmp = (Label != "" && Text == "") ? CStyling.Set(Label, [FontColor.grey]) : Text;
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(FontBgColor), typeof(FontColor), typeof(FontStyling)], Styling));
            Write(new Point(Pos.Absolute.X + 2, Pos.Absolute.Y + currentHeight++), tmp);

            // Bottom border
            tmp = Border(Get.BottomLeft) + string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2)) + Border(Get.BottomRight);
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = true;
            bool keepRunning = true;
            while(keepRunning)
            {
                SetPos(new Point(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1));
                ConsoleKeyInfo key = Console.ReadKey();
                switch(key.Key)
                {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Enter:
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.Escape:
                        return ControllerState.Previous;
                    case ConsoleKey.Backspace:
                        Text = Text != "" ? Text.Remove(Text.Length - 1, 1) : "";
                        break;
                    default:
                        if(Text.Length < maxLength - 1) Text += key.KeyChar;
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