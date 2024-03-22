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
        public List<CObject> Children { get; set; } = new List<CObject>();

        internal abstract void Render();
    }
}
