using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace sm
{
    internal class CBox : CObject, IPosition, IDimensions
    {
        public CBox(CObject _parent) : this(_parent, new Point(0, 0), new Dimensions(_parent.Dim.Width, _parent.Dim.Height)) { }
        public CBox(CObject _parent, Dimensions _dim) : this(_parent, new Point(0, 0), _dim) { }
        public CBox(CObject _parent, Point _pos) : this(_parent, _pos, new Dimensions(_parent.Dim.Width, _parent.Dim.Height)) { }
        public CBox(CObject _parent, Align _align, Dimensions _dim) : this(_parent, new Point(0, 0), _dim)
        {
            switch(_align)
            {
                case Align.Left:
                    Pos = new Position(Pos.Absolute, new Point(0, 0));
                    break;
                case Align.Middle:
                    int diff = (Parent.Dim.Width - Parent.Pos.Absolute.X) / 2 - (Dim.Width / 2);
                    Update(new Point(diff, 0));
                    break;
                case Align.Right:
                    Update(new Point(Parent.Dim.Width - Parent.Pos.Absolute.X - Dim.Width, 0));
                    break;
                default:
                    break;
            }
        }
        public CBox(CObject _parent, Point _pos, Dimensions _dim) : base(_parent, new Position(_pos, _pos), _dim)
        {
            Render();
        }

        internal override void Render()
        {
            if (Parent != null && Dim.Width >= Parent.Dim.Width) Dim = new Dimensions(Parent.Dim.Width - 4, Dim.Height);
            if (Parent != null && Dim.Height >= Parent.Dim.Height) Dim = new Dimensions(Dim.Width, Parent.Dim.Height - 2);
            if (Dim.Height < 3 || Dim.Width < 3) return;

            Remove(Pos.Absolute, Dim);

            int currentHeight = 0;
            string tmp;
            

            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.TopRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            for (int i = 0; i < Dim.Height - 2; i++)
            {
                tmp = $"{Border(Get.Vertical)}{string.Concat(Enumerable.Repeat(" ", Dim.Width - 2))}{Border(Get.Vertical)}";
                Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
            }

            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.BottomRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }
    }

    enum Align
    {
        Left,
        Middle,
        Right
    }
}
