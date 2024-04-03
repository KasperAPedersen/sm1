using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace sm
{
    internal class CLabel : CObject
    {
        CStyle Style;
        string Text = "";

        public CLabel(CObject _parent, Point _pos, Align _align, string _text, CStyle _style) : base(_parent, _pos, new Dimensions(_text.Length, 1))
        {
            Text = _text;
            Style = _style;

            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();

        }

        internal override void Render()
        {
            Text = Style.Set(Text, Style.Font);
            Write(Pos.Absolute, Text);
        }

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}
