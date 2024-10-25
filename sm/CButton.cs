using System.Drawing;

namespace sm
{
    internal class CButton : CObject, IController
    {
        private CStyle _style;
        public string Text { get; set; }

        public CButton(CObject parent, Point pos, Dimensions dim, Align align, string text, CStyle style) : base(parent, pos, new Dimensions(dim.Width < text.Length + 2 ? text.Length + 2 : dim.Width, dim.Height < 3 ? 3 : dim.Height))
        {
            Text = text;
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
            string tmp = "";

            tmp = $"{Border(Get.TopLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.TopRight)}";
            tmp = _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            int maxWidth = Dim.Width - 2;
            int diff = maxWidth - Text.Length;
            int diffRem = diff % 2;

            tmp = _style.Set(Border(Get.Vertical), _style.Border);
            tmp += BuildString(" ", diff / 2);

            tmp += _style.Set(Text, _style.Font);
            tmp += BuildString(" ", diff / 2 + diffRem);

            tmp += _style.Set(Border(Get.Vertical), _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.BottomRight)}";
            tmp = _style.Set(tmp, _style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override void ChangeStyling(CStyle style)
        {
            _style = style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal override ControllerState Init()
        {
            CStyle oldStyling = _style;
            ChangeStyling(new CStyleBuilder().AddFont(CRender.ActiveColor).Build());
            Console.CursorVisible = false;
            while (true)
            {
                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Tab:
                        ChangeStyling(oldStyling);
                        switch (key.Modifiers)
                        {
                            case ConsoleModifiers.Shift:
                                return ControllerState.Previous;
                            default:
                                return ControllerState.Next;
                        }
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                        ChangeStyling(oldStyling);
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                        ChangeStyling(oldStyling);
                        Render();
                        return ControllerState.Previous;
                    case ConsoleKey.Escape:
                        return ControllerState.Cancel;
                    case ConsoleKey.Enter:
                        return Text == "Confirm" ? ControllerState.Finish : ControllerState.Cancel;
                }

                Render();
            }
        }
    }
}
