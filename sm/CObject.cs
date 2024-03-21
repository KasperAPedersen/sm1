using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CObject : CContainer, IPosition, IDimensions
    {
        public CObject(CObject? _parent, Position _pos, Dimensions _dim)
        {
            if (_dim.Width >= Console.WindowWidth) _dim = new Dimensions(_dim.Width - 4, _dim.Height);
            if (_dim.Height >= Console.WindowHeight) _dim = new Dimensions(_dim.Width, _dim.Height - 2);

            if (_parent != null )
            {
                _pos = new Position(new Point(_parent.Pos.Absolute.X + _pos.Relative.X + 2, _parent.Pos.Absolute.Y + _pos.Relative.Y + 1), _pos.Relative);
                _parent.Children.Add(this);
            }

            Parent = _parent;
            Pos = _pos;
            Dim = _dim;
        }

        internal override void Render()
        {
            throw new NotImplementedException();
        }

        internal override void Update(Point _pos)
        {
            if (Parent != null && Pos.Absolute.X + _pos.X + Dim.Width > Parent.Dim.Width + 2) return;
            if (Parent != null && Pos.Absolute.Y + _pos.Y + Dim.Height > Parent.Dim.Height) return;

            Remove(Pos.Absolute, Dim);
            Pos = new Position(new Point(Pos.Absolute.X + _pos.X, Pos.Absolute.Y + _pos.Y), new Point(Pos.Relative.X + _pos.X, Pos.Relative.Y + _pos.Y));

            DrawChildren();
        }

        internal void DrawChildren()
        {
            Render();
            if (Children == null) return;

            foreach(CObject child in Children)
            {
                child.Pos = new Position(new Point(Pos.Absolute.X + child.Pos.Relative.X + 2, Pos.Absolute.Y + child.Pos.Relative.Y + 1), child.Pos.Relative);
                child.DrawChildren();
            }
        }
    }
}
