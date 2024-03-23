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
        public CObject(CObject? _parent, Point _pos, Dimensions _dim)
        {
            newObjPos(_parent, _pos, _dim);
            if (Parent != null) Parent.Children.Add(this);
        }

        internal override void Render()
        {
            throw new NotImplementedException();
        }

        internal override void Update(Point _pos)
        {
            Remove(Pos.Absolute, Dim);

            newObjPos(Parent, _pos, Dim);
            Render();
            foreach(CObject child in Children)
            {
                child.Update(_pos);
            }
        }

        internal override bool newObjPos(CObject? _parent, Point _pos, Dimensions _dim)
        {
            if (_parent != null && Dim.Width > _parent.Dim.Width - 4) return false;
            if (_parent != null && Dim.Height > _parent.Dim.Height - 2) return false;

            // Update rel pos if parent exists
            if (_pos.X < 2 && _parent != null) _pos = new Point(2, _pos.Y);
            if (_pos.Y < 1 && _parent != null) _pos = new Point(_pos.X, 1);

            if (_parent == null) _dim = new Dimensions(Console.WindowWidth, Console.WindowHeight - 2);

            if (_parent != null && _parent.Dim.Width <= _dim.Width) _dim = new Dimensions(_parent.Dim.Width - 4, _parent.Dim.Height);
            if (_parent != null && _parent.Dim.Height <= _dim.Height) _dim = new Dimensions(_dim.Width, _parent.Dim.Height - 2);

            // check for pos + width is out of parent
            if (_parent != null)
            {
                if (_parent.Pos.Absolute.X + _pos.X + _dim.Width > _parent.Dim.Width)
                {
                    int maxWidth = _parent.Dim.Width - 4;
                    int width = _pos.X + _dim.Width;
                    _pos = new Point(_pos.X + (maxWidth - width) + 2, _pos.Y);
                    
                }

                if(_parent.Pos.Absolute.Y + _pos.Y + _dim.Height > _parent.Dim.Height)
                {
                    int maxHeight = _parent.Dim.Height - 2;
                    int height = _pos.Y + _dim.Height;
                    _pos = new Point(_pos.X, _pos.Y + (maxHeight - height) + 1);
                    
                }
            }

            // Update abs pos if parent exists
            if (_parent != null)
            {
                Pos = new Position(new Point(_parent.Pos.Absolute.X + _pos.X, _parent.Pos.Absolute.Y + _pos.Y), _pos);
            }
            else
            {
                Pos = new Position(_pos, _pos);
            }

            Parent = _parent;
            Dim = _dim;
            return true;
        }

        internal override Point Aligner(Align _align, CObject _parent, Point _pos)
        {
            switch (_align)
            {
                case Align.Left:
                    _pos = new Point(0, 0);
                    break;
                case Align.Middle:
                    _pos = new Point((_parent.Dim.Width - Dim.Width) / 2, 0);
                    break;
                case Align.Right:
                    _pos = new Point((_parent.Dim.Width - Dim.Width), 0);
                    break;
                default:
                    break;
            }

            return _pos;
        }
    }
}
