using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CBox : CObject
    {
        public CStyle Style;
        public CBox(CObject _parent, CStyle _style) : this(_parent, new Point(0, 0), _parent.Dim, _style) { }
        public CBox(CObject _parent, Point _pos, Dimensions _dim, CStyle _style, Align _align = Align.None) : base(_parent, _pos, _dim)
        {
            if (Dim.Width < 3) Dim = new Dimensions(3, Dim.Height);
            if (Dim.Height < 3) Dim = new Dimensions(Dim.Width, 3);

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

            for (int i = 0; i < Dim.Height - 2; i++)
            {
                tmp = $"{Border(Get.Vertical)}{BuildString(" ", Dim.Width - 2)}{Border(Get.Vertical)}";
                tmp = Style.Set(tmp, Style.Border);
                Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            }

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
    }
}
