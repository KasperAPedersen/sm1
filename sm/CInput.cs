using System.Drawing;

namespace sm
{
    internal class CInput : CObject, IController
    {
        private CStyle _style;
        public string Text { get; set; } = "";
        private string Label { get; }
        private readonly int _maxLength;
        private bool _isActive;

        public CInput(CObject parent, Point pos, Dimensions dim, CStyle style) : this(parent, pos, dim, "", style) { }
        public CInput(CObject parent, Point pos, Dimensions dim, string label, CStyle style, Align align = Align.None) : base(parent, pos, dim)
        {
            ShouldRender = false;
            Label = label;
            _maxLength = dim.Width - 8;

            _style = style;
            Initialize(parent, pos, align);
        }
        
        private void Initialize(CObject parent, Point pos, Align align)
        {
            if (ShouldRender && NewObjPos(parent, Aligner(align, parent, pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp;

            if (Text.Length > Dim.Width - 4) Text = Text[..(Dim.Width - 4)]; 

            tmp = Border(Get.TopLeft) + BuildString(Border(Get.Horizontal), Dim.Width - 2) + Border(Get.TopRight);
            tmp = _isActive ? _style.Set(tmp, [CRender.ActiveColor]) : _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = Border(Get.Vertical) + BuildString(" ", Dim.Width - 2) + Border(Get.Vertical);
            tmp = _isActive ? _style.Set(tmp, [CRender.ActiveColor]) : _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);

            tmp = (Label != "" && Text == "") ? _style.Set(Label, [Color.grey]) : Text;
            tmp = _style.Set(tmp, _style.Font);
            Write(new Point(Pos.Absolute.X + 2, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = Border(Get.BottomLeft) + BuildString(Border(Get.Horizontal), Dim.Width - 2) + Border(Get.BottomRight);
            tmp = _isActive ? _style.Set(tmp, [CRender.ActiveColor]) : _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = true;
            UpdateActiveField(true);

            while (true)
            {

                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Tab:
                        switch(key.Modifiers)
                        {
                            case ConsoleModifiers.Shift:
                                UpdateActiveField(false);
                                return ControllerState.Previous;
                            default:
                                UpdateActiveField(false);
                                return ControllerState.Next;
                        }
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Enter:
                        UpdateActiveField(false);
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                        UpdateActiveField(false);
                        return ControllerState.Previous;
                    case ConsoleKey.Escape:
                        return ControllerState.Cancel;
                    case ConsoleKey.Backspace:
                        Text = Text != "" ? Text.Remove(Text.Length - 1, 1) : "";
                        break;
                    default:
                        if (Text.Length < _maxLength - 1) Text += key.KeyChar;
                        break;
                }

                Render();
            }
        }

        internal override void ChangeStyling(CStyle style)
        {
            _style = style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        private void UpdateActiveField(bool active)
        {
            _isActive = active;
            Render();
        }
    }
}