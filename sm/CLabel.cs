using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CLabel : CObject, IPosition, IDimensions
    {
        string Text {  get; set; }


        public CLabel(CObject _parent, string _text) : this(_parent, _text, Align.Left) { }
        public CLabel(CObject _parent, string _text, Align _align) : this(_parent, new Point(0, 0), _text, _align) { }
        public CLabel(CObject _parent, Point _pos, string _text, Align _align) : base(_parent, new Position(_pos, _pos), new Dimensions(_text.Length, 1))
        {
            Text = _text;

            switch (_align)
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

            Render();
        }

        internal override void Render()
        {
            
            Remove(Pos.Absolute, Dim);

            Write(Pos.Absolute, Text);
        }
    }
}
