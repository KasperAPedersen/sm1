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
        List<object> Styling = new List<object>();
        public CBox(CObject _parent) : this(_parent, new Point(0, 0), _parent.Dim) { }
        public CBox(CObject _parent, Point _pos, Dimensions _dim, Align _align = Align.None, List<object> _styles = null) : base(_parent, _pos, _dim)
        {
            if (Dim.Width < 3) Dim = new Dimensions(3, Dim.Height);
            if (Dim.Height < 3) Dim = new Dimensions(Dim.Width, 3);

            Styling = _styles;

            if (newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp = "";

            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.TopRight)}";
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            for(int i = 0; i < Dim.Height - 2; i++)
            {
                tmp = $"{Border(Get.Vertical)}{string.Concat(Enumerable.Repeat(" ", Dim.Width - 2))}{Border(Get.Vertical)}";
                tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
                Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            }

            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.BottomRight)}";
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override void ChangeStyling(List<object> _styles)
        {
            Styling = _styles;
            Remove(Pos.Absolute, Dim);
            Render();
        }
    }

    enum Align
    {
        None,
        Left,
        Middle,
        Right
    }
}
