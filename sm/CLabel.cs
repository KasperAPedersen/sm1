using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CLabel : CObject
    {
        string Text = "";

        public CLabel(CObject _parent, Point _pos, Align _align, string _text) : base(_parent, _pos, new Dimensions(_text.Length, 1))
        {
            Text = _text;

            if (newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();

        }

        internal override void Render()
        {
            Write(Pos.Absolute, Text);
        }
    }
}
