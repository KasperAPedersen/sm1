﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CButton : CObject, IPosition, IDimensions
    {
        string Text = "";

        public CButton(CObject _parent, Point _pos, Dimensions _dim, Align _align, string _text) : base(_parent, _pos, new Dimensions(_dim.Width < _text.Length + 2 ? _text.Length + 2 : _dim.Width, _dim.Height < 3 ? 3 : _dim.Height))
        {
            Text = _text;

            if (newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp = "";

            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.TopRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);


            int maxWidth = Dim.Width - 2;
            int diff = maxWidth - Text.Length;
            int diffRem = diff % 2;
            tmp = $"{Border(Get.Vertical)}{string.Concat(Enumerable.Repeat(" ", diff / 2))}{Text}{string.Concat(Enumerable.Repeat(" ", diff / 2 + diffRem))}{Border(Get.Vertical)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.BottomRight)}";
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }
    }
}