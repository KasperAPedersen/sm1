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
        List<object> Styling = [];
        string Text = "";

        public CLabel(CObject _parent, Point _pos, Align _align, string _text, List<object>? _styles = null) : base(_parent, _pos, new Dimensions(_text.Length, 1))
        {
            Text = _text;
            if(_styles != null) Styling = _styles;

            if (shouldRender && newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();

        }

        internal override void Render()
        {
            Text = CStyling.Set(Text, CStyling.Get([typeof(FontBgColor), typeof(FontColor), typeof(FontStyling)], Styling));
            Write(Pos.Absolute, Text);
        }

        internal override void ChangeStyling(List<object> _styles)
        {
            Styling = _styles;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}
