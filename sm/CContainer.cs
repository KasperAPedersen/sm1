using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal abstract class CContainer : CRender, IPosition, IDimensions
    {
        public Position Pos { get; set; }
        public Dimensions Dim { get; set; }
        public CObject? Parent { get; set; }
        public List<CObject> Children { get; set; } = [];
        public bool ShouldRender { get; set; } = true;

        internal abstract void Render();
        internal abstract ControllerState Init();
        internal abstract bool NewObjPos(CObject? _parent, Point _pos, Dimensions _dim);
        internal abstract void Update(Point _pos);
        internal abstract Point Aligner(Align _align, CObject _parent, Point _pos);
        internal abstract void ChangeStyling(CStyle _style);
    }
}
