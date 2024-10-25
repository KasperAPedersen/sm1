using System.Drawing;

namespace sm
{
    internal abstract class CContainer : CRender, IPosition, IDimensions
    {
        public Position Pos { get; set; }
        public Dimensions Dim { get; set; }
        protected CObject? Parent { get; set; }
        protected List<CObject> Children { get; set; } = [];
        public bool ShouldRender { get; set; } = true;

        internal abstract void Render();
        internal abstract ControllerState Init();
        internal abstract bool NewObjPos(CObject? parent, Point pos, Dimensions dim);
        internal abstract void Update(Point pos);
        internal abstract Point Aligner(Align align, CObject parent, Point pos);
        internal abstract void ChangeStyling(CStyle style);
    }
}
