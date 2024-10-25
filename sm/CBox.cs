using System.Drawing;

namespace sm
{
    internal class CBox : CObject
    {
        private CStyle _style;
        public CBox(CObject parent, CStyle style) : this(parent, new Point(0, 0), parent.Dim, style) { }
        public CBox(CObject parent, Point pos, Dimensions dim, CStyle style, Align align = Align.None) : base(parent, pos, dim)
        {
            if (Dim.Width < 3) Dim = new Dimensions(3, Dim.Height);
            if (Dim.Height < 3) Dim = new Dimensions(Dim.Width, 3);

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
            string borderTop = $"{Border(Get.TopLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.TopRight)}";
            string borderMiddle = $"{Border(Get.Vertical)}{BuildString(" ", Dim.Width - 2)}{Border(Get.Vertical)}";
            string borderBottom = $"{Border(Get.BottomLeft)}{BuildString(Border(Get.Horizontal), Dim.Width - 2)}{Border(Get.BottomRight)}";

            string styledBorderTop = _style.Set(borderTop, _style.Border);
            string styledBorderMiddle = _style.Set(borderMiddle, _style.Border);
            string styledBorderBottom = _style.Set(borderBottom, _style.Border);

            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), styledBorderTop);

            for (int i = 0; i < Dim.Height - 2; i++)
            {
                Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), styledBorderMiddle);
            }

            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), styledBorderBottom);
        }

        internal override void ChangeStyling(CStyle style)
        {
            _style = style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}
