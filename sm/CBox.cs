﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CBox : CObject, IPosition, IDimensions
    {
        public CBox(CObject _parent, Point _pos, Dimensions _dim, Align _align = Align.Left) : base(_parent, _pos, _dim)
        {
            switch (_align)
            {
                case Align.Left:
                    _pos = new Point(0, 0);
                    newObjPos(_parent, _pos, _dim);
                    break;
                case Align.Middle:
                    int diff = (_parent.Dim.Width - Dim.Width) / 2;
                    newObjPos(_parent, new Point(diff, 0), _dim);
                    break;
                case Align.Right:
                    newObjPos(_parent, new Point((_parent.Dim.Width - Dim.Width), 0), _dim);
                    break;
                default:
                    break;
            }

            Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp = "";

            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.TopRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            for(int i = 0; i < Dim.Height; i++)
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
