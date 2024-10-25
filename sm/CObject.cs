using System.Drawing;

namespace sm
{
    internal class CObject : CContainer
    {
        public CObject(CObject? parent, Point pos, Dimensions dim)
        {
            Initialize(parent, pos, dim);
            Parent?.Children.Add(this);
        }
        
        private void Initialize(CObject parent, Point pos, Dimensions dim)
        {
            NewObjPos(parent, pos, dim);
        }

        internal override void Render()
        {
            throw new NotImplementedException();
        }

        internal override ControllerState Init()
        {
            throw new NotImplementedException();
        }

        internal override void ChangeStyling(CStyle style)
        {
            throw new NotImplementedException();
        }

        internal override void Update(Point pos)
        {
            Remove(Pos.Absolute, Dim);

            NewObjPos(Parent, pos, Dim);
            Render();

            foreach(CObject child in Children) child.Update(pos);
        }

        internal void RenderChildren()
        {
            Render();
            foreach (CObject child in Children)
            {
                if(child.ShouldRender) child.RenderChildren();
            }
        }

        internal override bool NewObjPos(CObject? parent, Point pos, Dimensions dim)
        {
            if(parent != null)
            {
                if (Dim.Width > parent.Dim.Width - 4) return false;
                if (Dim.Height > parent.Dim.Height - 2) return false;

                // Update rel pos if parent exists
                if (pos.X < 2) pos = new Point(2, pos.Y);
                if (pos.Y < 1) pos = new Point(pos.X, 1);

                if (parent.Dim.Width <= dim.Width) dim = new Dimensions(parent.Dim.Width - 4, parent.Dim.Height);
                if (parent.Dim.Height <= dim.Height) dim = new Dimensions(dim.Width, parent.Dim.Height - 2);

                Pos = new Position(new Point(parent.Pos.Absolute.X + pos.X, parent.Pos.Absolute.Y + pos.Y), pos);
            } else
            {
                dim = new Dimensions(Console.WindowWidth, Console.WindowHeight - 2);
                Pos = new Position(pos, pos);
            }

            Parent = parent;
            Dim = dim;
            return true;
        }

        internal override Point Aligner(Align align, CObject parent, Point pos)
        {
            switch (align)
            {
                case Align.Left:
                    pos = new Point(0, pos.Y);
                    break;
                case Align.Middle:
                    pos = new Point((parent.Dim.Width - Dim.Width) / 2, pos.Y);
                    break;
                case Align.Right:
                    pos = new Point((parent.Dim.Width - Dim.Width) - 2, pos.Y);
                    break;
            }

            return pos;
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