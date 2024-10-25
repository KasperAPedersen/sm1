using System.Drawing;

namespace sm
{
    internal class CComboBox : CObject, IController
    {
        CStyle Style { get; set; }

        readonly List<string> _content;
        public string Text { get; set; } = "";
        public int SelectIndex { get; set; }
        private bool _isActive;

        public CComboBox(CObject parent, Point pos, Dimensions dim, List<string> content, CStyle style, Align align = Align.None) : base(parent, pos, dim)
        {
            Dim = new Dimensions(Dim.Width, 3);
            ShouldRender = false;
            _content = content;
            Style = style;

            Initialize(parent, pos, align);
        }
        
        private void Initialize(CObject parent, Point pos, Align align)
        {
            if (ShouldRender && NewObjPos(parent, Aligner(align, parent, pos), Dim)) Render();
        }

        internal override void Render()
        {
            Remove(Pos.Absolute, Dim);

            if (SelectIndex < 0) SelectIndex = _content.Count - 1;
            if (SelectIndex > _content.Count - 1) SelectIndex = 0;


            Text = _content[SelectIndex];
            string contentText = _content[SelectIndex];

            if (contentText.Length > Dim.Width - 8) contentText = contentText[..(Dim.Width - 8)];

            int currentHeight = 0;
            string tmp = "";

            tmp = $"{Border(Get.TopLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 7)}{Border(Get.HorizontalDown)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 4)}{Border(Get.TopRight)}";
            tmp = _isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);


            tmp = $"{Border(Get.Vertical)}";
            tmp += $" {contentText}{new string(' ', Dim.Width - 8 - Text.Length)}";
            tmp += $"{Border(Get.Vertical)}";
            tmp += $" {Border(Get.ArrowLeft)}{Border(Get.ArrowRight)} {Border(Get.Vertical)}";
            tmp = _isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 7)}{Border(Get.HorizontalUp)}";
            tmp += $"{BuildString(Border(Get.Horizontal), 4)}{Border(Get.BottomRight)}";
            tmp = _isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);
            

            Dim = new Dimensions(Dim.Width, currentHeight);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = false;
            UpdateActiveField(true);

            while (true)
            {
                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Tab:
                        Text = _content[SelectIndex];
                        switch (key.Modifiers)
                        {
                            case ConsoleModifiers.Shift:
                                UpdateActiveField(false);
                                return ControllerState.Previous;
                            default:
                                UpdateActiveField(false);
                                return ControllerState.Next;
                        }
                    case ConsoleKey.RightArrow:
                        SelectIndex++;
                        Render();
                        break;
                    case ConsoleKey.LeftArrow:
                        SelectIndex--;
                        Render();
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.DownArrow:
                        Text = _content[SelectIndex];
                        UpdateActiveField(false);
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                        Text = _content[SelectIndex];
                        UpdateActiveField(false);
                        return ControllerState.Previous;
                    case ConsoleKey.Escape:
                        UpdateActiveField(false);
                        return ControllerState.Cancel;
                }

                Render();
            }
        }

        internal override void ChangeStyling(CStyle style)
        {
            Style = style;
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
