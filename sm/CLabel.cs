using System.Drawing;

namespace sm
{
    internal class CLabel : CObject
    {
        private CStyle _style;
        private string _text;

        public CLabel(CObject parent, Point pos, Align align, string text, CStyle style) : base(parent, pos, new Dimensions(text.Length, 1))
        {
            _text = text;
            _style = style;

            Initialize(parent, pos, align);
        }
        
        private void Initialize(CObject parent, Point pos, Align align)
        {
            if (ShouldRender && NewObjPos(parent, Aligner(align, parent, pos), Dim)) Render();
        }

        internal override void Render()
        {
            _text = _style.Set(_text, _style.Font);
            Write(Pos.Absolute, _text);
        }

        internal override void ChangeStyling(CStyle style)
        {
            _style = style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}
