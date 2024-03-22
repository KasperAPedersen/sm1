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
        }

        internal override void Render()
        {
            throw new NotImplementedException();
        }

        internal void newObjPos(CObject _parent, Point _pos, Dimensions _dim)
        {
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
        }
    }
}
