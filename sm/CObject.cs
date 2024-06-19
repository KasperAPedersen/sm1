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
            NewObjPos(_parent, _pos, _dim);
            Parent?.Children.Add(this);
        }

        internal override void Render()
        {
            throw new NotImplementedException();
        }

        internal override ControllerState Init()
        {
            throw new NotImplementedException();
        }

        internal override void ChangeStyling(CStyle _style)
        {
            throw new NotImplementedException();
        }

        internal override void Update(Point _pos)
        {
            Remove(Pos.Absolute, Dim);

            NewObjPos(Parent, _pos, Dim);
            Render();

            foreach(CObject child in Children) child.Update(_pos);
        }

        internal void RenderChildren()
        {
            Render();
            foreach (CObject child in Children)
            {
                if(child.ShouldRender) child.RenderChildren();
            }
        }

        internal override bool NewObjPos(CObject? _parent, Point _pos, Dimensions _dim)
        {
            if(_parent != null)
            {
                if (Dim.Width > _parent.Dim.Width - 4) return false;
                if (Dim.Height > _parent.Dim.Height - 2) return false;

                // Update rel pos if parent exists
                if (_pos.X < 2) _pos = new Point(2, _pos.Y);
                if (_pos.Y < 1) _pos = new Point(_pos.X, 1);

                if (_parent.Dim.Width <= _dim.Width) _dim = new Dimensions(_parent.Dim.Width - 4, _parent.Dim.Height);
                if (_parent.Dim.Height <= _dim.Height) _dim = new Dimensions(_dim.Width, _parent.Dim.Height - 2);

                Pos = new Position(new Point(_parent.Pos.Absolute.X + _pos.X, _parent.Pos.Absolute.Y + _pos.Y), _pos);
            } else
            {
                _dim = new Dimensions(Console.WindowWidth, Console.WindowHeight - 2);
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
                    _pos = new Point(0, _pos.Y);
                    break;
                case Align.Middle:
                    _pos = new Point((_parent.Dim.Width - Dim.Width) / 2, _pos.Y);
                    break;
                case Align.Right:
                    _pos = new Point((_parent.Dim.Width - Dim.Width) - 2, _pos.Y);
                    break;
                default:
                    break;
            }

            return _pos;
        }
    }

    enum Align
    {
        None,
        Left,
        Middle,
        Right
    }
}